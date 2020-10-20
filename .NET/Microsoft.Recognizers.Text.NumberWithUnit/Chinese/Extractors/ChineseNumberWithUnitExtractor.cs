using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Chinese;
using Microsoft.Recognizers.Text.Matcher;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Chinese
{
    public class ChineseNumberWithUnitExtractor : NumberWithUnitExtractor
    {
        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;
        private static readonly Regex HalfUnitRegex = new Regex(NumbersWithUnitDefinitions.HalfUnitRegex, RegexFlags);
        private readonly INumberWithUnitExtractorConfiguration config;
        private readonly StringMatcher suffixMatcher = new StringMatcher(MatchStrategy.TrieTree, new NumberWithUnitTokenizer());
        private readonly StringMatcher prefixMatcher = new StringMatcher(MatchStrategy.TrieTree, new NumberWithUnitTokenizer());

        public ChineseNumberWithUnitExtractor(INumberWithUnitExtractorConfiguration config)
            : base(config)
        {
            this.config = config;

            if (this.config.SuffixList?.Count > 0)
            {
                suffixMatcher = BuildMatcherFromSet(this.config.SuffixList.Values.ToArray());
            }

            if (this.config.PrefixList?.Count > 0)
            {
                prefixMatcher = BuildMatcherFromSet(this.config.PrefixList.Values.ToArray());
            }
        }

        public override List<ExtractResult> Extract(string source)
        {
            List<ExtractResult> result = Extract_v1(source);
            IOrderedEnumerable<ExtractResult> numbers;
            var prefixMatches = prefixMatcher.Find(source).OrderBy(o => o.Start).ToList();
            var suffixMatches = suffixMatcher.Find(source).OrderBy(o => o.Start).ToList();

            // Remove matches with wrong length, e.g. both 'm2' and 'm 2' are extracted but only 'm2' represents a unit.
            for (int i = suffixMatches.Count - 1; i >= 0; i--)
            {
                var m = suffixMatches[i];
                if (m.CanonicalValues.All(l => l.Length != m.Length))
                {
                    suffixMatches.RemoveAt(i);
                }
            }

            if (prefixMatches.Count > 0 || suffixMatches.Count > 0)
            {
                numbers = this.config.UnitNumExtractor.Extract(source).OrderBy(o => o.Start);
            }
            else
            {
                numbers = null;
            }

            // Expand Chinese phrase to the `half` patterns when it follows closely origin phrase.
            if (HalfUnitRegex != null && numbers != null)
            {
                var match = new List<ExtractResult>();
                foreach (var number in numbers)
                {
                    if (HalfUnitRegex.Matches(number.Text).Count == 1)
                    {
                        match.Add(number);
                    }

                }

                if (match.Count > 0)
                {
                    var res = new List<ExtractResult>();
                    foreach (var er in result)
                    {
                        int start = (int)er.Start;
                        int length = (int)er.Length;
                        var match_suffix = new List<ExtractResult>();
                        foreach (var mr in match)
                        {
                            if (mr.Start == (start + length))
                            {
                                match_suffix.Add(mr);
                            }
                        }

                        if (match_suffix.Count == 1)
                        {
                            var mr = match_suffix[0];
                            er.Length += mr.Length;
                            er.Text += mr.Text;
                            var tmp = new List<ExtractResult>();
                            tmp.Add((ExtractResult)er.Data);
                            tmp.Add(mr);
                            er.Data = tmp;
                        }

                        res.Add(er);
                    }

                    result = res;
                }
            }

            return result;
        }

    }
}
