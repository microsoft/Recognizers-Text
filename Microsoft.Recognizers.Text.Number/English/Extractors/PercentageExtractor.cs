using Microsoft.Recognizers.Resources.English;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.English
{
    public sealed class PercentageExtractor : BasePercentageExtractor
    {
        public PercentageExtractor() : base(new NumberExtractor()) { }

        protected override ImmutableHashSet<Regex> InitRegexes()
        {
            HashSet<string> regexStrs = new HashSet<string>
            {
                Numeric.NumberWithSuffixPercentage,
                Numeric.NumberWithPrefixPercentage
            };

            return BuildRegexes(regexStrs);
        }
    }
}