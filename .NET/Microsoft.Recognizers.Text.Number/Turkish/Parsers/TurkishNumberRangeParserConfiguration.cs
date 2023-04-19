// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Turkish;

namespace Microsoft.Recognizers.Text.Number.Turkish
{
    public class TurkishNumberRangeParserConfiguration : BaseNumberRangeParserConfiguration
    {

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public TurkishNumberRangeParserConfiguration(INumberOptionsConfiguration config)
        {
            CultureInfo = new CultureInfo(config.Culture);

            var numConfig = new BaseNumberOptionsConfiguration(config);

            NumberExtractor = Turkish.NumberExtractor.GetInstance(NumberMode.Default, config.Options);
            OrdinalExtractor = Turkish.OrdinalExtractor.GetInstance(numConfig);
            NumberParser = new BaseNumberParser(new TurkishNumberParserConfiguration(config));

            MoreOrEqual = new Regex(NumbersDefinitions.MoreOrEqual, RegexFlags, RegexTimeOut);
            LessOrEqual = new Regex(NumbersDefinitions.LessOrEqual, RegexFlags, RegexTimeOut);
            MoreOrEqualSuffix = new Regex(NumbersDefinitions.MoreOrEqualSuffix, RegexFlags, RegexTimeOut);
            LessOrEqualSuffix = new Regex(NumbersDefinitions.LessOrEqualSuffix, RegexFlags, RegexTimeOut);
            MoreOrEqualSeparate = new Regex(NumbersDefinitions.OneNumberRangeMoreSeparateRegex, RegexFlags, RegexTimeOut);
            LessOrEqualSeparate = new Regex(NumbersDefinitions.OneNumberRangeLessSeparateRegex, RegexFlags, RegexTimeOut);
        }
    }
}
