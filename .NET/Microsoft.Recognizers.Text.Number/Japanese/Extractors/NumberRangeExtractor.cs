using Microsoft.Recognizers.Definitions.Japanese;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.Japanese
{
    public class NumberRangeExtractor : BaseNumberRangeExtractor
    {
        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }

        internal sealed override Regex AmbiguousFractionConnectorsRegex { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUMRANGE;

        public NumberRangeExtractor() : base(new NumberExtractor(), new OrdinalExtractor(), new BaseCJKNumberParser(new JapaneseNumberParserConfiguration()))
        {
            var regexes = new Dictionary<Regex, string>()
            {
                {
                    // ...と...の間
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex1, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , NumberRangeConstants.TWONUMBETWEEN
                },
                {
                    // より大きい...より小さい...
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex2, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , NumberRangeConstants.TWONUM
                },
                {
                    // より小さい...より大きい...
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex3, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , NumberRangeConstants.TWONUM
                },
                {
                    // ...と/から..., 20~30
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex4, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , NumberRangeConstants.TWONUMTILL
                },
                {
                    // 大なり|大きい|高い|大きく...
                    new Regex(NumbersDefinitions.OneNumberRangeMoreRegex1, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , NumberRangeConstants.MORE
                },
                {
                    // ...より大なり|大きい|高い|大きく
                    new Regex(NumbersDefinitions.OneNumberRangeMoreRegex2, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , NumberRangeConstants.MORE
                },
                {
                    // ...以上
                    new Regex(NumbersDefinitions.OneNumberRangeMoreRegex3, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , NumberRangeConstants.MORE
                },
                {
                    // 小なり|小さい|低い...
                    new Regex(NumbersDefinitions.OneNumberRangeLessRegex1, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , NumberRangeConstants.LESS
                },
                {
                    // ...より小なり|小さい|低い
                    new Regex(NumbersDefinitions.OneNumberRangeLessRegex2, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , NumberRangeConstants.LESS
                },
                {
                    // ...以下
                    new Regex(NumbersDefinitions.OneNumberRangeLessRegex3, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , NumberRangeConstants.LESS
                },
                {
                    // イコール...　｜　...等しい|
                    new Regex(NumbersDefinitions.OneNumberRangeEqualRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , NumberRangeConstants.EQUAL
                }
            };

            Regexes = regexes.ToImmutableDictionary();

            AmbiguousFractionConnectorsRegex = new Regex(NumbersDefinitions.AmbiguousFractionConnectorsRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }
    }
}
