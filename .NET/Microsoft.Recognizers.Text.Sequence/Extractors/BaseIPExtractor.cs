using Microsoft.Recognizers.Definitions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Microsoft.Recognizers.Text.Sequence
{
    class BaseIPExtractor : BaseSequenceExtractor
    {
        internal override ImmutableDictionary<Regex, string> Regexes { get; }
        protected sealed override string ExtractType { get; } = Constants.MODEL_IP;

        public BaseIPExtractor()
        {
            var regexes = new Dictionary<Regex, string>
            {
                {
                    new Regex(BaseIP.IPV4Regex), Constants.MODEL_IPV4
                },
                {
                    new Regex(BaseIP.IPV6Regex), Constants.MODEL_IPV6
                }
            };
            
            Regexes = regexes.ToImmutableDictionary();
        }

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
                        if (substr.StartsWith("::") &&
                            (start > 0 && char.IsLetterOrDigit(text[start - 1])))
                        {
                            continue;
                        }
                        else if (substr.EndsWith("::") && 
                            (i + 1 < text.Length && char.IsLetterOrDigit(text[i + 1])))
                        {
                            continue;
                        }

                        bool matchFunc(Match o) => o.Index == start && o.Length == length;

                        if (matchSource.Keys.Any(matchFunc))
                        {
                            var srcMatch = matchSource.Keys.First(matchFunc);
                            result.Add(new ExtractResult
                            {
                                Start = start,
                                Length = length,
                                Text = substr,
                                Type = ExtractType,
                                Data = matchSource.ContainsKey(srcMatch) ? matchSource[srcMatch] : null
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
