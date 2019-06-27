using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Japanese;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Japanese
{
    public class JapaneseDateExtractorConfiguration : IDateTimeExtractor
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_DATE; // "Date";

        public static readonly Regex MonthRegex = new Regex(DateTimeDefinitions.MonthRegex, RegexFlags);

        public static readonly Regex DayRegex = new Regex(DateTimeDefinitions.DayRegex, RegexFlags);

        public static readonly Regex DayRegexInJapanese = new Regex(DateTimeDefinitions.DateDayRegexInJapanese, RegexFlags);

        public static readonly Regex DayRegexNumInJapanese = new Regex(DateTimeDefinitions.DayRegexNumInJapanese, RegexFlags);

        public static readonly Regex MonthNumRegex = new Regex(DateTimeDefinitions.MonthNumRegex, RegexFlags);

        public static readonly Regex YearRegex = new Regex(DateTimeDefinitions.YearRegex, RegexFlags);

        public static readonly Regex ZeroToNineIntegerRegexJap = new Regex(DateTimeDefinitions.ZeroToNineIntegerRegexJap, RegexFlags);

        public static readonly Regex YearInJapaneseRegex = new Regex(DateTimeDefinitions.DateYearInJapaneseRegex, RegexFlags);

        public static readonly Regex WeekDayRegex = new Regex(DateTimeDefinitions.WeekDayRegex, RegexFlags);

        public static readonly Regex LunarRegex = new Regex(DateTimeDefinitions.LunarRegex, RegexFlags);

        public static readonly Regex ThisRegex = new Regex(DateTimeDefinitions.DateThisRegex, RegexFlags);

        public static readonly Regex LastRegex = new Regex(DateTimeDefinitions.DateLastRegex, RegexFlags);

        public static readonly Regex NextRegex = new Regex(DateTimeDefinitions.DateNextRegex, RegexFlags);

        public static readonly Regex SpecialDayRegex = new Regex(DateTimeDefinitions.SpecialDayRegex, RegexFlags);

        public static readonly Regex SpecialMonthRegex = new Regex(DateTimeDefinitions.SpecialMonthRegex, RegexFlags);

        public static readonly Regex SpecialYearRegex = new Regex(DateTimeDefinitions.SpecialYearRegex, RegexFlags);

        public static readonly Regex WeekDayOfMonthRegex = new Regex(DateTimeDefinitions.WeekDayOfMonthRegex, RegexFlags);

        public static readonly Regex ThisRe = new Regex(DateTimeDefinitions.DateThisRe, RegexFlags);

        public static readonly Regex LastRe = new Regex(DateTimeDefinitions.DateLastRe, RegexFlags);

        public static readonly Regex NextRe = new Regex(DateTimeDefinitions.DateNextRe, RegexFlags);

        public static readonly Regex UnitRegex = new Regex(DateTimeDefinitions.DateUnitRegex, RegexFlags);

        public static readonly Regex SpecialDate = new Regex(DateTimeDefinitions.SpecialDate, RegexFlags);

        public static readonly Regex[] DateRegexList =
        {
            // ２０１６年１２月１日
            new Regex(DateTimeDefinitions.DateRegexList1, RegexFlags),

            // 2015/12/23
            new Regex(DateTimeDefinitions.DateRegexList10, RegexFlags),

            // # ２０１６年１２月
            new Regex(DateTimeDefinitions.DateRegexList2, RegexFlags),

            // １２月１日
            new Regex(DateTimeDefinitions.DateRegexList9, RegexFlags),

            // (2015年)?(农历)?十月二十(星期三)?
            new Regex(DateTimeDefinitions.DateRegexList3, RegexFlags),

            // 7/23
            new Regex(DateTimeDefinitions.DateRegexList4, RegexFlags),

            // 23/7
            new Regex(DateTimeDefinitions.DateRegexList5, RegexFlags),

            DateTimeDefinitions.DefaultLanguageFallback == Constants.DefaultLanguageFallback_DMY ?

                // 23-3-2015
                new Regex(DateTimeDefinitions.DateRegexList7, RegexFlags) :

                // 3-23-2017
                new Regex(DateTimeDefinitions.DateRegexList6, RegexFlags),

            DateTimeDefinitions.DefaultLanguageFallback == Constants.DefaultLanguageFallback_DMY ?

                // 3-23-2017
                new Regex(DateTimeDefinitions.DateRegexList6, RegexFlags) :

                // 23-3-2015
                new Regex(DateTimeDefinitions.DateRegexList7, RegexFlags),

            // 2015-12-23
            new Regex(DateTimeDefinitions.DateRegexList8, RegexFlags),

            // 2016/12
            new Regex(DateTimeDefinitions.DateRegexList11, RegexFlags),
        };

        public static readonly Regex[] ImplicitDateList =
        {
            LunarRegex, SpecialDayRegex, ThisRegex, LastRegex, NextRegex,
            WeekDayRegex, WeekDayOfMonthRegex, SpecialMonthRegex, SpecialYearRegex, SpecialDate,
        };

        public static readonly Regex BeforeRegex = new Regex(DateTimeDefinitions.BeforeRegex, RegexFlags);

        public static readonly Regex AfterRegex = new Regex(DateTimeDefinitions.AfterRegex, RegexFlags);

        public static readonly Regex DateTimePeriodUnitRegex = new Regex(DateTimeDefinitions.DateTimePeriodUnitRegex, RegexFlags);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly JapaneseDurationExtractorConfiguration DurationExtractor = new JapaneseDurationExtractorConfiguration();

        public static List<Token> ExtractRaw(string text)
        {
            var tokens = new List<Token>();
            tokens.AddRange(BasicRegexMatch(text));
            tokens.AddRange(ImplicitDate(text));

            return tokens;
        }

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

        // Process case like "三天前" "两个月前"
        private List<Token> DurationWithBeforeAndAfter(string text, DateObject referenceTime)
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

                    if ((beforeMatch.Success && suffix.StartsWith(beforeMatch.Value)) ||
                        (afterMatch.Success && suffix.StartsWith(afterMatch.Value)))
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