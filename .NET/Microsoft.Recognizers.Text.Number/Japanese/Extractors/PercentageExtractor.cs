using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Japanese;

namespace Microsoft.Recognizers.Text.Number.Japanese
{
    public class PercentageExtractor : BaseNumberExtractor
    {
        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_PERCENTAGE;

        public PercentageExtractor()
        {
            var regexes = new Dictionary<Regex, string>()
            {
                {
                    //百パーセント 十五パーセント
                    new Regex(NumbersDefinitions.SimplePercentageRegex, RegexOptions.Singleline), RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.JAPANESE)
                },
                {
                    //19パーセント　１パーセント
                    new Regex(NumbersDefinitions.NumbersPercentagePointRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline), RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    //3,000パーセント  １，１２３パーセント
                    new Regex(NumbersDefinitions.NumbersPercentageWithSeparatorRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline), RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    //3.2 k パーセント
                    new Regex(NumbersDefinitions.NumbersPercentageWithMultiplierRegex, RegexOptions.Singleline), RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.NUMBER_SUFFIX)
                }
                ,
                {
                    //15kパーセント 
                    new Regex(NumbersDefinitions.SimpleNumbersPercentageWithMultiplierRegex, RegexOptions.Singleline), RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                // @TODO Example missing
                new Regex(NumbersDefinitions.SimpleIntegerPercentageRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline), RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    //2割引 ２.５割引
                    new Regex(NumbersDefinitions.NumbersFoldsPercentageRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline), RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.SPECIAL_SUFFIX)
                },
                {
                    //三割引 六点五折 七五折
                    new Regex(NumbersDefinitions.FoldsPercentageRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline), RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.SPECIAL_SUFFIX)
                },
                {
                    //5割 7割半
                    new Regex(NumbersDefinitions.SimpleFoldsPercentageRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline), RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.SPECIAL_SUFFIX)
                },
                {
                    //七割半
                    new Regex(NumbersDefinitions.SpecialsPercentageRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline), RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.SPECIAL_SUFFIX)
                },
                {
                    //2割 ２.５割
                    new Regex(NumbersDefinitions.NumbersSpecialsPercentageRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline), RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.SPECIAL_SUFFIX)
                },
                {
                    //三割
                    new Regex(NumbersDefinitions.SimpleSpecialsPercentageRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline), RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.SPECIAL_SUFFIX)
                },
                {
                    // @TODO Example missing
                    new Regex(NumbersDefinitions.SpecialsFoldsPercentageRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline), RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.SPECIAL_SUFFIX)
                }
            };

            Regexes = regexes.ToImmutableDictionary();
        }
    }
}
