﻿using Microsoft.Recognizers.Definitions.Chinese;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.Chinese
{
    public class NumberRangeExtractor : BaseNumberRangeExtractor
    {
        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUMRANGE;

        public NumberRangeExtractor() : base(new NumberExtractor(), new OrdinalExtractor())
        {
            var regexes = new Dictionary<Regex, string>()
            {
                {
                    // 在...和...之间
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex1, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "TwoNum"
                },
                {
                    // 大于...小于...
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex2, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "TwoNum"
                },
                {
                    // 小于...大于...
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex3, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "TwoNum"
                },
                {
                    // ...到/至...
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex4, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "TwoNum"
                },
                {
                    // 大于/多于/高于...
                    new Regex(NumbersDefinitions.OneNumberRangeMoreRegex1, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "OneNumMore"
                },
                {
                    // 比...大/高/多
                    new Regex(NumbersDefinitions.OneNumberRangeMoreRegex2, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "OneNumMore"
                },
                {
                    // ...多/以上/之上
                    new Regex(NumbersDefinitions.OneNumberRangeMoreRegex3, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "OneNumMore"
                },
                {
                    // 小于/少于/低于...
                    new Regex(NumbersDefinitions.OneNumberRangeLessRegex1, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "OneNumLess"
                },
                {
                    // 比...小/低/少
                    new Regex(NumbersDefinitions.OneNumberRangeLessRegex2, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "OneNumLess"
                },
                {
                    // .../以下/之下
                    new Regex(NumbersDefinitions.OneNumberRangeLessRegex3, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "OneNumLess"
                },
                {
                    // 等于...
                    new Regex(NumbersDefinitions.OneNumberRangeEqualRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "OneNumEqual"
                }
            };

            Regexes = regexes.ToImmutableDictionary();
        }
    }
}
