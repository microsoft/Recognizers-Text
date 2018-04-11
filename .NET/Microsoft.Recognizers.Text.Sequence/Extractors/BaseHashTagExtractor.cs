using Microsoft.Recognizers.Definitions;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Sequence
{
    public class BaseHashtagExtractor : BaseSequenceExtractor
    {
        internal override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_HASHTAG;

        public BaseHashtagExtractor()
        {
            var regexes = new Dictionary<Regex, string>
            {
                {
                    new Regex(BaseHashtag.HashtagRegex), Constants.HASHTAG_REGEX
                }
            };

            Regexes = regexes.ToImmutableDictionary();
        }
    }
}
