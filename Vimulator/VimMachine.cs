using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using vimword.Src.VimStatusDisplay;
using vimword.Vimulator.Modes;
using UserControl = vimword.Src.VimStatusDisplay.UserControl;

namespace vimword.Vimulator
{
    /// <summary>
    /// Core Vim state machine that manages mode transitions and delegates key handling.
    /// </summary>
    internal class VimMachine : IVimMachine, IModeContext
    {
        private Constants.Modes _mode;
        private readonly Dictionary<Constants.Modes, IVimMode> _modeMap;
        private readonly Microsoft.Office.Interop.Word.Application _app;
        private IVimMode _currentModeInstance;
        private string _keyBuffer = "";
        private int _currentLine = 1;
        private int _currentColumn = 1;

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region IVimMachine Implementation

        /// <summary>
        /// Gets or sets the current Vim mode. Setting triggers mode lifecycle events and UI notifications.
        /// </summary>
        public Constants.Modes CurrentMode
        {
            get => _mode;
            set
            {
                if (_mode != value)
                {
                    _currentModeInstance?.OnExit();
                    _mode = value;
                    _currentModeInstance = _modeMap[_mode];
                    _currentModeInstance.OnEnter(this);
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Handles a key press by routing to the current mode and managing transitions.
        /// </summary>
        public bool HandleKey(Keys key)
        {
            if (key == Keys.Escape)
            {
                CurrentMode = Constants.Modes.NORMAL;
                KeyBuffer = ""; // Clear key buffer on Escape
                UpdateCursorPosition();
                return true;
            }

            ModeTransitionResult result = _currentModeInstance.HandleKey(key);

            if (result.NextMode.HasValue)
            {
                CurrentMode = result.NextMode.Value;
                result.PostTransitionAction?.Invoke();
            }
            
            // Update cursor position after handling key
            UpdateCursorPosition();

            return result.Handled;
        }
        
        /// <summary>
        /// Gets the current line number (1-based).
        /// </summary>
        public int CurrentLine
        {
            get => _currentLine;
            private set
            {
                if (_currentLine != value)
                {
                    _currentLine = value;
                    OnPropertyChanged();
                }
            }
        }
        
        /// <summary>
        /// Gets the current column number (1-based).
        /// </summary>
        public int CurrentColumn
        {
            get => _currentColumn;
            private set
            {
                if (_currentColumn != value)
                {
                    _currentColumn = value;
                    OnPropertyChanged();
                }
            }
        }
        
        /// <summary>
        /// Updates the cursor position from the Word application.
        /// </summary>
        public void UpdateCursorPosition()
        {
            try
            {
                if (_app?.Selection != null)
                {
                    // Get line number (1-based)
                    int line = (int)_app.Selection.Information[WdInformation.wdFirstCharacterLineNumber];
                    
                    // Get column number (1-based)
                    int column = (int)_app.Selection.Information[WdInformation.wdFirstCharacterColumnNumber];
                    
                    CurrentLine = line;
                    CurrentColumn = column;
                }
            }
            catch
            {
                // Silently ignore errors when accessing Word selection
                // (can happen during certain Word operations)
            }
        }

        #endregion

        #region IModeContext Implementation

        public Microsoft.Office.Interop.Word.Application Application => _app;

        public void RequestModeChange(Constants.Modes mode, Action postTransition = null)
        {
            CurrentMode = mode;
            postTransition?.Invoke();
        }
        
        /// <summary>
        /// Gets or sets the current key buffer to display in UI (e.g., "5w").
        /// </summary>
        public string KeyBuffer
        {
            get => _keyBuffer;
            set
            {
                if (_keyBuffer != value)
                {
                    _keyBuffer = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Constructor

        public VimMachine(IEnumerable<IVimMode> modes, Microsoft.Office.Interop.Word.Application app)
        {
            _app = app;
            _modeMap = modes.ToDictionary(m => m.Mode);
            _mode = Constants.Modes.NORMAL;
            _currentModeInstance = _modeMap[_mode];
            _currentModeInstance.OnEnter(this);
            UpdateCursorPosition(); // Initialize cursor position
        }

        #endregion
    }
}
