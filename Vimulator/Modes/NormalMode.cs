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
        
        // Count prefix for motions (e.g., 5w moves forward 5 words)
        private int _count = 0;

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
            
            // Check for digit keys to build count (except 0 as first digit - that's a motion)
            if (chord.Modifiers == Constants.Modifiers.NONE && IsDigitKey(chord.Key))
            {
                int digit = GetDigitFromKey(chord.Key);
                
                // Special case: 0 as first digit is the "go to line start" motion, not a count
                if (digit == 0 && _count == 0)
                {
                    // Execute 0 motion (line start)
                    if (_motions.TryGetValue(chord, out var zeroMotion))
                    {
                        zeroMotion.Execute(_app, extend: false);
                        _context.KeyBuffer = ""; // Clear display
                        return new ModeTransitionResult { Handled = true };
                    }
                }
                else
                {
                    // Build count
                    _count = _count * 10 + digit;
                    _context.KeyBuffer = _count.ToString(); // Update display
                    return new ModeTransitionResult { Handled = true };
                }
            }
            
            // Check for mode transitions
            if (_keyActions.TryGetValue(chord, out var action))
            {
                _count = 0;
                _context.KeyBuffer = ""; // Clear display
                return action();
            }

            // Check for motions
            if (_motions.TryGetValue(chord, out var motion))
            {
                int count = _count == 0 ? 1 : _count;
                _count = 0;
                
                // Show the motion being executed
                string motionKey = GetKeyDisplayName(chord);
                _context.KeyBuffer = count > 1 ? count.ToString() + motionKey : motionKey;
                
                motion.Execute(_app, extend: false, count: count);
                
                // Clear display after motion executes
                _context.KeyBuffer = "";
                return new ModeTransitionResult { Handled = true };
            }

            _count = 0;
            _context.KeyBuffer = ""; // Clear display on unrecognized key
            return new ModeTransitionResult { Handled = true };
        }
        
        private bool IsDigitKey(Keys key)
        {
            return (key >= Keys.D0 && key <= Keys.D9);
        }
        
        private int GetDigitFromKey(Keys key)
        {
            return (int)key - (int)Keys.D0;
        }
        
        private string GetKeyDisplayName(KeyChord chord)
        {
            // Convert key to display string (e.g., Keys.W -> "w", Keys.D0 -> "0")
            string keyName = chord.Key.ToString();
            
            // Handle digit keys
            if (keyName.StartsWith("D") && keyName.Length == 2 && char.IsDigit(keyName[1]))
            {
                return keyName.Substring(1);
            }
            
            // Handle letter keys
            if (keyName.Length == 1)
            {
                return (chord.Modifiers & Constants.Modifiers.SHIFT) != 0 ? keyName : keyName.ToLower();
            }
            
            // Handle special keys
            if (chord.Key == Keys.OemMinus && (chord.Modifiers & Constants.Modifiers.SHIFT) != 0)
                return "_";
            
            return keyName.ToLower();
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
