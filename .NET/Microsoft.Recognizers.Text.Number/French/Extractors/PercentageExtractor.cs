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
                $@"(@{NumExtType})(\s*)(%|pourcentages|pourcents|pourcentage|pourcent)",
                $@"(%|pourcent|pourcent des|pourcentage de)(\s*)(@{NumExtType})"                
            };

            return BuildRegexes(regexStrs);
        }
    }
}
