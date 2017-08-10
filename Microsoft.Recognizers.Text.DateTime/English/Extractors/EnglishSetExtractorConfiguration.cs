using Microsoft.Recognizers.Resources.English;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.English
{
    public class EnglishSetExtractorConfiguration : ISetExtractorConfiguration
    {
        public static readonly Regex SetUnitRegex =
            new Regex(
                DateTimeDefinition.DurationUnitRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex PeriodicRegex = new Regex(
            DateTimeDefinition.PeriodicRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex EachUnitRegex = new Regex(
            DateTimeDefinition.EachUnitRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex EachPrefixRegex = new Regex(DateTimeDefinition.EachPrefixRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex SetLastRegex = new Regex(DateTimeDefinition.SetLastRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex EachDayRegex = new Regex(DateTimeDefinition.EachDayRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public EnglishSetExtractorConfiguration()
        {
            DurationExtractor = new BaseDurationExtractor(new EnglishDurationExtractorConfiguration());
            TimeExtractor = new BaseTimeExtractor(new EnglishTimeExtractorConfiguration());
            DateExtractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration());
            DateTimeExtractor = new BaseDateTimeExtractor(new EnglishDateTimeExtractorConfiguration());
            DatePeriodExtractor = new BaseDatePeriodExtractor(new EnglishDatePeriodExtractorConfiguration());
            TimePeriodExtractor = new BaseTimePeriodExtractor(new EnglishTimePeriodExtractorConfiguration());
            DateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new EnglishDateTimePeriodExtractorConfiguration());
        }

        public IExtractor DurationExtractor { get; }

        public IExtractor TimeExtractor { get; }

        public IExtractor DateExtractor { get; }

        public IExtractor DateTimeExtractor { get; }

        public IExtractor DatePeriodExtractor { get; }

        public IExtractor TimePeriodExtractor { get; }

        public IExtractor DateTimePeriodExtractor { get; }

        Regex ISetExtractorConfiguration.LastRegex => SetLastRegex;

        Regex ISetExtractorConfiguration.EachPrefixRegex => EachPrefixRegex;

        Regex ISetExtractorConfiguration.PeriodicRegex => PeriodicRegex;

        Regex ISetExtractorConfiguration.EachUnitRegex => EachUnitRegex;

        Regex ISetExtractorConfiguration.EachDayRegex => EachDayRegex;

        Regex ISetExtractorConfiguration.BeforeEachDayRegex => null;
    }
}