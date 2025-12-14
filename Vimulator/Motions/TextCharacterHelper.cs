using Microsoft.Office.Interop.Word;

namespace vimword.Vimulator.Motions
{
    /// <summary>
    /// Shared utility methods for text character operations used by motion classes.
    /// </summary>
    internal static class TextCharacterHelper
    {
        /// <summary>
        /// Checks if the character at the given position is a word character.
        /// Word characters are alphanumeric characters and underscores.
        /// </summary>
        public static bool IsWordChar(Document doc, int pos)
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

            char c = text[0];
            return char.IsLetterOrDigit(c) || c == '_';
        }

        /// <summary>
        /// Checks if the character at the given position is whitespace.
        /// </summary>
        public static bool IsWhitespace(Document doc, int pos)
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

        /// <summary>
        /// Checks if the character at the given position is punctuation (non-whitespace, non-word).
        /// </summary>
        public static bool IsPunctuation(Document doc, int pos)
        {
            return !IsWhitespace(doc, pos) && !IsWordChar(doc, pos);
        }
    }
}
