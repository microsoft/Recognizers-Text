using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.Matcher
{
    public class SimpleTokenizer : ITokenizer
    {
        public virtual List<Token> Tokenize(string input)
        {
            List<Token> tokens = new List<Token>();

            if (string.IsNullOrEmpty(input))
            {
                return tokens;
            }

            bool inToken = false;
            int tokenStart = 0;
            var chars = input.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                var c = chars[i];

                if (char.IsWhiteSpace(c))
                {
                    if (inToken)
                    {
                        tokens.Add(new Token(tokenStart, i - tokenStart, input.Substring(tokenStart, i - tokenStart)));
                        inToken = false;
                    }
                }
                else if (!char.IsLetterOrDigit(c) || IsCjk(c))
                {
                    if (inToken)
                    {
                        tokens.Add(new Token(tokenStart, i - tokenStart, input.Substring(tokenStart, i - tokenStart)));
                        inToken = false;
                    }

                    tokens.Add(new Token(i, 1, input.Substring(i, 1)));
                }
                else
                {
                    if (!inToken)
                    {
                        tokenStart = i;
                        inToken = true;
                    }
                }
            }

            if (inToken)
            {
                tokens.Add(new Token(tokenStart, chars.Length - tokenStart, input.Substring(tokenStart, chars.Length - tokenStart)));
            }

            return tokens;
        }

        // Check the character is Chinese by the unicode range (CJK Unified Ideographs, CJK Unified Ideographs Extension A)
        protected static bool IsChinese(char c)
        {
            ushort uc = (ushort)c;

            return (uc >= (ushort)0x4E00 && uc <= (ushort)0x9FBF) || (uc >= (ushort)0x3400 && uc <= (ushort)0x4DBF);
        }

        // Check the character is Japanese by the unicode range (Hiragana, Katakana, Katakana Pinyin)
        protected static bool IsJapanese(char c)
        {
            ushort uc = (ushort)c;

            return (uc >= 0x3040 && uc <= 0x309F) ||
                (uc >= 0x30A0 && uc <= (ushort)0x30FF) ||
                (uc >= (ushort)0xFF66 && uc <= (ushort)0xFF9D);
        }

        // Check the character is Korean by the unicode range (HangulSyllables, Hangul Jamo, Hangul Compatibility Jamo, Halfwidth Hangul Jamo)
        protected static bool IsKorean(char c)
        {
            ushort uc = (ushort)c;

            return (uc >= (ushort)0xAC00 && uc <= (ushort)0xD7AF) ||
                (uc >= (ushort)0x1100 && uc <= (ushort)0x11FF) ||
                (uc >= (ushort)0x3130 && uc <= (ushort)0x318F) ||
                (uc >= (ushort)0xFFB0 && uc <= (ushort)0xFFDC);
        }

        // Check the character is Chinese/Japanese/Korean.
        // For those languages which are not using whitespace delimited symbol, we only simply tokenize the sentence by each single character.
        protected static bool IsCjk(char c)
        {
            return IsChinese(c) || IsJapanese(c) || IsKorean(c);
        }
    }
}
