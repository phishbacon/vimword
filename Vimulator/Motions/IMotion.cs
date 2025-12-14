using Microsoft.Office.Interop.Word;

namespace vimword.Vimulator.Motions
{
    /// <summary>
    /// Represents a motion that can be executed to move the cursor or extend selection.
    /// </summary>
    public interface IMotion
    {
        /// <summary>
        /// Executes the motion.
        /// </summary>
        /// <param name="app">Word application instance</param>
        /// <param name="extend">Whether to extend selection (true for Visual mode, false for Normal mode)</param>
        void Execute(Application app, bool extend = false);
    }
}
