using Microsoft.Recognizers.Definitions.Chinese;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.Chinese
{
    public class NumberRangeExtractor : BaseNumberRangeExtractor
    {
        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }

        internal sealed override Regex AmbiguousFractionConnectorsRegex { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUMRANGE;

        public NumberRangeExtractor() : base(new NumberExtractor(), new OrdinalExtractor(), new BaseCJKNumberParser(new ChineseNumberParserConfiguration()))
        {
            var regexes = new Dictionary<Regex, string>()
            {
                {
                    // 在...和...之间
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex1, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , NumberRangeConstants.TWONUMBETWEEN
                },
                {
                    // 大于...小于...
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex2, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , NumberRangeConstants.TWONUM
                },
                {
                    // 小于...大于...
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex3, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , NumberRangeConstants.TWONUM
                },
                {
                    // ...到/至..., 20~30
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex4, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , NumberRangeConstants.TWONUMTILL
                },
                {
                    // 大于/多于/高于...
                    new Regex(NumbersDefinitions.OneNumberRangeMoreRegex1, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , NumberRangeConstants.MORE
                },
                {
                    // 比...大/高/多
                    new Regex(NumbersDefinitions.OneNumberRangeMoreRegex2, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , NumberRangeConstants.MORE
                },
                {
                    // ...多/以上/之上
                    new Regex(NumbersDefinitions.OneNumberRangeMoreRegex3, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , NumberRangeConstants.MORE
                },
                {
                    // 小于/少于/低于...
                    new Regex(NumbersDefinitions.OneNumberRangeLessRegex1, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , NumberRangeConstants.LESS
                },
                {
                    // 比...小/低/少
                    new Regex(NumbersDefinitions.OneNumberRangeLessRegex2, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , NumberRangeConstants.LESS
                },
                {
                    // .../以下/之下
                    new Regex(NumbersDefinitions.OneNumberRangeLessRegex3, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , NumberRangeConstants.LESS
                },
                {
                    // 等于...
                    new Regex(NumbersDefinitions.OneNumberRangeEqualRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , NumberRangeConstants.EQUAL
                }
            };

            Regexes = regexes.ToImmutableDictionary();

            AmbiguousFractionConnectorsRegex = new Regex(NumbersDefinitions.AmbiguousFractionConnectorsRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }
    }
}
