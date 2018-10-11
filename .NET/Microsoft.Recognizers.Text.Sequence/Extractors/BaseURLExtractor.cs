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

        private StringMatcher TldMatcher { get; }

        public BaseURLExtractor()
        {
            var regexes = new Dictionary<Regex, string>
            {
                {
                    new Regex(BaseURL.IpUrlRegex), Constants.URL_REGEX
                },
                {
                    new Regex(BaseURL.UrlRegex), Constants.URL_REGEX
                },
                {
                    new Regex(BaseURL.UrlRegex2), Constants.URL_REGEX
                }
            };

            Regexes = regexes.ToImmutableDictionary();

            TldMatcher = new StringMatcher();
            TldMatcher.Init(BaseURL.TldList);
        }

        public override bool IsValidMatch(Match match)
        {
            var isValidTld = false;
            var isIPUrl = match.Groups["IPurl"].Success;
            
            var tldString = match.Groups["Tld"].Value;
            var tldMatches = TldMatcher.Find(tldString);

            if (tldMatches.Any(o => o.Start == 0 && o.End == tldString.Length))
            {
                isValidTld = true;
            }

            return isValidTld || isIPUrl;
        }
    }
}
