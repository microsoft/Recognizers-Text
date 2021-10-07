﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.Matcher;

namespace Microsoft.Recognizers.Text.Sequence
{
    public class BaseIpExtractor : BaseSequenceExtractor
    {
        private IpConfiguration config;

        // The Ipv6 address regexes is written following the Recommendation: https://tools.ietf.org/html/rfc5952
        public BaseIpExtractor(IpConfiguration config)
        {
            this.config = config;

            var regexes = new Dictionary<Regex, string>
            {
                {
                    config.Ipv4Regex,
                    Constants.IP_REGEX_IPV4
                },
                {
                    config.Ipv6Regex,
                    Constants.IP_REGEX_IPV6
                },
            };

            Regexes = regexes.ToImmutableDictionary();
        }

        internal override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_IP;

        public override List<ExtractResult> Extract(string text)
        {
            var result = new List<ExtractResult>();

            if (string.IsNullOrEmpty(text))
            {
                return result;
            }

            var matchSource = new Dictionary<Match, string>();
            var matched = new bool[text.Length];

            var collections = Regexes.ToDictionary(o => o.Key.Matches(text), p => p.Value);
            foreach (var collection in collections)
            {
                foreach (Match m in collection.Key)
                {
                    for (var j = 0; j < m.Length; j++)
                    {
                        matched[m.Index + j] = true;
                    }

                    // Keep Source Data for extra information
                    matchSource.Add(m, collection.Value);
                }
            }

            var lastNotMatched = -1;
            for (var i = 0; i < text.Length; i++)
            {
                if (matched[i])
                {
                    if (i + 1 == text.Length || !matched[i + 1])
                    {
                        var start = lastNotMatched + 1;
                        var length = i - lastNotMatched;
                        var substr = text.Substring(start, length);
                        if (substr.StartsWith(Constants.IPV6_ELLIPSIS, StringComparison.Ordinal) &&
                            (start > 0 && char.IsLetterOrDigit(text[start - 1]) && !SimpleTokenizer.IsCjk(text[start - 1])))
                        {
                            continue;
                        }

                        if (substr.EndsWith(Constants.IPV6_ELLIPSIS, StringComparison.Ordinal) &&
                            (i + 1 < text.Length && char.IsLetterOrDigit(text[i + 1]) && !SimpleTokenizer.IsCjk(text[start + 1])))
                        {
                            continue;
                        }

                        bool MatchFunc(Match o) => o.Index == start && o.Length == length;

                        if (matchSource.Keys.Any(MatchFunc))
                        {
                            var srcMatch = matchSource.Keys.First(MatchFunc);
                            result.Add(new ExtractResult
                            {
                                Start = start,
                                Length = length,
                                Text = substr,
                                Type = ExtractType,
                                Data = matchSource.ContainsKey(srcMatch) ? matchSource[srcMatch] : null,
                            });
                        }
                    }
                }
                else
                {
                    lastNotMatched = i;
                }
            }

            return result;
        }
    }
}
