using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Definitions.French;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime.French
{
    public class FrenchDateParserConfiguration : IDateParserConfiguration
    {
        public string DateTokenPrefix { get; }

        public IExtractor IntegerExtractor { get; }

        public IExtractor OrdinalExtractor { get; }

        public IExtractor CardinalExtractor { get; }

        public IParser NumberParser { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateTimeParser DurationParser { get; }

        public IImmutableDictionary<string, string> UnitMap { get; }

        public IEnumerable<Regex> DateRegexes { get; }

        public Regex OnRegex { get; }

        public Regex SpecialDayRegex { get; }

        public Regex NextRegex { get; }

        public Regex ThisRegex { get; }

        public Regex LastRegex { get; }

        public Regex UnitRegex { get; }

        public Regex WeekDayRegex { get; }

        public Regex StrictWeekDay { get; }

        public Regex MonthRegex { get; }

        public Regex WeekDayOfMonthRegex { get; }

        public Regex ForTheRegex { get; }

        public Regex WeekDayAndDayOfMothRegex { get; }

        public Regex RelativeMonthRegex { get; }

        public IImmutableDictionary<string, int> DayOfMonth { get; }

        public IImmutableDictionary<string, int> DayOfWeek { get; }

        public IImmutableDictionary<string, int> MonthOfYear { get; }

        public IImmutableDictionary<string, int> CardinalMap { get; }

        public IDateTimeUtilityConfiguration UtilityConfiguration { get; }


        public FrenchDateParserConfiguration(ICommonDateTimeParserConfiguration config)
        {
            DateTokenPrefix = DateTimeDefinitions.DateTokenPrefix;
            IntegerExtractor = config.IntegerExtractor;
            OrdinalExtractor = config.OrdinalExtractor;
            CardinalExtractor = config.CardinalExtractor;
            NumberParser = config.NumberParser;
            DurationExtractor = config.DurationExtractor;
            DurationParser = config.DurationParser;
            DateRegexes = FrenchDateExtractorConfiguration.DateRegexList;
            OnRegex = FrenchDateExtractorConfiguration.OnRegex;
            SpecialDayRegex = FrenchDateExtractorConfiguration.SpecialDayRegex;
            NextRegex = FrenchDateExtractorConfiguration.NextRegex;
            ThisRegex = FrenchDateExtractorConfiguration.ThisRegex;
            LastRegex = FrenchDateExtractorConfiguration.LastRegex;
            UnitRegex = FrenchDateExtractorConfiguration.DateUnitRegex;
            WeekDayRegex = FrenchDateExtractorConfiguration.WeekDayRegex;
            StrictWeekDay = FrenchDateExtractorConfiguration.StrictWeekDay;
            MonthRegex = FrenchDateExtractorConfiguration.MonthRegex;
            WeekDayOfMonthRegex = FrenchDateExtractorConfiguration.WeekDayOfMonthRegex;
            ForTheRegex = FrenchDateExtractorConfiguration.ForTheRegex;
            WeekDayAndDayOfMothRegex = FrenchDateExtractorConfiguration.WeekDayAndDayOfMothRegex;
            RelativeMonthRegex = FrenchDateExtractorConfiguration.RelativeMonthRegex;
            DayOfMonth = config.DayOfMonth;
            DayOfWeek = config.DayOfWeek;
            MonthOfYear = config.MonthOfYear;
            CardinalMap = config.CardinalMap;
            UnitMap = config.UnitMap;
            UtilityConfiguration = config.UtilityConfiguration;
        }

        public int GetSwiftDay(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();

            var swift = 0;
            if (trimedText.Equals("aujourd'hui") || trimedText.Equals("auj")) //today
            {
                swift = 0;
            }
            else if (trimedText.Equals("demain") || trimedText.Equals("a2m1") || 
                     trimedText.Equals("lendemain") || trimedText.Equals("jour suivant"))
            {
                swift = 1;
            }
            else if (trimedText.Equals("hier")) // yesterday
            {
                swift = -1;
            }
            else if (trimedText.EndsWith("après demain") || // day after tomorrow
                     trimedText.EndsWith("après-demain"))
            {
                swift = 2;
            }
            else if (trimedText.StartsWith("avant-hier") || // day before yesterday
                     trimedText.StartsWith("avant hier"))
            {
                swift = -2;
            }
            else if (trimedText.EndsWith("dernier")) // dernier
            {
                swift = -1;
            }
            return swift;
        }

        public int GetSwiftMonth(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            var swift = 0;
            if (trimedText.EndsWith("prochaine") || trimedText.EndsWith("prochain"))
            {
                swift = 1;
            }
            else if (trimedText.Equals("dernière") || trimedText.Equals("dernières") ||
                    trimedText.Equals("derniere") || trimedText.Equals("dernieres"))
            {
                swift = -1;
            }
            return swift;
        }

        public bool IsCardinalLast(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            return (trimedText.Equals("dernière") || trimedText.Equals("dernières") ||
                    trimedText.Equals("derniere") || trimedText.Equals("dernieres"));
        }
    }
}
