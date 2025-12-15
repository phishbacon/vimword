using Microsoft.Office.Interop.Word;

namespace vimword.Vimulator.Motions
{
    /// <summary>
    /// Moves cursor/selection to the end of a word or WORD.
    /// A word consists of alphanumeric characters and underscores, punctuation is separate.
    /// A WORD consists of any non-whitespace characters (includes punctuation).
    /// </summary>
    internal class WordEndMotion : IMotion
    {
        private readonly bool _includePunctuation;

        public MotionDirection Direction => MotionDirection.Forward;
        public bool IncludesTarget => true;  // Positions ON last character, needs +1 for Visual mode

        public WordEndMotion(bool includePunctuation = false)
        {
            _includePunctuation = includePunctuation;
        }

        public void Execute(Application app, bool extend = false, int count = 1)
        {
            // Execute the motion count times
            for (int i = 0; i < count; i++)
            {
                var selection = app.Selection;
                var doc = selection.Document;
                var docEnd = doc.Range().End;
                
                var pos = extend ? selection.End : selection.Start;

                if (pos >= docEnd - 1)
                {
                    break; // Can't move further
                }

                if (_includePunctuation)
                {
                    // WORD motion: simpler logic, just non-whitespace
                    bool atEndOfWord = false;
                    if (pos + 1 < docEnd)
                    {
                        atEndOfWord = !TextCharacterHelper.IsWhitespace(doc, pos) && TextCharacterHelper.IsWhitespace(doc, pos + 1);
                    }

                    if (atEndOfWord || TextCharacterHelper.IsWhitespace(doc, pos))
                    {
                        pos++;

                        while (pos < docEnd && TextCharacterHelper.IsWhitespace(doc, pos))
                        {
                            pos++;
                        }

                        if (pos >= docEnd)
                        {
                            pos = docEnd - 1;
                        }
                        else
                        {
                            while (pos < docEnd && !TextCharacterHelper.IsWhitespace(doc, pos))
                            {
                                pos++;
                            }
                            pos--;
                        }
                    }
                    else
                    {
                        while (pos < docEnd && !TextCharacterHelper.IsWhitespace(doc, pos))
                        {
                            pos++;
                        }
                        pos--;
                    }
                }
                else
                {
                    // word motion: punctuation is separate
                    bool onWordChar = TextCharacterHelper.IsWordChar(doc, pos);
                    bool onPunct = TextCharacterHelper.IsPunctuation(doc, pos);
                    bool onWhitespace = TextCharacterHelper.IsWhitespace(doc, pos);

                    if (onWhitespace)
                    {
                        // On whitespace - skip to next word and find its end
                        pos++;
                        while (pos < docEnd && TextCharacterHelper.IsWhitespace(doc, pos))
                        {
                            pos++;
                        }

                        if (pos >= docEnd)
                        {
                            pos = docEnd - 1;
                        }
                        else
                        {
                            bool nextIsWordChar = TextCharacterHelper.IsWordChar(doc, pos);
                            if (nextIsWordChar)
                            {
                                while (pos < docEnd && TextCharacterHelper.IsWordChar(doc, pos))
                                {
                                    pos++;
                                }
                            }
                            else
                            {
                                while (pos < docEnd && TextCharacterHelper.IsPunctuation(doc, pos))
                                {
                                    pos++;
                                }
                            }
                            pos--;
                        }
                    }
                    else if (onWordChar)
                    {
                        // On word char - check if we're at the last char of this word
                        bool atLastChar = false;
                        if (pos + 1 < docEnd)
                        {
                            atLastChar = !TextCharacterHelper.IsWordChar(doc, pos + 1);
                        }
                        else
                        {
                            atLastChar = true; // At document end
                        }

                        if (atLastChar)
                        {
                            // Already at end of current word, move to end of next word
                            pos++;
                            while (pos < docEnd && TextCharacterHelper.IsWhitespace(doc, pos))
                            {
                                pos++;
                            }

                            if (pos >= docEnd)
                            {
                                pos = docEnd - 1;
                            }
                            else
                            {
                                bool nextIsWordChar = TextCharacterHelper.IsWordChar(doc, pos);
                                if (nextIsWordChar)
                                {
                                    while (pos < docEnd && TextCharacterHelper.IsWordChar(doc, pos))
                                    {
                                        pos++;
                                    }
                                }
                                else
                                {
                                    while (pos < docEnd && TextCharacterHelper.IsPunctuation(doc, pos))
                                    {
                                        pos++;
                                    }
                                }
                                pos--;
                            }
                        }
                        else
                        {
                            // Not at end yet - move to end of current word
                            while (pos < docEnd && TextCharacterHelper.IsWordChar(doc, pos))
                            {
                                pos++;
                            }
                            pos--;
                        }
                    }
                    else if (onPunct)
                    {
                        // On punctuation - check if we're at the last char of this punct sequence
                        bool atLastChar = false;
                        if (pos + 1 < docEnd)
                        {
                            atLastChar = !TextCharacterHelper.IsPunctuation(doc, pos + 1);
                        }
                        else
                        {
                            atLastChar = true;
                        }

                        if (atLastChar)
                        {
                            // Already at end, move to end of next word/punct
                            pos++;
                            while (pos < docEnd && TextCharacterHelper.IsWhitespace(doc, pos))
                            {
                                pos++;
                            }

                            if (pos >= docEnd)
                            {
                                pos = docEnd - 1;
                            }
                            else
                            {
                                bool nextIsWordChar = TextCharacterHelper.IsWordChar(doc, pos);
                                if (nextIsWordChar)
                                {
                                    while (pos < docEnd && TextCharacterHelper.IsWordChar(doc, pos))
                                    {
                                        pos++;
                                    }
                                }
                                else
                                {
                                    while (pos < docEnd && TextCharacterHelper.IsPunctuation(doc, pos))
                                    {
                                        pos++;
                                    }
                                }
                                pos--;
                            }
                        }
                        else
                        {
                            // Not at end yet - move to end of current punct sequence
                            while (pos < docEnd && TextCharacterHelper.IsPunctuation(doc, pos))
                            {
                                pos++;
                            }
                            pos--;
                        }
                    }
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
        }
    }
}
