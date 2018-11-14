using System;
using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.Matcher
{
    public class NumberWithUnitTokenizer : SimpleTokenizer
    {
        /* The main difference between this strategy and SimpleTokenizer is for cases like
         * 'Bob's $ 100 cash'. 's' and '$' are independent tokens in SimpleTokenizer.
         * However, 's$' will return these two tokens too. Here, we let 's$' be a single
         * token to avoid the false positive.
         * Besides, letter and digits can't be mixed as a token. For cases like '200ml'.
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
                // Let 's$', 'us$' etc. as a token
                else if (c != '$' && (!char.IsLetterOrDigit(c) || IsCjk(c)))
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
                    if (inToken && i > 0)
                    {
                        // Split tokens like '200ml' to two separate tokens, i.e., '200' and 'ml'
                        char prevChar = chars[i - 1];
                        if (char.IsLetter(c) && char.IsDigit(prevChar))
                        {
                            tokens.Add(new Token(tokenStart, i - tokenStart, input.Substring(tokenStart, i - tokenStart)));
                            tokenStart = i;
                        }
                        
                        // For cases like '$100' or '$100' shouldn't be a token
                        if ((char.IsDigit(c) && prevChar == '$') || (c == '$' && char.IsDigit(prevChar)))
                        {
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
    }
}