using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Spanish;
using Microsoft.Recognizers.Text.DateTime.Spanish.Utilities;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public class SpanishDateTimeExtractorConfiguration : BaseDateTimeOptionsConfiguration, IDateTimeExtractorConfiguration
    {
        public static readonly Regex PrepositionRegex =
            new Regex(DateTimeDefinitions.PrepositionRegex, RegexFlags);

        public static readonly Regex NowRegex =
            new Regex(DateTimeDefinitions.NowRegex, RegexFlags);

        public static readonly Regex SuffixRegex =
            new Regex(DateTimeDefinitions.SuffixRegex, RegexFlags);

        // TODO: modify it according to the corresponding English regex
        public static readonly Regex TimeOfDayRegex =
            new Regex(DateTimeDefinitions.TimeOfDayRegex, RegexFlags);

        public static readonly Regex SpecificTimeOfDayRegex =
            new Regex(DateTimeDefinitions.SpecificTimeOfDayRegex, RegexFlags);

        public static readonly Regex TimeOfTodayAfterRegex =
            new Regex(DateTimeDefinitions.TimeOfTodayAfterRegex, RegexFlags);

        public static readonly Regex TimeOfTodayBeforeRegex =
            new Regex(DateTimeDefinitions.TimeOfTodayBeforeRegex, RegexFlags);

        public static readonly Regex SimpleTimeOfTodayAfterRegex =
            new Regex(DateTimeDefinitions.SimpleTimeOfTodayAfterRegex, RegexFlags);

        public static readonly Regex SimpleTimeOfTodayBeforeRegex =
            new Regex(DateTimeDefinitions.SimpleTimeOfTodayBeforeRegex, RegexFlags);

        public static readonly Regex SpecificEndOfRegex =
            new Regex(DateTimeDefinitions.SpecificEndOfRegex, RegexFlags);

        public static readonly Regex UnspecificEndOfRegex =
            new Regex(DateTimeDefinitions.UnspecificEndOfRegex, RegexFlags);

        public static readonly Regex YearRegex =
            new Regex(DateTimeDefinitions.YearRegex, RegexFlags);

        public static readonly Regex YearSuffix =
            new Regex(DateTimeDefinitions.YearSuffix, RegexFlags);

        // TODO: add this for Spanish
        public static readonly Regex UnitRegex =
            new Regex(DateTimeDefinitions.UnitRegex, RegexFlags);

        public static readonly Regex ConnectorRegex =
            new Regex(DateTimeDefinitions.ConnectorRegex, RegexFlags);

        public static readonly Regex NumberAsTimeRegex =
            new Regex(DateTimeDefinitions.NumberAsTimeRegex, RegexFlags);

        public static readonly Regex DateNumberConnectorRegex =
            new Regex(DateTimeDefinitions.DateNumberConnectorRegex, RegexFlags);

        public static readonly Regex SuffixAfterRegex =
            new Regex(DateTimeDefinitions.SuffixAfterRegex, RegexFlags);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public SpanishDateTimeExtractorConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            var numOptions = NumberOptions.None;
            if ((config.Options & DateTimeOptions.NoProtoCache) != 0)
            {
                numOptions = NumberOptions.NoProtoCache;
            }

            var numConfig = new BaseNumberOptionsConfiguration(config.Culture, numOptions);

            IntegerExtractor = Number.Spanish.IntegerExtractor.GetInstance(numConfig);

            DatePointExtractor = new BaseDateExtractor(new SpanishDateExtractorConfiguration(this));
            TimePointExtractor = new BaseTimeExtractor(new SpanishTimeExtractorConfiguration(this));
            DurationExtractor = new BaseDurationExtractor(new SpanishDurationExtractorConfiguration(this));
            UtilityConfiguration = new SpanishDatetimeUtilityConfiguration();
        }

        public IExtractor IntegerExtractor { get; }

        public IDateExtractor DatePointExtractor { get; }

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

        Regex IDateTimeExtractorConfiguration.SpecificEndOfRegex => SpecificEndOfRegex;

        Regex IDateTimeExtractorConfiguration.UnspecificEndOfRegex => UnspecificEndOfRegex;

        Regex IDateTimeExtractorConfiguration.UnitRegex => UnitRegex;

        Regex IDateTimeExtractorConfiguration.NumberAsTimeRegex => NumberAsTimeRegex;

        Regex IDateTimeExtractorConfiguration.DateNumberConnectorRegex => DateNumberConnectorRegex;

        Regex IDateTimeExtractorConfiguration.YearRegex => YearRegex;

        Regex IDateTimeExtractorConfiguration.YearSuffix => YearSuffix;

        Regex IDateTimeExtractorConfiguration.SuffixAfterRegex => SuffixAfterRegex;

        public bool IsConnector(string text)
        {
            text = text.Trim();
            return string.IsNullOrEmpty(text) ||
                   PrepositionRegex.IsMatch(text) ||
                   ConnectorRegex.IsMatch(text);
        }
    }
}