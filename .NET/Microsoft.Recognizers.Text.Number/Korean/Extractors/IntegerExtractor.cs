using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Korean;

namespace Microsoft.Recognizers.Text.Number.Korean
{
    public class IntegerExtractor : BaseNumberExtractor
    {
        public IntegerExtractor(KoreanNumberExtractorMode mode = KoreanNumberExtractorMode.Default)
        {
            var regexes = new Dictionary<Regex, TypeTag>
            {
                {
                    // 123456,  －１２３４５６
                    new Regex(NumbersDefinitions.NumbersSpecialsChars, RegexOptions.Singleline),
                    RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    // 15k,  16 G
                    new Regex(NumbersDefinitions.NumbersSpecialsCharsWithSuffix, RegexOptions.Singleline),
                    RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    // 1,234,  ２，３３２，１１１
                    new Regex(NumbersDefinitions.DottedNumbersSpecialsChar, RegexOptions.Singleline),
                    RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    // 마이너스 일, 마이너스 오
                    new Regex(NumbersDefinitions.ZeroToNineIntegerSpecialsChars, RegexOptions.Singleline),
                    RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.KOREAN)
                },
            };

            switch (mode)
            {
                case KoreanNumberExtractorMode.Default:
                    // 일백오십오
                    regexes.Add(
                        new Regex(NumbersDefinitions.NumbersWithAllowListRegex, RegexOptions.Singleline),
                        RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.KOREAN));
                    break;

                case KoreanNumberExtractorMode.ExtractAll:
                    // 일백오십오, 사직구장, "사직구장" from "사(it is homonym, seems like four(4) or other chinese character)"
                    // Uses no allow lists and extracts all potential integers (useful in Units, for example).
                    regexes.Add(
                        new Regex(NumbersDefinitions.NumbersAggressiveRegex, RegexOptions.Singleline),
                        RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.KOREAN));
                    break;
            }

            Regexes = regexes.ToImmutableDictionary();
        }

        internal sealed override ImmutableDictionary<Regex, TypeTag> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_INTEGER;
    }
}
