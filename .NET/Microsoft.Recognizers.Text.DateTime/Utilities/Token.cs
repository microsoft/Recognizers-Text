using System.Collections.Generic;
using System.Linq;

using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class Token
    {
        public Token(int s, int e)
        {
            Start = s;
            End = e;
        }

        public int Start { get; }
        public int End { get; }

        public int Length
        {
            get
            {
                if (End < Start)
                {
                    return 0;
                }
                return End - Start;
            }
        }

        public static List<ExtractResult> MergeAllTokens(List<Token> tokens, string text, string extractorName)
        {
            var ret = new List<ExtractResult>();

            tokens = tokens.OrderBy(s => s.Start).ThenByDescending(s => s.Length).ToList();
            var mergedTokens = new List<Token>();
            foreach (var token in tokens)
            {
                if (token != null)
                {
                    var bAdd = true;
                    for (var index = 0; index < mergedTokens.Count && bAdd; index++)
                    {
                        //is included in one of the current token
                        if (token.Start >= mergedTokens[index].Start && token.End <= mergedTokens[index].End)
                        {
                            bAdd = false;
                        }

                        //if it contains overlap
                        if (token.Start > mergedTokens[index].Start && token.Start < mergedTokens[index].End)
                        {
                            bAdd = false;
                        }

                        // include one of the token, should replace the included one
                        if (token.Start <= mergedTokens[index].Start && token.End >= mergedTokens[index].End)
                        {
                            bAdd = false;
                            mergedTokens[index] = token;
                        }
                    }

                    if (bAdd)
                    {
                        mergedTokens.Add(token);
                    }
                }
            }

            foreach (var token in mergedTokens)
            {
                var start = token.Start;
                var length = token.Length;
                var substr = text.Substring(start, length);

                var er = new ExtractResult
                {
                    Start = start,
                    Length = length,
                    Text = substr,
                    Type = extractorName,
                    Data = null
                };

                ret.Add(er);
            }

            return ret;
        }
    }
}