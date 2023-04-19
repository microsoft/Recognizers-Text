// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Arabic;

namespace Microsoft.Recognizers.Text.Number.Arabic
{
    public class ArabicNumberRangeParserConfiguration : BaseNumberRangeParserConfiguration
    {
        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public ArabicNumberRangeParserConfiguration(INumberOptionsConfiguration config)
        {
            if (config.Culture == "ar-*")
            {
                CultureInfo = new CultureInfo("ar");
            }
            else
            {
                CultureInfo = new CultureInfo(config.Culture);
            }

            var numConfig = new BaseNumberOptionsConfiguration(config.Culture, config.Options);

            NumberExtractor = Arabic.NumberExtractor.GetInstance(numConfig);
            OrdinalExtractor = Arabic.OrdinalExtractor.GetInstance(numConfig);
            NumberParser = new BaseNumberParser(new ArabicNumberParserConfiguration(config));

            MoreOrEqual = new Regex(NumbersDefinitions.MoreOrEqual, RegexFlags, RegexTimeOut);
            LessOrEqual = new Regex(NumbersDefinitions.LessOrEqual, RegexFlags, RegexTimeOut);
            MoreOrEqualSuffix = new Regex(NumbersDefinitions.MoreOrEqualSuffix, RegexFlags, RegexTimeOut);
            LessOrEqualSuffix = new Regex(NumbersDefinitions.LessOrEqualSuffix, RegexFlags, RegexTimeOut);
            MoreOrEqualSeparate = new Regex(NumbersDefinitions.OneNumberRangeMoreSeparateRegex, RegexFlags, RegexTimeOut);
            LessOrEqualSeparate = new Regex(NumbersDefinitions.OneNumberRangeLessSeparateRegex, RegexFlags, RegexTimeOut);
        }
    }
}
