using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.English;

namespace Microsoft.Recognizers.Text.DateTime.English
{
    public class EnglishDateTimePeriodParserConfiguration : BaseOptionsConfiguration, IDateTimePeriodParserConfiguration
    {
        public static readonly Regex MorningStartEndRegex =
            new Regex(DateTimeDefinitions.MorningStartEndRegex, RegexOptions.Singleline);

        public static readonly Regex AfternoonStartEndRegex =
            new Regex(DateTimeDefinitions.AfternoonStartEndRegex, RegexOptions.Singleline);

        public static readonly Regex EveningStartEndRegex =
            new Regex(DateTimeDefinitions.EveningStartEndRegex, RegexOptions.Singleline);

        public static readonly Regex NightStartEndRegex =
            new Regex(DateTimeDefinitions.NightStartEndRegex, RegexOptions.Singleline);

        public EnglishDateTimePeriodParserConfiguration(ICommonDateTimeParserConfiguration config)
            : base(config)
        {
            TokenBeforeDate = DateTimeDefinitions.TokenBeforeDate;

            DateExtractor = config.DateExtractor;
            TimeExtractor = config.TimeExtractor;
            DateTimeExtractor = config.DateTimeExtractor;
            TimePeriodExtractor = config.TimePeriodExtractor;
            CardinalExtractor = config.CardinalExtractor;
            DurationExtractor = config.DurationExtractor;
            NumberParser = config.NumberParser;
            DateParser = config.DateParser;
            TimeParser = config.TimeParser;
            TimePeriodParser = config.TimePeriodParser;
            DurationParser = config.DurationParser;
            DateTimeParser = config.DateTimeParser;

            PureNumberFromToRegex = EnglishTimePeriodExtractorConfiguration.PureNumFromTo;
            PureNumberBetweenAndRegex = EnglishTimePeriodExtractorConfiguration.PureNumBetweenAnd;
            SpecificTimeOfDayRegex = EnglishDateTimeExtractorConfiguration.SpecificTimeOfDayRegex;
            TimeOfDayRegex = EnglishDateTimeExtractorConfiguration.TimeOfDayRegex;
            PreviousPrefixRegex = EnglishDatePeriodExtractorConfiguration.PreviousPrefixRegex;
            FutureRegex = EnglishDatePeriodExtractorConfiguration.NextPrefixRegex;
            FutureSuffixRegex = EnglishDatePeriodExtractorConfiguration.FutureSuffixRegex;
            NumberCombinedWithUnitRegex = EnglishDateTimePeriodExtractorConfiguration.TimeNumberCombinedWithUnit;
            UnitRegex = EnglishTimePeriodExtractorConfiguration.TimeUnitRegex;
            PeriodTimeOfDayWithDateRegex = EnglishDateTimePeriodExtractorConfiguration.PeriodTimeOfDayWithDateRegex;
            RelativeTimeUnitRegex = EnglishDateTimePeriodExtractorConfiguration.RelativeTimeUnitRegex;
            RestOfDateTimeRegex = EnglishDateTimePeriodExtractorConfiguration.RestOfDateTimeRegex;
            AmDescRegex = EnglishDateTimePeriodExtractorConfiguration.AmDescRegex;
            PmDescRegex = EnglishDateTimePeriodExtractorConfiguration.PmDescRegex;
            WithinNextPrefixRegex = EnglishDateTimePeriodExtractorConfiguration.WithinNextPrefixRegex;
            PrefixDayRegex = EnglishDateTimePeriodExtractorConfiguration.PrefixDayRegex;
            BeforeRegex = EnglishDateTimePeriodExtractorConfiguration.BeforeRegex;
            AfterRegex = EnglishDateTimePeriodExtractorConfiguration.AfterRegex;
            UnitMap = config.UnitMap;
            Numbers = config.Numbers;
        }

        public string TokenBeforeDate { get; }

        public IDateExtractor DateExtractor { get; }

        public IDateTimeExtractor TimeExtractor { get; }

        public IDateTimeExtractor DateTimeExtractor { get; }

        public IDateTimeExtractor TimePeriodExtractor { get; }

        public IExtractor CardinalExtractor { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        public IParser NumberParser { get; }

        public IDateTimeParser DateParser { get; }

        public IDateTimeParser TimeParser { get; }

        public IDateTimeParser DateTimeParser { get; }

        public IDateTimeParser TimePeriodParser { get; }

        public IDateTimeParser DurationParser { get; }

        public Regex PureNumberFromToRegex { get; }

        public Regex PureNumberBetweenAndRegex { get; }

        public Regex SpecificTimeOfDayRegex { get; }

        public Regex TimeOfDayRegex { get; }

        public Regex PreviousPrefixRegex { get; }

        public Regex FutureRegex { get; }

        public Regex FutureSuffixRegex { get; }

        public Regex NumberCombinedWithUnitRegex { get; }

        public Regex UnitRegex { get; }

        public Regex PeriodTimeOfDayWithDateRegex { get; }

        public Regex RelativeTimeUnitRegex { get; }

        public Regex RestOfDateTimeRegex { get; }

        public Regex AmDescRegex { get; }

        public Regex PmDescRegex { get; }

        public Regex WithinNextPrefixRegex { get; }

        public Regex PrefixDayRegex { get; }

        public Regex BeforeRegex { get; }

        public Regex AfterRegex { get; }

        public IImmutableDictionary<string, string> UnitMap { get; }

        public IImmutableDictionary<string, int> Numbers { get; }

        public bool GetMatchedTimeRange(string text, out string timeStr, out int beginHour, out int endHour, out int endMin)
        {
            var trimmedText = text.Trim().ToLowerInvariant();

            beginHour = 0;
            endHour = 0;
            endMin = 0;
            if (MorningStartEndRegex.IsMatch(trimmedText))
            {
                timeStr = "TMO";
                beginHour = 8;
                endHour = Constants.HalfDayHourCount;
            }
            else if (AfternoonStartEndRegex.IsMatch(trimmedText))
            {
                timeStr = "TAF";
                beginHour = Constants.HalfDayHourCount;
                endHour = 16;
            }
            else if (EveningStartEndRegex.IsMatch(trimmedText))
            {
                timeStr = "TEV";
                beginHour = 16;
                endHour = 20;
            }
            else if (NightStartEndRegex.IsMatch(trimmedText))
            {
                timeStr = "TNI";
                beginHour = 20;
                endHour = 23;
                endMin = 59;
            }
            else
            {
                timeStr = null;
                return false;
            }

            return true;
        }

        public int GetSwiftPrefix(string text)
        {
            var trimmedText = text.Trim().ToLowerInvariant();

            var swift = 0;
            if (trimmedText.StartsWith("next"))
            {
                swift = 1;
            }
            else if (trimmedText.StartsWith("last"))
            {
                swift = -1;
            }

            return swift;
        }
    }
}
