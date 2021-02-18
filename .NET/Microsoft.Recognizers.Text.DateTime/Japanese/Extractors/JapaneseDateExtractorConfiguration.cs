using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Japanese;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Japanese
{
    public class JapaneseDateExtractorConfiguration : AbstractYearExtractor, IDateTimeExtractor
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_DATE; // "Date";

        public static readonly Regex MonthRegex = RegexCache.Get(DateTimeDefinitions.MonthRegex, RegexFlags);

        public static readonly Regex DayRegex = RegexCache.Get(DateTimeDefinitions.DayRegex, RegexFlags);

        public static readonly Regex DayRegexInJapanese = RegexCache.Get(DateTimeDefinitions.DateDayRegexInJapanese, RegexFlags);

        public static readonly Regex DayRegexNumInJapanese = RegexCache.Get(DateTimeDefinitions.DayRegexNumInJapanese, RegexFlags);

        public static readonly Regex MonthNumRegex = RegexCache.Get(DateTimeDefinitions.MonthNumRegex, RegexFlags);

        public static readonly Regex YearRegex = RegexCache.Get(DateTimeDefinitions.YearRegex, RegexFlags);

        public static readonly Regex RelativeRegex = RegexCache.Get(DateTimeDefinitions.RelativeRegex, RegexFlags);

        public static readonly Regex ZeroToNineIntegerRegexJap = RegexCache.Get(DateTimeDefinitions.ZeroToNineIntegerRegexJap, RegexFlags);

        public static readonly Regex YearInJapaneseRegex = RegexCache.Get(DateTimeDefinitions.DateYearInJapaneseRegex, RegexFlags);

        public static readonly Regex WeekDayRegex = RegexCache.Get(DateTimeDefinitions.WeekDayRegex, RegexFlags);

        public static readonly Regex LunarRegex = RegexCache.Get(DateTimeDefinitions.LunarRegex, RegexFlags);

        public static readonly Regex ThisRegex = RegexCache.Get(DateTimeDefinitions.DateThisRegex, RegexFlags);

        public static readonly Regex LastRegex = RegexCache.Get(DateTimeDefinitions.DateLastRegex, RegexFlags);

        public static readonly Regex NextRegex = RegexCache.Get(DateTimeDefinitions.DateNextRegex, RegexFlags);

        public static readonly Regex SpecialDayRegex = RegexCache.Get(DateTimeDefinitions.SpecialDayRegex, RegexFlags);

        public static readonly Regex SpecialMonthRegex = RegexCache.Get(DateTimeDefinitions.SpecialMonthRegex, RegexFlags);

        public static readonly Regex SpecialYearRegex = RegexCache.Get(DateTimeDefinitions.SpecialYearRegex, RegexFlags);

        public static readonly Regex WeekDayOfMonthRegex = RegexCache.Get(DateTimeDefinitions.WeekDayOfMonthRegex, RegexFlags);

        public static readonly Regex ThisRe = RegexCache.Get(DateTimeDefinitions.ThisPrefixRegex, RegexFlags);

        public static readonly Regex LastRe = RegexCache.Get(DateTimeDefinitions.LastPrefixRegex, RegexFlags);

        public static readonly Regex NextRe = RegexCache.Get(DateTimeDefinitions.NextPrefixRegex, RegexFlags);

        public static readonly Regex UnitRegex = RegexCache.Get(DateTimeDefinitions.DateUnitRegex, RegexFlags);

        public static readonly Regex SpecialDate = RegexCache.Get(DateTimeDefinitions.SpecialDate, RegexFlags);

        public static readonly Regex[] DateRegexList =
        {
            // ２０１６年１２月１日
            RegexCache.Get(DateTimeDefinitions.DateRegexList1, RegexFlags),

            // 2015/12/23
            RegexCache.Get(DateTimeDefinitions.DateRegexList10, RegexFlags),

            // # ２０１６年１２月
            RegexCache.Get(DateTimeDefinitions.DateRegexList2, RegexFlags),

            // １２月１日
            RegexCache.Get(DateTimeDefinitions.DateRegexList9, RegexFlags),

            // (2015年)?(农历)?十月二十(星期三)?
            RegexCache.Get(DateTimeDefinitions.DateRegexList3, RegexFlags),

            // 7/23
            RegexCache.Get(DateTimeDefinitions.DateRegexList4, RegexFlags),

            // 23/7
            RegexCache.Get(DateTimeDefinitions.DateRegexList5, RegexFlags),

            DateTimeDefinitions.DefaultLanguageFallback == Constants.DefaultLanguageFallback_DMY ?

                // 23-3-2015
                RegexCache.Get(DateTimeDefinitions.DateRegexList7, RegexFlags) :

                // 3-23-2017
                RegexCache.Get(DateTimeDefinitions.DateRegexList6, RegexFlags),

            DateTimeDefinitions.DefaultLanguageFallback == Constants.DefaultLanguageFallback_DMY ?

                // 3-23-2017
                RegexCache.Get(DateTimeDefinitions.DateRegexList6, RegexFlags) :

                // 23-3-2015
                RegexCache.Get(DateTimeDefinitions.DateRegexList7, RegexFlags),

            // 2015-12-23
            RegexCache.Get(DateTimeDefinitions.DateRegexList8, RegexFlags),

            // 2016/12
            RegexCache.Get(DateTimeDefinitions.DateRegexList11, RegexFlags),
        };

        public static readonly Regex[] ImplicitDateList =
        {
            LunarRegex, SpecialDayRegex, ThisRegex, LastRegex, NextRegex,
            WeekDayRegex, WeekDayOfMonthRegex, SpecialMonthRegex, SpecialYearRegex, SpecialDate,
        };

        public static readonly Regex BeforeRegex = RegexCache.Get(DateTimeDefinitions.BeforeRegex, RegexFlags);

        public static readonly Regex AfterRegex = RegexCache.Get(DateTimeDefinitions.AfterRegex, RegexFlags);

        public static readonly Regex DateTimePeriodUnitRegex = RegexCache.Get(DateTimeDefinitions.DateTimePeriodUnitRegex, RegexFlags);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly JapaneseDurationExtractorConfiguration DurationExtractor = new JapaneseDurationExtractorConfiguration();

        public JapaneseDateExtractorConfiguration(IDateExtractorConfiguration config = null)
            : base(config)
        {
        }

        public static List<Token> ExtractRaw(string text)
        {
            var tokens = new List<Token>();
            tokens.AddRange(BasicRegexMatch(text));
            tokens.AddRange(ImplicitDate(text));

            return tokens;
        }

        public override List<ExtractResult> Extract(string text)
        {
            return Extract(text, DateObject.Now);
        }

        public override List<ExtractResult> Extract(string text, DateObject referenceTime)
        {
            var tokens = new List<Token>();
            tokens.AddRange(BasicRegexMatch(text));
            tokens.AddRange(ImplicitDate(text));
            tokens.AddRange(DurationWithAgoAndLater(text, referenceTime));

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
        private List<Token> DurationWithAgoAndLater(string text, DateObject referenceTime)
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

                    if ((beforeMatch.Success && suffix.StartsWith(beforeMatch.Value, StringComparison.Ordinal)) ||
                        (afterMatch.Success && suffix.StartsWith(afterMatch.Value, StringComparison.Ordinal)))
                    {
                        var metadata = new Metadata() { IsDurationWithAgoAndLater = true };
                        ret.Add(new Token(er.Start ?? 0, (er.Start + er.Length ?? 0) + 1, metadata));
                    }
                }
            }

            return ret;
        }
    }
}