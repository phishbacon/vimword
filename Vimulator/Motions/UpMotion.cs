using Microsoft.Office.Interop.Word;

namespace vimword.Vimulator.Motions
{
    /// <summary>
    /// Moves cursor/selection up by one line.
    /// </summary>
    internal class UpMotion : IMotion
    {
        public void Execute(Application app, bool extend = false)
        {
            var moveType = extend ? WdMovementType.wdExtend : WdMovementType.wdMove;
            app.Selection.MoveUp(WdUnits.wdLine, 1, moveType);
        }
    }
}
