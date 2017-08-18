using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.Number.English;
using Microsoft.Recognizers.Resources.English;

namespace Microsoft.Recognizers.Text.DateTime.English
{
    public class EnglishDateTimePeriodExtractorConfiguration : IDateTimePeriodExtractorConfiguration
    {
        public EnglishDateTimePeriodExtractorConfiguration()
        {
            CardinalExtractor = new CardinalExtractor();
            SingleDateExtractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration());
            SingleTimeExtractor = new BaseTimeExtractor(new EnglishTimeExtractorConfiguration());
            SingleDateTimeExtractor = new BaseDateTimeExtractor(new EnglishDateTimeExtractorConfiguration());
        }
        
        private static readonly Regex[] SimpleCases = new Regex[]
        {
            EnglishTimePeriodExtractorConfiguration.PureNumFromTo,
            EnglishTimePeriodExtractorConfiguration.PureNumBetweenAnd
        };

        public IEnumerable<Regex> SimpleCasesRegex => SimpleCases;

        public Regex PrepositionRegex => EnglishTimePeriodExtractorConfiguration.PrepositionRegex;

        public Regex TillRegex => EnglishTimePeriodExtractorConfiguration.TillRegex;

        private static readonly Regex PeriodNightRegex = new Regex(DateTimeDefinitions.PeriodNightRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private static readonly Regex PeriodSpecificNightRegex = new Regex(DateTimeDefinitions.PeriodSpecificNightRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public Regex NightRegex => PeriodNightRegex;

        public Regex SpecificNightRegex => PeriodSpecificNightRegex;

        private static readonly Regex TimeUnitRegex =
            new Regex(DateTimeDefinitions.TimeUnitRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private static readonly Regex TimeFollowedUnit = new Regex(DateTimeDefinitions.TimeFollowedUnit,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex TimeNumberCombinedWithUnit = new Regex(DateTimeDefinitions.TimeNumberCombinedWithUnit,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex TimePeriodNightWithDateRegex = new Regex(DateTimeDefinitions.PeriodNightWithDateRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public Regex FollowedUnit => TimeFollowedUnit;

        Regex IDateTimePeriodExtractorConfiguration.NumberCombinedWithUnit => TimeNumberCombinedWithUnit;
        
        public Regex UnitRegex => TimeUnitRegex;

        public Regex PastRegex => EnglishDatePeriodExtractorConfiguration.PastRegex;

        public Regex FutureRegex => EnglishDatePeriodExtractorConfiguration.FutureRegex;

        public Regex WeekDayRegex => new Regex(DateTimeDefinitions.WeekDayRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        Regex IDateTimePeriodExtractorConfiguration.PeriodNightWithDateRegex => TimePeriodNightWithDateRegex;

        public IExtractor CardinalExtractor { get; }

        public IExtractor SingleDateExtractor { get; }

        public IExtractor SingleTimeExtractor { get; }

        public IExtractor SingleDateTimeExtractor { get; }

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
