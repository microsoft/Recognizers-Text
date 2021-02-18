using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Chinese;

namespace Microsoft.Recognizers.Text.Number.Chinese
{
    public class NumberRangeExtractor : BaseNumberRangeExtractor
    {
        public NumberRangeExtractor(INumberOptionsConfiguration config)
            : base(new NumberExtractor(new BaseNumberOptionsConfiguration(config)),
                   new OrdinalExtractor(new BaseNumberOptionsConfiguration(config)),
                   new BaseCJKNumberParser(new ChineseNumberParserConfiguration(config)),
                   config)
        {
            var regexes = new Dictionary<Regex, string>()
            {
                {
                    // 在...和...之间
                    RegexCache.Get(NumbersDefinitions.TwoNumberRangeRegex1, RegexOptions.Singleline),
                    NumberRangeConstants.TWONUMBETWEEN
                },
                {
                    // 大于...小于...
                    RegexCache.Get(NumbersDefinitions.TwoNumberRangeRegex2, RegexOptions.Singleline),
                    NumberRangeConstants.TWONUM
                },
                {
                    // 小于...大于...
                    RegexCache.Get(NumbersDefinitions.TwoNumberRangeRegex3, RegexOptions.Singleline),
                    NumberRangeConstants.TWONUM
                },
                {
                    // ...到/至..., 20~30
                    RegexCache.Get(NumbersDefinitions.TwoNumberRangeRegex4, RegexOptions.Singleline),
                    NumberRangeConstants.TWONUMTILL
                },
                {
                    // 大于/多于/高于...
                    RegexCache.Get(NumbersDefinitions.OneNumberRangeMoreRegex1, RegexOptions.Singleline),
                    NumberRangeConstants.MORE
                },
                {
                    // 比...大/高/多
                    RegexCache.Get(NumbersDefinitions.OneNumberRangeMoreRegex2, RegexOptions.Singleline),
                    NumberRangeConstants.MORE
                },
                {
                    // ...多/以上/之上
                    RegexCache.Get(NumbersDefinitions.OneNumberRangeMoreRegex3, RegexOptions.Singleline),
                    NumberRangeConstants.MORE
                },
                {
                    // 小于/少于/低于...
                    RegexCache.Get(NumbersDefinitions.OneNumberRangeLessRegex1, RegexOptions.Singleline),
                    NumberRangeConstants.LESS
                },
                {
                    // 比...小/低/少
                    RegexCache.Get(NumbersDefinitions.OneNumberRangeLessRegex2, RegexOptions.Singleline),
                    NumberRangeConstants.LESS
                },
                {
                    // .../以下/之下
                    RegexCache.Get(NumbersDefinitions.OneNumberRangeLessRegex3, RegexOptions.Singleline),
                    NumberRangeConstants.LESS
                },
                {
                    // 等于...
                    RegexCache.Get(NumbersDefinitions.OneNumberRangeEqualRegex, RegexOptions.Singleline),
                    NumberRangeConstants.EQUAL
                },
            };

            Regexes = regexes.ToImmutableDictionary();

            AmbiguousFractionConnectorsRegex =
                RegexCache.Get(NumbersDefinitions.AmbiguousFractionConnectorsRegex, RegexOptions.Singleline);
        }

        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }

        internal sealed override Regex AmbiguousFractionConnectorsRegex { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUMRANGE;
    }
}
