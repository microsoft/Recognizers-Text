using System;
using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.Matcher
{
    public class NumberWithUnitTokenizer : SimpleTokenizer
    {
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
                if (!char.IsLetter(c) || IsCjk(c))
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
    }
}