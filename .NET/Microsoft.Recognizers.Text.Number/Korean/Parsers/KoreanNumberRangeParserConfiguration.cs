// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Korean;

namespace Microsoft.Recognizers.Text.Number.Korean
{
    public class KoreanNumberRangeParserConfiguration : BaseNumberRangeParserConfiguration
    {

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public KoreanNumberRangeParserConfiguration(INumberOptionsConfiguration config)
        {
            CultureInfo = new CultureInfo(config.Culture);

            var numConfig = new BaseNumberOptionsConfiguration(config);

            NumberExtractor = new NumberExtractor(numConfig);
            OrdinalExtractor = Japanese.OrdinalExtractor.GetInstance(numConfig);
            NumberParser = new BaseCJKNumberParser(new KoreanNumberParserConfiguration(config));

            MoreOrEqual = new Regex(NumbersDefinitions.MoreOrEqual, RegexFlags, RegexTimeOut);
            LessOrEqual = new Regex(NumbersDefinitions.LessOrEqual, RegexFlags, RegexTimeOut);
            MoreOrEqualSuffix = new Regex(NumbersDefinitions.MoreOrEqualSuffix, RegexFlags, RegexTimeOut);
            LessOrEqualSuffix = new Regex(NumbersDefinitions.LessOrEqualSuffix, RegexFlags, RegexTimeOut);
            MoreOrEqualSeparate = new Regex(NumbersDefinitions.OneNumberRangeMoreSeparateRegex, RegexFlags, RegexTimeOut);
            LessOrEqualSeparate = new Regex(NumbersDefinitions.OneNumberRangeLessSeparateRegex, RegexFlags, RegexTimeOut);
        }
    }
}
