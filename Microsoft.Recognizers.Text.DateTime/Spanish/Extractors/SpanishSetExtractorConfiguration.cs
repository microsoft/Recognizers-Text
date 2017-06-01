using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.Extractors;

namespace Microsoft.Recognizers.Text.DateTime.Spanish.Extractors
{
    public class SpanishSetExtractorConfiguration : ISetExtractorConfiguration
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_SET;

        public static readonly Regex PeriodicRegex = new Regex(
            @"\b(?<periodic>a\s*diario|diariamente|mensualmente|semanalmente|quincenalmente|anualmente)\b",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private static readonly string EachExpression = @"cada|tod[oa]s\s*(l[oa]s)?";

        public static readonly Regex EachUnitRegex = new Regex(
            $@"(?<each>({EachExpression})\s*{SpanishDurationExtractorConfiguration.UnitRegex})", 
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex EachPrefixRegex = new Regex(
            $@"(?<each>({EachExpression})\s*$)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex EachDayRegex = new Regex(
            $@"\s*({EachExpression})\s*d[ií]as\s*\b",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex BeforeEachDayRegex = new Regex(
            $@"({EachExpression})\s*d[ií]as(\s+a\s+las?)?\s*\b",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public SpanishSetExtractorConfiguration()
        {
            DurationExtractor = new BaseDurationExtractor(new SpanishDurationExtractorConfiguration());
            TimeExtractor = new BaseTimeExtractor(new SpanishTimeExtractorConfiguration());
            DateExtractor = new BaseDateExtractor(new SpanishDateExtractorConfiguration());
            DateTimeExtractor = new BaseDateTimeExtractor(new SpanishDateTimeExtractorConfiguration());
        }

        public IExtractor DurationExtractor { get; }

        public IExtractor TimeExtractor { get; }

        public IExtractor DateExtractor { get; }

        public IExtractor DateTimeExtractor { get; }

        Regex ISetExtractorConfiguration.LastRegex => SpanishDateExtractorConfiguration.LastRegex;

        Regex ISetExtractorConfiguration.EachPrefixRegex => EachPrefixRegex;

        Regex ISetExtractorConfiguration.PeriodicRegex => PeriodicRegex;

        Regex ISetExtractorConfiguration.EachUnitRegex => EachUnitRegex;

        Regex ISetExtractorConfiguration.EachDayRegex => EachDayRegex;

        Regex ISetExtractorConfiguration.BeforeEachDayRegex => BeforeEachDayRegex;
    }
}
