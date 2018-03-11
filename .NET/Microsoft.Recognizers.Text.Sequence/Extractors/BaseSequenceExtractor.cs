using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Sequence
{
    public abstract class BaseSequenceExtractor : IExtractor
    {
        internal abstract ImmutableDictionary<Regex, string> Regexes { get; }

        protected virtual string ExtractType { get; } = "";

        public virtual List<ExtractResult> Extract(string text)
        {
            var result = new List<ExtractResult>();

            if (string.IsNullOrEmpty(text))
            {
                return result;
            }

            var matchSource = new Dictionary<Match, string>();
            var matched = new bool[text.Length];

            //Traverse every match results to see each position in the text is matched or not.
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

            //Form the extracted results from all the matched intervals in the text.
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
