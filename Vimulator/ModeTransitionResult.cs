using System;

namespace vimword.Vimulator
{
    /// <summary>
    /// Represents the result of handling a key press in a VimMode.
    /// </summary>
    public class ModeTransitionResult
    {
        /// <summary>
        /// Whether the key was handled (true) or should be passed to Word (false).
        /// </summary>
        public bool Handled { get; set; }

        /// <summary>
        /// Optional mode to transition to. If null, stay in current mode.
        /// </summary>
        public Constants.Modes? NextMode { get; set; }

        /// <summary>
        /// Optional action to execute after mode transition (e.g., move cursor after 'a').
        /// </summary>
        public Action PostTransitionAction { get; set; }
    }
}
