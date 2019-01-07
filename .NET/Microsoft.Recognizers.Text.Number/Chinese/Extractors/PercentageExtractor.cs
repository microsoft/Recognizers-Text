using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Chinese;

namespace Microsoft.Recognizers.Text.Number.Chinese
{
    public class PercentageExtractor : BaseNumberExtractor
    {
        public PercentageExtractor()
        {
            var regexes = new Dictionary<Regex, TypeTag>
            {
                {
                    // 二十个百分点,  四点五个百分点
                    new Regex(NumbersDefinitions.PercentagePointRegex, RegexOptions.Singleline), RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.CHINESE)
                },
                {
                    // 百分之五十  百分之一点五
                    new Regex(NumbersDefinitions.SimplePercentageRegex, RegexOptions.Singleline), RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.CHINESE)
                },
                {
                    // 百分之５６.２　百分之１２
                    new Regex(NumbersDefinitions.NumbersPercentagePointRegex, RegexOptions.Singleline), RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    // 百分之3,000  百分之１，１２３
                    new Regex(NumbersDefinitions.NumbersPercentageWithSeparatorRegex, RegexOptions.Singleline), RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    // 百分之3.2 k
                    new Regex(NumbersDefinitions.NumbersPercentageWithMultiplierRegex, RegexOptions.Singleline), RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    // 12.56个百分点  ０.４个百分点
                    new Regex(NumbersDefinitions.FractionPercentagePointRegex, RegexOptions.Singleline), RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    // 15,123个百分点  １１１，１１１个百分点
                    new Regex(NumbersDefinitions.FractionPercentageWithSeparatorRegex, RegexOptions.Singleline), RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    // 12.1k个百分点  １５.1k个百分点
                    new Regex(NumbersDefinitions.FractionPercentageWithMultiplierRegex, RegexOptions.Singleline), RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    // 百分之22  百分之１２０
                    new Regex(NumbersDefinitions.SimpleNumbersPercentageRegex, RegexOptions.Singleline), RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    // 百分之15k
                    new Regex(NumbersDefinitions.SimpleNumbersPercentageWithMultiplierRegex, RegexOptions.Singleline), RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    // 百分之1,111  百分之９，９９９
                    new Regex(NumbersDefinitions.SimpleNumbersPercentagePointRegex, RegexOptions.Singleline), RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    // 12个百分点
                    new Regex(NumbersDefinitions.IntegerPercentageRegex, RegexOptions.Singleline), RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    // 12k个百分点
                    new Regex(NumbersDefinitions.IntegerPercentageWithMultiplierRegex, RegexOptions.Singleline), RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    // 2,123个百分点
                    new Regex(NumbersDefinitions.NumbersFractionPercentageRegex, RegexOptions.Singleline), RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    // 32.5%
                    new Regex(NumbersDefinitions.SimpleIntegerPercentageRegex, RegexOptions.Singleline), RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    // 2折 ２.５折
                    new Regex(NumbersDefinitions.NumbersFoldsPercentageRegex, RegexOptions.Singleline), RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.SPECIAL_SUFFIX)
                },
                {
                    // 三折 六点五折 七五折
                    new Regex(NumbersDefinitions.FoldsPercentageRegex, RegexOptions.Singleline), RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.SPECIAL_SUFFIX)
                },
                {
                    // 5成 6成半 6成4
                    new Regex(NumbersDefinitions.SimpleFoldsPercentageRegex, RegexOptions.Singleline), RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.SPECIAL_SUFFIX)
                },
                {
                    // 七成半 七成五
                    new Regex(NumbersDefinitions.SpecialsPercentageRegex, RegexOptions.Singleline), RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.SPECIAL_SUFFIX)
                },
                {
                    // 2成 ２.５成
                    new Regex(NumbersDefinitions.NumbersSpecialsPercentageRegex, RegexOptions.Singleline), RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.SPECIAL_SUFFIX)
                },
                {
                    // 三成 六点五成
                    new Regex(NumbersDefinitions.SimpleSpecialsPercentageRegex, RegexOptions.Singleline), RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.SPECIAL_SUFFIX)
                },
                {
                    // 打对折 半成
                    new Regex(NumbersDefinitions.SpecialsFoldsPercentageRegex, RegexOptions.Singleline), RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.SPECIAL_SUFFIX)
                },
            };

            Regexes = regexes.ToImmutableDictionary();
        }

        internal sealed override ImmutableDictionary<Regex, TypeTag> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_PERCENTAGE;
    }
}