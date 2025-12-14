using System.Windows.Forms;

namespace vimword.Vimulator.Modes
{
    /// <summary>
    /// Interface for all Vim modes. Each mode handles keys differently and owns its behavior.
    /// </summary>
    public interface IVimMode
    {
        /// <summary>
        /// Gets the mode type this implementation represents.
        /// </summary>
        Constants.Modes Mode { get; }

        /// <summary>
        /// Called when entering this mode.
        /// </summary>
        void OnEnter(IModeContext context);

        /// <summary>
        /// Called when exiting this mode.
        /// </summary>
        void OnExit();

        /// <summary>
        /// Handles a key press in this mode.
        /// </summary>
        ModeTransitionResult HandleKey(Keys key);
    }
}
