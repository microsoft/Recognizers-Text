using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Chinese;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Chinese;
using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Chinese
{
    public class ChineseDatePeriodExtractorConfiguration : IDateTimeExtractor
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_DATEPERIOD; // "DatePeriod";

        public static readonly Regex TillRegex = new Regex(DateTimeDefinitions.DatePeriodTillRegex, RegexFlags);

        public static readonly Regex DayRegex = new Regex(DateTimeDefinitions.DayRegex, RegexFlags);

        public static readonly Regex DayRegexInChinese = new Regex(DateTimeDefinitions.DatePeriodDayRegexInChinese, RegexFlags);

        public static readonly Regex MonthNumRegex = new Regex(DateTimeDefinitions.MonthNumRegex, RegexFlags);

        public static readonly Regex ThisRegex = new Regex(DateTimeDefinitions.DatePeriodThisRegex, RegexFlags);

        public static readonly Regex LastRegex = new Regex(DateTimeDefinitions.DatePeriodLastRegex, RegexFlags);

        public static readonly Regex NextRegex = new Regex(DateTimeDefinitions.DatePeriodNextRegex, RegexFlags);

        public static readonly Regex RelativeMonthRegex = new Regex(DateTimeDefinitions.RelativeMonthRegex, RegexFlags);

        public static readonly Regex MonthRegex = new Regex(DateTimeDefinitions.MonthRegex, RegexFlags);

        public static readonly Regex YearRegex = new Regex(DateTimeDefinitions.YearRegex, RegexFlags);

        public static readonly Regex StrictYearRegex = new Regex(DateTimeDefinitions.StrictYearRegex, RegexFlags);

        public static readonly Regex YearRegexInNumber = new Regex(DateTimeDefinitions.YearRegexInNumber, RegexFlags);

        public static readonly Regex ZeroToNineIntegerRegexChs = new Regex(DateTimeDefinitions.ZeroToNineIntegerRegexChs, RegexFlags);

        public static readonly Regex YearInChineseRegex = new Regex(DateTimeDefinitions.DatePeriodYearInChineseRegex, RegexFlags);

        public static readonly Regex MonthSuffixRegex = new Regex(DateTimeDefinitions.MonthSuffixRegex, RegexFlags);

        // for case "(从)?(2017年)?一月十日到十二日"
        public static readonly Regex SimpleCasesRegex = new Regex(DateTimeDefinitions.SimpleCasesRegex, RegexFlags);

        public static readonly Regex YearAndMonth = new Regex(DateTimeDefinitions.YearAndMonth, RegexFlags);

        // 2017.12, 2017-12, 2017/12, 12/2017
        public static readonly Regex PureNumYearAndMonth = new Regex(DateTimeDefinitions.PureNumYearAndMonth, RegexFlags);

        public static readonly Regex OneWordPeriodRegex = new Regex(DateTimeDefinitions.OneWordPeriodRegex, RegexFlags);

        public static readonly Regex WeekOfMonthRegex = new Regex(DateTimeDefinitions.WeekOfMonthRegex, RegexFlags);

        public static readonly Regex UnitRegex = new Regex(DateTimeDefinitions.UnitRegex, RegexFlags);

        public static readonly Regex FollowedUnit = new Regex(DateTimeDefinitions.FollowedUnit, RegexFlags);

        public static readonly Regex NumberCombinedWithUnit = new Regex(DateTimeDefinitions.NumberCombinedWithUnit, RegexFlags);

        public static readonly Regex YearToYear = new Regex(DateTimeDefinitions.YearToYear, RegexFlags);

        public static readonly Regex YearToYearSuffixRequired = new Regex(DateTimeDefinitions.YearToYearSuffixRequired, RegexFlags);

        public static readonly Regex MonthToMonth = new Regex(DateTimeDefinitions.MonthToMonth, RegexFlags);

        public static readonly Regex MonthToMonthSuffixRequired = new Regex(DateTimeDefinitions.MonthToMonthSuffixRequired, RegexFlags);

        public static readonly Regex PastRegex = new Regex(DateTimeDefinitions.PastRegex, RegexFlags);

        public static readonly Regex FutureRegex = new Regex(DateTimeDefinitions.FutureRegex, RegexFlags);

        public static readonly Regex SeasonRegex = new Regex(DateTimeDefinitions.SeasonRegex, RegexFlags);

        public static readonly Regex SeasonWithYear = new Regex(DateTimeDefinitions.SeasonWithYear, RegexFlags);

        public static readonly Regex QuarterRegex = new Regex(DateTimeDefinitions.QuarterRegex, RegexFlags);

        public static readonly Regex DecadeRegex = new Regex(DateTimeDefinitions.DecadeRegex, RegexFlags);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly ChineseDateExtractorConfiguration DatePointExtractor = new ChineseDateExtractorConfiguration();

        private static readonly Regex[] SimpleCasesRegexes =
        {
            SimpleCasesRegex,
            OneWordPeriodRegex,
            StrictYearRegex,
            YearToYear,
            YearToYearSuffixRequired,
            MonthToMonth,
            MonthToMonthSuffixRequired,
            YearAndMonth,
            PureNumYearAndMonth,
            YearInChineseRegex,
            WeekOfMonthRegex,
            SeasonWithYear,
            QuarterRegex,
            DecadeRegex,
        };

        private readonly IntegerExtractor integerExtractor;

        public ChineseDatePeriodExtractorConfiguration(IDateTimeOptionsConfiguration config)
        {
            var numOptions = NumberOptions.None;
            if ((config.Options & DateTimeOptions.NoProtoCache) != 0)
            {
                numOptions = NumberOptions.NoProtoCache;
            }

            var numConfig = new BaseNumberOptionsConfiguration(config.Culture, numOptions);

            integerExtractor = new IntegerExtractor(numConfig);
        }

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

                var middleStr = text.Substring(middleBegin, middleEnd - middleBegin).Trim();

                if (TillRegex.IsExactMatch(middleStr, trim: true))
                {
                    var periodBegin = er[idx].Start ?? 0;
                    var periodEnd = (er[idx + 1].Start ?? 0) + (er[idx + 1].Length ?? 0);

                    // handle "从"
                    var beforeStr = text.Substring(0, periodBegin);
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
        private List<Token> MatchNumberWithUnit(string text)
        {
            var ret = new List<Token>();

            var durations = new List<Token>();
            var ers = integerExtractor.Extract(text);

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

                var match = PastRegex.MatchEnd(beforeStr, trim: true);

                if (match.Success)
                {
                    ret.Add(new Token(match.Index, duration.End));
                    continue;
                }

                match = FutureRegex.MatchEnd(beforeStr, trim: true);

                if (match.Success)
                {
                    ret.Add(new Token(match.Index, duration.End));
                }
            }

            return ret;
        }
    }
}