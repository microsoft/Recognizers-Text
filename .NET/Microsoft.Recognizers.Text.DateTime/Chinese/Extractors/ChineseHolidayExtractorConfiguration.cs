// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Chinese;

namespace Microsoft.Recognizers.Text.DateTime.Chinese
{
    public class ChineseHolidayExtractorConfiguration : BaseDateTimeOptionsConfiguration, ICJKHolidayExtractorConfiguration
    {

        public static readonly Regex LunarHolidayRegex = new Regex(DateTimeDefinitions.LunarHolidayRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex[] HolidayRegexList =
        {
            new Regex(DateTimeDefinitions.HolidayRegexList1, RegexFlags, RegexTimeOut),
            new Regex(DateTimeDefinitions.HolidayRegexList2, RegexFlags, RegexTimeOut),
            LunarHolidayRegex,
        };

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public ChineseHolidayExtractorConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
        }

        public IEnumerable<Regex> HolidayRegexes => HolidayRegexList;
    }
}