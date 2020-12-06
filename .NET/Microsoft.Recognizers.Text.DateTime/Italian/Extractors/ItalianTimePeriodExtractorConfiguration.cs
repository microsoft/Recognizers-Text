using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Italian;
using Microsoft.Recognizers.Text.DateTime.Italian.Utilities;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Text.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Italian
{
    public class ItalianTimePeriodExtractorConfiguration : BaseDateTimeOptionsConfiguration, ITimePeriodExtractorConfiguration
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_TIMEPERIOD; // "TimePeriod";

        public static readonly Regex TillRegex =
            RegexCache.Get(DateTimeDefinitions.RestrictedTillRegex, RegexFlags);

        public static readonly Regex FullTillRegex =
            RegexCache.Get(DateTimeDefinitions.TillRegex, RegexFlags);

        public static readonly Regex HourRegex =
            RegexCache.Get(DateTimeDefinitions.HourRegex, RegexFlags);

        public static readonly Regex PeriodHourNumRegex =
            RegexCache.Get(DateTimeDefinitions.PeriodHourNumRegex, RegexFlags);

        public static readonly Regex PeriodDescRegex =
            RegexCache.Get(DateTimeDefinitions.PeriodDescRegex, RegexFlags);

        public static readonly Regex PmRegex =
            RegexCache.Get(DateTimeDefinitions.PmRegex, RegexFlags);

        public static readonly Regex AmRegex =
            RegexCache.Get(DateTimeDefinitions.AmRegex, RegexFlags);

        public static readonly Regex PureNumFromTo =
            RegexCache.Get(DateTimeDefinitions.PureNumFromTo, RegexFlags);

        public static readonly Regex PureNumBetweenAnd =
            RegexCache.Get(DateTimeDefinitions.PureNumBetweenAnd, RegexFlags);

        public static readonly Regex SpecificTimeFromTo =
            RegexCache.Get(DateTimeDefinitions.SpecificTimeFromTo, RegexFlags);

        public static readonly Regex SpecificTimeBetweenAnd =
            RegexCache.Get(DateTimeDefinitions.SpecificTimeBetweenAnd, RegexFlags);

        public static readonly Regex PrepositionRegex =
            RegexCache.Get(DateTimeDefinitions.PrepositionRegex, RegexFlags);

        public static readonly Regex TimeOfDayRegex =
            RegexCache.Get(DateTimeDefinitions.TimeOfDayRegex, RegexFlags);

        public static readonly Regex SpecificTimeOfDayRegex =
            RegexCache.Get(DateTimeDefinitions.SpecificTimeOfDayRegex, RegexFlags);

        public static readonly Regex TimeUnitRegex =
            RegexCache.Get(DateTimeDefinitions.TimeUnitRegex, RegexFlags);

        public static readonly Regex TimeFollowedUnit =
            RegexCache.Get(DateTimeDefinitions.TimeFollowedUnit, RegexFlags);

        public static readonly Regex TimeNumberCombinedWithUnit =
            RegexCache.Get(DateTimeDefinitions.TimeNumberCombinedWithUnit, RegexFlags);

        public static readonly Regex GeneralEndingRegex =
            RegexCache.Get(DateTimeDefinitions.GeneralEndingRegex, RegexFlags);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly Regex FromRegex =
            RegexCache.Get(DateTimeDefinitions.FromRegex, RegexFlags);

        private static readonly Regex RangePrefixRegex =
            RegexCache.Get(DateTimeDefinitions.RangePrefixRegex, RegexFlags);

        private static readonly Regex ConnectorAndRegex =
            RegexCache.Get(DateTimeDefinitions.ConnectorAndRegex, RegexFlags);

        private static readonly Regex BeforeRegex =
            RegexCache.Get(DateTimeDefinitions.BeforeRegex, RegexFlags);

        private static readonly Regex RangePmRegex =
            RegexCache.Get(DateTimeDefinitions.RangePmRegex, RegexFlags);

        public ItalianTimePeriodExtractorConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            TokenBeforeDate = DateTimeDefinitions.TokenBeforeDate;
            SingleTimeExtractor = new BaseTimeExtractor(new ItalianTimeExtractorConfiguration(this));
            UtilityConfiguration = new ItalianDatetimeUtilityConfiguration();
            IntegerExtractor = Number.Italian.IntegerExtractor.GetInstance();
            TimeZoneExtractor = new BaseTimeZoneExtractor(new ItalianTimeZoneExtractorConfiguration(this));
        }

        public string TokenBeforeDate { get; }

        public IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        public IDateTimeExtractor SingleTimeExtractor { get; }

        public IExtractor IntegerExtractor { get; }

        public IDateTimeExtractor TimeZoneExtractor { get; }

        public IEnumerable<Regex> SimpleCasesRegex => new[]
        {
            PureNumFromTo, PureNumBetweenAnd, AmRegex, RangePmRegex, SpecificTimeFromTo, SpecificTimeBetweenAnd,
        };

        public IEnumerable<Regex> PureNumberRegex => new[] { PureNumFromTo, PureNumBetweenAnd };

        bool ITimePeriodExtractorConfiguration.CheckBothBeforeAfter => DateTimeDefinitions.CheckBothBeforeAfter;

        Regex ITimePeriodExtractorConfiguration.TillRegex => TillRegex;

        Regex ITimePeriodExtractorConfiguration.TimeOfDayRegex => TimeOfDayRegex;

        Regex ITimePeriodExtractorConfiguration.GeneralEndingRegex => GeneralEndingRegex;

        public static bool HasConnectorToken(string text)
        {
            return ConnectorAndRegexCache.IsMatch(text);
        }

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
            var beforeMatch = BeforeRegex.MatchEnd(text, false);
            var fromMatch = RangePrefixRegex.MatchEnd(text, false);

            if (beforeMatch.Success)
            {
                index = beforeMatch.Index;
            }
            else if (fromMatch.Success)
            {
                index = fromMatch.Index;

                return fromMatch.Success;
            }

            return beforeMatch.Success;
        }

        public bool IsConnectorToken(string text)
        {
            return ConnectorAndRegexCache.IsMatch(text) || FullTillRegex.IsExactMatch(text, false);
        }

        public List<ExtractResult> ApplyPotentialPeriodAmbiguityHotfix(string text, List<ExtractResult> timePeriodErs) => TimePeriodFunctions.ApplyPotentialPeriodAmbiguityHotfix(text, timePeriodErs);
    }
}
