using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Chinese;

namespace Microsoft.Recognizers.Text.Number.Chinese
{
    public class IntegerExtractor : BaseNumberExtractor
    {
        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_INTEGER;
        
        public IntegerExtractor(ChineseNumberMode mode = ChineseNumberMode.Default)
        {
            var regexes = new Dictionary<Regex, string>()
            {
                {
                    // 123456,  －１２３４５６
                    new Regex(NumbersDefinitions.NumbersSpecialsChars, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "IntegerNum"
                },
                {
                    //15k,  16 G
                    new Regex(NumbersDefinitions.NumbersSpecialsCharsWithSuffix, RegexOptions.Singleline)
                    , "IntegerNum"
                },
                {
                    //1,234,  ２，３３２，１１１
                    new Regex(NumbersDefinitions.DottedNumbersSpecialsChar, RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "IntegerNum"
                },
                {
                    //半百  半打
                    new Regex(NumbersDefinitions.NumbersWithHalfDozen, RegexOptions.Singleline)
                    , "IntegerChs"
                },
                {
                    //一打  五十打
                    new Regex(NumbersDefinitions.NumbersWithDozen, RegexOptions.Singleline)
                    , "IntegerChs"
                }
            };
            switch (mode)
            {
                case ChineseNumberMode.Default:
                    //一百五十五,  负一亿三百二十二, avoid 五十五点五个百分点
                    regexes.Add(
                        new Regex(NumbersDefinitions.NumbersWithoutPercent, RegexOptions.Singleline),
                        "IntegerChs");
                    break;
                case ChineseNumberMode.ExtractAll:
                    //一百五十五,  负一亿三百二十二, avoid 五十五点五个百分点
                    regexes.Add(
                        new Regex(NumbersDefinitions.NumbersWithPercent, RegexOptions.Singleline),
                        "IntegerChs");
                    break;
            }

            Regexes = regexes.ToImmutableDictionary();
        }
    }
}