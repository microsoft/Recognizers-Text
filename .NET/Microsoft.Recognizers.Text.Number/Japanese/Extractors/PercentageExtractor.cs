using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Japanese;

namespace Microsoft.Recognizers.Text.Number.Japanese
{
    public class PercentageExtractor : BaseNumberExtractor
    {

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public PercentageExtractor(BaseNumberOptionsConfiguration config)
        {
            var regexes = new Dictionary<Regex, TypeTag>
            {
                {
                    // 百パーセント 十五パーセント
                    new Regex(NumbersDefinitions.SimplePercentageRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.JAPANESE)
                },
                {
                    // 19パーセント　１パーセント
                    new Regex(NumbersDefinitions.NumbersPercentagePointRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    // 3,000パーセント  １，１２３パーセント
                    new Regex(NumbersDefinitions.NumbersPercentageWithSeparatorRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    // 3.2 k パーセント
                    new Regex(NumbersDefinitions.NumbersPercentageWithMultiplierRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    // 15kパーセント
                    new Regex(NumbersDefinitions.SimpleNumbersPercentageWithMultiplierRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    // @TODO Example missing
                    new Regex(NumbersDefinitions.SimpleIntegerPercentageRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    // 2割引 ２.５割引
                    new Regex(NumbersDefinitions.NumbersFoldsPercentageRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.SPECIAL_SUFFIX)
                },
                {
                    // 三割引 六点五折 七五折
                    new Regex(NumbersDefinitions.FoldsPercentageRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.SPECIAL_SUFFIX)
                },
                {
                    // 5割 7割半
                    new Regex(NumbersDefinitions.SimpleFoldsPercentageRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.SPECIAL_SUFFIX)
                },
                {
                    // 七割半
                    new Regex(NumbersDefinitions.SpecialsPercentageRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.SPECIAL_SUFFIX)
                },
                {
                    // 2割 ２.５割
                    new Regex(NumbersDefinitions.NumbersSpecialsPercentageRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.SPECIAL_SUFFIX)
                },
                {
                    // 三割
                    new Regex(NumbersDefinitions.SimpleSpecialsPercentageRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.SPECIAL_SUFFIX)
                },
                {
                    // @TODO Example missing
                    new Regex(NumbersDefinitions.SpecialsFoldsPercentageRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.SPECIAL_SUFFIX)
                },
            };

            Regexes = regexes.ToImmutableDictionary();
        }

        internal sealed override ImmutableDictionary<Regex, TypeTag> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_PERCENTAGE;
    }
}
