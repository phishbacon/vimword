using Microsoft.Office.Interop.Word;

namespace vimword.Vimulator.Motions
{
    /// <summary>
    /// Moves cursor/selection to the end of the line (last character before line break).
    /// Implements Vim's '$' motion.
    /// </summary>
    internal class LineEndMotion : IMotion
    {
        public MotionDirection Direction => MotionDirection.Forward;
        public bool IncludesTarget => true;  // Positions ON last character before line break

        public void Execute(Application app, bool extend = false)
        {
            var selection = app.Selection;
            
            // Move to the end of the current line
            selection.EndKey(WdUnits.wdLine, extend ? WdMovementType.wdExtend : WdMovementType.wdMove);
        }
    }
}
