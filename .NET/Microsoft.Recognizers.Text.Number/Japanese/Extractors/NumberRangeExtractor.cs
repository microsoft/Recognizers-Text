// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

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
            : base(new NumberExtractor(new BaseNumberOptionsConfiguration(config)),
                   OrdinalExtractor.GetInstance(new BaseNumberOptionsConfiguration(config.Culture, config.Options)),
                   new BaseCJKNumberParser(new JapaneseNumberParserConfiguration(config)),
                   config)
        {

            var regexes = new Dictionary<Regex, string>
            {
                {
                    // ...と...の間
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex1, RegexFlags, RegexTimeOut),
                    NumberRangeConstants.TWONUMBETWEEN
                },
                {
                    // より大きい...より小さい...
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex2, RegexFlags, RegexTimeOut),
                    NumberRangeConstants.TWONUM
                },
                {
                    // より小さい...より大きい...
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex3, RegexFlags, RegexTimeOut),
                    NumberRangeConstants.TWONUM
                },
                {
                    // ...と/から..., 20~30
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex4, RegexFlags, RegexTimeOut),
                    NumberRangeConstants.TWONUMTILL
                },
                {
                    // 大なり|大きい|高い|大きく...
                    new Regex(NumbersDefinitions.OneNumberRangeMoreRegex1, RegexFlags, RegexTimeOut),
                    NumberRangeConstants.MORE
                },
                {
                    // ...以上
                    new Regex(NumbersDefinitions.OneNumberRangeMoreRegex3, RegexFlags, RegexTimeOut),
                    NumberRangeConstants.MORE
                },
                {
                    // 少なくとも|多くて|最大...
                    new Regex(NumbersDefinitions.OneNumberRangeMoreRegex4, RegexFlags, RegexTimeOut),
                    NumberRangeConstants.MORE
                },
                {
                    // ...以上
                    new Regex(NumbersDefinitions.OneNumberRangeMoreRegex5, RegexFlags, RegexTimeOut),
                    NumberRangeConstants.MORE
                },
                {
                    // ...以上
                    new Regex(NumbersDefinitions.TwoNumberRangeMoreSuffix, RegexFlags, RegexTimeOut),
                    NumberRangeConstants.MORE
                },
                {
                    // 小なり|小さい|低い...
                    new Regex(NumbersDefinitions.OneNumberRangeLessRegex1, RegexFlags, RegexTimeOut),
                    NumberRangeConstants.LESS
                },
                {
                    // ...以下
                    new Regex(NumbersDefinitions.OneNumberRangeLessRegex3, RegexFlags, RegexTimeOut),
                    NumberRangeConstants.LESS
                },
                {
                    // ...以下
                    new Regex(NumbersDefinitions.OneNumberRangeLessRegex4, RegexFlags, RegexTimeOut),
                    NumberRangeConstants.LESS
                },
                {
                    // イコール...　｜　...等しい|
                    new Regex(NumbersDefinitions.OneNumberRangeEqualRegex, RegexFlags, RegexTimeOut),
                    NumberRangeConstants.EQUAL
                },
            };

            Regexes = regexes.ToImmutableDictionary();

            AmbiguousFractionConnectorsRegex =
                new Regex(NumbersDefinitions.AmbiguousFractionConnectorsRegex, RegexFlags, RegexTimeOut);
        }

        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }

        internal sealed override Regex AmbiguousFractionConnectorsRegex { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUMRANGE;
    }
}
