using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Japanese;
using Microsoft.Recognizers.Text.Number.Japanese;
using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Japanese
{
    public class JapaneseDatePeriodExtractorConfiguration : IDateTimeExtractor
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_DATEPERIOD; // "DatePeriod";

        public static readonly Regex TillRegex = RegexCache.Get(DateTimeDefinitions.DatePeriodTillRegex, RegexFlags);

        public static readonly Regex DayRegex = RegexCache.Get(DateTimeDefinitions.DayRegex, RegexFlags);

        public static readonly Regex DayRegexForPeriod = RegexCache.Get(DateTimeDefinitions.DayRegexForPeriod, RegexFlags);

        public static readonly Regex DayRegexInJapanese = RegexCache.Get(DateTimeDefinitions.DatePeriodDayRegexInJapanese, RegexFlags);

        public static readonly Regex MonthNumRegex = RegexCache.Get(DateTimeDefinitions.MonthNumRegex, RegexFlags);

        public static readonly Regex ThisRegex = RegexCache.Get(DateTimeDefinitions.DatePeriodThisRegex, RegexFlags);

        public static readonly Regex LastRegex = RegexCache.Get(DateTimeDefinitions.DatePeriodLastRegex, RegexFlags);

        public static readonly Regex NextRegex = RegexCache.Get(DateTimeDefinitions.DatePeriodNextRegex, RegexFlags);

        public static readonly Regex RelativeMonthRegex = RegexCache.Get(DateTimeDefinitions.RelativeMonthRegex, RegexFlags);

        public static readonly Regex MonthRegex = RegexCache.Get(DateTimeDefinitions.MonthRegex, RegexFlags);

        public static readonly Regex YearRegex = RegexCache.Get(DateTimeDefinitions.YearRegex, RegexFlags);

        public static readonly Regex StrictYearRegex = RegexCache.Get(DateTimeDefinitions.StrictYearRegex, RegexFlags);

        public static readonly Regex YearRegexInNumber = RegexCache.Get(DateTimeDefinitions.YearRegexInNumber, RegexFlags);

        public static readonly Regex ZeroToNineIntegerRegexJap = RegexCache.Get(DateTimeDefinitions.ZeroToNineIntegerRegexJap, RegexFlags);

        public static readonly Regex YearInJapaneseRegex = RegexCache.Get(DateTimeDefinitions.DatePeriodYearInJapaneseRegex, RegexFlags);

        public static readonly Regex MonthSuffixRegex = RegexCache.Get(DateTimeDefinitions.MonthSuffixRegex, RegexFlags);

        // for case "(从)?(2017年)?一月十日到十二日"
        public static readonly Regex SimpleCasesRegex = RegexCache.Get(DateTimeDefinitions.SimpleCasesRegex, RegexFlags);

        public static readonly Regex YearAndMonth = RegexCache.Get(DateTimeDefinitions.YearAndMonth, RegexFlags);

        public static readonly Regex SimpleYearAndMonth = RegexCache.Get(DateTimeDefinitions.SimpleYearAndMonth, RegexFlags);

        // 2017.12, 2017-12, 2017/12, 12/2017
        public static readonly Regex PureNumYearAndMonth = RegexCache.Get(DateTimeDefinitions.PureNumYearAndMonth, RegexFlags);

        public static readonly Regex OneWordPeriodRegex = RegexCache.Get(DateTimeDefinitions.OneWordPeriodRegex, RegexFlags);

        public static readonly Regex WeekOfMonthRegex = RegexCache.Get(DateTimeDefinitions.WeekOfMonthRegex, RegexFlags);

        public static readonly Regex UnitRegex = RegexCache.Get(DateTimeDefinitions.UnitRegex, RegexFlags);

        public static readonly Regex FollowedUnit = RegexCache.Get(DateTimeDefinitions.FollowedUnit, RegexFlags);

        public static readonly Regex NumberCombinedWithUnit = RegexCache.Get(DateTimeDefinitions.NumberCombinedWithUnit, RegexFlags);

        public static readonly Regex YearToYear = RegexCache.Get(DateTimeDefinitions.YearToYear, RegexFlags);

        public static readonly Regex MonthToMonth = RegexCache.Get(DateTimeDefinitions.MonthToMonth, RegexFlags);

        public static readonly Regex DayToDay = RegexCache.Get(DateTimeDefinitions.DayToDay, RegexFlags);

        public static readonly Regex MonthDayRange = RegexCache.Get(DateTimeDefinitions.MonthDayRange, RegexFlags);

        public static readonly Regex YearMonthRange = RegexCache.Get(DateTimeDefinitions.YearMonthRange, RegexFlags);

        public static readonly Regex YearMonthDayRange = RegexCache.Get(DateTimeDefinitions.YearMonthDayRange, RegexFlags);

        public static readonly Regex PastRegex = RegexCache.Get(DateTimeDefinitions.PastRegex, RegexFlags);

        public static readonly Regex FutureRegex = RegexCache.Get(DateTimeDefinitions.FutureRegex, RegexFlags);

        public static readonly Regex SeasonRegex = RegexCache.Get(DateTimeDefinitions.SeasonRegex, RegexFlags);

        public static readonly Regex SeasonWithYear = RegexCache.Get(DateTimeDefinitions.SeasonWithYear, RegexFlags);

        public static readonly Regex QuarterRegex = RegexCache.Get(DateTimeDefinitions.QuarterRegex, RegexFlags);

        public static readonly Regex DecadeRegex = RegexCache.Get(DateTimeDefinitions.DecadeRegex, RegexFlags);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

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

                    // @TODO move hardcoded values to resources file

                    // handle "从"
                    var beforeStr = text.Substring(0, periodBegin);
                    if (beforeStr.Trim().EndsWith("从", StringComparison.Ordinal))
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
                var beforeStr = text.Substring(0, duration.Start);
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