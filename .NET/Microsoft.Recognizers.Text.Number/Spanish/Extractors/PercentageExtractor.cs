using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Spanish;
using Microsoft.Recognizers.Text.Number.Extractors;

namespace Microsoft.Recognizers.Text.Number.Spanish
{
    public sealed class PercentageExtractor : BasePercentageExtractor
    {
        public PercentageExtractor() : base(new NumberExtractor()) { }

        protected override ImmutableHashSet<Regex> InitRegexes()
        {
            HashSet<string> regexStrs = new HashSet<string>
            {
                NumbersDefinitions.NumberWithPrefixPercentage
            };

            return BuildRegexes(regexStrs);
        }
    }
}