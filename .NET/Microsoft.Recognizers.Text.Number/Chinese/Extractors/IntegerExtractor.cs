using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Chinese;
using Microsoft.Recognizers.Text.Number.Config;

namespace Microsoft.Recognizers.Text.Number.Chinese
{
    public class IntegerExtractor : BaseNumberExtractor
    {
        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public IntegerExtractor(BaseNumberOptionsConfiguration config, CJKNumberExtractorMode mode = CJKNumberExtractorMode.Default)
        {
            var regexes = new Dictionary<Regex, TypeTag>()
            {
                {
                    // 123456,  －１２３４５６
                    new Regex(NumbersDefinitions.NumbersSpecialsChars, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    // 15k,  16 G
                    new Regex(NumbersDefinitions.NumbersSpecialsCharsWithSuffix, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    // 1,234,  ２，３３２，１１１
                    new Regex(NumbersDefinitions.DottedNumbersSpecialsChar, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    // 半百  半打
                    new Regex(NumbersDefinitions.NumbersWithHalfDozen, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.CHINESE)
                },
                {
                    // 半
                    new Regex(NumbersDefinitions.HalfUnitRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.CHINESE)
                },
                {
                    // 一打  五十打
                    new Regex(NumbersDefinitions.NumbersWithDozen, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.CHINESE)
                },
            };

            switch (mode)
            {
                case CJKNumberExtractorMode.Default:
                    // 一百五十五, 负一亿三百二十二.
                    // Uses an allow list to avoid extracting "四" from "四川"
                    regexes.Add(
                        new Regex(NumbersDefinitions.NumbersWithAllowListRegex, RegexFlags),
                        RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.CHINESE));
                    break;

                case CJKNumberExtractorMode.ExtractAll:
                    // 一百五十五, 负一亿三百二十二, "四" from "四川".
                    // Uses no allow lists and extracts all potential integers (useful in Units, for example).
                    regexes.Add(
                        new Regex(NumbersDefinitions.NumbersAggressiveRegex, RegexFlags),
                        RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.CHINESE));
                    break;
            }

            Regexes = regexes.ToImmutableDictionary();
        }

        internal sealed override ImmutableDictionary<Regex, TypeTag> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_INTEGER;
    }
}