using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.Number.Chinese;

namespace Microsoft.Recognizers.Text.DateTime.Chinese
{
    public class DatePeriodExtractorChs : IExtractor
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_DATEPERIOD; // "DatePeriod";

        public static readonly Regex TillRegex = new Regex(@"(?<till>到|至|--|-|—|——)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex DayRegex =
            new Regex(
                @"(?<day>01|02|03|04|05|06|07|08|09|10|11|12|13|14|15|16|17|18|19|20|21|22|23|24|25|26|27|28|29|30|31|1|2|3|4|5|6|7|8|9)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex DayRegexInChinese =
            new Regex(
                @"(?<day>初一|三十|一日|十一日|二十一日|三十一日|二日|三日|四日|五日|六日|七日|八日|九日|十二日|十三日|十四日|十五日|十六日|十七日|十八日|十九日|二十二日|二十三日|二十四日|二十五日|二十六日|二十七日|二十八日|二十九日|一日|十一日|十日|二十一日|二十日|三十一日|三十日|二日|三日|四日|五日|六日|七日|八日|九日|十二日|十三日|十四日|十五日|十六日|十七日|十八日|十九日|二十二日|二十三日|二十四日|二十五日|二十六日|二十七日|二十八日|二十九日|十日|二十日|三十日|10日|11日|12日|13日|14日|15日|16日|17日|18日|19日|1日|20日|21日|22日|23日|24日|25日|26日|27日|28日|29日|2日|30日|31日|3日|4日|5日|6日|7日|8日|9日|一号|十一号|二十一号|三十一号|二号|三号|四号|五号|六号|七号|八号|九号|十二号|十三号|十四号|十五号|十六号|十七号|十八号|十九号|二十二号|二十三号|二十四号|二十五号|二十六号|二十七号|二十八号|二十九号|一号|十一号|十号|二十一号|二十号|三十一号|三十号|二号|三号|四号|五号|六号|七号|八号|九号|十二号|十三号|十四号|十五号|十六号|十七号|十八号|十九号|二十二号|二十三号|二十四号|二十五号|二十六号|二十七号|二十八号|二十九号|十号|二十号|三十号|10号|11号|12号|13号|14号|15号|16号|17号|18号|19号|1号|20号|21号|22号|23号|24号|25号|26号|27号|28号|29号|2号|30号|31号|3号|4号|5号|6号|7号|8号|9号|一|十一|二十一|三十一|二|三|四|五|六|七|八|九|十二|十三|十四|十五|十六|十七|十八|十九|二十二|二十三|二十四|二十五|二十六|二十七|二十八|二十九|一|十一|十|二十一|二十|三十一|三十|二|三|四|五|六|七|八|九|十二|十三|十四|十五|十六|十七|十八|十九|二十二|二十三|二十四|二十五|二十六|二十七|二十八|二十九|十|二十|三十||廿|卅)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthNumRegex =
            new Regex(@"(?<month>01|02|03|04|05|06|07|08|09|10|11|12|1|2|3|4|5|6|7|8|9)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex ThisRegex = new Regex(@"这个|这一个|这|这一",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex LastRegex = new Regex(@"上个|上一个|上|上一",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex NextRegex = new Regex(@"下个|下一个|下|下一",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex RelativeMonthRegex =
            new Regex(string.Format(@"(?<relmonth>({0}|{1}|{2})\s*月)", ThisRegex, LastRegex, NextRegex),
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthRegex =
            new Regex
                (@"(?<month>正月|一月|二月|三月|四月|五月|六月|七月|八月|九月|十月|十一月|十二月|01月|02月|03月|04月|05月|06月|07月|08月|09月|10月|11月|12月|1月|2月|3月|4月|5月|6月|7月|8月|9月|大年)",
                    RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex YearRegex = new Regex(@"(?<year>19\d{2}|20\d{2})年?|(?<year>\d\d)年",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex StrictYearRegex = new Regex(@"(?<year>19\d{2}|20\d{2})年?",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex YearRegexInNumber = new Regex(@"(?<year>19\d{2}|20\d{2})",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex ZeroToNineIntegerRegexChs = new Regex(@"[一二三四五六七八九零壹贰叁肆伍陆柒捌玖〇两俩倆仨]",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex YearInChineseRegex =
            new Regex(string.Format(@"(?<yearchs>({0}{0}{0}{0}|{0}{0}))年", ZeroToNineIntegerRegexChs),
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthSuffixRegex =
            new Regex($@"(?<msuf>({RelativeMonthRegex}|{MonthRegex}))",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // for case "(从)?(2017年)?一月十日到十二日" 
        public static readonly Regex SimpleCasesRegex =
            new Regex(
                string.Format(@"((从)\s*)?(({3}|{4})\s*)?{2}({0}|{5})\s*{1}\s*({0}|{5})((\s+|\s*,\s*){3})?",
                    DayRegexInChinese, TillRegex, MonthSuffixRegex, YearRegex, YearInChineseRegex, DayRegex),
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex YearAndMonth =
            new Regex(string.Format(@"({0}|{1}){2}", YearInChineseRegex, YearRegex, MonthRegex),
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // 2017.12, 2017-12, 2017/12, 12/2017
        public static readonly Regex PureNumYearAndMonth =
            new Regex(string.Format(@"({0}\s*[-\.\/]\s*{1})|({1}\s*\/\s*{0})", YearRegexInNumber, MonthNumRegex),
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex OneWordPeriodRegex =
            new Regex(
                string.Format(@"(((明年|今年|去年)\s*)?{0}|({1}|{2}|{3})\s*(周末|周|月|年)|周末|今年|明年|去年|前年|后年)", MonthRegex,
                    ThisRegex, LastRegex, NextRegex),
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex WeekOfMonthRegex =
            new Regex(
                $@"(?<wom>{MonthSuffixRegex}的(?<cardinal>第一|第二|第三|第四|第五|最后一)\s*周\s*)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex UnitRegex = new Regex(@"(?<unit>年|(个)?月|周|日|天)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex FollowedUnit = new Regex($@"^\s*{UnitRegex}\b",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex NumberCombinedWithUnit =
            new Regex($@"(?<num>\d+(\.\d*)?){UnitRegex}",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex YearToYear = new Regex(
            string.Format(@"({0}|{1}){2}({0}|{1})", YearInChineseRegex, YearRegex, TillRegex),
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthToMonth = new Regex(string.Format(@"({0}){1}({0})", MonthRegex, YearRegex),
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex PastRegex = new Regex(@"(?<past>(前|上|之前))",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex FutureRegex =
            new Regex(@"(?<past>(后|下|之后))", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex SeasonRegex = new Regex(@"(?<season>春|夏|秋|冬)(天|季)?",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex SeasonWithYear =
            new Regex(
                string.Format(@"(({0}|{1}|(?<yearrel>明年|今年|去年))(的)?)?{2}", YearRegex, YearInChineseRegex, SeasonRegex),
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex QuarterRegex = new Regex(
            string.Format(@"(({0}|{1}|(?<yearrel>明年|今年|去年))(的)?)?(第(?<cardinal>1|2|3|4|一|二|三|四)季度)", YearRegex,
                YearInChineseRegex),
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private static readonly DateExtractorChs _datePointExtractor = new DateExtractorChs();
        private static readonly IntegerExtractor _integerExtractor = new IntegerExtractor();

        private static readonly Regex[] SimpleCasesRegexes =
        {
            SimpleCasesRegex,
            OneWordPeriodRegex,
            StrictYearRegex,
            YearToYear,
            YearAndMonth,
            PureNumYearAndMonth,
            YearInChineseRegex,
            WeekOfMonthRegex,
            SeasonWithYear,
            QuarterRegex
        };

        public List<ExtractResult> Extract(string text)
        {
            var tokens = new List<Token>();
            tokens.AddRange(MatchSimpleCases(text));
            tokens.AddRange(MergeTwoTimePoints(text));
            tokens.AddRange(MatchNumberWithUnit(text));

            return Token.MergeAllTokens(tokens, text, ExtractorName);
        }

        // match pattern in simple case
        private List<Token> MatchSimpleCases(string text)
        {
            var ret = new List<Token>();
            foreach (var regex in SimpleCasesRegexes)
            {
                var matches = regex.Matches(text);
                foreach (Match match in matches)
                {
                    ret.Add(new Token(match.Index, match.Index + match.Length));
                }
            }
            return ret;
        }

        // merge two date
        private List<Token> MergeTwoTimePoints(string text)
        {
            var ret = new List<Token>();
            var er = _datePointExtractor.Extract(text);
            if (er.Count <= 1)
            {
                return ret;
            }

            // merge '{TimePoint} 到 {TimePoint}'
            var idx = 0;
            while (idx < er.Count - 1)
            {
                var middleBegin = er[idx].Start + er[idx].Length ?? 0;
                var middleEnd = er[idx + 1].Start ?? 0;
                if (middleBegin >= middleEnd)
                {
                    idx++;
                    continue;
                }

                var middleStr = text.Substring(middleBegin, middleEnd - middleBegin).Trim();
                var match = TillRegex.Match(middleStr);
                if (match.Success && match.Index == 0 && match.Length == middleStr.Length)
                {
                    var periodBegin = er[idx].Start ?? 0;
                    var periodEnd = (er[idx + 1].Start ?? 0) + (er[idx + 1].Length ?? 0);

                    // handle "从"
                    var beforeStr = text.Substring(0, periodBegin).ToLowerInvariant();
                    if (beforeStr.Trim().EndsWith("从"))
                    {
                        periodBegin = beforeStr.LastIndexOf("从");
                    }

                    ret.Add(new Token(periodBegin, periodEnd));
                    idx += 2;
                    continue;
                }
                idx++;
            }

            return ret;
        }

        // extract case like "前两年" "前三个月"
        private List<Token> MatchNumberWithUnit(string text)
        {
            var ret = new List<Token>();

            var durations = new List<Token>();
            var ers = _integerExtractor.Extract(text);
            foreach (var er in ers)
            {
                var afterStr = text.Substring(er.Start + er.Length ?? 0);
                var match = FollowedUnit.Match(afterStr);
                if (match.Success && match.Index == 0)
                {
                    durations.Add(new Token(er.Start ?? 0, (er.Start + er.Length ?? 0) + match.Length));
                }
            }
            if (NumberCombinedWithUnit.IsMatch(text))
            {
                var matches = NumberCombinedWithUnit.Matches(text);
                foreach (Match match in matches)
                {
                    durations.Add(new Token(match.Index, match.Index + match.Length));
                }
            }

            foreach (var duration in durations)
            {
                var beforeStr = text.Substring(0, duration.Start).ToLowerInvariant();
                if (string.IsNullOrWhiteSpace(beforeStr))
                {
                    continue;
                }
                var match = PastRegex.Match(beforeStr);
                if (match.Success && string.IsNullOrWhiteSpace(beforeStr.Substring(match.Index + match.Length)))
                {
                    ret.Add(new Token(match.Index, duration.End));
                    continue;
                }
                match = FutureRegex.Match(beforeStr);
                if (match.Success && string.IsNullOrWhiteSpace(beforeStr.Substring(match.Index + match.Length)))
                {
                    ret.Add(new Token(match.Index, duration.End));
                }
            }

            return ret;
        }
    }
}