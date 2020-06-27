using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Portuguese;

namespace Microsoft.Recognizers.Text.Number.Portuguese
{
    public sealed class PercentageExtractor : BasePercentageExtractor
    {
        public PercentageExtractor(BaseNumberOptionsConfiguration config)
            : base(NumberExtractor.GetInstance(config))
        {
            Options = config.Options;
            Regexes = InitRegexes();
        }

        protected override NumberOptions Options { get; }

        protected override ImmutableHashSet<Regex> InitRegexes()
        {
            var regexStrs = new HashSet<string>
            {
                NumbersDefinitions.NumberWithSuffixPercentage,
            };

            var regexSet = BuildRegexes(regexStrs);

            return regexSet;
        }
    }
}