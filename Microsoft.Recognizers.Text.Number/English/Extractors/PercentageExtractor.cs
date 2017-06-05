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
                $@"(@{numExtType})(\s*)(%|per cents|per cent|cents|cent|percentage|percents|percent)",
                $@"(per cent of|percent of|percents of)(\s*)(@{numExtType})"
            };

            return BuildRegexes(regexStrs);
        }
    }
}