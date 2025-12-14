using Microsoft.Office.Interop.Word;

namespace vimword.Vimulator.Motions
{
    /// <summary>
    /// Moves cursor/selection forward by one word or WORD.
    /// A word consists of alphanumeric characters and underscores, punctuation is separate.
    /// A WORD consists of any non-whitespace characters (includes punctuation).
    /// </summary>
    internal class WordForwardMotion : IMotion
    {
        private readonly bool _includePunctuation;

        public MotionDirection Direction => MotionDirection.Forward;
        public bool IncludesTarget => true;  // Positions ON first char of next word, needs +1 for Visual

        public WordForwardMotion(bool includePunctuation = false)
        {
            _includePunctuation = includePunctuation;
        }

        public void Execute(Application app, bool extend = false)
        {
            var selection = app.Selection;
            var doc = selection.Document;
            var docEnd = doc.Range().End;
            
            var pos = extend ? selection.End : selection.Start;

            if (pos >= docEnd - 1)
            {
                return;
            }

            if (_includePunctuation)
            {
                // WORD motion: punctuation is part of the word
                // Skip current non-whitespace sequence
                while (pos < docEnd && !TextCharacterHelper.IsWhitespace(doc, pos))
                {
                    pos++;
                }

                // Skip whitespace to start of next WORD
                while (pos < docEnd && TextCharacterHelper.IsWhitespace(doc, pos))
                {
                    pos++;
                }
            }
            else
            {
                // word motion: punctuation is separate
                bool startOnWord = TextCharacterHelper.IsWordChar(doc, pos);
                bool startOnPunct = TextCharacterHelper.IsPunctuation(doc, pos);

                if (startOnWord)
                {
                    while (pos < docEnd && TextCharacterHelper.IsWordChar(doc, pos))
                    {
                        pos++;
                    }
                }
                else if (startOnPunct)
                {
                    while (pos < docEnd && TextCharacterHelper.IsPunctuation(doc, pos))
                    {
                        pos++;
                    }
                }

                // Skip whitespace to start of next word/punctuation
                while (pos < docEnd && TextCharacterHelper.IsWhitespace(doc, pos))
                {
                    pos++;
                }
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
    }
}
