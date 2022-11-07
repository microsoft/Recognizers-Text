// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Dutch;

namespace Microsoft.Recognizers.Text.Number.Dutch
{
    public class DutchNumberRangeParserConfiguration : INumberRangeParserConfiguration
    {

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public DutchNumberRangeParserConfiguration(INumberOptionsConfiguration config)
        {
            CultureInfo = new CultureInfo(config.Culture);

            var numConfig = new BaseNumberOptionsConfiguration(config.Culture, config.Options);

            NumberExtractor = Dutch.NumberExtractor.GetInstance(numConfig);
            OrdinalExtractor = Dutch.OrdinalExtractor.GetInstance(numConfig);

            // @TODO Change init to follow design in other languages
            NumberParser = new BaseNumberParser(new DutchNumberParserConfiguration(config));

            MoreOrEqual = new Regex(NumbersDefinitions.MoreOrEqual, RegexFlags, RegexTimeOut);
            LessOrEqual = new Regex(NumbersDefinitions.LessOrEqual, RegexFlags, RegexTimeOut);
            MoreOrEqualSuffix = new Regex(NumbersDefinitions.MoreOrEqualSuffix, RegexFlags, RegexTimeOut);
            LessOrEqualSuffix = new Regex(NumbersDefinitions.LessOrEqualSuffix, RegexFlags, RegexTimeOut);
            MoreOrEqualSeparate = new Regex(NumbersDefinitions.OneNumberRangeMoreSeparateRegex, RegexFlags, RegexTimeOut);
            LessOrEqualSeparate = new Regex(NumbersDefinitions.OneNumberRangeLessSeparateRegex, RegexFlags, RegexTimeOut);
        }

        public CultureInfo CultureInfo { get; private set; }

        public IExtractor NumberExtractor { get; private set; }

        public IExtractor OrdinalExtractor { get; private set; }

        public IParser NumberParser { get; private set; }

        public Regex MoreOrEqual { get; private set; }

        public Regex LessOrEqual { get; private set; }

        public Regex MoreOrEqualSuffix { get; private set; }

        public Regex LessOrEqualSuffix { get; private set; }

        public Regex MoreOrEqualSeparate { get; private set; }

        public Regex LessOrEqualSeparate { get; private set; }

        protected static TimeSpan RegexTimeOut => NumberRecognizer.GetTimeout(MethodBase.GetCurrentMethod().DeclaringType);
    }
}
