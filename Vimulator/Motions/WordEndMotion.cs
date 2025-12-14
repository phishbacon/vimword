using Microsoft.Office.Interop.Word;

namespace vimword.Vimulator.Motions
{
    /// <summary>
    /// Moves cursor/selection to the end of the word.
    /// Implements Vim's 'e' motion with character-by-character scanning.
    /// A word consists of alphanumeric characters, underscores, and punctuation (non-whitespace).
    /// </summary>
    internal class WordEndMotion : IMotion
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

            // Check if we're at the end of a word/punct (next char is different type or whitespace)
            bool atEndOfSequence = false;
            if (pos + 1 < docEnd)
            {
                bool currentIsWord = IsWordChar(doc, pos);
                bool currentIsPunct = !currentIsWord && !IsWhitespace(doc, pos);
                bool nextIsWhitespace = IsWhitespace(doc, pos + 1);
                bool nextIsDifferentType = false;
                
                if (!nextIsWhitespace)
                {
                    bool nextIsWord = IsWordChar(doc, pos + 1);
                    nextIsDifferentType = (currentIsWord && !nextIsWord) || (currentIsPunct && nextIsWord);
                }
                
                atEndOfSequence = nextIsWhitespace || nextIsDifferentType;
            }

            // If at end of sequence or on whitespace, move forward to find next word/punct
            if (atEndOfSequence || IsWhitespace(doc, pos))
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
                    bool nextIsWordChar = IsWordChar(doc, pos);

                    if (nextIsWordChar)
                    {
                        // Move through word characters
                        while (pos < docEnd && IsWordChar(doc, pos))
                        {
                            pos++;
                        }
                    }
                    else
                    {
                        // Move through punctuation
                        while (pos < docEnd && !IsWhitespace(doc, pos) && !IsWordChar(doc, pos))
                        {
                            pos++;
                        }
                    }

                    pos--;
                }
            }
            else
            {
                // We're in the middle of a word/punct, move to end of current sequence
                bool onWordChar = IsWordChar(doc, pos);

                if (onWordChar)
                {
                    // Move through word characters
                    while (pos < docEnd && IsWordChar(doc, pos))
                    {
                        pos++;
                    }
                }
                else
                {
                    // Move through punctuation
                    while (pos < docEnd && !IsWhitespace(doc, pos) && !IsWordChar(doc, pos))
                    {
                        pos++;
                    }
                }
                
                pos--;
            }

            // Ensure we don't go backwards from where we started
            int startPos = extend ? selection.End : selection.Start;
            if (pos < startPos)
            {
                pos = startPos;
            }

            // Apply the movement
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
