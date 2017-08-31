using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.French
{
    public class FrenchMergedExtractorConfiguration : IMergedExtractorConfiguration
    {
        public static readonly Regex BeforeRegex =
            new Regex(@"\b(avant)\b", RegexOptions.IgnoreCase | RegexOptions.Singleline); // avant - 'before'

        public static readonly Regex AfterRegex =
            new Regex(@"\b(apr[eè]s)\b", RegexOptions.IgnoreCase | RegexOptions.Singleline); // ensuite/puis are for adverbs, i.e 'i ate and then i walked', so we'll use apres 

        public static readonly Regex FromToRegex =
            new Regex(@"\b(du).+(au)\b.+", RegexOptions.IgnoreCase | RegexOptions.Singleline); // 'Je vais du lundi au mecredi' - I will go from monday to weds

        public IExtractor DateExtractor { get; }

        public IExtractor TimeExtractor { get; }

        public IExtractor DateTimeExtractor { get; }

        public IExtractor DatePeriodExtractor { get; }

        public IExtractor TimePeriodExtractor { get; }

        public IExtractor DateTimePeriodExtractor { get; }

        public IExtractor DurationExtractor { get; }

        public IExtractor GetExtractor { get; }

        public IExtractor HolidayExtractor { get; }

        public FrenchMergedExtractorConfiguration()
        {
            DateExtractor = new BaseDateExtractor(new FrenchDateExtractorConfiguration());
            TimeExtractor = new BaseTimeExtractor(new FrenchTimeExtractorConfiguration());
            DateTimeExtractor = new BaseDateTimeExtractor(new FrenchDateTimeExtractorConfiguration());
            DatePeriodExtractor = new BaseDatePeriodExtractor(new FrenchDatePeriodExtractorConfiguration());
            TimePeriodExtractor = new BaseTimePeriodExtractor(new FrenchTimePeriodExtractorConfiguration());
            DateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new FrenchDateTimePeriodExtractorConfiguration());
            DurationExtractor = new BaseDurationExtractor(new FrenchDurationExtractorConfiguration());
            GetExtractor = new BaseSetExtractor(new FrenchSetExtractorConfiguration());
            HolidayExtractor = new BaseHolidayExtractor(new FrenchHolidayExtractorConfiguration());
        }

        Regex IMergedExtractorConfiguration.AfterRegex => AfterRegex;
        Regex IMergedExtractorConfiguration.BeforeRegex => BeforeRegex;
        Regex IMergedExtractorConfiguration.FromToRegex => FromToRegex;
    }
}
