using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Italian;

namespace Microsoft.Recognizers.Text.DateTime.Italian
{
    public class ItalianSetExtractorConfiguration : BaseOptionsConfiguration, ISetExtractorConfiguration
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_SET;

        public static readonly Regex SetUnitRegex =
            new Regex(DateTimeDefinitions.DurationUnitRegex, RegexOptions.Singleline);

        public static readonly Regex PeriodicRegex =
            new Regex(
                DateTimeDefinitions.PeriodicRegex, // TODO: Decide between adjective and adverb, i.e monthly - 'mensuel' vs 'mensuellement'
                RegexOptions.Singleline);

        public static readonly Regex EachUnitRegex = new Regex(
            DateTimeDefinitions.EachUnitRegex,
            RegexOptions.Singleline);

        public static readonly Regex EachPrefixRegex = new Regex(
            DateTimeDefinitions.EachPrefixRegex,
            RegexOptions.Singleline);

        public static readonly Regex EachDayRegex = new Regex(
            DateTimeDefinitions.EachDayRegex,
            RegexOptions.Singleline);

        public static readonly Regex SetLastRegex = new Regex(
            DateTimeDefinitions.SetLastRegex,
            RegexOptions.Singleline);

        public static readonly Regex SetWeekDayRegex =
            new Regex(DateTimeDefinitions.SetWeekDayRegex, RegexOptions.Singleline);

        public static readonly Regex SetEachRegex =
            new Regex(DateTimeDefinitions.SetEachRegex, RegexOptions.Singleline);

        public ItalianSetExtractorConfiguration(IOptionsConfiguration config)
            : base(config)
        {
            DurationExtractor = new BaseDurationExtractor(new ItalianDurationExtractorConfiguration(this));
            TimeExtractor = new BaseTimeExtractor(new ItalianTimeExtractorConfiguration(this));
            DateExtractor = new BaseDateExtractor(new ItalianDateExtractorConfiguration(this));
            DateTimeExtractor = new BaseDateTimeExtractor(new ItalianDateTimeExtractorConfiguration(this));
            DatePeriodExtractor = new BaseDatePeriodExtractor(new ItalianDatePeriodExtractorConfiguration(this));
            TimePeriodExtractor = new BaseTimePeriodExtractor(new ItalianTimePeriodExtractorConfiguration(this));
            DateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new ItalianDateTimePeriodExtractorConfiguration(this));
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
