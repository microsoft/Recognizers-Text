using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.French
{
    public sealed class PercentageExtractor : BasePercentageExtractor
    {
        public PercentageExtractor() : base(new NumberExtractor()) { }
        
        protected override ImmutableHashSet<Regex> InitRegexes()
        {
            HashSet<string> regexStrs = new HashSet<string>
            {
                $@"{numExtType}(\s*)(%| pour cent|pourcentage|pour cent|pourcent|pourcents|pour cents|pourcentage|pourcentages)",
                $@"(pour cent des|pourcentage de)(\s*)(@{numExtType})"
            };

            return BuildRegexes(regexStrs);
        }
    }
}
