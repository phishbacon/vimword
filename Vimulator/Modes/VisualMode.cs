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
        private readonly Dictionary<Keys, IMotion> _motions;
        private readonly Dictionary<Keys, IMotion> _bigMotions;
        private IModeContext _context;
        
        // Track the anchor point (where Visual mode started)
        private int _anchorPosition;
        
        // Track which end is "active" (moving)
        private enum ActiveEnd { Start, End }
        private ActiveEnd _activeEnd;

        public VisualMode(Microsoft.Office.Interop.Word.Application app)
        {
            _app = app;
            
            _motions = new Dictionary<Keys, IMotion>
            {
                // Character motions
                [Keys.H] = new LeftMotion(),
                [Keys.L] = new RightMotion(),
                [Keys.K] = new UpMotion(),
                [Keys.J] = new DownMotion(),
                
                // word motions (lowercase)
                [Keys.W] = new WordForwardMotion(),
                [Keys.B] = new WordBackMotion(),
                [Keys.E] = new WordEndMotion()
            };

            _bigMotions = new Dictionary<Keys, IMotion>
            {
                // WORD motions (Shift + key)
                [Keys.W] = new WordForwardBigMotion(),
                [Keys.B] = new WordBackBigMotion(),
                [Keys.E] = new WordEndBigMotion()
            };
        }

        public Constants.Modes Mode => Constants.Modes.VISUAL;

        public void OnEnter(IModeContext context)
        {
            _context = context;

            // Store the anchor position when entering Visual mode
            _anchorPosition = _app.Selection.Start;
            _activeEnd = ActiveEnd.End;  // Initially, End is active (extends forward)

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
            Keys baseKey = key & Keys.KeyCode;
            bool shiftPressed = (Control.ModifierKeys & Keys.Shift) == Keys.Shift;

            // Determine if this is a forward or backward motion
            bool isBackwardMotion = (baseKey == Keys.B || baseKey == Keys.H);
            bool isForwardMotion = (baseKey == Keys.W || baseKey == Keys.E || baseKey == Keys.L);

            IMotion motion = null;

            // Check for Shift variant first
            if (shiftPressed && _bigMotions.TryGetValue(baseKey, out motion))
            {
                ExecuteMotionWithDirection(motion, isBackwardMotion, isForwardMotion);
                return new ModeTransitionResult { Handled = true };
            }

            // Check for normal motion
            if (_motions.TryGetValue(baseKey, out motion))
            {
                ExecuteMotionWithDirection(motion, isBackwardMotion, isForwardMotion);
                return new ModeTransitionResult { Handled = true };
            }

            return new ModeTransitionResult { Handled = true };
        }

        private void ExecuteMotionWithDirection(IMotion motion, bool isBackwardMotion, bool isForwardMotion)
        {
            var selection = _app.Selection;
            
            // Determine if we're shrinking or extending
            // Shrinking: motion goes toward the anchor
            // Extending: motion goes away from the anchor
            bool isShrinking = false;
            int positionToMoveFrom;

            if (_activeEnd == ActiveEnd.End)
            {
                // End is active (selection extends forward from anchor)
                if (isBackwardMotion)
                {
                    // Backward motion with End active = shrinking
                    isShrinking = true;
                    positionToMoveFrom = selection.End;  // Shrink from End
                }
                else
                {
                    // Forward motion with End active = extending
                    isShrinking = false;
                    positionToMoveFrom = selection.End;  // Extend from End
                }
            }
            else // _activeEnd == ActiveEnd.Start
            {
                // Start is active (selection extends backward from anchor)
                if (isForwardMotion)
                {
                    // Forward motion with Start active = shrinking
                    isShrinking = true;
                    positionToMoveFrom = selection.Start;  // Shrink from Start
                }
                else
                {
                    // Backward motion with Start active = extending
                    isShrinking = false;
                    positionToMoveFrom = selection.Start;  // Extend from Start
                }
            }

            // Collapse to the position we're moving from
            selection.SetRange(positionToMoveFrom, positionToMoveFrom);
            
            // Execute the motion to get the new position
            motion.Execute(_app, extend: false);
            
            // Get the new position after motion
            int newPosition = selection.Start;

            // Apply the new selection based on whether we're shrinking or extending
            if (_activeEnd == ActiveEnd.End)
            {
                // End is/was active
                if (newPosition < _anchorPosition)
                {
                    // Crossed the anchor - switch to Start being active
                    _activeEnd = ActiveEnd.Start;
                    selection.SetRange(newPosition, _anchorPosition);
                }
                else
                {
                    // Didn't cross anchor - Start at anchor, End at new position
                    selection.SetRange(_anchorPosition, newPosition);
                }
            }
            else // _activeEnd == ActiveEnd.Start
            {
                // Start is/was active
                if (newPosition > _anchorPosition)
                {
                    // Crossed the anchor - switch to End being active
                    _activeEnd = ActiveEnd.End;
                    selection.SetRange(_anchorPosition, newPosition);
                }
                else
                {
                    // Didn't cross anchor - Start at new position, End at anchor
                    selection.SetRange(newPosition, _anchorPosition);
                }
            }

            // Update active end based on motion direction (for next iteration)
            if (isForwardMotion && newPosition > _anchorPosition)
            {
                _activeEnd = ActiveEnd.End;
            }
            else if (isBackwardMotion && newPosition < _anchorPosition)
            {
                _activeEnd = ActiveEnd.Start;
            }
        }
    }
}
