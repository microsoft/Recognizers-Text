using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.Matcher
{
    public class NumberWithUnitTokenizer : SimpleTokenizer
    {
        private static HashSet<char> specialTokenCharacters = new HashSet<char> { '$' };

        /* The main difference between this strategy and SimpleTokenizer is for cases like
         * 'Bob's $ 100 cash'. 's' and '$' are independent tokens in SimpleTokenizer.
         * However, 's$' will return these two tokens too. Here, we let 's$' be a single
         * token to avoid the false positive.
         * Besides, letters and digits can't be mixed as a token. For cases like '200ml'.
         * '200ml' will be a token in SimpleTokenizer. Here, '200' and 'ml' are independent tokens.
         */
        public override List<Token> Tokenize(string input)
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
                else if ((!specialTokenCharacters.Contains(c) && !char.IsLetterOrDigit(c)) || IsChinese(c) || IsJapanese(c))
                {
                    // Non-splittable currency units (as "$") are treated as regular letters. For instance, 'us$' should be a single token
                    if (inToken)
                    {
                        tokens.Add(new Token(tokenStart, i - tokenStart, input.Substring(tokenStart, i - tokenStart)));
                        inToken = false;
                    }

                    tokens.Add(new Token(i, 1, input.Substring(i, 1)));
                }
                else
                {
                    if (inToken && i > 0)
                    {
                        char preChar = chars[i - 1];
                        if (IsSplittableUnit(c, preChar))
                        {
                            // Split if letters or non-splittable units are adjacent with digits.
                            tokens.Add(new Token(tokenStart, i - tokenStart, input.Substring(tokenStart, i - tokenStart)));
                            tokenStart = i;
                        }
                    }

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

        private bool IsSplittableUnit(char curChar, char preChar)
        {
            // To handle cases like '200ml', digits and letters cannot be mixed as a single token. '200ml' will be tokenized to '200' and 'ml'.
            if ((char.IsLetter(curChar) && char.IsDigit(preChar)) || (char.IsDigit(curChar) && char.IsLetter(preChar)))
            {
                return true;
            }

            // Non-splittable currency units can't be mixed with digits. For example, '$100' or '100$' will be tokenized to '$' and '100', '1$50' will be tokenized to '1', '$', and '50'
            if ((char.IsDigit(curChar) && specialTokenCharacters.Contains(preChar)) || (specialTokenCharacters.Contains(curChar) && char.IsDigit(preChar)))
            {
                return true;
            }

            // Non-splittable currency units adjacent with letters are treated as regular token characters. For instance, 's$' or 'u$d' are single tokens.
            return false;
        }
    }
}