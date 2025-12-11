using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace vimword.Vimulator.Modes
{
    internal class VisualMode : IVimMode
    {
        public Constants.Modes Mode
        {
            get
            {
                return Constants.Modes.VISUAL;
            }
        }
        public bool HandleKey(Keys key)
        {
            return false;
        }
    }
}
