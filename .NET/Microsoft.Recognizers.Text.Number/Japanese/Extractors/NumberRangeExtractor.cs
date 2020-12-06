using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Japanese;

namespace Microsoft.Recognizers.Text.Number.Japanese
{
    public class NumberRangeExtractor : BaseNumberRangeExtractor
    {

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public NumberRangeExtractor(INumberOptionsConfiguration config)
            : base(new NumberExtractor(),
                   new OrdinalExtractor(),
                   new BaseCJKNumberParser(new JapaneseNumberParserConfiguration(config)),
                   config)
        {

            var regexes = new Dictionary<Regex, string>
            {
                {
                    // ...と...の間
                    RegexCache.Get(NumbersDefinitions.TwoNumberRangeRegex1, RegexFlags),
                    NumberRangeConstants.TWONUMBETWEEN
                },
                {
                    // より大きい...より小さい...
                    RegexCache.Get(NumbersDefinitions.TwoNumberRangeRegex2, RegexFlags),
                    NumberRangeConstants.TWONUM
                },
                {
                    // より小さい...より大きい...
                    RegexCache.Get(NumbersDefinitions.TwoNumberRangeRegex3, RegexFlags),
                    NumberRangeConstants.TWONUM
                },
                {
                    // ...と/から..., 20~30
                    RegexCache.Get(NumbersDefinitions.TwoNumberRangeRegex4, RegexFlags),
                    NumberRangeConstants.TWONUMTILL
                },
                {
                    // 大なり|大きい|高い|大きく...
                    RegexCache.Get(NumbersDefinitions.OneNumberRangeMoreRegex1, RegexFlags),
                    NumberRangeConstants.MORE
                },
                {
                    // ...以上
                    RegexCache.Get(NumbersDefinitions.OneNumberRangeMoreRegex3, RegexFlags),
                    NumberRangeConstants.MORE
                },
                {
                    // 少なくとも|多くて|最大...
                    RegexCache.Get(NumbersDefinitions.OneNumberRangeMoreRegex4, RegexFlags),
                    NumberRangeConstants.MORE
                },
                {
                    // ...以上
                    RegexCache.Get(NumbersDefinitions.OneNumberRangeMoreRegex5, RegexFlags),
                    NumberRangeConstants.MORE
                },
                {
                    // ...以上
                    RegexCache.Get(NumbersDefinitions.TwoNumberRangeMoreSuffix, RegexFlags),
                    NumberRangeConstants.MORE
                },
                {
                    // 小なり|小さい|低い...
                    RegexCache.Get(NumbersDefinitions.OneNumberRangeLessRegex1, RegexFlags),
                    NumberRangeConstants.LESS
                },
                {
                    // ...以下
                    RegexCache.Get(NumbersDefinitions.OneNumberRangeLessRegex3, RegexFlags),
                    NumberRangeConstants.LESS
                },
                {
                    // ...以下
                    RegexCache.Get(NumbersDefinitions.OneNumberRangeLessRegex4, RegexFlags),
                    NumberRangeConstants.LESS
                },
                {
                    // イコール...　｜　...等しい|
                    RegexCache.Get(NumbersDefinitions.OneNumberRangeEqualRegex, RegexFlags),
                    NumberRangeConstants.EQUAL
                },
            };

            Regexes = regexes.ToImmutableDictionary();

            AmbiguousFractionConnectorsRegex =
                RegexCache.Get(NumbersDefinitions.AmbiguousFractionConnectorsRegex, RegexFlags);
        }

        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }

        internal sealed override Regex AmbiguousFractionConnectorsRegex { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUMRANGE;
    }
}
