using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.French;

namespace Microsoft.Recognizers.Text.Number.French
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
            HashSet<string> regexStrs = new HashSet<string>
            {
                NumbersDefinitions.NumberWithSuffixPercentage,
                NumbersDefinitions.NumberWithPrefixPercentage,
            };

            return BuildRegexes(regexStrs);
        }
    }
}
