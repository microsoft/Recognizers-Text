using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Japanese;

namespace Microsoft.Recognizers.Text.Number.Japanese
{
    public class PercentageExtractor : BaseNumberExtractor
    {

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public PercentageExtractor()
        {
            var regexes = new Dictionary<Regex, TypeTag>
            {
                {
                    // 百パーセント 十五パーセント
                    RegexCache.Get(NumbersDefinitions.SimplePercentageRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.JAPANESE)
                },
                {
                    // 19パーセント　１パーセント
                    RegexCache.Get(NumbersDefinitions.NumbersPercentagePointRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    // 3,000パーセント  １，１２３パーセント
                    RegexCache.Get(NumbersDefinitions.NumbersPercentageWithSeparatorRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    // 3.2 k パーセント
                    RegexCache.Get(NumbersDefinitions.NumbersPercentageWithMultiplierRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    // 15kパーセント
                    RegexCache.Get(NumbersDefinitions.SimpleNumbersPercentageWithMultiplierRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    // @TODO Example missing
                    RegexCache.Get(NumbersDefinitions.SimpleIntegerPercentageRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    // 2割引 ２.５割引
                    RegexCache.Get(NumbersDefinitions.NumbersFoldsPercentageRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.SPECIAL_SUFFIX)
                },
                {
                    // 三割引 六点五折 七五折
                    RegexCache.Get(NumbersDefinitions.FoldsPercentageRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.SPECIAL_SUFFIX)
                },
                {
                    // 5割 7割半
                    RegexCache.Get(NumbersDefinitions.SimpleFoldsPercentageRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.SPECIAL_SUFFIX)
                },
                {
                    // 七割半
                    RegexCache.Get(NumbersDefinitions.SpecialsPercentageRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.SPECIAL_SUFFIX)
                },
                {
                    // 2割 ２.５割
                    RegexCache.Get(NumbersDefinitions.NumbersSpecialsPercentageRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.SPECIAL_SUFFIX)
                },
                {
                    // 三割
                    RegexCache.Get(NumbersDefinitions.SimpleSpecialsPercentageRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.SPECIAL_SUFFIX)
                },
                {
                    // @TODO Example missing
                    RegexCache.Get(NumbersDefinitions.SpecialsFoldsPercentageRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.SPECIAL_SUFFIX)
                },
            };

            Regexes = regexes.ToImmutableDictionary();
        }

        internal sealed override ImmutableDictionary<Regex, TypeTag> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_PERCENTAGE;
    }
}
