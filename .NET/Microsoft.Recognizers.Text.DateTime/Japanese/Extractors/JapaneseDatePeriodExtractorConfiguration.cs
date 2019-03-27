using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Japanese;
using Microsoft.Recognizers.Text.Number.Japanese;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Japanese
{
    public class JapaneseDatePeriodExtractorConfiguration : IDateTimeExtractor
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_DATEPERIOD; // "DatePeriod";

        public static readonly Regex TillRegex = new Regex(DateTimeDefinitions.DatePeriodTillRegex, RegexOptions.Singleline);

        public static readonly Regex DayRegex = new Regex(DateTimeDefinitions.DayRegex, RegexOptions.Singleline);

        public static readonly Regex DayRegexForPeriod = new Regex(DateTimeDefinitions.DayRegexForPeriod, RegexOptions.Singleline);

        public static readonly Regex DayRegexInJapanese = new Regex(DateTimeDefinitions.DatePeriodDayRegexInJapanese, RegexOptions.Singleline);

        public static readonly Regex MonthNumRegex = new Regex(DateTimeDefinitions.MonthNumRegex, RegexOptions.Singleline);

        public static readonly Regex ThisRegex = new Regex(DateTimeDefinitions.DatePeriodThisRegex, RegexOptions.Singleline);

        public static readonly Regex LastRegex = new Regex(DateTimeDefinitions.DatePeriodLastRegex, RegexOptions.Singleline);

        public static readonly Regex NextRegex = new Regex(DateTimeDefinitions.DatePeriodNextRegex, RegexOptions.Singleline);

        public static readonly Regex RelativeMonthRegex = new Regex(DateTimeDefinitions.RelativeMonthRegex, RegexOptions.Singleline);

        public static readonly Regex MonthRegex = new Regex(DateTimeDefinitions.MonthRegex, RegexOptions.Singleline);

        public static readonly Regex YearRegex = new Regex(DateTimeDefinitions.DatePeriodYearRegex, RegexOptions.Singleline);

        public static readonly Regex StrictYearRegex = new Regex(DateTimeDefinitions.StrictYearRegex, RegexOptions.Singleline);

        public static readonly Regex YearRegexInNumber = new Regex(DateTimeDefinitions.YearRegexInNumber, RegexOptions.Singleline);

        public static readonly Regex ZeroToNineIntegerRegexJap = new Regex(DateTimeDefinitions.ZeroToNineIntegerRegexJap, RegexOptions.Singleline);

        public static readonly Regex YearInJapaneseRegex = new Regex(DateTimeDefinitions.DatePeriodYearInJapaneseRegex, RegexOptions.Singleline);

        public static readonly Regex MonthSuffixRegex = new Regex(DateTimeDefinitions.MonthSuffixRegex, RegexOptions.Singleline);

        // for case "(从)?(2017年)?一月十日到十二日"
        public static readonly Regex SimpleCasesRegex = new Regex(DateTimeDefinitions.SimpleCasesRegex, RegexOptions.Singleline);

        public static readonly Regex YearAndMonth = new Regex(DateTimeDefinitions.YearAndMonth, RegexOptions.Singleline);

        public static readonly Regex SimpleYearAndMonth = new Regex(DateTimeDefinitions.SimpleYearAndMonth, RegexOptions.Singleline);

        // 2017.12, 2017-12, 2017/12, 12/2017
        public static readonly Regex PureNumYearAndMonth = new Regex(DateTimeDefinitions.PureNumYearAndMonth, RegexOptions.Singleline);

        public static readonly Regex OneWordPeriodRegex = new Regex(DateTimeDefinitions.OneWordPeriodRegex, RegexOptions.Singleline);

        public static readonly Regex WeekOfMonthRegex = new Regex(DateTimeDefinitions.WeekOfMonthRegex, RegexOptions.Singleline);

        public static readonly Regex UnitRegex = new Regex(DateTimeDefinitions.UnitRegex, RegexOptions.Singleline);

        public static readonly Regex FollowedUnit = new Regex(DateTimeDefinitions.FollowedUnit, RegexOptions.Singleline);

        public static readonly Regex NumberCombinedWithUnit = new Regex(DateTimeDefinitions.NumberCombinedWithUnit, RegexOptions.Singleline);

        public static readonly Regex YearToYear = new Regex(DateTimeDefinitions.YearToYear, RegexOptions.Singleline);

        public static readonly Regex MonthToMonth = new Regex(DateTimeDefinitions.MonthToMonth, RegexOptions.Singleline);

        public static readonly Regex DayToDay = new Regex(DateTimeDefinitions.DayToDay, RegexOptions.Singleline);

        public static readonly Regex MonthDayRange = new Regex(DateTimeDefinitions.MonthDayRange, RegexOptions.Singleline);

        public static readonly Regex YearMonthRange = new Regex(DateTimeDefinitions.YearMonthRange, RegexOptions.Singleline);

        public static readonly Regex YearMonthDayRange = new Regex(DateTimeDefinitions.YearMonthDayRange, RegexOptions.Singleline);

        public static readonly Regex PastRegex = new Regex(DateTimeDefinitions.PastRegex, RegexOptions.Singleline);

        public static readonly Regex FutureRegex = new Regex(DateTimeDefinitions.FutureRegex, RegexOptions.Singleline);

        public static readonly Regex SeasonRegex = new Regex(DateTimeDefinitions.SeasonRegex, RegexOptions.Singleline);

        public static readonly Regex SeasonWithYear = new Regex(DateTimeDefinitions.SeasonWithYear, RegexOptions.Singleline);

        public static readonly Regex QuarterRegex = new Regex(DateTimeDefinitions.QuarterRegex, RegexOptions.Singleline);

        public static readonly Regex DecadeRegex = new Regex(DateTimeDefinitions.DecadeRegex, RegexOptions.Singleline);

        private static readonly JapaneseDateExtractorConfiguration DatePointExtractor = new JapaneseDateExtractorConfiguration();
        private static readonly IntegerExtractor IntegerExtractor = new IntegerExtractor();

        private static readonly Regex[] SimpleCasesRegexes =
        {
            SimpleCasesRegex,
            OneWordPeriodRegex,
            StrictYearRegex,
            YearToYear,
            MonthToMonth,
            DayToDay,
            MonthDayRange,
            YearMonthRange,
            MonthDayRange,
            YearMonthDayRange,
            PureNumYearAndMonth,
            YearInJapaneseRegex,
            WeekOfMonthRegex,
            SeasonWithYear,
            QuarterRegex,
            DecadeRegex,
        };

        public List<ExtractResult> Extract(string text)
        {
            return Extract(text, DateObject.Now);
        }

        public List<ExtractResult> Extract(string text, DateObject referenceTime)
        {
            var tokens = new List<Token>();
            tokens.AddRange(MatchSimpleCases(text));
            tokens.AddRange(MergeTwoTimePoints(text, referenceTime));
            tokens.AddRange(MatchNumberWithUnit(text));

            return Token.MergeAllTokens(tokens, text, ExtractorName);
        }

        // match pattern in simple case
        private static List<Token> MatchSimpleCases(string text)
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
        private static List<Token> MergeTwoTimePoints(string text, DateObject referenceTime)
        {
            var ret = new List<Token>();
            var er = DatePointExtractor.Extract(text, referenceTime);
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

                var middleStr = text.Substring(middleBegin, middleEnd - middleBegin);

                if (TillRegex.IsExactMatch(middleStr, trim: true))
                {
                    var periodBegin = er[idx].Start ?? 0;
                    var periodEnd = (er[idx + 1].Start ?? 0) + (er[idx + 1].Length ?? 0);

                    // handle "从"
                    var beforeStr = text.Substring(0, periodBegin).ToLowerInvariant();
                    if (beforeStr.Trim().EndsWith("从"))
                    {
                        periodBegin = beforeStr.LastIndexOf("从", StringComparison.Ordinal);
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
        private static List<Token> MatchNumberWithUnit(string text)
        {
            var ret = new List<Token>();

            var durations = new List<Token>();
            var ers = IntegerExtractor.Extract(text);
            foreach (var er in ers)
            {
                var afterStr = text.Substring(er.Start + er.Length ?? 0);
                var match = FollowedUnit.MatchBegin(afterStr, trim: true);

                if (match.Success)
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