using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using vimword.Vimulator.Motions;

namespace vimword.Vimulator.Modes
{
    /// <summary>
    /// Visual Mode - for selecting text character-by-character.
    /// Motion keys extend the selection rather than just moving the cursor.
    /// Tracks selection anchor and direction to properly handle forward/backward motions.
    /// </summary>
    internal class VisualMode : IVimMode
    {
        private readonly Microsoft.Office.Interop.Word.Application _app;
        private readonly Dictionary<KeyChord, IMotion> _motions;
        private IModeContext _context;
        
        private int _anchorPosition;
        
        private enum ActiveEnd { Start, End }
        private ActiveEnd _activeEnd;

        public VisualMode(Microsoft.Office.Interop.Word.Application app)
        {
            _app = app;
            
            _motions = new Dictionary<KeyChord, IMotion>
            {
                // Character motions
                [new KeyChord(Keys.H)] = new LeftMotion(),
                [new KeyChord(Keys.L)] = new RightMotion(),
                [new KeyChord(Keys.K)] = new UpMotion(),
                [new KeyChord(Keys.J)] = new DownMotion(),
                
                // Word motions (lowercase)
                [new KeyChord(Keys.W)] = new WordForwardMotion(includePunctuation: false),
                [new KeyChord(Keys.B)] = new WordBackMotion(includePunctuation: false),
                [new KeyChord(Keys.E)] = new WordEndMotion(includePunctuation: false),
                
                // Word motions (uppercase/WORD - with Shift)
                [new KeyChord(Keys.W, Constants.Modifiers.SHIFT)] = new WordForwardMotion(includePunctuation: true),
                [new KeyChord(Keys.B, Constants.Modifiers.SHIFT)] = new WordBackMotion(includePunctuation: true),
                [new KeyChord(Keys.E, Constants.Modifiers.SHIFT)] = new WordEndMotion(includePunctuation: true),
                
                // Line motions
                [new KeyChord(Keys.D0)] = new LineStartMotion(),
                [new KeyChord(Keys.D4, Constants.Modifiers.SHIFT)] = new LineEndMotion(),
                [new KeyChord(Keys.OemMinus, Constants.Modifiers.SHIFT)] = new FirstNonBlankMotion()
            };
        }

        public Constants.Modes Mode => Constants.Modes.VISUAL;

        public void OnEnter(IModeContext context)
        {
            _context = context;

            _anchorPosition = _app.Selection.Start;
            _activeEnd = ActiveEnd.End;

            if (_app.Selection.Start == _app.Selection.End)
            {
                _app.Selection.End = _app.Selection.Start + 1;
            }
        }

        public void OnExit()
        {
            _app.Selection.Collapse(WdCollapseDirection.wdCollapseStart);
        }

        public ModeTransitionResult HandleKey(Keys key)
        {
            var chord = KeyChord.FromKeys(key);

            if (_motions.TryGetValue(chord, out var motion))
            {
                ExecuteMotionWithDirection(motion);
                return new ModeTransitionResult { Handled = true };
            }

            return new ModeTransitionResult { Handled = true };
        }

        private void ExecuteMotionWithDirection(IMotion motion)
        {
            var selection = _app.Selection;
            var doc = selection.Document;
            
            int positionToMoveFrom;
            bool isShrinking = false;

            if (_activeEnd == ActiveEnd.End)
            {
                positionToMoveFrom = selection.End;
                // If End is active and motion is backward, we're shrinking
                isShrinking = motion.Direction == MotionDirection.Backward;
            }
            else
            {
                positionToMoveFrom = selection.Start;
                // If Start is active and motion is forward, we're shrinking
                isShrinking = motion.Direction == MotionDirection.Forward;
            }

            selection.SetRange(positionToMoveFrom, positionToMoveFrom);
            
            motion.Execute(_app, extend: false);
            
            int newPosition = selection.Start;
            
            // When extending with motions that position ON target (e, $), add +1 to include that character
            if (motion.IncludesTarget && _activeEnd == ActiveEnd.End && !isShrinking)
            {
                newPosition = selection.Start + 1;
            }
            
            // When shrinking with backward motion from End, subtract 1 to exclude the character we land on
            // This is the opposite of the +1 we do for 'e' when extending
            if (isShrinking && _activeEnd == ActiveEnd.End && motion.Direction == MotionDirection.Backward)
            {
                if (newPosition > 0)
                {
                    newPosition = newPosition - 1;
                }
                
                // If shrinking would move past the anchor, just stop at the anchor
                if (newPosition <= _anchorPosition)
                {
                    newPosition = _anchorPosition;
                }
            }
            
            // Similarly for shrinking forward from Start
            if (isShrinking && _activeEnd == ActiveEnd.Start && motion.Direction == MotionDirection.Forward)
            {
                // If shrinking would move past the anchor, just stop at the anchor
                if (newPosition >= _anchorPosition)
                {
                    newPosition = _anchorPosition;
                }
            }

            if (_activeEnd == ActiveEnd.End)
            {
                if (newPosition < _anchorPosition)
                {
                    _activeEnd = ActiveEnd.Start;
                    selection.SetRange(newPosition, _anchorPosition);
                }
                else if (newPosition == _anchorPosition)
                {
                    // Collapsed at anchor - next backward motion should extend Start backward
                    _activeEnd = ActiveEnd.Start;
                    selection.SetRange(newPosition, _anchorPosition);
                }
                else
                {
                    selection.SetRange(_anchorPosition, newPosition);
                }
            }
            else
            {
                if (newPosition > _anchorPosition)
                {
                    _activeEnd = ActiveEnd.End;
                    selection.SetRange(_anchorPosition, newPosition);
                }
                else if (newPosition == _anchorPosition)
                {
                    // Collapsed at anchor - next forward motion should extend End forward
                    _activeEnd = ActiveEnd.End;
                    selection.SetRange(_anchorPosition, newPosition);
                }
                else
                {
                    selection.SetRange(newPosition, _anchorPosition);
                }
            }

            // Update active end based on motion direction
            if (motion.Direction == MotionDirection.Forward && newPosition > _anchorPosition)
            {
                _activeEnd = ActiveEnd.End;
            }
            else if (motion.Direction == MotionDirection.Backward && newPosition < _anchorPosition)
            {
                _activeEnd = ActiveEnd.Start;
            }
        }
    }
}
