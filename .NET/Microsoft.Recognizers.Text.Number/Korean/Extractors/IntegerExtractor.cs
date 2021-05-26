using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Korean;
using Microsoft.Recognizers.Text.Number.Config;

namespace Microsoft.Recognizers.Text.Number.Korean
{
    public class IntegerExtractor : BaseNumberExtractor
    {

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public IntegerExtractor(BaseNumberOptionsConfiguration config, CJKNumberExtractorMode mode = CJKNumberExtractorMode.Default)
        {
            var regexes = new Dictionary<Regex, TypeTag>
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
                    // 마이너스 일, 마이너스 오
                    new Regex(NumbersDefinitions.ZeroToNineIntegerSpecialsChars, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.KOREAN)
                },
                {
                    // 마이너스 일, 마이너스 오
                    new Regex(NumbersDefinitions.NumbersSpecialsChars, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.KOREAN)
                },
                {
                    // 다스
                    new Regex(NumbersDefinitions.NumbersWithDozen, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.KOREAN)
                },
                {
                    // 3백21
                    new Regex(NumbersDefinitions.NativeCumKoreanRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    // 스물여섯
                    new Regex(NumbersDefinitions.NativeSingleRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.KOREAN)
                },
            };

            switch (mode)
            {
                case CJKNumberExtractorMode.Default:
                    // 일백오십오
                    regexes.Add(
                        new Regex(NumbersDefinitions.NumbersWithAllowListRegex, RegexFlags),
                        RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.KOREAN));
                    break;

                case CJKNumberExtractorMode.ExtractAll:
                    // 일백오십오, 사직구장, "사직구장" from "사(it is homonym, seems like four(4) or other chinese character)"
                    // Uses no allow lists and extracts all potential integers (useful in Units, for example).
                    regexes.Add(
                        new Regex(NumbersDefinitions.NumbersAggressiveRegex, RegexFlags),
                        RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.KOREAN));
                    regexes.Add(
                        new Regex(NumbersDefinitions.InexactNumberUnitRegex, RegexFlags),
                        RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.KOREAN));
                    break;
            }

            Regexes = regexes.ToImmutableDictionary();
        }

        internal sealed override ImmutableDictionary<Regex, TypeTag> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_INTEGER;
    }
}
