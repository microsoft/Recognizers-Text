namespace Microsoft.Recognizers.Text.Choice.Utilities
{
    using System.Collections.Generic;

    public static class UnicodeUtils
    {
        public static bool IsEmoji(string letter)
        {
            const int WhereEmojiLive = 0xFFFF; // Supplementary Unicode Plane. This is where emoji live
            return char.IsHighSurrogate(letter[0]) && char.ConvertToUtf32(letter, 0) > WhereEmojiLive;
        }

        public static IEnumerable<string> Letters(string text)
        {
            char? codePoint = null;
            foreach (char c in text)
            {
                if (codePoint != null)
                {
                    yield return new string(new[] { codePoint.Value, c });
                    codePoint = null;
                }
                else if (!char.IsHighSurrogate(c))
                {
                    yield return c.ToString();
                }
                else
                {
                    codePoint = c;
                }
            }
        }
    }
}
