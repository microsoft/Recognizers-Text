using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions;

namespace Microsoft.Recognizers.Text.Sequence
{
    public class BaseHashtagExtractor : BaseSequenceExtractor
    {
        public BaseHashtagExtractor()
        {
            var regexes = new Dictionary<Regex, string>
            {
                {
                    new Regex(BaseHashtag.HashtagRegex),
                    Constants.HASHTAG_REGEX
                },
            };

            Regexes = regexes.ToImmutableDictionary();
        }

        internal override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_HASHTAG;
    }
}
