using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace vimword.Vimulator.Modes
{
    /// <summary>
    /// Insert Mode - allows normal text entry like a regular text editor.
    /// Most keys are passed through to Word for normal typing.
    /// </summary>
    internal class InsertMode : IVimMode
    {
        private readonly Microsoft.Office.Interop.Word.Application _app;
        private IModeContext _context;

        public InsertMode(Microsoft.Office.Interop.Word.Application app)
        {
            _app = app;
        }

        public Constants.Modes Mode => Constants.Modes.INSERT;

        public void OnEnter(IModeContext context)
        {
            _context = context;
        }

        public void OnExit()
        {
        }

        public ModeTransitionResult HandleKey(Keys key)
        {
            return new ModeTransitionResult { Handled = false };
        }
    }
}
