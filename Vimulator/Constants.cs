using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vimword.Vimulator
{
    public static class Constants
    {
        public enum Modes
        {
            NORMAL,
            INSERT,
            VISUAL,
            VISUALLINE,
            COMMAND
        }

        public static string[] ModeText = { NormalMode, InsertMode, VisualMode, VisualLineMode, CommandMode };

        private const string NormalMode = "NORMAL";
        private const string InsertMode = "INSERT";
        private const string VisualMode = "VISUAL";
        private const string VisualLineMode = "VISUAL-LINE";
        private const string CommandMode = "COMMAND";
    }
}
