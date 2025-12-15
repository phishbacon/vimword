using Microsoft.Office.Interop.Word;

namespace vimword.Vimulator.Motions
{
    /// <summary>
    /// Moves cursor/selection back by one word or WORD.
    /// A word consists of alphanumeric characters and underscores, punctuation is separate.
    /// A WORD consists of any non-whitespace characters (includes punctuation).
    /// </summary>
    internal class WordBackMotion : IMotion
    {
        private readonly bool _includePunctuation;

        public MotionDirection Direction => MotionDirection.Backward;
        public bool IncludesTarget => false;

        public WordBackMotion(bool includePunctuation = false)
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
                
                if (extend)
                {
                    int originalEnd = selection.End;
                    var pos = selection.Start;

                    if (pos <= 0)
                    {
                        break; // Can't move further
                    }

                    pos--;

                    while (pos > 0 && TextCharacterHelper.IsWhitespace(doc, pos))
                    {
                        pos--;
                    }

                    if (_includePunctuation)
                    {
                        // WORD motion: skip all non-whitespace
                        while (pos > 0 && !TextCharacterHelper.IsWhitespace(doc, pos))
                        {
                            pos--;
                        }
                    }
                    else
                    {
                        // word motion: punctuation is separate
                        bool onWord = TextCharacterHelper.IsWordChar(doc, pos);

                        if (onWord)
                        {
                            while (pos > 0 && TextCharacterHelper.IsWordChar(doc, pos))
                            {
                                pos--;
                            }
                        }
                        else
                        {
                            while (pos > 0 && TextCharacterHelper.IsPunctuation(doc, pos))
                            {
                                pos--;
                            }
                        }
                    }

                    if (pos > 0 || (pos == 0 && TextCharacterHelper.IsWhitespace(doc, 0)))
                    {
                        pos++;
                    }

                    selection.SetRange(pos, originalEnd);
                }
                else
                {
                    var pos = selection.Start;

                    if (pos <= 0)
                    {
                        break;
                    }

                    pos--;

                    while (pos > 0 && TextCharacterHelper.IsWhitespace(doc, pos))
                    {
                        pos--;
                    }

                    if (_includePunctuation)
                    {
                        // WORD motion
                        while (pos > 0 && !TextCharacterHelper.IsWhitespace(doc, pos))
                        {
                            pos--;
                        }
                    }
                    else
                    {
                        // word motion
                        bool onWord = TextCharacterHelper.IsWordChar(doc, pos);

                        if (onWord)
                        {
                            while (pos > 0 && TextCharacterHelper.IsWordChar(doc, pos))
                            {
                                pos--;
                            }
                        }
                        else
                        {
                            while (pos > 0 && TextCharacterHelper.IsPunctuation(doc, pos))
                            {
                                pos--;
                            }
                        }
                    }

                    if (pos > 0 || (pos == 0 && TextCharacterHelper.IsWhitespace(doc, 0)))
                    {
                        pos++;
                    }

                    selection.SetRange(pos, pos);
                }
            }
        }
    }
}
