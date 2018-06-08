using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Text.Matcher;

namespace Microsoft.Recognizers.Text.Sequence
{
    public class BaseURLExtractor : BaseSequenceExtractor
    {
        internal override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_URL;

        private static Regex UrlPrefixRegex { get; } =
            new Regex(BaseURL.UrlPrefixRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.RightToLeft);

        private static Regex UrlSuffixRegex { get; } =
            new Regex(BaseURL.UrlSuffixRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private StringMatcher TldMatcher { get; }

        public BaseURLExtractor()
        {
            var regexes = new Dictionary<Regex, string>
            {
                {
                    new Regex(BaseURL.IpUrlRegex), Constants.URL_REGEX
                }
            };

            Regexes = regexes.ToImmutableDictionary();

            TldMatcher = new StringMatcher(BaseURL.TldList);
            TldMatcher.Build();
        }

        public override List<ExtractResult> Extract(string text)
        {
            var ret = base.Extract(text);
            var ers = new List<ExtractResult>();

            var tldMatches = TldMatcher.Find(text);
            var matched = new bool[text.Length];

            // Process all matches from right to left to avoid overlapping.
            tldMatches.Reverse();

            foreach (var tld in tldMatches)
            {
                // Skip segment that has been processed before.
                if (matched[tld.End - 1])
                {
                    continue;
                }

                var prefixString = text.Substring(0, tld.Start);
                var match = UrlPrefixRegex.Match(prefixString);

                if (match.Success && match.Index + match.Length == tld.Start)
                {
                    var erStart = match.Index;
                    var prefixLength = match.Length;
                    var suffixString = text.Substring(tld.End);

                    match = UrlSuffixRegex.Match(suffixString);

                    if (match.Success && match.Index == 0)
                    {
                        var erLength = prefixLength + tld.Length + match.Length;
                        ers.Add(new ExtractResult
                        {
                            Start = erStart,
                            Length = erLength,
                            Text = text.Substring(erStart, erLength),
                            Type = ExtractType,
                            Data = Constants.URL_REGEX
                        });

                        for (var i = 0; i < erLength; i++)
                        {
                            matched[i + erStart] = true;
                        }
                    }
                }
            }

            ers.Reverse();
            ret.AddRange(ers);

            return ret;
        }
    }
}
