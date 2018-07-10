using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.French;

namespace Microsoft.Recognizers.Text.Number.French
{
    public class FractionExtractor : BaseNumberExtractor
    {
        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_FRACTION; // "Fraction";

        public FractionExtractor()
        {
            this.Regexes = new Dictionary<Regex, string>
            {
                {
                    new Regex(NumbersDefinitions.FractionNotationWithSpacesRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , RegexTagGenerator.GenerateRegexTag(Constants.FRACTION_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    new Regex(NumbersDefinitions.FractionNotationRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , RegexTagGenerator.GenerateRegexTag(Constants.FRACTION_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    new Regex(NumbersDefinitions.FractionNounRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , RegexTagGenerator.GenerateRegexTag(Constants.FRACTION_PREFIX, Constants.FRENCH)
                },
                {
                    new Regex(NumbersDefinitions.FractionNounWithArticleRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , RegexTagGenerator.GenerateRegexTag(Constants.FRACTION_PREFIX, Constants.FRENCH)
                },
                {
                    new Regex(NumbersDefinitions.FractionPrepositionRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , RegexTagGenerator.GenerateRegexTag(Constants.FRACTION_PREFIX, Constants.FRENCH)
                }
            }.ToImmutableDictionary();
        }
    }
}
