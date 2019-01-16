using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.German;

namespace Microsoft.Recognizers.Text.Number.German
{
    public sealed class PercentageExtractor : BasePercentageExtractor
    {
        public PercentageExtractor(NumberOptions options = NumberOptions.None)
            : base(NumberExtractor.GetInstance(options: options))
        {
            Options = options;
            Regexes = InitRegexes();
        }

        protected override NumberOptions Options { get; }

        protected override ImmutableHashSet<Regex> InitRegexes()
        {
            var regexStrs = new HashSet<string>
            {
                NumbersDefinitions.NumberWithSuffixPercentage,
                NumbersDefinitions.NumberWithPrefixPercentage,
            };

            return BuildRegexes(regexStrs);
        }
    }
}