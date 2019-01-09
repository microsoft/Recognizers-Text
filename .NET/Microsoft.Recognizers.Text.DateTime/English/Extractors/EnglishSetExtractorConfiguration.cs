using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.English;

namespace Microsoft.Recognizers.Text.DateTime.English
{
    public class EnglishSetExtractorConfiguration : BaseOptionsConfiguration, ISetExtractorConfiguration
    {
        public static readonly Regex SetUnitRegex =
            new Regex(DateTimeDefinitions.DurationUnitRegex, RegexOptions.Singleline);

        public static readonly Regex PeriodicRegex =
            new Regex(DateTimeDefinitions.PeriodicRegex, RegexOptions.Singleline);

        public static readonly Regex EachUnitRegex =
            new Regex(DateTimeDefinitions.EachUnitRegex, RegexOptions.Singleline);

        public static readonly Regex EachPrefixRegex =
            new Regex(DateTimeDefinitions.EachPrefixRegex, RegexOptions.Singleline);

        public static readonly Regex SetLastRegex =
            new Regex(DateTimeDefinitions.SetLastRegex, RegexOptions.Singleline);

        public static readonly Regex EachDayRegex =
            new Regex(DateTimeDefinitions.EachDayRegex, RegexOptions.Singleline);

        public static readonly Regex SetWeekDayRegex =
            new Regex(DateTimeDefinitions.SetWeekDayRegex, RegexOptions.Singleline);

        public static readonly Regex SetEachRegex =
            new Regex(DateTimeDefinitions.SetEachRegex, RegexOptions.Singleline);

        public EnglishSetExtractorConfiguration(IOptionsConfiguration config)
            : base(config)
        {
            DurationExtractor = new BaseDurationExtractor(new EnglishDurationExtractorConfiguration(this));
            TimeExtractor = new BaseTimeExtractor(new EnglishTimeExtractorConfiguration(this));
            DateExtractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration(this));
            DateTimeExtractor = new BaseDateTimeExtractor(new EnglishDateTimeExtractorConfiguration(this));
            DatePeriodExtractor = new BaseDatePeriodExtractor(new EnglishDatePeriodExtractorConfiguration(this));
            TimePeriodExtractor = new BaseTimePeriodExtractor(new EnglishTimePeriodExtractorConfiguration(this));
            DateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new EnglishDateTimePeriodExtractorConfiguration(this));
        }

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateTimeExtractor TimeExtractor { get; }

        public IDateExtractor DateExtractor { get; }

        public IDateTimeExtractor DateTimeExtractor { get; }

        public IDateTimeExtractor DatePeriodExtractor { get; }

        public IDateTimeExtractor TimePeriodExtractor { get; }

        public IDateTimeExtractor DateTimePeriodExtractor { get; }

        Regex ISetExtractorConfiguration.LastRegex => SetLastRegex;

        Regex ISetExtractorConfiguration.EachPrefixRegex => EachPrefixRegex;

        Regex ISetExtractorConfiguration.PeriodicRegex => PeriodicRegex;

        Regex ISetExtractorConfiguration.EachUnitRegex => EachUnitRegex;

        Regex ISetExtractorConfiguration.EachDayRegex => EachDayRegex;

        Regex ISetExtractorConfiguration.BeforeEachDayRegex => null;

        Regex ISetExtractorConfiguration.SetWeekDayRegex => SetWeekDayRegex;

        Regex ISetExtractorConfiguration.SetEachRegex => SetEachRegex;
    }
}