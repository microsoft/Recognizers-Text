using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.English;

namespace Microsoft.Recognizers.Text.DateTime.English
{
    public class EnglishSetExtractorConfiguration : BaseOptionsConfiguration, ISetExtractorConfiguration
    {
        public static readonly Regex SetUnitRegex =
            new Regex(DateTimeDefinitions.DurationUnitRegex, RegexFlags);

        public static readonly Regex PeriodicRegex =
            new Regex(DateTimeDefinitions.PeriodicRegex, RegexFlags);

        public static readonly Regex EachUnitRegex =
            new Regex(DateTimeDefinitions.EachUnitRegex, RegexFlags);

        public static readonly Regex EachPrefixRegex =
            new Regex(DateTimeDefinitions.EachPrefixRegex, RegexFlags);

        public static readonly Regex SetLastRegex =
            new Regex(DateTimeDefinitions.SetLastRegex, RegexFlags);

        public static readonly Regex EachDayRegex =
            new Regex(DateTimeDefinitions.EachDayRegex, RegexFlags);

        public static readonly Regex SetWeekDayRegex =
            new Regex(DateTimeDefinitions.SetWeekDayRegex, RegexFlags);

        public static readonly Regex SetEachRegex =
            new Regex(DateTimeDefinitions.SetEachRegex, RegexFlags);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

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