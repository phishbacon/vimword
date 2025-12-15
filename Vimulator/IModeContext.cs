using Microsoft.Office.Interop.Word;
using System;

namespace vimword.Vimulator
{
    /// <summary>
    /// Provides context and services to VimMode implementations.
    /// Allows modes to communicate with VimMachine and access Word application.
    /// </summary>
    public interface IModeContext
    {
        /// <summary>
        /// Gets the Word Application instance for document manipulation.
        /// </summary>
        Application Application { get; }

        /// <summary>
        /// Requests a mode change with optional post-transition action.
        /// </summary>
        void RequestModeChange(Constants.Modes mode, Action postTransition = null);

        /// <summary>
        /// Gets or sets the current key buffer (e.g., "5w" for count + motion).
        /// Used to display pending keys in the status bar.
        /// </summary>
        string KeyBuffer { get; set; }
    }
}
