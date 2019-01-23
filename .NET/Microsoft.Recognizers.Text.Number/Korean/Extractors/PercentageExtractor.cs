using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Korean;

namespace Microsoft.Recognizers.Text.Number.Korean
{
    public class PercentageExtractor : BaseNumberExtractor
    {
        public PercentageExtractor()
        {
            var regexes = new Dictionary<Regex, TypeTag>()
            {
                {
                    // 백퍼센트 십오퍼센트
                    new Regex(NumbersDefinitions.SimplePercentageRegex, RegexOptions.Singleline),
                    RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.KOREAN)
                },
                {
                    // 19퍼센트　１퍼센트
                    new Regex(NumbersDefinitions.NumbersPercentagePointRegex, RegexOptions.Singleline),
                    RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    // 3,000퍼센트  １，１２３퍼센트
                    new Regex(NumbersDefinitions.NumbersPercentageWithSeparatorRegex, RegexOptions.Singleline),
                    RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    // 3.2 k 퍼센트
                    new Regex(NumbersDefinitions.NumbersPercentageWithMultiplierRegex, RegexOptions.Singleline),
                    RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    // 15k퍼센트
                    new Regex(NumbersDefinitions.SimpleNumbersPercentageWithMultiplierRegex, RegexOptions.Singleline),
                    RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                // 마이너스십삼퍼센트
                new Regex(NumbersDefinitions.SimpleIntegerPercentageRegex, RegexOptions.Singleline),
                RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.NUMBER_SUFFIX)
                },
            };

            Regexes = regexes.ToImmutableDictionary();
        }

        internal sealed override ImmutableDictionary<Regex, TypeTag> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_PERCENTAGE;
    }
}
