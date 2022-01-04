// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Swedish;

namespace Microsoft.Recognizers.Text.DateTime.Swedish
{
    public class SwedishHolidayExtractorConfiguration : BaseDateTimeOptionsConfiguration, IHolidayExtractorConfiguration
    {
        public static readonly Regex YearRegex =
            new Regex(DateTimeDefinitions.YearRegex, RegexFlags);

        public static readonly Regex H =
            new Regex(DateTimeDefinitions.HolidayRegex, RegexFlags);

        public static readonly Regex[] HolidayRegexList =
        {
            H,
        };

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public SwedishHolidayExtractorConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
        }

        public IEnumerable<Regex> HolidayRegexes => HolidayRegexList;
    }
}