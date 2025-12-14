using Microsoft.Office.Interop.Word;

namespace vimword.Vimulator.Motions
{
    /// <summary>
    /// Moves cursor/selection to the first non-whitespace character of the line.
    /// Implements Vim's '_' motion.
    /// </summary>
    internal class FirstNonBlankMotion : IMotion
    {
        public MotionDirection Direction => MotionDirection.Neutral;
        public bool IncludesTarget => false;

        public void Execute(Application app, bool extend = false)
        {
            var selection = app.Selection;
            var doc = selection.Document;
            
            // Move to the start of the line
            selection.HomeKey(WdUnits.wdLine, WdMovementType.wdMove);
            
            int lineStart = selection.Start;
            int pos = lineStart;
            
            // Get document end to avoid going past it
            int docEnd = doc.Range().End;
            
            // Find the first non-whitespace character
            // Stop at line break or document end
            while (pos < docEnd)
            {
                var charText = doc.Range(pos, pos + 1).Text;
                
                // Check if we hit a line break
                if (charText == "\r" || charText == "\n")
                {
                    // Empty line - stay at start
                    pos = lineStart;
                    break;
                }
                
                // Check if whitespace
                if (!TextCharacterHelper.IsWhitespace(doc, pos))
                {
                    // Found first non-whitespace
                    break;
                }
                
                pos++;
            }
            
            // Move to that position
            selection.Start = pos;
            selection.End = pos;
        }
    }
}
