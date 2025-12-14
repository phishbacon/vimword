using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using vimword.Vimulator.Motions;

namespace vimword.Vimulator.Modes
{
    /// <summary>
    /// Normal Mode - default mode for navigation and commands.
    /// </summary>
    internal class NormalMode : IVimMode
    {
        private readonly Microsoft.Office.Interop.Word.Application _app;
        private readonly Dictionary<KeyChord, Func<ModeTransitionResult>> _keyActions;
        private readonly Dictionary<KeyChord, IMotion> _motions;
        private IModeContext _context;

        public NormalMode(Microsoft.Office.Interop.Word.Application app)
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
                [new KeyChord(Keys.D4, Constants.Modifiers.SHIFT)] = new LineEndMotion(),           // $ (Shift+4)
                [new KeyChord(Keys.OemMinus, Constants.Modifiers.SHIFT)] = new FirstNonBlankMotion() // _ (Shift+-)
            };
            
            _keyActions = new Dictionary<KeyChord, Func<ModeTransitionResult>>
            {
                [new KeyChord(Keys.I)] = InsertAtCursor,
                [new KeyChord(Keys.A)] = AppendAfterCursor,
                [new KeyChord(Keys.V)] = EnterVisualMode
            };
        }

        public Constants.Modes Mode => Constants.Modes.NORMAL;

        public void OnEnter(IModeContext context)
        {
            _context = context;
        }

        public void OnExit()
        {
        }

        public ModeTransitionResult HandleKey(Keys key)
        {
            var chord = KeyChord.FromKeys(key);
            
            // Check for mode transitions first
            if (_keyActions.TryGetValue(chord, out var action))
            {
                return action();
            }

            // Check for motions
            if (_motions.TryGetValue(chord, out var motion))
            {
                motion.Execute(_app, extend: false);
                return new ModeTransitionResult { Handled = true };
            }

            return new ModeTransitionResult { Handled = true };
        }

        #region Mode Transitions

        private ModeTransitionResult InsertAtCursor()
        {
            return new ModeTransitionResult
            {
                Handled = true,
                NextMode = Constants.Modes.INSERT
            };
        }

        private ModeTransitionResult AppendAfterCursor()
        {
            return new ModeTransitionResult
            {
                Handled = true,
                NextMode = Constants.Modes.INSERT,
                PostTransitionAction = () => _app.Selection.Start++
            };
        }

        private ModeTransitionResult EnterVisualMode()
        {
            return new ModeTransitionResult
            {
                Handled = true,
                NextMode = Constants.Modes.VISUAL
            };
        }

        #endregion
    }
}
