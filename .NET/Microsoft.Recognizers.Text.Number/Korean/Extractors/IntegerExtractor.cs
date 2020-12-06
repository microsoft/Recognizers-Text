using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Korean;

namespace Microsoft.Recognizers.Text.Number.Korean
{
    public class IntegerExtractor : BaseNumberExtractor
    {

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public IntegerExtractor(KoreanNumberExtractorMode mode = KoreanNumberExtractorMode.Default)
        {
            var regexes = new Dictionary<Regex, TypeTag>
            {
                {
                    // 123456,  －１２３４５６
                    RegexCache.Get(NumbersDefinitions.NumbersSpecialsChars, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    // 15k,  16 G
                    RegexCache.Get(NumbersDefinitions.NumbersSpecialsCharsWithSuffix, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    // 1,234,  ２，３３２，１１１
                    RegexCache.Get(NumbersDefinitions.DottedNumbersSpecialsChar, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    // 마이너스 일, 마이너스 오
                    RegexCache.Get(NumbersDefinitions.ZeroToNineIntegerSpecialsChars, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.KOREAN)
                },
                {
                    // 마이너스 일, 마이너스 오
                    RegexCache.Get(NumbersDefinitions.NumbersSpecialsChars, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.KOREAN)
                },
                {
                    // 다스
                    RegexCache.Get(NumbersDefinitions.NumbersWithDozen, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.KOREAN)
                },
                {
                    // 3백21
                    RegexCache.Get(NumbersDefinitions.NativeCumKoreanRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    // 스물여섯
                    RegexCache.Get(NumbersDefinitions.NativeSingleRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.KOREAN)
                },
            };

            switch (mode)
            {
                case KoreanNumberExtractorMode.Default:
                    // 일백오십오
                    regexes.Add(
                        RegexCache.Get(NumbersDefinitions.NumbersWithAllowListRegex, RegexFlags),
                        RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.KOREAN));
                    break;

                case KoreanNumberExtractorMode.ExtractAll:
                    // 일백오십오, 사직구장, "사직구장" from "사(it is homonym, seems like four(4) or other chinese character)"
                    // Uses no allow lists and extracts all potential integers (useful in Units, for example).
                    regexes.Add(
                        RegexCache.Get(NumbersDefinitions.NumbersAggressiveRegex, RegexFlags),
                        RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.KOREAN));
                    break;
            }

            Regexes = regexes.ToImmutableDictionary();
        }

        internal sealed override ImmutableDictionary<Regex, TypeTag> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_INTEGER;
    }
}
