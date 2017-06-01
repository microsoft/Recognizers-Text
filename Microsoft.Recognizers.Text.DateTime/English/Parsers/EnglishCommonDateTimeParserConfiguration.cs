using Microsoft.Recognizers.Text.DateTime.Parsers;
using Microsoft.Recognizers.Text.DateTime.English.Extractors;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.Recognizers.Text.DateTime.Extractors;
using Microsoft.Recognizers.Text.Number.English.Extractors;
using Microsoft.Recognizers.Text.Number.English.Parsers;
using Microsoft.Recognizers.Text.Number.Parsers;

namespace Microsoft.Recognizers.Text.DateTime.English.Parsers
{
    public class EnglishCommonDateTimeParserConfiguration : BaseDateParserConfiguration
    {
        public EnglishCommonDateTimeParserConfiguration()
        {
            UnitMap = InitUnitMap();
            UnitValueMap = InitUnitValueMap();
            SeasonMap = InitSeasonMap();
            CardinalMap = InitCardinalMap();
            DayOfWeek = InitDayOfWeek();
            MonthOfYear = InitMonthOfYear();
            Numbers = InitNumbers();
            CardinalExtractor = new CardinalExtractor();
            IntegerExtractor = new IntegerExtractor();
            OrdinalExtractor = new OrdinalExtractor();
            NumberParser = new BaseNumberParser(new EnglishNumberParserConfiguration());
            DateExtractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration());
            TimeExtractor = new BaseTimeExtractor(new EnglishTimeExtractorConfiguration());
            DateTimeExtractor = new BaseDateTimeExtractor(new EnglishDateTimeExtractorConfiguration());
            DurationExtractor = new BaseDurationExtractor(new EnglishDurationExtractorConfiguration());
            DateParser = new BaseDateParser(new EnglishDateParserConfiguration(this));
            TimeParser = new TimeParser(new EnglishTimeParserConfiguration(this));
            DateTimeParser = new BaseDateTimeParser(new EnglishDateTimeParserConfiguration(this));
            DurationParser = new BaseDurationParser(new EnglishDurationParserConfiguration(this));
        }

        private static ImmutableDictionary<string, string> InitUnitMap()
        {
            return new Dictionary<string, string>
            {
                {"years", "Y"},
                {"year", "Y"},
                {"months", "MON"},
                {"month", "MON"},
                {"weeks", "W"},
                {"week", "W"},
                {"days", "D"},
                {"day", "D"},
                {"hours", "H"},
                {"hour", "H"},
                {"hrs", "H"},
                {"hr", "H"},
                {"h", "H"},
                {"minutes", "M"},
                {"minute", "M"},
                {"mins", "M"},
                {"min", "M"},
                {"seconds", "S"},
                {"second", "S"},
                {"secs", "S"},
                {"sec", "S"}
            }.ToImmutableDictionary();
        }
        private static ImmutableDictionary<string, long> InitUnitValueMap()
        {
            return new Dictionary<string, long>
            {
                {"years", 31536000},
                {"year", 31536000},
                {"months", 2592000},
                {"month", 2592000},
                {"weeks", 604800},
                {"week", 604800},
                {"days", 86400},
                {"day", 86400},
                {"hours", 3600},
                {"hour", 3600},
                {"hrs", 3600},
                {"hr", 3600},
                {"h", 3600},
                {"minutes", 60},
                {"minute", 60},
                {"mins", 60},
                {"min", 60},
                {"seconds", 1},
                {"second", 1},
                {"secs", 1},
                {"sec", 1}
            }.ToImmutableDictionary();
        }
        private static ImmutableDictionary<string, string> InitSeasonMap()
        {
            return new Dictionary<string, string>
            {
                {"spring", "SP"},
                {"summer", "SU"},
                {"fall", "FA"},
                {"autumn", "FA"},
                {"winter", "WI"}
            }.ToImmutableDictionary();
        }
        private static ImmutableDictionary<string, int> InitSeasonValueMap()
        {
            return new Dictionary<string, int>
            {
                {"SP", 3},
                {"SU", 6},
                {"FA", 9},
                {"WI", 12}
            }.ToImmutableDictionary();
        }
        private static ImmutableDictionary<string, int> InitCardinalMap()
        {
            return new Dictionary<string, int>
            {
                {"first", 1},
                {"1st", 1},
                {"second", 2},
                {"2nd", 2},
                {"third", 3},
                {"3rd", 3},
                {"fourth", 4},
                {"4th", 4},
                {"fifth", 5},
                {"5th", 5}
            }.ToImmutableDictionary();
        }

        private static ImmutableDictionary<string, int> InitDayOfWeek()
        {
            return new Dictionary<string, int>
            {
                {"monday", 1},
                {"tuesday", 2},
                {"wednesday", 3},
                {"thursday", 4},
                {"friday", 5},
                {"saturday", 6},
                {"sunday", 0},
                {"mon", 1},
                {"tue", 2},
                {"wed", 3},
                {"wedn", 3},
                {"weds", 3},
                {"thu", 4},
                {"thur", 4},
                {"thurs", 4},
                {"fri", 5},
                {"sat", 6},
                {"sun", 0}
            }.ToImmutableDictionary();
        }
        private static ImmutableDictionary<string, int> InitMonthOfYear()
        {
            return new Dictionary<string, int>
            {
                {"january", 1},
                {"february", 2},
                {"march", 3},
                {"april", 4},
                {"may", 5},
                {"june", 6},
                {"july", 7},
                {"august", 8},
                {"september", 9},
                {"october", 10},
                {"november", 11},
                {"december", 12},
                {"jan", 1},
                {"feb", 2},
                {"mar", 3},
                {"apr", 4},
                {"jun", 6},
                {"jul", 7},
                {"aug", 8},
                {"sep", 9},
                {"sept", 9},
                {"oct", 10},
                {"nov", 11},
                {"dec", 12},
                {"1", 1},
                {"2", 2},
                {"3", 3},
                {"4", 4},
                {"5", 5},
                {"6", 6},
                {"7", 7},
                {"8", 8},
                {"9", 9},
                {"10", 10},
                {"11", 11},
                {"12", 12},
                {"01", 1},
                {"02", 2},
                {"03", 3},
                {"04", 4},
                {"05", 5},
                {"06", 6},
                {"07", 7},
                {"08", 8},
                {"09", 9}
            }.ToImmutableDictionary();
        }
        private static ImmutableDictionary<string, int> InitNumbers()
        {
            return new Dictionary<string, int>
            {
                {"zero", 0},
                {"one", 1},
                {"a", 1},
                {"an", 1},
                {"two", 2},
                {"three", 3},
                {"four", 4},
                {"five", 5},
                {"six", 6},
                {"seven", 7},
                {"eight", 8},
                {"nine", 9},
                {"ten", 10},
                {"eleven", 11},
                {"twelve", 12},
                {"thirteen", 13},
                {"fourteen", 14},
                {"fifteen", 15},
                {"sixteen", 16},
                {"seventeen", 17},
                {"eighteen", 18},
                {"nineteen", 19},
                {"twenty", 20},
                {"twenty one", 21},
                {"twenty two", 22},
                {"twenty three", 23},
                {"twenty four", 24},
                {"twenty five", 25},
                {"twenty six", 26},
                {"twenty seven", 27},
                {"twenty eight", 28},
                {"twenty nine", 29},
                {"thirty", 30},
                {"thirty one", 31},
                {"thirty two", 32},
                {"thirty three", 33},
                {"thirty four", 34},
                {"thirty five", 35},
                {"thirty six", 36},
                {"thirty seven", 37},
                {"thirty eight", 38},
                {"thirty nine", 39},
                {"forty", 40},
                {"forty one", 41},
                {"forty two", 42},
                {"forty three", 43},
                {"forty four", 44},
                {"forty five", 45},
                {"forty six", 46},
                {"forty seven", 47},
                {"forty eight", 48},
                {"forty nine", 49},
                {"fifty", 50},
                {"fifty one", 51},
                {"fifty two", 52},
                {"fifty three", 53},
                {"fifty four", 54},
                {"fifty five", 55},
                {"fifty six", 56},
                {"fifty seven", 57},
                {"fifty eight", 58},
                {"fifty nine", 59},
                {"sixty", 60},
                {"sixty one", 61},
                {"sixty two", 62},
                {"sixty three", 63},
                {"sixty four", 64},
                {"sixty five", 65},
                {"sixty six", 66},
                {"sixty seven", 67},
                {"sixty eight", 68},
                {"sixty nine", 69},
                {"seventy", 70},
                {"seventy one", 71},
                {"seventy two", 72},
                {"seventy three", 73},
                {"seventy four", 74},
                {"seventy five", 75},
                {"seventy six", 76},
                {"seventy seven", 77},
                {"seventy eight", 78},
                {"seventy nine", 79},
                {"eighty", 80},
                {"eighty one", 81},
                {"eighty two", 82},
                {"eighty three", 83},
                {"eighty four", 84},
                {"eighty five", 85},
                {"eighty six", 86},
                {"eighty seven", 87},
                {"eighty eight", 88},
                {"eighty nine", 89},
                {"ninety", 90},
                {"ninety one", 91},
                {"ninety two", 92},
                {"ninety three", 93},
                {"ninety four", 94},
                {"ninety five", 95},
                {"ninety six", 96},
                {"ninety seven", 97},
                {"ninety eight", 98},
                {"ninety nine", 99},
                {"one hundred", 100}
            }.ToImmutableDictionary();
        }

        public override IImmutableDictionary<string, int> DayOfMonth
        {
            get
            {
                return base.DayOfMonth.AddRange(new Dictionary<string, int>
                {
                    { "1st", 1},
                    { "2nd", 2},
                    { "3rd", 3},
                    { "4th", 4},
                    { "5th", 5},
                    { "6th", 6},
                    { "7th", 7},
                    { "8th", 8},
                    { "9th", 9},
                    { "10th", 10},
                    { "11th", 11},
                    { "11st", 11},
                    { "12th", 12},
                    { "12nd", 12},
                    { "13th", 13},
                    { "13rd", 13},
                    { "14th", 14},
                    { "15th", 15},
                    { "16th", 16},
                    { "17th", 17},
                    { "18th", 18},
                    { "19th", 19},
                    { "20th", 20},
                    { "21st", 21},
                    { "22nd", 22},
                    { "23rd", 23},
                    { "24th", 24},
                    { "25th", 25},
                    { "26th", 26},
                    { "27th", 27},
                    { "28th", 28},
                    { "29th", 29},
                    { "30th", 30},
                    { "31st", 31}
                });
            }
        }
    }
}