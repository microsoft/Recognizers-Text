﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.InternalCache;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class Token : ICloneableType<Token>
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

        public static List<Token> GetTokenFromRegex(Regex regex, string text)
        {
            var ret = new List<Token>();

            if (regex == null)
            {
                return ret;
            }

            var matches = regex.Matches(text);
            foreach (Match match in matches)
            {
                ret.Add(new Token(match.Index, match.Index + match.Length));
            }

            return ret;
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
                    bool shouldAdd = true;
                    for (var index = 0; index < mergedTokens.Count && shouldAdd; index++)
                    {
                        // It is included in one of the current tokens
                        if (token.Start >= mergedTokens[index].Start && token.End <= mergedTokens[index].End)
                        {
                            shouldAdd = false;
                        }

                        // If it contains overlaps
                        if (token.Start > mergedTokens[index].Start && token.Start < mergedTokens[index].End)
                        {
                            shouldAdd = false;
                        }

                        // It includes one of the tokens and should replace the included one
                        if (token.Start <= mergedTokens[index].Start && token.End >= mergedTokens[index].End)
                        {
                            shouldAdd = false;
                            mergedTokens[index] = token;
                        }
                    }

                    if (shouldAdd)
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
                    Metadata = token.Metadata,
                };

                ret.Add(er);
            }

            return ret;
        }

        public Token Clone()
        {
            return (Token)MemberwiseClone();
        }
    }
}