using Microsoft.Office.Interop.Word;

namespace vimword.Vimulator.Motions
{
    /// <summary>
    /// Moves cursor/selection back by one WORD (Shift+B).
    /// A WORD is any sequence of non-whitespace characters (includes punctuation).
    /// In Visual mode, shrinks forward selections or extends backward selections.
    /// </summary>
    internal class WordBackBigMotion : IMotion
    {
        public void Execute(Application app, bool extend = false)
        {
            var selection = app.Selection;
            var doc = selection.Document;

            if (extend)
            {
                // Detect if we should shrink or extend
                bool shouldShrink = (selection.End - selection.Start) > 1;
                
                if (shouldShrink)
                {
                    // Shrink by moving End backward
                    var pos = selection.End;

                    if (pos <= selection.Start + 1)
                    {
                        // Switch to extending
                        shouldShrink = false;
                    }
                    else
                    {
                        pos--;

                        while (pos > selection.Start && IsWhitespace(doc, pos))
                        {
                            pos--;
                        }

                        while (pos > selection.Start && !IsWhitespace(doc, pos))
                        {
                            pos--;
                        }

                        if (pos > selection.Start)
                        {
                            pos++;
                        }
                        else
                        {
                            pos = selection.Start + 1;
                        }

                        selection.End = pos;
                        return;
                    }
                }
                
                // Extend by moving Start backward - MUST preserve End
                int originalEnd = selection.End;  // Save the end position
                var extendPos = selection.Start;

                if (extendPos <= 0)
                {
                    return;
                }

                extendPos--;

                while (extendPos > 0 && IsWhitespace(doc, extendPos))
                {
                    extendPos--;
                }

                while (extendPos > 0 && !IsWhitespace(doc, extendPos))
                {
                    extendPos--;
                }

                if (extendPos > 0 || IsWhitespace(doc, 0))
                {
                    extendPos++;
                }

                // Set both Start and End explicitly to maintain selection
                selection.Start = extendPos;
                selection.End = originalEnd;  // Restore the end position
            }
            else
            {
                // Normal mode
                var pos = selection.Start;

                if (pos <= 0)
                {
                    return;
                }

                pos--;

                while (pos > 0 && IsWhitespace(doc, pos))
                {
                    pos--;
                }

                while (pos > 0 && !IsWhitespace(doc, pos))
                {
                    pos--;
                }

                if (pos > 0 || IsWhitespace(doc, 0))
                {
                    pos++;
                }

                selection.Start = pos;
                selection.End = pos;
            }
        }

        private bool IsWhitespace(Document doc, int pos)
        {
            if (pos < 0 || pos >= doc.Range().End)
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
