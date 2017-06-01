using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.Number.Chinese.Extractors;
using Microsoft.Recognizers.Text.Number.Chinese.Parsers;
using Microsoft.Recognizers.Text.Number.Parsers;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Chinese.Extractors
{
    public class DateExtractorChs : IExtractor
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_DATE; // "Date";

        public static readonly Regex MonthRegex =
            new Regex(
                @"(?<month>正月|一月|二月|三月|四月|五月|六月|七月|八月|九月|十月|十一月|十二月|01月|02月|03月|04月|05月|06月|07月|08月|09月|10月|11月|12月|1月|2月|3月|4月|5月|6月|7月|8月|9月|大年)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex DayRegex =
            new Regex(
                @"(?<day>01|02|03|04|05|06|07|08|09|10|11|12|13|14|15|16|17|18|19|20|21|22|23|24|25|26|27|28|29|30|31|1|2|3|4|5|6|7|8|9)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex DayRegexInChinese =
            new Regex(
                @"(?<day>初一|三十|一日|十一日|二十一日|三十一日|二日|三日|四日|五日|六日|七日|八日|九日|十二日|十三日|十四日|十五日|十六日|十七日|十八日|十九日|二十二日|二十三日|二十四日|二十五日|二十六日|二十七日|二十八日|二十九日|一日|十一日|十日|二十一日|二十日|三十一日|三十日|二日|三日|四日|五日|六日|七日|八日|九日|十二日|十三日|十四日|十五日|十六日|十七日|十八日|十九日|二十二日|二十三日|二十四日|二十五日|二十六日|二十七日|二十八日|二十九日|十日|二十日|三十日|10日|11日|12日|13日|14日|15日|16日|17日|18日|19日|1日|20日|21日|22日|23日|24日|25日|26日|27日|28日|29日|2日|30日|31日|3日|4日|5日|6日|7日|8日|9日|一号|十一号|二十一号|三十一号|二号|三号|四号|五号|六号|七号|八号|九号|十二号|十三号|十四号|十五号|十六号|十七号|十八号|十九号|二十二号|二十三号|二十四号|二十五号|二十六号|二十七号|二十八号|二十九号|一号|十一号|十号|二十一号|二十号|三十一号|三十号|二号|三号|四号|五号|六号|七号|八号|九号|十二号|十三号|十四号|十五号|十六号|十七号|十八号|十九号|二十二号|二十三号|二十四号|二十五号|二十六号|二十七号|二十八号|二十九号|十号|二十号|三十号|10号|11号|12号|13号|14号|15号|16号|17号|18号|19号|1号|20号|21号|22号|23号|24号|25号|26号|27号|28号|29号|2号|30号|31号|3号|4号|5号|6号|7号|8号|9号)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex DayRegexNumInChinese =
            new Regex(
                @"(?<day>一|十一|二十一|三十一|二|三|四|五|六|七|八|九|十二|十三|十四|十五|十六|十七|十八|十九|二十二|二十三|二十四|二十五|二十六|二十七|二十八|二十九|一|十一|十|二十一|二十|三十一|三十|二|三|四|五|六|七|八|九|十二|十三|十四|十五|十六|十七|十八|十九|二十二|二十三|二十四|二十五|二十六|二十七|二十八|二十九|十|二十|廿|卅)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthNumRegex =
            new Regex(@"(?<month>01|02|03|04|05|06|07|08|09|10|11|12|1|2|3|4|5|6|7|8|9)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex YearRegex = new Regex(@"(?<year>19\d{2}|20\d{2}|9\d|0\d|1\d)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex ZeroToNineIntegerRegexChs = new Regex(@"[一二三四五六七八九零壹贰叁肆伍陆柒捌玖〇两千俩倆仨]",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex YearInChineseRegex =
            new Regex(string.Format(@"(?<yearchs>({0}{0}{0}{0}|{0}{0}))", ZeroToNineIntegerRegexChs),
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex WeekDayRegex =
            new Regex(@"(?<weekday>周日|周天|周一|周二|周三|周四|周五|周六|星期一|星期二|星期三|星期四|星期五|星期六|星期天|星期天)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex LunarRegex = new Regex(@"(农历|初一|正月|大年)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex ThisRegex = new Regex(string.Format(@"(这个|这一个|这|这一|本){0}", WeekDayRegex),
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex LastRegex = new Regex(string.Format(@"(上个|上一个|上|上一){0}", WeekDayRegex),
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex NextRegex = new Regex(string.Format(@"(下个|下一个|下|下一){0}", WeekDayRegex),
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex SpecialDayRegex = new Regex(@"(最近|前天|后天|昨天|明天|今天|今日|明日|昨日)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex WeekDayOfMonthRegex =
            new Regex(
                string.Format(@"((({1}|{2})的\s*)(?<cardinal>第一个|第二个|第三个|第四个|第五个|最后一个)\s*{0})", WeekDayRegex, MonthRegex,
                    MonthNumRegex), RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex ThisRe = new Regex(@"这个|这一个|这|这一|本|今",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex LastRe = new Regex(@"上个|上一个|上|上一|去",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex NextRe = new Regex(@"下个|下一个|下|下一|明",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex SpecialDate =
            new Regex(
                string.Format(@"(?<thisyear>({0}|{1}|{2})年)?(?<thismonth>({0}|{1}|{2})月)?{3}", ThisRe, LastRe, NextRe,
                    DayRegexInChinese), RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex UnitRegex = new Regex(@"(?<unit>年|个月|周|日|天)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex[] DateRegexList =
        {
            // (农历)?(2016年)?一月三日(星期三)?
            new Regex(
                string.Format(@"({4}(\s*))?((({2}|{5})年)(\s*))?{0}(\s*){1}((\s*|,|，){3})?({6}|{7})?", MonthRegex,
                    DayRegexInChinese, YearRegex, WeekDayRegex, LunarRegex, YearInChineseRegex, BeforeRegex,
                    AfterRegex), RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (2015年)?(农历)?十月初一(星期三)?
            new Regex(
                string.Format(@"((({2}|{5})年)(\s*))?({3}(\s*))?{0}(\s*){1}((\s*|,|，){4})?({6}|{7})?", MonthRegex,
                    DayRegexInChinese, YearRegex, LunarRegex, WeekDayRegex, YearInChineseRegex, BeforeRegex,
                    AfterRegex), RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (2015年)?(农历)?十月二十(星期三)?
            new Regex(
                string.Format(@"((({2}|{5})年)(\s*))?({3}(\s*))?{0}(\s*)({1}|{8})((\s*|,|，){4})?({6}|{7})?", MonthRegex,
                    DayRegexNumInChinese, YearRegex, LunarRegex, WeekDayRegex, YearInChineseRegex, BeforeRegex,
                    AfterRegex, DayRegex), RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // 7/23
            new Regex($@"{MonthNumRegex}\s*/\s*{DayRegex}((\s+|\s*,\s*){YearRegex})?",
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // 23/7
            new Regex($@"{DayRegex}\s*/\s*{MonthNumRegex}((\s+|\s*,\s*){YearRegex})?",
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // 3-23-2017
            new Regex($@"{MonthNumRegex}\s*[/\\\-]\s*{DayRegex}\s*[/\\\-]\s*{YearRegex}",
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // 23-3-2015
            new Regex(string.Format(@"\b{1}\s*[/\\\-]\s*{0}\s*[/\\\-]\s*{2}", MonthNumRegex, DayRegex, YearRegex),
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // 2015-12-23
            new Regex($@"{YearRegex}\s*[/\\\-\. ]\s*{MonthNumRegex}\s*[/\\\-\. ]\s*{DayRegex}",
                RegexOptions.IgnoreCase | RegexOptions.Singleline)
        };


        public static readonly Regex[] ImplicitDateList =
        {
            LunarRegex, SpecialDayRegex, ThisRegex, LastRegex, NextRegex,
            WeekDayRegex, WeekDayOfMonthRegex, SpecialDate
        };

        public static readonly Regex BeforeRegex = new Regex(@"之前|前",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex AfterRegex = new Regex(@"之后|后",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private static readonly IntegerExtractor _integerExtractor = new IntegerExtractor();

        private static readonly IParser _integerParser =
            AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Integer,
                new ChineseNumberParserConfiguration());

        private static readonly DurationExtractorChs _durationExtractor = new DurationExtractorChs();

        public List<ExtractResult> Extract(string text)
        {
            var tokens = new List<Token>();
            tokens.AddRange(BasicRegexMatch(text));
            tokens.AddRange(ImplicitDate(text));
            tokens.AddRange(DurationWithBeforeAndAfter(text));

            return Token.MergeAllTokens(tokens, text, ExtractorName);
        }

        public List<Token> ExtractRaw(string text)
        {
            var tokens = new List<Token>();
            tokens.AddRange(BasicRegexMatch(text));
            tokens.AddRange(ImplicitDate(text));

            return tokens;
        }

        // match basic patterns in DateRegexList
        private List<Token> BasicRegexMatch(string text)
        {
            var ret = new List<Token>();
            foreach (var regex in DateRegexList)
            {
                var matches = regex.Matches(text);
                foreach (Match match in matches)
                {
                    ret.Add(new Token(match.Index, match.Index + match.Length));
                }
            }
            return ret;
        }

        // match several other cases
        private List<Token> ImplicitDate(string text)
        {
            var ret = new List<Token>();
            foreach (var regex in ImplicitDateList)
            {
                var matches = regex.Matches(text);
                foreach (Match match in matches)
                {
                    ret.Add(new Token(match.Index, match.Index + match.Length));
                }
            }
            return ret;
        }

        // process case like "三天前" "两个月前"
        private List<Token> DurationWithBeforeAndAfter(string text)
        {
            var ret = new List<Token>();
            var duration_er = _durationExtractor.Extract(text);
            foreach (var er in duration_er)
            {
                var pos = (int)er.Start + (int)er.Length;
                if (pos < text.Length)
                {
                    var tmp = text.Substring(pos, 1);
                    if (tmp.Equals("前") || tmp.Equals("后"))
                    {
                        ret.Add(new Token(er.Start ?? 0, (er.Start + er.Length ?? 0) + 1));
                    }
                }
            }
            return ret;
        }
    }
}