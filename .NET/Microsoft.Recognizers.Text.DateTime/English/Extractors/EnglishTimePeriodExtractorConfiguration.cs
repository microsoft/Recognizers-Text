using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.English.Utilities;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Definitions.English;

namespace Microsoft.Recognizers.Text.DateTime.English
{
    public class EnglishTimePeriodExtractorConfiguration : ITimePeriodExtractorConfiguration
    {
        public static readonly Regex TillRegex = 
            new Regex(DateTimeDefinitions.TillRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex HourRegex =
            new Regex(DateTimeDefinitions.HourRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex PeriodHourNumRegex =
            new Regex(DateTimeDefinitions.PeriodHourNumRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex PeriodDescRegex = 
            new Regex(DateTimeDefinitions.DescRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex PmRegex =
            new Regex(DateTimeDefinitions.PmRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex AmRegex = 
            new Regex(DateTimeDefinitions.AmRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex PureNumFromTo =
            new Regex(DateTimeDefinitions.PureNumFromTo, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex PureNumBetweenAnd =
            new Regex(DateTimeDefinitions.PureNumBetweenAnd, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex PrepositionRegex = 
            new Regex(DateTimeDefinitions.PrepositionRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex TimeOfDayRegex =
            new Regex(DateTimeDefinitions.TimeOfDayRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex SpecificTimeOfDayRegex =
            new Regex(DateTimeDefinitions.SpecificTimeOfDayRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex TimeUnitRegex =
            new Regex(DateTimeDefinitions.TimeUnitRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex TimeFollowedUnit = 
            new Regex(DateTimeDefinitions.TimeFollowedUnit, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex TimeNumberCombinedWithUnit =
            new Regex(DateTimeDefinitions.TimeNumberCombinedWithUnit, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public EnglishTimePeriodExtractorConfiguration()
        {
            SingleTimeExtractor = new BaseTimeExtractor(new EnglishTimeExtractorConfiguration());
            UtilityConfiguration = new EnlighDatetimeUtilityConfiguration();
        }

        public IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        public IExtractor SingleTimeExtractor { get; }

        public IEnumerable<Regex> SimpleCasesRegex => new[] { PureNumFromTo, PureNumBetweenAnd };

        Regex ITimePeriodExtractorConfiguration.TillRegex => TillRegex;

        Regex ITimePeriodExtractorConfiguration.TimeOfDayRegex => TimeOfDayRegex;

        public bool GetFromTokenIndex(string text, out int index)
        {
            index = -1;
            if (text.EndsWith("from"))
            {
                index = text.LastIndexOf("from", StringComparison.Ordinal);
                return true;
            }

            return false;
        }

        public bool GetBetweenTokenIndex(string text, out int index)
        {
            index = -1;
            if (text.EndsWith("between"))
            {
                index = text.LastIndexOf("between", StringComparison.Ordinal);
                return true;
            }

            return false;
        }

        public bool HasConnectorToken(string text)
        {
            return text.Equals("and");
        }
    }
}