using Microsoft.Office.Interop.Word;

namespace vimword.Vimulator.Motions
{
    /// <summary>
    /// Moves cursor/selection up by one line.
    /// </summary>
    internal class UpMotion : IMotion
    {
        public MotionDirection Direction => MotionDirection.Neutral;
        public bool IncludesTarget => false;

        public void Execute(Application app, bool extend = false, int count = 1)
        {
            var moveType = extend ? WdMovementType.wdExtend : WdMovementType.wdMove;
            app.Selection.MoveUp(WdUnits.wdLine, count, moveType);
        }
    }
}
