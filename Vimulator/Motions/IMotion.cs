using Microsoft.Office.Interop.Word;

namespace vimword.Vimulator.Motions
{
    /// <summary>
    /// Direction of a motion relative to cursor position.
    /// </summary>
    public enum MotionDirection
    {
        /// <summary>Motion moves backward (left/up) from cursor.</summary>
        Backward,
        /// <summary>Motion moves forward (right/down) from cursor.</summary>
        Forward,
        /// <summary>Motion direction is neutral or vertical.</summary>
        Neutral
    }

    /// <summary>
    /// Represents a motion that can be executed to move the cursor or extend selection.
    /// </summary>
    public interface IMotion
    {
        /// <summary>
        /// Gets the direction this motion moves relative to the cursor.
        /// Used by Visual mode to determine selection behavior.
        /// </summary>
        MotionDirection Direction { get; }

        /// <summary>
        /// Gets whether this motion positions cursor ON the target character.
        /// If true, Visual mode adds +1 to select through the character.
        /// If false, the motion already moves past the target.
        /// </summary>
        bool IncludesTarget { get; }

        /// <summary>
        /// Executes the motion.
        /// </summary>
        /// <param name="app">Word application instance</param>
        /// <param name="extend">Whether to extend selection (true for Visual mode, false for Normal mode)</param>
        void Execute(Application app, bool extend = false);
    }
}
