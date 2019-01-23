using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Portuguese;
using Microsoft.Recognizers.Text.DateTime.Portuguese.Utilities;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Portuguese
{
    public class PortugueseTimePeriodExtractorConfiguration : BaseOptionsConfiguration, ITimePeriodExtractorConfiguration
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_TIMEPERIOD; // "TimePeriod";

        public static readonly Regex HourNumRegex = new Regex(DateTimeDefinitions.TimeHourNumRegex, RegexOptions.Singleline);
        public static readonly Regex PureNumFromTo = new Regex(DateTimeDefinitions.PureNumFromTo, RegexOptions.Singleline);
        public static readonly Regex PureNumBetweenAnd = new Regex(DateTimeDefinitions.PureNumBetweenAnd, RegexOptions.Singleline);
        public static readonly Regex SpecificTimeFromTo = new Regex(DateTimeDefinitions.SpecificTimeFromTo, RegexOptions.Singleline);
        public static readonly Regex SpecificTimeBetweenAnd = new Regex(DateTimeDefinitions.SpecificTimeBetweenAnd, RegexOptions.Singleline);
        public static readonly Regex UnitRegex = new Regex(DateTimeDefinitions.TimeUnitRegex, RegexOptions.Singleline);
        public static readonly Regex FollowedUnit = new Regex(DateTimeDefinitions.TimeFollowedUnit, RegexOptions.Singleline);
        public static readonly Regex NumberCombinedWithUnit = new Regex(DateTimeDefinitions.TimeNumberCombinedWithUnit, RegexOptions.Singleline);

        // TODO: add this according to coresponding English regex
        public static readonly Regex TimeOfDayRegex = new Regex(string.Empty, RegexOptions.Singleline);
        public static readonly Regex GeneralEndingRegex =
            new Regex(DateTimeDefinitions.GeneralEndingRegex, RegexOptions.Singleline);

        public static readonly Regex TillRegex = new Regex(
            DateTimeDefinitions.TillRegex,
            RegexOptions.Singleline);

        private static readonly Regex FromRegex = new Regex(DateTimeDefinitions.FromRegex, RegexOptions.Singleline);
        private static readonly Regex ConnectorAndRegex = new Regex(DateTimeDefinitions.ConnectorAndRegex, RegexOptions.Singleline);
        private static readonly Regex BetweenRegex = new Regex(DateTimeDefinitions.BetweenRegex, RegexOptions.Singleline);

        public PortugueseTimePeriodExtractorConfiguration(IOptionsConfiguration config)
            : base(config)
        {
            TokenBeforeDate = DateTimeDefinitions.TokenBeforeDate;
            SingleTimeExtractor = new BaseTimeExtractor(new PortugueseTimeExtractorConfiguration(this));
            UtilityConfiguration = new PortugueseDatetimeUtilityConfiguration();
            IntegerExtractor = Number.English.IntegerExtractor.GetInstance();
            TimeZoneExtractor = new BaseTimeZoneExtractor(new PortugueseTimeZoneExtractorConfiguration(this));
        }

        public string TokenBeforeDate { get; }

        public IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        public IDateTimeExtractor SingleTimeExtractor { get; }

        public IExtractor IntegerExtractor { get; }

        public IDateTimeExtractor TimeZoneExtractor { get; }

        public IEnumerable<Regex> SimpleCasesRegex => new Regex[] { PureNumFromTo, PureNumBetweenAnd };

        public IEnumerable<Regex> PureNumberRegex => new Regex[] { PureNumFromTo, PureNumBetweenAnd };

        Regex ITimePeriodExtractorConfiguration.TillRegex => TillRegex;

        Regex ITimePeriodExtractorConfiguration.TimeOfDayRegex => PortugueseDateTimeExtractorConfiguration.TimeOfDayRegex;

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
