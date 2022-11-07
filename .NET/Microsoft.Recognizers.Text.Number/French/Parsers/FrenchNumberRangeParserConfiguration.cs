// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.French;

namespace Microsoft.Recognizers.Text.Number.French
{
    public class FrenchNumberRangeParserConfiguration : INumberRangeParserConfiguration
    {

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public FrenchNumberRangeParserConfiguration(INumberOptionsConfiguration config)
        {
            CultureInfo = new CultureInfo(config.Culture);

            var numConfig = new BaseNumberOptionsConfiguration(config.Culture, config.Options);

            NumberExtractor = French.NumberExtractor.GetInstance(numConfig);
            OrdinalExtractor = French.OrdinalExtractor.GetInstance(numConfig);
            NumberParser = new BaseNumberParser(new FrenchNumberParserConfiguration(config));

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
