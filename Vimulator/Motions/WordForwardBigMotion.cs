using Microsoft.Office.Interop.Word;

namespace vimword.Vimulator.Motions
{
    /// <summary>
    /// Moves cursor/selection forward by one WORD (Shift+W).
    /// A WORD is any sequence of non-whitespace characters (includes punctuation).
    /// </summary>
    internal class WordForwardBigMotion : IMotion
    {
        public void Execute(Application app, bool extend = false)
        {
            var selection = app.Selection;
            var doc = selection.Document;
            var docEnd = doc.Range().End;
            
            // In Visual mode, start from end of selection
            var pos = extend ? selection.End : selection.Start;

            if (pos >= docEnd - 1)
            {
                return;
            }

            // Skip current WORD (non-whitespace)
            while (pos < docEnd && !IsWhitespace(doc, pos))
            {
                pos++;
            }

            // Skip whitespace to start of next WORD
            while (pos < docEnd && IsWhitespace(doc, pos))
            {
                pos++;
            }

            if (extend)
            {
                selection.End = pos;
            }
            else
            {
                selection.Start = pos;
                selection.End = pos;
            }
        }

        private bool IsWhitespace(Document doc, int pos)
        {
            if (pos >= doc.Range().End)
            {
                return false;
            }

            var text = doc.Range(pos, pos + 1).Text;
            if (string.IsNullOrEmpty(text) || text.Length == 0)
            {
                return false;
            }

            return char.IsWhiteSpace(text[0]);
        }
    }
}
