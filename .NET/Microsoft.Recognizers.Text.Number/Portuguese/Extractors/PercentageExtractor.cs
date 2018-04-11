using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Portuguese;

namespace Microsoft.Recognizers.Text.Number.Portuguese
{
    public sealed class PercentageExtractor : BasePercentageExtractor
    {
        protected override NumberOptions Options { get; }

        public PercentageExtractor(NumberOptions options = NumberOptions.None) : base(
            new NumberExtractor(options: options))
        {
            Options = options;
            Regexes = InitRegexes();
        }

        protected override ImmutableHashSet<Regex> InitRegexes()
        {
            var regexStrs = new HashSet<string>
            {
                NumbersDefinitions.NumberWithSuffixPercentage
            };

            var regexSet = BuildRegexes(regexStrs);

            return regexSet;
        }
    }
}