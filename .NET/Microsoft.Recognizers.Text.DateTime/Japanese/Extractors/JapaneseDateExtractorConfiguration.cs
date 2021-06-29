using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Japanese;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Japanese;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Japanese
{
    public class JapaneseDateExtractorConfiguration : BaseDateTimeOptionsConfiguration, ICJKDateExtractorConfiguration
    {

        public static readonly Regex WeekDayRegex = new Regex(DateTimeDefinitions.WeekDayRegex, RegexFlags);

        public static readonly Regex LunarRegex = new Regex(DateTimeDefinitions.LunarRegex, RegexFlags);

        public static readonly Regex ThisRegex = new Regex(DateTimeDefinitions.DateThisRegex, RegexFlags);

        public static readonly Regex LastRegex = new Regex(DateTimeDefinitions.DateLastRegex, RegexFlags);

        public static readonly Regex NextRegex = new Regex(DateTimeDefinitions.DateNextRegex, RegexFlags);

        public static readonly Regex SpecialDayRegex = new Regex(DateTimeDefinitions.SpecialDayRegex, RegexFlags);

        public static readonly Regex WeekDayOfMonthRegex = new Regex(DateTimeDefinitions.WeekDayOfMonthRegex, RegexFlags);

        public static readonly Regex SpecialDate = new Regex(DateTimeDefinitions.SpecialDate, RegexFlags);

        public static readonly Regex SpecialDayWithNumRegex = new Regex(DateTimeDefinitions.SpecialDayWithNumRegex, RegexFlags);

        public static readonly Regex BeforeRegex = new Regex(DateTimeDefinitions.BeforeRegex, RegexFlags);

        public static readonly Regex AfterRegex = new Regex(DateTimeDefinitions.AfterRegex, RegexFlags);

        public static readonly Regex DateTimePeriodUnitRegex = new Regex(DateTimeDefinitions.DateTimePeriodUnitRegex, RegexFlags);

        public static readonly Regex MonthRegex = new Regex(DateTimeDefinitions.MonthRegex, RegexFlags);
        public static readonly Regex DayRegex = new Regex(DateTimeDefinitions.DayRegex, RegexFlags);
        public static readonly Regex DayRegexInCJK = new Regex(DateTimeDefinitions.DateDayRegexInCJK, RegexFlags);
        public static readonly Regex DayRegexNumInCJK = new Regex(DateTimeDefinitions.DayRegexNumInCJK, RegexFlags);
        public static readonly Regex MonthNumRegex = new Regex(DateTimeDefinitions.MonthNumRegex, RegexFlags);
        public static readonly Regex WeekDayAndDayRegex = new Regex(DateTimeDefinitions.WeekDayAndDayRegex, RegexFlags);
        public static readonly Regex DurationRelativeDurationUnitRegex = new Regex(DateTimeDefinitions.DurationRelativeDurationUnitRegex, RegexFlags);
        public static readonly Regex YearRegex = new Regex(DateTimeDefinitions.YearRegex, RegexFlags);
        public static readonly Regex RelativeRegex = new Regex(DateTimeDefinitions.RelativeRegex, RegexFlags);
        public static readonly Regex ZeroToNineIntegerRegexCJK = new Regex(DateTimeDefinitions.ZeroToNineIntegerRegexCJK, RegexFlags);
        public static readonly Regex YearInCJKRegex = new Regex(DateTimeDefinitions.DateYearInCJKRegex, RegexFlags);
        public static readonly Regex ThisRe = new Regex(DateTimeDefinitions.ThisPrefixRegex, RegexFlags);
        public static readonly Regex LastRe = new Regex(DateTimeDefinitions.LastPrefixRegex, RegexFlags);
        public static readonly Regex NextRe = new Regex(DateTimeDefinitions.NextPrefixRegex, RegexFlags);
        public static readonly Regex UnitRegex = new Regex(DateTimeDefinitions.DateUnitRegex, RegexFlags);
        public static readonly Regex DynastyYearRegex = new Regex(DateTimeDefinitions.DynastyYearRegex, RegexFlags);
        public static readonly string DynastyStartYear = DateTimeDefinitions.DynastyStartYear;
        public static readonly ImmutableDictionary<string, int> DynastyYearMap = DateTimeDefinitions.DynastyYearMap.ToImmutableDictionary();

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public JapaneseDateExtractorConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            var durationConfig = new BaseDateTimeOptionsConfiguration(config.Culture, DateTimeOptions.None);

            DurationExtractor = new BaseCJKDurationExtractor(new JapaneseDurationExtractorConfiguration(durationConfig));

            ImplicitDateList = new List<Regex>
            {
                LunarRegex, SpecialDayWithNumRegex, SpecialDayRegex, ThisRegex, LastRegex, NextRegex,
                WeekDayRegex, WeekDayOfMonthRegex, SpecialDate, WeekDayAndDayRegex,
            };

            // ２０１６年１２月１日
            var dateRegex1 = new Regex(DateTimeDefinitions.DateRegexList1, RegexFlags);

            // # ２０１６年１２月 (this is not a Date)
            // var dateRegex2 = new Regex(DateTimeDefinitions.DateRegexList2, RegexFlags);

            // (2015年)?(农历)?十月二十(星期三)?
            var dateRegex3 = new Regex(DateTimeDefinitions.DateRegexList3, RegexFlags);

            // 2015-12-23
            var dateRegex8 = new Regex(DateTimeDefinitions.DateRegexList8, RegexFlags);

            // 23/7
            var dateRegex5 = new Regex(DateTimeDefinitions.DateRegexList5, RegexFlags);

            // 7/23
            var dateRegex4 = new Regex(DateTimeDefinitions.DateRegexList4, RegexFlags);

            // 23-3-2017
            var dateRegex7 = new Regex(DateTimeDefinitions.DateRegexList7, RegexFlags);

            // 3-23-2015
            var dateRegex6 = new Regex(DateTimeDefinitions.DateRegexList6, RegexFlags);

            // １２月１日
            var dateRegex9 = new Regex(DateTimeDefinitions.DateRegexList9, RegexFlags);

            // 2015/12/23
            var dateRegex10 = new Regex(DateTimeDefinitions.DateRegexList10, RegexFlags);

            // 2016/12 (this is not a Date)
            // var dateRegex11 = new Regex(DateTimeDefinitions.DateRegexList11, RegexFlags);

            // Regex precedence where the order between D and M varies is controlled by DefaultLanguageFallback
            var enableDmy = DateTimeDefinitions.DefaultLanguageFallback == Constants.DefaultLanguageFallback_DMY;

            DateRegexList = new List<Regex> { dateRegex1, dateRegex10, /*dateRegex2, */dateRegex9, dateRegex3, dateRegex4, dateRegex5 };
            DateRegexList = DateRegexList.Concat(
                enableDmy ?
                new[] { dateRegex7, dateRegex6, dateRegex8/*, dateRegex11*/ } :
                new[] { dateRegex6, dateRegex7, dateRegex8/*, dateRegex11*/ });

            NormalizeCharMap = DateTimeDefinitions.NormalizeCharMap;

        }

        public IEnumerable<Regex> DateRegexList { get; }

        public IEnumerable<Regex> ImplicitDateList { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        Regex ICJKDateExtractorConfiguration.DateTimePeriodUnitRegex => DateTimePeriodUnitRegex;

        Regex ICJKDateExtractorConfiguration.BeforeRegex => BeforeRegex;

        Regex ICJKDateExtractorConfiguration.AfterRegex => AfterRegex;

        public Dictionary<char, char> NormalizeCharMap { get; }

    }
}