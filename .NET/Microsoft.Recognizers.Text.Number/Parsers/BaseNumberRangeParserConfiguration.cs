// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number
{
    public abstract class BaseNumberRangeParserConfiguration : INumberRangeParserConfiguration
    {
        public static TimeSpan RegexTimeOut => NumberRecognizer.GetTimeout(MethodBase.GetCurrentMethod().DeclaringType);

        public CultureInfo CultureInfo { get; set; }

        public IExtractor NumberExtractor { get; set; }

        public IExtractor OrdinalExtractor { get; set; }

        public IParser NumberParser { get; set; }

        public Regex MoreOrEqual { get; set; }

        public Regex LessOrEqual { get; set; }

        public Regex MoreOrEqualSuffix { get; set; }

        public Regex LessOrEqualSuffix { get; set; }

        public Regex MoreOrEqualSeparate { get; set; }

        public Regex LessOrEqualSeparate { get; set; }
    }
}
