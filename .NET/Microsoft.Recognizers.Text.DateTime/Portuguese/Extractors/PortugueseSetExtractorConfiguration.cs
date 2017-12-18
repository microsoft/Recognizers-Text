using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Portuguese;

namespace Microsoft.Recognizers.Text.DateTime.Portuguese
{
    public class PortugueseSetExtractorConfiguration : BaseOptionsConfiguration, ISetExtractorConfiguration
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_SET;

        public static readonly Regex PeriodicRegex = new Regex(DateTimeDefinitions.PeriodicRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        public static readonly Regex EachUnitRegex = new Regex(DateTimeDefinitions.EachUnitRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        public static readonly Regex EachPrefixRegex = new Regex(DateTimeDefinitions.EachPrefixRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        public static readonly Regex EachDayRegex = new Regex(DateTimeDefinitions.EachDayRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        public static readonly Regex BeforeEachDayRegex = new Regex(DateTimeDefinitions.BeforeEachDayRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        //TODO: write the below regex according to the corresponding one of English
        public static readonly Regex SetWeekDayRegex = new Regex($@"^[\.]", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex SetEachRegex = new Regex(DateTimeDefinitions.SetEachRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public PortugueseSetExtractorConfiguration() : base(DateTimeOptions.None)
        {
            DurationExtractor = new BaseDurationExtractor(new PortugueseDurationExtractorConfiguration());
            TimeExtractor = new BaseTimeExtractor(new PortugueseTimeExtractorConfiguration());
            DateExtractor = new BaseDateExtractor(new PortugueseDateExtractorConfiguration());
            DateTimeExtractor = new BaseDateTimeExtractor(new PortugueseDateTimeExtractorConfiguration());
            DatePeriodExtractor = new BaseDatePeriodExtractor(new PortugueseDatePeriodExtractorConfiguration());
            TimePeriodExtractor = new BaseTimePeriodExtractor(new PortugueseTimePeriodExtractorConfiguration());
            DateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new PortugueseDateTimePeriodExtractorConfiguration());
        }

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateTimeExtractor TimeExtractor { get; }

        public IDateTimeExtractor DateExtractor { get; }

        public IDateTimeExtractor DateTimeExtractor { get; }

        public IDateTimeExtractor DatePeriodExtractor { get; }

        public IDateTimeExtractor TimePeriodExtractor { get; }

        public IDateTimeExtractor DateTimePeriodExtractor { get; }

        Regex ISetExtractorConfiguration.LastRegex => PortugueseDateExtractorConfiguration.LastDateRegex;

        Regex ISetExtractorConfiguration.EachPrefixRegex => EachPrefixRegex;

        Regex ISetExtractorConfiguration.PeriodicRegex => PeriodicRegex;

        Regex ISetExtractorConfiguration.EachUnitRegex => EachUnitRegex;

        Regex ISetExtractorConfiguration.EachDayRegex => EachDayRegex;

        Regex ISetExtractorConfiguration.BeforeEachDayRegex => BeforeEachDayRegex;

        Regex ISetExtractorConfiguration.SetWeekDayRegex => SetWeekDayRegex;

        Regex ISetExtractorConfiguration.SetEachRegex => SetEachRegex;
    }
}
