using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.Chinese.Extractors;
using Microsoft.Recognizers.Text.DateTime.Parsers;

namespace Microsoft.Recognizers.Text.DateTime.Chinese.Parsers
{
    public class ChineseDateTimeParserConfiguration : IFullDateTimeParserConfiguration
    {
        public ChineseDateTimeParserConfiguration()
        {
            DateParser = new DateParser(this);
            TimeParser = new TimeParserChs(this);
            DateTimeParser = new DateTimeParserChs(this);
            DatePeriodParser = new DatePeriodParserChs(this);
            TimePeriodParser = new TimePeriodParserChs(this);
            DateTimePeriodParser = new DateTimePeriodParserChs(this);
            DurationParser = new DurationParserChs(this);
            SetParser = new SetParserChs(this);
            HolidayParser = new HolidayParserChs(this);
            UnitMap = InitUnitMap();
            UnitValueMap = InitUnitValueMap();
            SeasonMap = InitSeasonMap();
            SeasonValueMap = InitSeasonValueMap();
            CardinalMap = InitCardinalMap();
            DayOfMonth = InitDayOfMonth();
            DayOfWeek = InitDayOfWeek();
            MonthOfYear = InitMonthOfYear();
            Numbers = InitNumbers();
            DateRegexList = DateExtractorChs.DateRegexList;
            NextRegex = DateExtractorChs.NextRegex;
            ThisRegex = DateExtractorChs.ThisRegex;
            LastRegex = DateExtractorChs.LastRegex;
            StrictWeekDayRegex = DateExtractorChs.WeekDayRegex;
            WeekDayOfMonthRegex = DateExtractorChs.WeekDayOfMonthRegex;
        }
        public string Before => @"(前|之前)$";

        public string After => @"(后|之后)$";

        public string LastWeekDayToken => "最后一个";
        public string NextMonthToken => "下一个";
        public string LastMonthToken => "上一个";
        public string DatePrefix => " ";

        #region internalParsers

        public IDateTimeParser DateParser { get; }

        public IDateTimeParser TimeParser { get; }

        public IDateTimeParser DateTimeParser { get; }

        public IDateTimeParser DatePeriodParser { get; }

        public IDateTimeParser TimePeriodParser { get; }

        public IDateTimeParser DateTimePeriodParser { get; }

        public IDateTimeParser DurationParser { get; }

        public IDateTimeParser SetParser { get; }

        public IDateTimeParser HolidayParser { get; }

        #endregion

        #region Dictionaries

        public ImmutableDictionary<string, string> UnitMap { get; }

        public ImmutableDictionary<string, long> UnitValueMap { get; }

        public ImmutableDictionary<string, string> SeasonMap { get; }

        public ImmutableDictionary<string, int> SeasonValueMap { get; }

        public ImmutableDictionary<string, int> CardinalMap { get; }

        public ImmutableDictionary<string, int> DayOfMonth { get; }

        public ImmutableDictionary<string, int> DayOfWeek { get; }

        public ImmutableDictionary<string, int> MonthOfYear { get; }

        public ImmutableDictionary<string, int> Numbers { get; }

        #endregion

        #region Regexes

        public IEnumerable<Regex> DateRegexList { get; }

        public Regex NextRegex { get; }

        public Regex ThisRegex { get; }

        public Regex LastRegex { get; }

        public Regex StrictWeekDayRegex { get; }

        public Regex WeekDayOfMonthRegex { get; }

        #endregion

        private static ImmutableDictionary<string, string> InitUnitMap()
        {
            return new Dictionary<string, string>
            {
                {"年", "Y"},
                {"月", "MON"},
                {"个月", "MON"},
                {"日", "D"},
                {"周", "W"},
                {"天", "D"},
                {"小时", "H"},
                {"时", "H"},
                {"分钟", "M"},
                {"分", "M"},
                {"秒钟", "S"},
                {"秒", "S"},
                {"星期", "W"}
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
                {"春", "SP"},
                {"夏", "SU"},
                {"秋", "FA"},
                {"冬", "WI"}
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
                {"一", 1},
                {"二", 2},
                {"三", 3},
                {"四", 4},
                {"五", 5},
                {"1", 1},
                {"2", 2},
                {"3", 3},
                {"4", 4},
                {"5", 5},
                {"第一个", 1},
                {"第二个", 2},
                {"第三个", 3},
                {"第四个", 4},
                {"第五个", 5},
                {"第一", 1},
                {"第二", 2},
                {"第三", 3},
                {"第四", 4},
                {"第五", 5}
            }.ToImmutableDictionary();
        }
        private static ImmutableDictionary<string, int> InitDayOfMonth()
        {
            return new Dictionary<string, int>
            {
                {"01", 1},
                {"02", 2},
                {"03", 3},
                {"04", 4},
                {"05", 5},
                {"06", 6},
                {"07", 7},
                {"08", 8},
                {"09", 9},
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
                {"13", 13},
                {"14", 14},
                {"15", 15},
                {"16", 16},
                {"17", 17},
                {"18", 18},
                {"19", 19},
                {"20", 20},
                {"21", 21},
                {"22", 22},
                {"23", 23},
                {"24", 24},
                {"25", 25},
                {"26", 26},
                {"27", 27},
                {"28", 28},
                {"29", 29},
                {"30", 30},
                {"31", 31},
                {"1日", 1},
                {"2日", 2},
                {"3日", 3},
                {"4日", 4},
                {"5日", 5},
                {"6日", 6},
                {"7日", 7},
                {"8日", 8},
                {"9日", 9},
                {"10日", 10},
                {"11日", 11},
                {"12日", 12},
                {"13日", 13},
                {"14日", 14},
                {"15日", 15},
                {"16日", 16},
                {"17日", 17},
                {"18日", 18},
                {"19日", 19},
                {"20日", 20},
                {"21日", 21},
                {"22日", 22},
                {"23日", 23},
                {"24日", 24},
                {"25日", 25},
                {"26日", 26},
                {"27日", 27},
                {"28日", 28},
                {"29日", 29},
                {"30日", 30},
                {"31日", 31},
                {"一日", 1},
                {"十一日", 11},
                {"二十日", 20},
                {"十日", 10},
                {"二十一日", 21},
                {"三十一日", 31},
                {"二日", 2},
                {"三日", 3},
                {"四日", 4},
                {"五日", 5},
                {"六日", 6},
                {"七日", 7},
                {"八日", 8},
                {"九日", 9},
                {"十二日", 12},
                {"十三日", 13},
                {"十四日", 14},
                {"十五日", 15},
                {"十六日", 16},
                {"十七日", 17},
                {"十八日", 18},
                {"十九日", 19},
                {"二十二日", 22},
                {"二十三日", 23},
                {"二十四日", 24},
                {"二十五日", 25},
                {"二十六日", 26},
                {"二十七日", 27},
                {"二十八日", 28},
                {"二十九日", 29},
                {"三十日", 30},
                {"1号", 1},
                {"2号", 2},
                {"3号", 3},
                {"4号", 4},
                {"5号", 5},
                {"6号", 6},
                {"7号", 7},
                {"8号", 8},
                {"9号", 9},
                {"10号", 10},
                {"11号", 11},
                {"12号", 12},
                {"13号", 13},
                {"14号", 14},
                {"15号", 15},
                {"16号", 16},
                {"17号", 17},
                {"18号", 18},
                {"19号", 19},
                {"20号", 20},
                {"21号", 21},
                {"22号", 22},
                {"23号", 23},
                {"24号", 24},
                {"25号", 25},
                {"26号", 26},
                {"27号", 27},
                {"28号", 28},
                {"29号", 29},
                {"30号", 30},
                {"31号", 31},
                {"一号", 1},
                {"十一号", 11},
                {"二十号", 20},
                {"十号", 10},
                {"二十一号", 21},
                {"三十一号", 31},
                {"二号", 2},
                {"三号", 3},
                {"四号", 4},
                {"五号", 5},
                {"六号", 6},
                {"七号", 7},
                {"八号", 8},
                {"九号", 9},
                {"十二号", 12},
                {"十三号", 13},
                {"十四号", 14},
                {"十五号", 15},
                {"十六号", 16},
                {"十七号", 17},
                {"十八号", 18},
                {"十九号", 19},
                {"二十二号", 22},
                {"二十三号", 23},
                {"二十四号", 24},
                {"二十五号", 25},
                {"二十六号", 26},
                {"二十七号", 27},
                {"二十八号", 28},
                {"二十九号", 29},
                {"三十号", 30},
                {"初一", 32},
                {"三十", 30},
                {"一", 1},
                {"十一", 11},
                {"二十", 20},
                {"十", 10},
                {"二十一", 21},
                {"三十一", 31},
                {"二", 2},
                {"三", 3},
                {"四", 4},
                {"五", 5},
                {"六", 6},
                {"七", 7},
                {"八", 8},
                {"九", 9},
                {"十二", 12},
                {"十三", 13},
                {"十四", 14},
                {"十五", 15},
                {"十六", 16},
                {"十七", 17},
                {"十八", 18},
                {"十九", 19},
                {"二十二", 22},
                {"二十三", 23},
                {"二十四", 24},
                {"二十五", 25},
                {"二十六", 26},
                {"二十七", 27},
                {"二十八", 28},
                {"二十九", 29}
            }.ToImmutableDictionary();
        }
        private static ImmutableDictionary<string, int> InitDayOfWeek()
        {
            return new Dictionary<string, int>
            {
                {"星期一", 1},
                {"星期二", 2},
                {"星期三", 3},
                {"星期四", 4},
                {"星期五", 5},
                {"星期六", 6},
                {"星期天", 0},
                {"星期日", 0},
                {"周一", 1},
                {"周二", 2},
                {"周三", 3},
                {"周四", 4},
                {"周五", 5},
                {"周六", 6},
                {"周日", 0},
                {"周天", 0}
            }.ToImmutableDictionary();
        }
        private static ImmutableDictionary<string, int> InitMonthOfYear()
        {
            return new Dictionary<string, int>
            {
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
                {"09", 9},
                {"一月", 1},
                {"二月", 2},
                {"三月", 3},
                {"四月", 4},
                {"五月", 5},
                {"六月", 6},
                {"七月", 7},
                {"八月", 8},
                {"九月", 9},
                {"十月", 10},
                {"十一月", 11},
                {"十二月", 12},
                {"1月", 1},
                {"2月", 2},
                {"3月", 3},
                {"4月", 4},
                {"5月", 5},
                {"6月", 6},
                {"7月", 7},
                {"8月", 8},
                {"9月", 9},
                {"10月", 10},
                {"11月", 11},
                {"12月", 12},
                {"01月", 1},
                {"02月", 2},
                {"03月", 3},
                {"04月", 4},
                {"05月", 5},
                {"06月", 6},
                {"07月", 7},
                {"08月", 8},
                {"09月", 9},
                {"正月", 13},
                {"大年", 13}
            }.ToImmutableDictionary();
        }
        private static ImmutableDictionary<string, int> InitNumbers()
        {
            return new Dictionary<string, int>
            {
            }.ToImmutableDictionary();
        }
    }
}
