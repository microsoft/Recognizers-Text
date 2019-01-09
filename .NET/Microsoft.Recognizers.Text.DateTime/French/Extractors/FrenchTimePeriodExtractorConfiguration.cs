using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.French;
using Microsoft.Recognizers.Text.DateTime.French.Utilities;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.French
{
    public class FrenchTimePeriodExtractorConfiguration : BaseOptionsConfiguration, ITimePeriodExtractorConfiguration
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_TIMEPERIOD; // "TimePeriod";

        public static readonly Regex TillRegex =
            new Regex(DateTimeDefinitions.TillRegex, RegexOptions.Singleline);

        public static readonly Regex HourRegex =
            new Regex(DateTimeDefinitions.HourRegex, RegexOptions.Singleline);

        public static readonly Regex PeriodHourNumRegex =
            new Regex(DateTimeDefinitions.PeriodHourNumRegex, RegexOptions.Singleline);

        public static readonly Regex PeriodDescRegex =
            new Regex(DateTimeDefinitions.PeriodDescRegex, RegexOptions.Singleline);

        public static readonly Regex PmRegex =
            new Regex(DateTimeDefinitions.PmRegex, RegexOptions.Singleline);

        public static readonly Regex AmRegex =
            new Regex(DateTimeDefinitions.AmRegex, RegexOptions.Singleline);

        public static readonly Regex PureNumFromTo =
            new Regex(DateTimeDefinitions.PureNumFromTo, RegexOptions.Singleline);

        public static readonly Regex PureNumBetweenAnd =
            new Regex(DateTimeDefinitions.PureNumBetweenAnd, RegexOptions.Singleline);

        public static readonly Regex SpecificTimeFromTo =
            new Regex(DateTimeDefinitions.SpecificTimeFromTo, RegexOptions.Singleline);

        public static readonly Regex SpecificTimeBetweenAnd =
            new Regex(DateTimeDefinitions.SpecificTimeBetweenAnd, RegexOptions.Singleline);

        public static readonly Regex PrepositionRegex =
            new Regex(DateTimeDefinitions.PrepositionRegex, RegexOptions.Singleline);

        public static readonly Regex TimeOfDayRegex =
            new Regex(DateTimeDefinitions.TimeOfDayRegex, RegexOptions.Singleline);

        public static readonly Regex SpecificTimeOfDayRegex =
            new Regex(DateTimeDefinitions.SpecificTimeOfDayRegex, RegexOptions.Singleline);

        public static readonly Regex TimeUnitRegex =
            new Regex(DateTimeDefinitions.TimeUnitRegex, RegexOptions.Singleline);

        public static readonly Regex TimeFollowedUnit =
            new Regex(DateTimeDefinitions.TimeFollowedUnit, RegexOptions.Singleline);

        public static readonly Regex TimeNumberCombinedWithUnit =
            new Regex(DateTimeDefinitions.TimeNumberCombinedWithUnit, RegexOptions.Singleline);

        public static readonly Regex GeneralEndingRegex =
            new Regex(DateTimeDefinitions.GeneralEndingRegex, RegexOptions.Singleline);

        private static readonly Regex FromRegex =
            new Regex(DateTimeDefinitions.FromRegex2, RegexOptions.Singleline);

        private static readonly Regex ConnectorAndRegex =
            new Regex(DateTimeDefinitions.ConnectorAndRegex, RegexOptions.Singleline);

        private static readonly Regex BeforeRegex =
            new Regex(DateTimeDefinitions.BeforeRegex2, RegexOptions.Singleline);

        public FrenchTimePeriodExtractorConfiguration(IOptionsConfiguration config)
            : base(config)
        {
            TokenBeforeDate = DateTimeDefinitions.TokenBeforeDate;
            SingleTimeExtractor = new BaseTimeExtractor(new FrenchTimeExtractorConfiguration(this));
            UtilityConfiguration = new FrenchDatetimeUtilityConfiguration();
            IntegerExtractor = Number.English.IntegerExtractor.GetInstance();
            TimeZoneExtractor = new BaseTimeZoneExtractor(new FrenchTimeZoneExtractorConfiguration(this));
        }

        public string TokenBeforeDate { get; }

        public IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        public IDateTimeExtractor SingleTimeExtractor { get; }

        public IDateTimeExtractor TimeZoneExtractor { get; }

        public IExtractor IntegerExtractor { get; }

        public IEnumerable<Regex> SimpleCasesRegex => new Regex[] { PureNumFromTo, PureNumBetweenAnd, PmRegex, AmRegex };

        public IEnumerable<Regex> PureNumberRegex => new Regex[] { PureNumFromTo, PureNumBetweenAnd };

        Regex ITimePeriodExtractorConfiguration.TillRegex => TillRegex;

        Regex ITimePeriodExtractorConfiguration.TimeOfDayRegex => FrenchDateTimeExtractorConfiguration.TimeOfDayRegex;

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

            var beforeMatch = BeforeRegex.Match(text);
            if (beforeMatch.Success)
            {
                index = beforeMatch.Index;
            }

            return beforeMatch.Success;
        }

        public bool IsConnectorToken(string text)
        {
            return ConnectorAndRegex.IsMatch(text);
        }
    }
}
