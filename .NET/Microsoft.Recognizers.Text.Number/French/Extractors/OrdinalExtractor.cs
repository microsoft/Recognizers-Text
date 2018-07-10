using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.French;

namespace Microsoft.Recognizers.Text.Number.French
{
    public class OrdinalExtractor : BaseNumberExtractor
    {
        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_ORDINAL; // "Ordinal";

        public OrdinalExtractor()
        {
            this.Regexes = new Dictionary<Regex, string>
            {
                {
                    new Regex(NumbersDefinitions.OrdinalSuffixRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline), RegexTagGenerator.GenerateRegexTag(Constants.ORDINAL_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    new Regex(NumbersDefinitions.OrdinalFrenchRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline), RegexTagGenerator.GenerateRegexTag(Constants.ORDINAL_PREFIX, Constants.FRENCH)
                }
            }.ToImmutableDictionary();
        }
    }
}
