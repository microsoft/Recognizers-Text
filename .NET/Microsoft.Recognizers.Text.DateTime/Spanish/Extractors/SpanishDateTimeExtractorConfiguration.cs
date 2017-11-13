using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Spanish;
using Microsoft.Recognizers.Text.DateTime.Spanish.Utilities;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public class SpanishDateTimeExtractorConfiguration : IDateTimeExtractorConfiguration
    {
        public static readonly Regex PrepositionRegex = new Regex(DateTimeDefinitions.PrepositionRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        public static readonly Regex NowRegex = new Regex(DateTimeDefinitions.NowRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        public static readonly Regex SuffixRegex = new Regex(DateTimeDefinitions.SuffixRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        //TODO: modify it according to the corresponding English regex
        public static readonly Regex TimeOfDayRegex = new Regex(DateTimeDefinitions.TimeOfDayRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        public static readonly Regex SpecificTimeOfDayRegex = new Regex(DateTimeDefinitions.SpecificTimeOfDayRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        public static readonly Regex TimeOfTodayAfterRegex = new Regex(DateTimeDefinitions.TimeOfTodayAfterRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        public static readonly Regex TimeOfTodayBeforeRegex = new Regex(DateTimeDefinitions.TimeOfTodayBeforeRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        public static readonly Regex SimpleTimeOfTodayAfterRegex = new Regex(DateTimeDefinitions.SimpleTimeOfTodayAfterRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        public static readonly Regex SimpleTimeOfTodayBeforeRegex = new Regex(DateTimeDefinitions.SimpleTimeOfTodayBeforeRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        public static readonly Regex TheEndOfRegex = new Regex(DateTimeDefinitions.TheEndOfRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        //TODO: add this for Spanish
        public static readonly Regex UnitRegex = new Regex(DateTimeDefinitions.UnitRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        public static readonly Regex ConnectorRegex = new Regex(DateTimeDefinitions.ConnectorRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public SpanishDateTimeExtractorConfiguration()
        {
            DatePointExtractor = new BaseDateExtractor(new SpanishDateExtractorConfiguration());
            TimePointExtractor = new BaseTimeExtractor(new SpanishTimeExtractorConfiguration());
            DurationExtractor = new BaseDurationExtractor(new SpanishDurationExtractorConfiguration());
            UtilityConfiguration = new SpanishDatetimeUtilityConfiguration();
        }

        public IDateTimeExtractor DatePointExtractor { get; }

        public IDateTimeExtractor TimePointExtractor { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        Regex IDateTimeExtractorConfiguration.NowRegex => NowRegex;

        Regex IDateTimeExtractorConfiguration.SuffixRegex => SuffixRegex;

        Regex IDateTimeExtractorConfiguration.TimeOfTodayAfterRegex => TimeOfTodayAfterRegex;

        Regex IDateTimeExtractorConfiguration.SimpleTimeOfTodayAfterRegex => SimpleTimeOfTodayAfterRegex;

        Regex IDateTimeExtractorConfiguration.TimeOfTodayBeforeRegex => TimeOfTodayBeforeRegex;

        Regex IDateTimeExtractorConfiguration.SimpleTimeOfTodayBeforeRegex => SimpleTimeOfTodayBeforeRegex;

        Regex IDateTimeExtractorConfiguration.TimeOfDayRegex => TimeOfDayRegex;

        Regex IDateTimeExtractorConfiguration.TheEndOfRegex => TheEndOfRegex;

        Regex IDateTimeExtractorConfiguration.UnitRegex => UnitRegex;

        public bool IsConnector(string text)
        {
            text = text.Trim();
            return (string.IsNullOrEmpty(text)
                    || PrepositionRegex.IsMatch(text)
                    || ConnectorRegex.IsMatch(text));
        }
    }
}