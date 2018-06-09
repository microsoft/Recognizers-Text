using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Text.Matcher;

namespace Microsoft.Recognizers.Text.Sequence
{
    public class BaseURLExtractor : BaseSequenceExtractor
    {
        internal override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_URL;

        private static Regex UrlRegex { get; } =
            new Regex(BaseURL.UrlRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

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

            TldMatcher = new StringMatcher();
            TldMatcher.Init(BaseURL.TldList);
        }

        public override List<ExtractResult> Extract(string text)
        {
            var ret = base.Extract(text);
            var urlMatches = UrlRegex.Matches(text);

            foreach (Match urlMatch in urlMatches)
            {
                var tldString = urlMatch.Groups["Tld"].Value;
                var tldMatches = TldMatcher.Find(tldString);

                if (tldMatches.Any(o => o.Start == 0 && o.End == tldString.Length))
                {
                    ret.Add(new ExtractResult
                    {
                        Start = urlMatch.Index,
                        Length = urlMatch.Length,
                        Text = urlMatch.Value,
                        Type = ExtractType,
                        Data = Constants.URL_REGEX
                    });
                }
            }

            return ret;
        }
    }
}
