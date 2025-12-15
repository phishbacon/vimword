using Microsoft.Office.Interop.Word;

namespace vimword.Vimulator.Motions
{
    /// <summary>
    /// Moves cursor/selection right by one character.
    /// Does not wrap to next line.
    /// </summary>
    internal class RightMotion : IMotion
    {
        public MotionDirection Direction => MotionDirection.Forward;
        public bool IncludesTarget => false;

        public void Execute(Application app, bool extend = false, int count = 1)
        {
            var selection = app.Selection;
            
            // Execute the motion count times
            for (int i = 0; i < count; i++)
            {
                // Get current line number
                int currentLine = (int)selection.Information[WdInformation.wdFirstCharacterLineNumber];
                
                // Save positions before moving
                int originalStart = selection.Start;
                int originalEnd = selection.End;
                
                // Try to move right
                var moveType = extend ? WdMovementType.wdExtend : WdMovementType.wdMove;
                selection.MoveRight(WdUnits.wdCharacter, 1, moveType);
                
                // Check if we're still on the same line
                int newLine = (int)selection.Information[WdInformation.wdFirstCharacterLineNumber];
                
                // If we moved to a different line, undo the move and stop
                if (newLine != currentLine)
                {
                    selection.Start = originalStart;
                    selection.End = originalEnd;
                    break;
                }
            }
        }
    }
}
