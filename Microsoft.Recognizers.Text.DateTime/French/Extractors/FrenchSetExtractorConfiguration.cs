using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.French
{
    public class FrenchSetExtractorConfiguration : ISetExtractorConfiguration
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_SET;

        public static readonly Regex PeriodicRegex = new Regex(
            @"\b(?<periodic>quotidien|mensuel|hebdomadaire|bihebdomadaire|annuel|annuellement)\b", // TODO: Decide between adjective and adverb, i.e monthly - 'mensuel' vs 'mensuellement' 
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private static readonly string EachExpression = @"chaque|(tou[ts]?)\s*(l[ea](s)?)?";

        public static readonly Regex EachUnitRegex = new Regex(
            $@"(?<each>({EachExpression})\s*{FrenchDurationExtractorConfiguration.DurationUnitRegex})",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex EachPrefixRegex = new Regex(
            $@"(?<each>({EachExpression})\s*$)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex EachDayRegex = new Regex(
            $@"\s*({EachExpression})\s*jours\s*\b",        // the noun is 'journees' but short hand is used in convo, i.e 'thirty days' - 'trente jours'
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex BeforeEachDayRegex = new Regex(
            $@"({EachExpression})\s*jous(\s+[àa]\s)?\s*\b",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public SpanishSetExtractorConfiguration()
        {
            DurationExtractor = new BaseDurationExtractor(new FrenchDurationExtractorConfiguration());
            TimeExtractor = new BaseTimeExtractor(new FrenchTimeExtractorConfiguration());
            DateExtractor = new BaseDateExtractor(new FrenchDateExtractorConfiguration());
            DateTimeExtractor = new BaseDateTimeExtractor(new FrenchDateTimeExtractorConfiguration());
            DatePeriodExtractor = new BaseDatePeriodExtractor(new FrenchDatePeriodExtractorConfiguration());
            TimePeriodExtractor = new BaseTimePeriodExtractor(new FrenchTimePeriodExtractorConfiguration());
            DateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new FrenchDateTimePeriodExtractorConfiguration());
        }

        public IExtractor DurationExtractor { get; }

        public IExtractor TimeExtractor { get; }

        public IExtractor DateExtractor { get; }

        public IExtractor DateTimeExtractor { get; }

        public IExtractor DatePeriodExtractor { get; }

        public IExtractor TimePeriodExtractor { get; }

        public IExtractor DateTimePeriodExtractor { get; }

        Regex ISetExtractorConfiguration.LastRegex => FrenchDateExtractorConfiguration.LastDateRegex;

        Regex ISetExtractorConfiguration.EachPrefixRegex => EachPrefixRegex;

        Regex ISetExtractorConfiguration.PeriodicRegex => PeriodicRegex;

        Regex ISetExtractorConfiguration.EachUnitRegex => EachUnitRegex;

        Regex ISetExtractorConfiguration.EachDayRegex => EachDayRegex;

        Regex ISetExtractorConfiguration.BeforeEachDayRegex => BeforeEachDayRegex;
    }
}
