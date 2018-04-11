using Microsoft.Recognizers.Definitions;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Sequence
{
    public class BaseURLExtractor : BaseSequenceExtractor
    {
        internal override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_URL;

        public BaseURLExtractor()
        {
            var regexes = new Dictionary<Regex, string>
            {
                {
                    new Regex(BaseURL.URLRegex), Constants.URL_REGEX
                }
            };

            Regexes = regexes.ToImmutableDictionary();
        }
    }
}
