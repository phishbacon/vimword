using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vimword.Vimulator
{
    /// <summary>
    /// Global constants for the Vim emulator.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Vim modes that change how key presses are interpreted.
        /// </summary>
        public enum Modes
        {
            NORMAL,
            INSERT,
            VISUAL,
            VISUALLINE,
            COMMAND
        }

        /// <summary>
        /// Display text for each mode shown in status bar.
        /// </summary>
        public static string[] ModeText = { NormalMode, InsertMode, VisualMode, VisualLineMode, CommandMode };

        private const string NormalMode = "NORMAL";
        private const string InsertMode = "INSERT";
        private const string VisualMode = "VISUAL";
        private const string VisualLineMode = "VISUAL-LINE";
        private const string CommandMode = "COMMAND";
    }
}
