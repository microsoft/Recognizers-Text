using System.Collections.Generic;
using System.Linq;

using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class Token
    {
        public Token(int s, int e, Metadata metadata = null)
        {
            Start = s;
            End = e;
            Metadata = metadata;
        }

        public int Start { get; }
        public int End { get; }
        public Metadata Metadata { get; }


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
                        // It is included in one of the current tokens
                        if (token.Start >= mergedTokens[index].Start && token.End <= mergedTokens[index].End)
                        {
                            bAdd = false;
                        }

                        // If it contains overlaps
                        if (token.Start > mergedTokens[index].Start && token.Start < mergedTokens[index].End)
                        {
                            bAdd = false;
                        }

                        // It includes one of the tokens and should replace the included one
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
                    Data = null,
                    Metadata = token.Metadata
                };

                ret.Add(er);
            }

            return ret;
        }
    }
}