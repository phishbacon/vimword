using Microsoft.Office.Interop.Word;

namespace vimword.Vimulator.Motions
{
    /// <summary>
    /// Moves cursor/selection to the start of the line (column 0).
    /// Implements Vim's '0' motion.
    /// </summary>
    internal class LineStartMotion : IMotion
    {
        public MotionDirection Direction => MotionDirection.Backward;
        public bool IncludesTarget => false;

        public void Execute(Application app, bool extend = false)
        {
            var selection = app.Selection;
            
            // Move to the start of the current line
            selection.HomeKey(WdUnits.wdLine, extend ? WdMovementType.wdExtend : WdMovementType.wdMove);
        }
    }
}
