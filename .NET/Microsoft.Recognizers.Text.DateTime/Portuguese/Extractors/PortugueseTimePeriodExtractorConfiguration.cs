using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Portuguese;
using Microsoft.Recognizers.Text.DateTime.Portuguese.Utilities;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime.Portuguese
{
    public class PortugueseTimePeriodExtractorConfiguration : BaseOptionsConfiguration, ITimePeriodExtractorConfiguration
    {
        public string TokenBeforeDate { get; }

        public static readonly string ExtractorName = Constants.SYS_DATETIME_TIMEPERIOD; //"TimePeriod";

        public static readonly Regex HourNumRegex = new Regex(DateTimeDefinitions.TimeHourNumRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        public static readonly Regex PureNumFromTo = new Regex(DateTimeDefinitions.PureNumFromTo, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        public static readonly Regex PureNumBetweenAnd = new Regex(DateTimeDefinitions.PureNumBetweenAnd, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        public static readonly Regex SpecificTimeFromTo = new Regex(DateTimeDefinitions.SpecificTimeFromTo, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        public static readonly Regex SpecificTimeBetweenAnd = new Regex(DateTimeDefinitions.SpecificTimeBetweenAnd, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        public static readonly Regex UnitRegex = new Regex(DateTimeDefinitions.TimeUnitRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        public static readonly Regex FollowedUnit = new Regex(DateTimeDefinitions.TimeFollowedUnit, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        public static readonly Regex NumberCombinedWithUnit = new Regex(DateTimeDefinitions.TimeNumberCombinedWithUnit, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly Regex FromRegex = new Regex(DateTimeDefinitions.FromRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly Regex ConnectorAndRegex = new Regex(DateTimeDefinitions.ConnectorAndRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly Regex BetweenRegex = new Regex(DateTimeDefinitions.BetweenRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        //TODO: add this according to coresponding English regex
        public static readonly Regex TimeOfDayRegex = new Regex(@"", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex GeneralEndingRegex =
            new Regex(DateTimeDefinitions.GeneralEndingRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex TillRegex = new Regex(DateTimeDefinitions.TillRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public PortugueseTimePeriodExtractorConfiguration() : base(DateTimeOptions.None)
        {
            TokenBeforeDate = DateTimeDefinitions.TokenBeforeDate;
            SingleTimeExtractor = new BaseTimeExtractor(new PortugueseTimeExtractorConfiguration());
            UtilityConfiguration = new PortugueseDatetimeUtilityConfiguration();
            IntegerExtractor = Number.English.IntegerExtractor.GetInstance();
        }

        public IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        public IDateTimeExtractor SingleTimeExtractor { get; }

        public IExtractor IntegerExtractor { get; }

        public IEnumerable<Regex> SimpleCasesRegex => new Regex[] { PureNumFromTo, PureNumBetweenAnd };

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

        public bool HasConnectorToken(string text)
        {
            return ConnectorAndRegex.IsMatch(text);
        }
    }
}
