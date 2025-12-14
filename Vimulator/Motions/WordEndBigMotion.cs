using Microsoft.Office.Interop.Word;

namespace vimword.Vimulator.Motions
{
    /// <summary>
    /// Moves cursor/selection to the end of a WORD (Shift+E).
    /// A WORD is any sequence of non-whitespace characters (includes punctuation).
    /// </summary>
    internal class WordEndBigMotion : IMotion
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

            // Check if we're at the end of a WORD (next char is whitespace)
            bool atEndOfWord = false;
            if (pos + 1 < docEnd)
            {
                atEndOfWord = !IsWhitespace(doc, pos) && IsWhitespace(doc, pos + 1);
            }

            // If at end of WORD or on whitespace, move to next WORD
            if (atEndOfWord || IsWhitespace(doc, pos))
            {
                pos++;

                // Skip whitespace
                while (pos < docEnd && IsWhitespace(doc, pos))
                {
                    pos++;
                }

                if (pos >= docEnd)
                {
                    pos = docEnd - 1;
                }
                else
                {
                    // Move through the WORD
                    while (pos < docEnd && !IsWhitespace(doc, pos))
                    {
                        pos++;
                    }
                    pos--;
                }
            }
            else
            {
                // We're in middle of WORD, move to end of current WORD
                while (pos < docEnd && !IsWhitespace(doc, pos))
                {
                    pos++;
                }
                pos--;
            }

            int startPos = extend ? selection.End : selection.Start;
            if (pos < startPos)
            {
                pos = startPos;
            }

            if (extend)
            {
                selection.End = pos + 1;
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
