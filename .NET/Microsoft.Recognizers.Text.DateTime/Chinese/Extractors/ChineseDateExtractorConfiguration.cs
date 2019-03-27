using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Chinese;

using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Chinese
{
    public class ChineseDateExtractorConfiguration : IDateTimeExtractor
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_DATE; // "Date";

        public static readonly Regex MonthRegex = new Regex(DateTimeDefinitions.MonthRegex, RegexOptions.Singleline);

        public static readonly Regex DayRegex = new Regex(DateTimeDefinitions.DayRegex, RegexOptions.Singleline);

        public static readonly Regex DayRegexInChinese = new Regex(DateTimeDefinitions.DateDayRegexInChinese, RegexOptions.Singleline);

        public static readonly Regex DayRegexNumInChinese = new Regex(DateTimeDefinitions.DayRegexNumInChinese, RegexOptions.Singleline);

        public static readonly Regex MonthNumRegex = new Regex(DateTimeDefinitions.MonthNumRegex, RegexOptions.Singleline);

        public static readonly Regex YearRegex = new Regex(DateTimeDefinitions.YearRegex, RegexOptions.Singleline);

        public static readonly Regex ZeroToNineIntegerRegexChs = new Regex(DateTimeDefinitions.ZeroToNineIntegerRegexChs, RegexOptions.Singleline);

        public static readonly Regex YearInChineseRegex = new Regex(DateTimeDefinitions.DateYearInChineseRegex, RegexOptions.Singleline);

        public static readonly Regex WeekDayRegex = new Regex(DateTimeDefinitions.WeekDayRegex, RegexOptions.Singleline);

        public static readonly Regex LunarRegex = new Regex(DateTimeDefinitions.LunarRegex, RegexOptions.Singleline);

        public static readonly Regex ThisRegex = new Regex(DateTimeDefinitions.DateThisRegex, RegexOptions.Singleline);

        public static readonly Regex LastRegex = new Regex(DateTimeDefinitions.DateLastRegex, RegexOptions.Singleline);

        public static readonly Regex NextRegex = new Regex(DateTimeDefinitions.DateNextRegex, RegexOptions.Singleline);

        public static readonly Regex SpecialDayRegex = new Regex(DateTimeDefinitions.SpecialDayRegex, RegexOptions.Singleline);

        public static readonly Regex WeekDayOfMonthRegex = new Regex(DateTimeDefinitions.WeekDayOfMonthRegex, RegexOptions.Singleline);

        public static readonly Regex ThisRe = new Regex(DateTimeDefinitions.DateThisRe, RegexOptions.Singleline);

        public static readonly Regex LastRe = new Regex(DateTimeDefinitions.DateLastRe, RegexOptions.Singleline);

        public static readonly Regex NextRe = new Regex(DateTimeDefinitions.DateNextRe, RegexOptions.Singleline);

        public static readonly Regex SpecialDate = new Regex(DateTimeDefinitions.SpecialDate, RegexOptions.Singleline);

        public static readonly Regex UnitRegex = new Regex(DateTimeDefinitions.DateUnitRegex, RegexOptions.Singleline);

        public static readonly Regex[] DateRegexList =
        {
            // (农历)?(2016年)?一月三日(星期三)?
            new Regex(DateTimeDefinitions.DateRegexList1, RegexOptions.Singleline),

            // (2015年)?(农历)?十月初一(星期三)?
            new Regex(DateTimeDefinitions.DateRegexList2, RegexOptions.Singleline),

            // (2015年)?(农历)?十月二十(星期三)?
            new Regex(DateTimeDefinitions.DateRegexList3, RegexOptions.Singleline),

            // 7/23
            new Regex(DateTimeDefinitions.DateRegexList4, RegexOptions.Singleline),

            // 23/7
            new Regex(DateTimeDefinitions.DateRegexList5, RegexOptions.Singleline),

            DateTimeDefinitions.DefaultLanguageFallback == Constants.DefaultLanguageFallback_DMY ?

                // 23-3-2015
                new Regex(DateTimeDefinitions.DateRegexList7, RegexOptions.Singleline) :

                // 3-23-2017
                new Regex(DateTimeDefinitions.DateRegexList6, RegexOptions.Singleline),

            DateTimeDefinitions.DefaultLanguageFallback == Constants.DefaultLanguageFallback_DMY ?

                // 3-23-2017
                new Regex(DateTimeDefinitions.DateRegexList6, RegexOptions.Singleline) :

                // 23-3-2015
                new Regex(DateTimeDefinitions.DateRegexList7, RegexOptions.Singleline),

            // 2015-12-23
            new Regex(DateTimeDefinitions.DateRegexList8, RegexOptions.Singleline),
        };

        public static readonly Regex[] ImplicitDateList =
        {
            LunarRegex, SpecialDayRegex, ThisRegex, LastRegex, NextRegex,
            WeekDayRegex, WeekDayOfMonthRegex, SpecialDate,
        };

        public static readonly Regex BeforeRegex = new Regex(DateTimeDefinitions.BeforeRegex, RegexOptions.Singleline);

        public static readonly Regex AfterRegex = new Regex(DateTimeDefinitions.AfterRegex, RegexOptions.Singleline);

        public static readonly Regex DateTimePeriodUnitRegex = new Regex(DateTimeDefinitions.DateTimePeriodUnitRegex, RegexOptions.Singleline);

        private static readonly ChineseDurationExtractorConfiguration DurationExtractor = new ChineseDurationExtractorConfiguration();

        public List<ExtractResult> Extract(string text)
        {
            return Extract(text, DateObject.Now);
        }

        public List<ExtractResult> Extract(string text, DateObject referenceTime)
        {
            var tokens = new List<Token>();
            tokens.AddRange(BasicRegexMatch(text));
            tokens.AddRange(ImplicitDate(text));
            tokens.AddRange(DurationWithBeforeAndAfter(text, referenceTime));

            return Token.MergeAllTokens(tokens, text, ExtractorName);
        }

        // Match basic patterns in DateRegexList
        private static List<Token> BasicRegexMatch(string text)
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

        // Match several other implicit cases
        private static List<Token> ImplicitDate(string text)
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
        private static List<Token> DurationWithBeforeAndAfter(string text, DateObject referenceTime)
        {
            var ret = new List<Token>();
            var durationEr = DurationExtractor.Extract(text, referenceTime);
            foreach (var er in durationEr)
            {
                // Only handles date durations here
                // Cases with dateTime durations will be handled in DateTime Extractor
                if (DateTimePeriodUnitRegex.Match(er.Text).Success)
                {
                    continue;
                }

                var pos = (int)er.Start + (int)er.Length;
                if (pos < text.Length)
                {
                    var suffix = text.Substring(pos);
                    var beforeMatch = BeforeRegex.Match(suffix);
                    var afterMatch = AfterRegex.Match(suffix);

                    if ((beforeMatch.Success && suffix.StartsWith(beforeMatch.Value)) || (afterMatch.Success && suffix.StartsWith(afterMatch.Value)))
                    {
                        var metadata = new Metadata() { IsDurationWithBeforeAndAfter = true };
                        ret.Add(new Token(er.Start ?? 0, (er.Start + er.Length ?? 0) + 1, metadata));
                    }
                }
            }

            return ret;
        }
    }
}