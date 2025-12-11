using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using vimword.Vimulator.Modes;

namespace vimword.Vimulator
{
    public interface IVimMachine
    {
        Constants.Modes CurrentMode { get; set; }
        bool HandleKey(Keys key);
        void OnPropertyChanged([CallerMemberName] string propertyName = "");
        PropertyChangedEventHandler ModeChanged { get; set; }
    }
}
