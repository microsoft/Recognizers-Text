using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Turkish;

namespace Microsoft.Recognizers.Text.DateTime.Turkish
{
    public class TurkishTimeZoneParserConfiguration : BaseDateTimeOptionsConfiguration, ITimeZoneParserConfiguration
    {
        public static readonly string TimeZoneEndRegex = TimeZoneDefinitions.TimeZoneEndRegex;

        public static readonly Dictionary<string, int> FullToMinMapping = TimeZoneDefinitions.FullToMinMapping;

        public static readonly Regex DirectUtcRegex =
            new Regex(TimeZoneDefinitions.DirectUtcRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Dictionary<string, int> AbbrToMinMapping = TimeZoneDefinitions.AbbrToMinMapping;

        public TurkishTimeZoneParserConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
        }

        string ITimeZoneParserConfiguration.TimeZoneEndRegex => TimeZoneEndRegex;

        Dictionary<string, int> ITimeZoneParserConfiguration.FullToMinMapping => FullToMinMapping;

        Regex ITimeZoneParserConfiguration.DirectUtcRegex => DirectUtcRegex;

        Dictionary<string, int> ITimeZoneParserConfiguration.AbbrToMinMapping => AbbrToMinMapping;
    }
}
