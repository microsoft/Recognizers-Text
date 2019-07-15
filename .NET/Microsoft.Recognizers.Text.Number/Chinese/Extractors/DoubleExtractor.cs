using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Chinese;

namespace Microsoft.Recognizers.Text.Number.Chinese
{
    public class DoubleExtractor : BaseNumberExtractor
    {

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public DoubleExtractor()
        {
            var regexes = new Dictionary<Regex, TypeTag>
            {
                {
                    new Regex(NumbersDefinitions.DoubleSpecialsChars, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.DOUBLE_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    // (-)2.5, can avoid cases like ip address xx.xx.xx.xx
                    new Regex(NumbersDefinitions.DoubleSpecialsCharsWithNegatives, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.DOUBLE_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    // (-).2
                    new Regex(NumbersDefinitions.SimpleDoubleSpecialsChars, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.DOUBLE_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    // 1.0 K
                    new Regex(NumbersDefinitions.DoubleWithMultiplierRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.DOUBLE_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    // １５.２万
                    new Regex(NumbersDefinitions.DoubleWithThousandsRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.DOUBLE_PREFIX, Constants.CHINESE)
                },
                {
                    // 四十五点三三
                    new Regex(NumbersDefinitions.DoubleAllFloatRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.DOUBLE_PREFIX, Constants.CHINESE)
                },
                {
                    // 2e6, 21.2e0
                    new Regex(NumbersDefinitions.DoubleExponentialNotationRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.DOUBLE_PREFIX, Constants.POWER_SUFFIX)
                },
                {
                    // 2^5
                    new Regex(NumbersDefinitions.DoubleScientificNotationRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.DOUBLE_PREFIX, Constants.POWER_SUFFIX)
                },
            };
            Regexes = regexes.ToImmutableDictionary();
        }

        internal sealed override ImmutableDictionary<Regex, TypeTag> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_DOUBLE;
    }
}