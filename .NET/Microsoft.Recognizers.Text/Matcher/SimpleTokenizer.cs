using System;
using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.Matcher
{
    public class SimpleTokenizer : ITokenizer
    {
        public List<Token> Tokenize(string input)
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
        private bool IsChinese(char c)
        {
            UInt16 uc = (UInt16) c;

            return (uc >= (UInt16)0x4E00 && uc <= (UInt16)0x9FBF) || (uc >= (UInt16)0x3400 && uc <= (UInt16)0x4DBF);
        }

        // Check the character is Japanese by the unicode range (Hiragana, Katakana, Katakana Pinyin)
        private bool IsJapanese(char c)
        {
            UInt16 uc = (UInt16) c;

            return ((uc >= (UInt16)0x3040 && uc <= (UInt16)0x309F) || 
                (uc >= (UInt16)0x30A0 && uc <= (UInt16)0x30FF) || 
                (uc >= (UInt16)0xFF66 && uc <= (UInt16)0xFF9D));
        }

        // Check the character is Korean by the unicode range (HangulSyllables, Hangul Jamo, Hangul Compatibility Jamo, Halfwidth Hangul Jamo)
        private bool IsKorean(char c)
        {
            UInt16 uc = (UInt16) c;

            return ((uc >= (UInt16)0xAC00 && uc <= (UInt16)0xD7AF) || 
                (uc >= (UInt16)0x1100 && uc <= (UInt16)0x11FF) || 
                (uc >= (UInt16)0x3130 && uc <= (UInt16)0x318F) ||
                (uc >= (UInt16)0xFFB0 && uc <= (UInt16)0xFFDC));
        }

        // Check the character is Chinese/Japanese/Korean.
        // For those languages which are not using whitespace delimited symbol, we only simply tokenize the sentence by each single character.
        private bool IsCjk(char c)
        {
            return IsChinese(c) || IsJapanese(c) || IsKorean(c);
        }
    }
}
