using Microsoft.Office.Interop.Word;

namespace vimword.Vimulator.Motions
{
    /// <summary>
    /// Moves cursor/selection forward by one word.
    /// A word consists of alphanumeric characters and underscores.
    /// Punctuation is treated as a separate word.
    /// </summary>
    internal class WordForwardMotion : IMotion
    {
        public void Execute(Application app, bool extend = false)
        {
            var selection = app.Selection;
            var doc = selection.Document;
            var docEnd = doc.Range().End;
            
            // In Visual mode (extend=true), start from the end of selection
            // In Normal mode (extend=false), start from the start of selection
            var pos = extend ? selection.End : selection.Start;

            if (pos >= docEnd - 1)
            {
                return;
            }

            // Determine what we're on
            bool startOnWord = IsWordChar(doc, pos);
            bool startOnPunct = !startOnWord && !IsWhitespace(doc, pos);

            // Skip current sequence
            if (startOnWord)
            {
                // Skip word characters
                while (pos < docEnd && IsWordChar(doc, pos))
                {
                    pos++;
                }
            }
            else if (startOnPunct)
            {
                // Skip punctuation
                while (pos < docEnd && !IsWhitespace(doc, pos) && !IsWordChar(doc, pos))
                {
                    pos++;
                }
            }

            // Skip whitespace to start of next word/punctuation
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

        private bool IsWordChar(Document doc, int pos)
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

            char c = text[0];
            return char.IsLetterOrDigit(c) || c == '_';
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
