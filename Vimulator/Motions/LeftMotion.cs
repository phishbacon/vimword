using Microsoft.Office.Interop.Word;

namespace vimword.Vimulator.Motions
{
    /// <summary>
    /// Moves cursor/selection left by one character.
    /// Does not wrap to previous line.
    /// </summary>
    internal class LeftMotion : IMotion
    {
        public void Execute(Application app, bool extend = false)
        {
            var selection = app.Selection;
            
            // Get current line number
            int currentLine = (int)selection.Information[WdInformation.wdFirstCharacterLineNumber];
            
            // Save positions before moving
            int originalStart = selection.Start;
            int originalEnd = selection.End;
            
            // Try to move left
            var moveType = extend ? WdMovementType.wdExtend : WdMovementType.wdMove;
            selection.MoveLeft(WdUnits.wdCharacter, 1, moveType);
            
            // Check if we're still on the same line
            int newLine = (int)selection.Information[WdInformation.wdFirstCharacterLineNumber];
            
            // If we moved to a different line, undo the move
            if (newLine != currentLine)
            {
                selection.Start = originalStart;
                selection.End = originalEnd;
            }
        }
    }
}
