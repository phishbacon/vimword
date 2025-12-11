using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace vimword.Vimulator.Modes
{
    internal class InsertMode : IVimMode
    {
        public Constants.Modes Mode
        {
            get
            {
                return Constants.Modes.INSERT;
            }
        }
        public bool HandleKey(Keys key)
        {
            return false;
        }
    }
}
