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
        public BaseURLExtractor()
        {
            var regexes = new Dictionary<Regex, string>
            {
                {
                    new Regex(BaseURL.IpUrlRegex, RegexOptions.Compiled),
                    Constants.URL_REGEX
                },
                {
                    new Regex(BaseURL.UrlRegex, RegexOptions.Compiled),
                    Constants.URL_REGEX
                },
                {
                    new Regex(BaseURL.UrlRegex2, RegexOptions.Compiled),
                    Constants.URL_REGEX
                },
            };

            Regexes = regexes.ToImmutableDictionary();
            AmbiguousTimeTerm = new Regex(BaseURL.AmbiguousTimeTerm, RegexOptions.Compiled);

            TldMatcher = new StringMatcher();
            TldMatcher.Init(BaseURL.TldList);
        }

        internal override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_URL;

        private StringMatcher TldMatcher { get; }

        private Regex AmbiguousTimeTerm { get; }

        public override bool IsValidMatch(Match match)
        {
            var isValidTld = false;
            var isIPUrl = match.Groups["IPurl"].Success;

            if (!isIPUrl)
            {
                var tldString = match.Groups["Tld"].Value;
                var tldMatches = TldMatcher.Find(tldString);

                if (tldMatches.Any(o => o.Start == 0 && o.End == tldString.Length))
                {
                    isValidTld = true;
                }
            }

            // For cases like "7.am" or "8.pm" which are more likely time terms.
            if (AmbiguousTimeTerm.IsMatch(match.Value))
            {
                return false;
            }

            return isValidTld || isIPUrl;
        }
    }
}
