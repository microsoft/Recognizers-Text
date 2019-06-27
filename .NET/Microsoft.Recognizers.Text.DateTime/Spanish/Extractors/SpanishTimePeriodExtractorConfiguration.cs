using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Spanish;
using Microsoft.Recognizers.Text.DateTime.Spanish.Utilities;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public class SpanishTimePeriodExtractorConfiguration : BaseOptionsConfiguration, ITimePeriodExtractorConfiguration
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_TIMEPERIOD; // "TimePeriod";

        public static readonly Regex HourNumRegex =
            new Regex(DateTimeDefinitions.TimeHourNumRegex, RegexFlags);

        public static readonly Regex PureNumFromTo =
            new Regex(DateTimeDefinitions.PureNumFromTo, RegexFlags);

        public static readonly Regex PureNumBetweenAnd =
            new Regex(DateTimeDefinitions.PureNumBetweenAnd, RegexFlags);

        public static readonly Regex SpecificTimeFromTo =
            new Regex(DateTimeDefinitions.SpecificTimeFromTo, RegexFlags);

        public static readonly Regex SpecificTimeBetweenAnd =
            new Regex(DateTimeDefinitions.SpecificTimeBetweenAnd, RegexFlags);

        public static readonly Regex UnitRegex =
            new Regex(DateTimeDefinitions.TimeUnitRegex, RegexFlags);

        public static readonly Regex FollowedUnit =
            new Regex(DateTimeDefinitions.TimeFollowedUnit, RegexFlags);

        public static readonly Regex NumberCombinedWithUnit =
            new Regex(DateTimeDefinitions.TimeNumberCombinedWithUnit, RegexFlags);

        // TODO: add this according to corresponding English regex
        public static readonly Regex TimeOfDayRegex =
            new Regex(string.Empty, RegexFlags);

        public static readonly Regex GeneralEndingRegex =
            new Regex(DateTimeDefinitions.GeneralEndingRegex, RegexFlags);

        public static readonly Regex TillRegex =
            new Regex(DateTimeDefinitions.TillRegex, RegexFlags);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly Regex FromRegex =
            new Regex(DateTimeDefinitions.FromRegex, RegexFlags);

        private static readonly Regex ConnectorAndRegex =
            new Regex(DateTimeDefinitions.ConnectorAndRegex, RegexFlags);

        private static readonly Regex BetweenRegex =
            new Regex(DateTimeDefinitions.BetweenRegex, RegexFlags);

        public SpanishTimePeriodExtractorConfiguration(IOptionsConfiguration config)
            : base(config)
        {
            TokenBeforeDate = DateTimeDefinitions.TokenBeforeDate;
            SingleTimeExtractor = new BaseTimeExtractor(new SpanishTimeExtractorConfiguration(this));
            UtilityConfiguration = new SpanishDatetimeUtilityConfiguration();
            IntegerExtractor = Number.English.IntegerExtractor.GetInstance();
            TimeZoneExtractor = new BaseTimeZoneExtractor(new SpanishTimeZoneExtractorConfiguration(this));
        }

        public string TokenBeforeDate { get; }

        public IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        public IDateTimeExtractor SingleTimeExtractor { get; }

        public IExtractor IntegerExtractor { get; }

        public IDateTimeExtractor TimeZoneExtractor { get; }

        public IEnumerable<Regex> SimpleCasesRegex => new Regex[] { PureNumFromTo, PureNumBetweenAnd, SpecificTimeFromTo, SpecificTimeBetweenAnd };

        public IEnumerable<Regex> PureNumberRegex => new Regex[] { PureNumFromTo, PureNumBetweenAnd };

        Regex ITimePeriodExtractorConfiguration.TillRegex => TillRegex;

        Regex ITimePeriodExtractorConfiguration.TimeOfDayRegex => SpanishDateTimeExtractorConfiguration.TimeOfDayRegex;

        Regex ITimePeriodExtractorConfiguration.GeneralEndingRegex => GeneralEndingRegex;

        public bool GetFromTokenIndex(string text, out int index)
        {
            index = -1;
            var fromMatch = FromRegex.Match(text);
            if (fromMatch.Success)
            {
                index = fromMatch.Index;
            }

            return fromMatch.Success;
        }

        public bool GetBetweenTokenIndex(string text, out int index)
        {
            index = -1;
            var match = BetweenRegex.Match(text);
            if (match.Success)
            {
                index = match.Index;
            }

            return match.Success;
        }

        public bool IsConnectorToken(string text)
        {
            return ConnectorAndRegex.IsMatch(text);
        }
    }
}
