using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public class SpanishMergedExtractorConfiguration : IMergedExtractorConfiguration
    {
        public static readonly Regex BeforeRegex = new Regex(@"(antes(\s+de(\s+las?)?)?)", 
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex AfterRegex = new Regex(@"(despues(\s*de(\s+las?)?)?)", 
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        //TODO: change this to Spanish if there is same requirement of split from A to B as two time points
        public static readonly Regex FromToRegex = new Regex(@"\b(from).+(to)\b.+",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public IExtractor DateExtractor { get; }

        public IExtractor TimeExtractor { get; }

        public IExtractor DateTimeExtractor { get; }

        public IExtractor DatePeriodExtractor { get; }

        public IExtractor TimePeriodExtractor { get; }

        public IExtractor DateTimePeriodExtractor { get; }

        public IExtractor DurationExtractor { get; }

        public IExtractor GetExtractor { get; }

        public IExtractor HolidayExtractor { get; }

        public SpanishMergedExtractorConfiguration()
        {
            DateExtractor = new BaseDateExtractor(new SpanishDateExtractorConfiguration());
            TimeExtractor = new BaseTimeExtractor(new SpanishTimeExtractorConfiguration());
            DateTimeExtractor = new BaseDateTimeExtractor(new SpanishDateTimeExtractorConfiguration());
            DatePeriodExtractor = new BaseDatePeriodExtractor(new SpanishDatePeriodExtractorConfiguration());
            TimePeriodExtractor = new BaseTimePeriodExtractor(new SpanishTimePeriodExtractorConfiguration());
            DateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new SpanishDateTimePeriodExtractorConfiguration());
            DurationExtractor = new BaseDurationExtractor(new SpanishDurationExtractorConfiguration());
            GetExtractor = new BaseSetExtractor(new SpanishSetExtractorConfiguration());
            HolidayExtractor = new BaseHolidayExtractor(new SpanishHolidayExtractorConfiguration());
        }

        Regex IMergedExtractorConfiguration.AfterRegex => AfterRegex;
        Regex IMergedExtractorConfiguration.BeforeRegex => BeforeRegex;
        Regex IMergedExtractorConfiguration.FromToRegex => FromToRegex;
    }
}
