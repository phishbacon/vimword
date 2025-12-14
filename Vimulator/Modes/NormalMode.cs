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
        private readonly Dictionary<Keys, Func<ModeTransitionResult>> _keyActions;
        private readonly Dictionary<Keys, IMotion> _motions;
        private readonly Dictionary<Keys, IMotion> _bigMotions;
        private IModeContext _context;

        public NormalMode(Microsoft.Office.Interop.Word.Application app)
        {
            _app = app;
            
            _motions = new Dictionary<Keys, IMotion>
            {
                [Keys.H] = new LeftMotion(),
                [Keys.L] = new RightMotion(),
                [Keys.K] = new UpMotion(),
                [Keys.J] = new DownMotion(),
                [Keys.W] = new WordForwardMotion(),
                [Keys.B] = new WordBackMotion(),
                [Keys.E] = new WordEndMotion()
            };

            _bigMotions = new Dictionary<Keys, IMotion>
            {
                [Keys.W] = new WordForwardBigMotion(),
                [Keys.B] = new WordBackBigMotion(),
                [Keys.E] = new WordEndBigMotion()
            };
            
            _keyActions = new Dictionary<Keys, Func<ModeTransitionResult>>
            {
                [Keys.I] = InsertAtCursor,
                [Keys.A] = AppendAfterCursor,
                [Keys.V] = EnterVisualMode
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
            Keys baseKey = key & Keys.KeyCode;
            
            // Check for mode transitions first
            if (_keyActions.TryGetValue(baseKey, out var action))
            {
                return action();
            }

            // Check for motions
            bool shiftPressed = (Control.ModifierKeys & Keys.Shift) == Keys.Shift;
            IMotion motion = null;

            if (shiftPressed && _bigMotions.TryGetValue(baseKey, out motion))
            {
                motion.Execute(_app, extend: false);
                return new ModeTransitionResult { Handled = true };
            }

            if (_motions.TryGetValue(baseKey, out motion))
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
