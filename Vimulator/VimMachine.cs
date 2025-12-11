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
    internal class VimMachine : IVimMachine
    {
        private Constants.Modes _mode;
        private readonly IEnumerable<IVimMode> _modes;
        private event PropertyChangedEventHandler _modeChanged;

        public Constants.Modes CurrentMode
        {
            get
            {
                return _mode; 
            }
            set
            {
                if (_mode != value)
                {
                    _mode = value;
                    OnPropertyChanged();
                }
            }
        }

        public PropertyChangedEventHandler ModeChanged
        {
            get
            {
                return _modeChanged;
            } 
            set
            {
                if (_modeChanged != value)
                {
                    _modeChanged = value;
                }
            }
        }

        public VimMachine(IEnumerable<IVimMode> modes)
        {
            _mode = Constants.Modes.NORMAL;
            _modes = modes;
        }

        public bool HandleKey(Keys key)
        {
            if (key == Keys.Escape)
            {
                CurrentMode = Constants.Modes.NORMAL;
                return true;
            }
            switch (_mode)
            {
                case Constants.Modes.NORMAL:
                    switch (key)
                    {
                        case Keys.I:
                            CurrentMode = Constants.Modes.INSERT;
                            return true;
                        case Keys.V:
                            CurrentMode = Constants.Modes.VISUAL;
                            return true;
                        default:
                            return _modes.First(m => m.Mode == _mode).HandleKey(key);
                    }
            }
            return _modes.First(m => m.Mode == _mode).HandleKey(key);
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            _modeChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
