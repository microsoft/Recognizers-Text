using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.German;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime.German
{
    public class GermanDateTimePeriodParserConfiguration : BaseOptionsConfiguration,IDateTimePeriodParserConfiguration
    {
        public string TokenBeforeDate { get; }

        public IDateTimeExtractor DateExtractor { get; }

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

        public Regex PastRegex { get; }

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

        public GermanDateTimePeriodParserConfiguration(ICommonDateTimeParserConfiguration config) : base(config.Options)
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

            PureNumberFromToRegex = GermanTimePeriodExtractorConfiguration.PureNumFromTo;
            PureNumberBetweenAndRegex = GermanTimePeriodExtractorConfiguration.PureNumBetweenAnd;
            SpecificTimeOfDayRegex = GermanDateTimeExtractorConfiguration.SpecificTimeOfDayRegex;
            TimeOfDayRegex = GermanDateTimeExtractorConfiguration.TimeOfDayRegex;
            PastRegex = GermanDatePeriodExtractorConfiguration.PastPrefixRegex;
            FutureRegex = GermanDatePeriodExtractorConfiguration.NextPrefixRegex;
            FutureSuffixRegex = GermanDatePeriodExtractorConfiguration.FutureSuffixRegex;
            NumberCombinedWithUnitRegex = GermanDateTimePeriodExtractorConfiguration.TimeNumberCombinedWithUnit;
            UnitRegex = GermanTimePeriodExtractorConfiguration.TimeUnitRegex;
            PeriodTimeOfDayWithDateRegex = GermanDateTimePeriodExtractorConfiguration.PeriodTimeOfDayWithDateRegex;
            RelativeTimeUnitRegex = GermanDateTimePeriodExtractorConfiguration.RelativeTimeUnitRegex;
            RestOfDateTimeRegex = GermanDateTimePeriodExtractorConfiguration.RestOfDateTimeRegex;
            AmDescRegex = GermanDateTimePeriodExtractorConfiguration.AmDescRegex;
            PmDescRegex = GermanDateTimePeriodExtractorConfiguration.PmDescRegex;
            WithinNextPrefixRegex = GermanDateTimePeriodExtractorConfiguration.WithinNextPrefixRegex;
            PrefixDayRegex = GermanDateTimePeriodExtractorConfiguration.PrefixDayRegex;
            BeforeRegex = GermanDateTimePeriodExtractorConfiguration.BeforeRegex;
            AfterRegex = GermanDateTimePeriodExtractorConfiguration.AfterRegex;
            UnitMap = config.UnitMap;
            Numbers = config.Numbers;
        }

        public static readonly Regex MorningStartEndRegex = 
            new Regex(DateTimeDefinitions.MorningStartEndRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex AfternoonStartEndRegex = 
            new Regex(DateTimeDefinitions.AfternoonStartEndRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex EveningStartEndRegex = 
            new Regex(DateTimeDefinitions.EveningStartEndRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex NightStartEndRegex = 
            new Regex(DateTimeDefinitions.NightStartEndRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public bool GetMatchedTimeRange(string text, out string timeStr, out int beginHour, out int endHour, out int endMin)
        {
            var trimedText = text.Trim().ToLowerInvariant();

            beginHour = 0;
            endHour = 0;
            endMin = 0;
            if (MorningStartEndRegex.IsMatch(trimedText))
            {
                timeStr = "TMO";
                beginHour = 8;
                endHour = Constants.HalfDayHourCount;
            }
            else if (AfternoonStartEndRegex.IsMatch(trimedText))
            {
                timeStr = "TAF";
                beginHour = Constants.HalfDayHourCount;
                endHour = 16;
            }
            else if (EveningStartEndRegex.IsMatch(trimedText))
            {
                timeStr = "TEV";
                beginHour = 16;
                endHour = 20;
            }
            else if (NightStartEndRegex.IsMatch(trimedText))
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
            var trimedText = text.Trim().ToLowerInvariant();

            var swift = 0;
            if (trimedText.StartsWith("nächster") || trimedText.StartsWith("nächste") || trimedText.StartsWith("nächsten") || trimedText.StartsWith("nächstes"))
            {
                swift = 1;
            }
            else if (trimedText.StartsWith("letzter") || trimedText.StartsWith("letzte") || trimedText.StartsWith("letzten") || trimedText.StartsWith("letztes"))
            {
                swift = -1;
            }

            return swift;
        }
    }
}
