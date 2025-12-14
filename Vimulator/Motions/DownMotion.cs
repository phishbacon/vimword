using Microsoft.Office.Interop.Word;

namespace vimword.Vimulator.Motions
{
    /// <summary>
    /// Moves cursor/selection down by one line.
    /// </summary>
    internal class DownMotion : IMotion
    {
        public void Execute(Application app, bool extend = false)
        {
            var moveType = extend ? WdMovementType.wdExtend : WdMovementType.wdMove;
            app.Selection.MoveDown(WdUnits.wdLine, 1, moveType);
        }
    }
}
