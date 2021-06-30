using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Korean;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Korean;
using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Korean
{
    public class KoreanDatePeriodExtractorConfiguration : BaseDateTimeOptionsConfiguration, ICJKDatePeriodExtractorConfiguration
    {
        public static readonly Regex TillRegex = new Regex(DateTimeDefinitions.DatePeriodTillRegex, RegexFlags);

        public static readonly Regex RangePrefixRegex = new Regex(DateTimeDefinitions.DatePeriodRangePrefixRegex, RegexFlags);

        public static readonly Regex RangeSuffixRegex = new Regex(DateTimeDefinitions.DatePeriodRangeSuffixRegex, RegexFlags);

        public static readonly Regex StrictYearRegex = new Regex(DateTimeDefinitions.StrictYearRegex, RegexFlags);

        public static readonly Regex YearInCJKRegex = new Regex(DateTimeDefinitions.DatePeriodYearInCJKRegex, RegexFlags);

        // for case "(에서)? (2017)? 12 월 10 일"
        public static readonly Regex SimpleCasesRegex = new Regex(DateTimeDefinitions.SimpleCasesRegex, RegexFlags);

        public static readonly Regex YearAndMonth = new Regex(DateTimeDefinitions.YearAndMonth, RegexFlags);

        // 2017.12, 2017-12, 2017/12, 12/2017
        public static readonly Regex PureNumYearAndMonth = new Regex(DateTimeDefinitions.PureNumYearAndMonth, RegexFlags);

        public static readonly Regex SimpleYearAndMonth = new Regex(DateTimeDefinitions.SimpleYearAndMonth, RegexFlags);

        public static readonly Regex OneWordPeriodRegex = new Regex(DateTimeDefinitions.OneWordPeriodRegex, RegexFlags);

        public static readonly Regex WeekOfMonthRegex = new Regex(DateTimeDefinitions.WeekOfMonthRegex, RegexFlags);

        public static readonly Regex WeekOfYearRegex = new Regex(DateTimeDefinitions.WeekOfYearRegex, RegexFlags);

        public static readonly Regex FollowedUnit = new Regex(DateTimeDefinitions.FollowedUnit, RegexFlags);

        public static readonly Regex NumberCombinedWithUnit = new Regex(DateTimeDefinitions.NumberCombinedWithUnit, RegexFlags);

        public static readonly Regex YearToYear = new Regex(DateTimeDefinitions.YearToYear, RegexFlags);

        public static readonly Regex YearToYearSuffixRequired = new Regex(DateTimeDefinitions.YearToYearSuffixRequired, RegexFlags);

        public static readonly Regex MonthToMonth = new Regex(DateTimeDefinitions.MonthToMonth, RegexFlags);

        public static readonly Regex MonthToMonthSuffixRequired = new Regex(DateTimeDefinitions.MonthToMonthSuffixRequired, RegexFlags);

        public static readonly Regex DayToDay = new Regex(DateTimeDefinitions.DayToDay, RegexFlags);

        public static readonly Regex WeekToWeek = new Regex(DateTimeDefinitions.WeekToWeek, RegexFlags);

        public static readonly Regex DayRegexForPeriod = new Regex(DateTimeDefinitions.DayRegexForPeriod, RegexFlags);

        public static readonly Regex PastRegex = new Regex(DateTimeDefinitions.PastRegex, RegexFlags);

        public static readonly Regex FutureRegex = new Regex(DateTimeDefinitions.FutureRegex, RegexFlags);

        public static readonly Regex SeasonWithYear = new Regex(DateTimeDefinitions.SeasonWithYear, RegexFlags);

        public static readonly Regex QuarterRegex = new Regex(DateTimeDefinitions.QuarterRegex, RegexFlags);

        public static readonly Regex DecadeRegex = new Regex(DateTimeDefinitions.DecadeRegex, RegexFlags);

        public static readonly Regex RelativePeriodRegex = new Regex(DateTimeDefinitions.RelativePeriodRegex, RegexFlags);

        public static readonly Regex SpecialMonthRegex = new Regex(DateTimeDefinitions.SpecialMonthRegex, RegexFlags);

        public static readonly Regex SpecialYearRegex = new Regex(DateTimeDefinitions.SpecialYearRegex, RegexFlags);

        public static readonly Regex DayRegex = new Regex(DateTimeDefinitions.DayRegex, RegexFlags);
        public static readonly Regex DayRegexInCJK = new Regex(DateTimeDefinitions.DatePeriodDayRegexInCJK, RegexFlags);
        public static readonly Regex MonthNumRegex = new Regex(DateTimeDefinitions.MonthNumRegex, RegexFlags);
        public static readonly Regex ThisRegex = new Regex(DateTimeDefinitions.DatePeriodThisRegex, RegexFlags);
        public static readonly Regex LastRegex = new Regex(DateTimeDefinitions.DatePeriodLastRegex, RegexFlags);
        public static readonly Regex NextRegex = new Regex(DateTimeDefinitions.DatePeriodNextRegex, RegexFlags);
        public static readonly Regex RelativeMonthRegex = new Regex(DateTimeDefinitions.RelativeMonthRegex, RegexFlags);
        public static readonly Regex MonthRegex = new Regex(DateTimeDefinitions.MonthRegex, RegexFlags);
        public static readonly Regex YearRegex = new Regex(DateTimeDefinitions.YearRegex, RegexFlags);
        public static readonly Regex YearRegexInNumber = new Regex(DateTimeDefinitions.YearRegexInNumber, RegexFlags);
        public static readonly Regex ZeroToNineIntegerRegexCJK = new Regex(DateTimeDefinitions.ZeroToNineIntegerRegexCJK, RegexFlags);
        public static readonly Regex MonthSuffixRegex = new Regex(DateTimeDefinitions.MonthSuffixRegex, RegexFlags);
        public static readonly Regex UnitRegex = new Regex(DateTimeDefinitions.UnitRegex, RegexFlags);
        public static readonly Regex SeasonRegex = new Regex(DateTimeDefinitions.SeasonRegex, RegexFlags);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly Regex[] SimpleCasesRegexes =
        {
            SimpleCasesRegex,
            OneWordPeriodRegex,
            YearRegex,
            StrictYearRegex,
            YearAndMonth,
            PureNumYearAndMonth,
            YearInCJKRegex,
            SpecialMonthRegex,
            SpecialYearRegex,
            WeekOfMonthRegex,
            WeekOfYearRegex,
            WeekToWeek,
            SeasonWithYear,
            QuarterRegex,
            DecadeRegex,
            RelativePeriodRegex,
        };

        public KoreanDatePeriodExtractorConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            DatePointExtractor = new BaseCJKDateExtractor(new KoreanDateExtractorConfiguration(this));

            var numOptions = NumberOptions.None;
            if ((config.Options & DateTimeOptions.NoProtoCache) != 0)
            {
                numOptions = NumberOptions.NoProtoCache;
            }

            var numConfig = new BaseNumberOptionsConfiguration(config.Culture, numOptions);

            IntegerExtractor = new IntegerExtractor(numConfig);
        }

        public IDateTimeExtractor DatePointExtractor { get; }

        public IExtractor IntegerExtractor { get; }

        IEnumerable<Regex> ICJKDatePeriodExtractorConfiguration.SimpleCasesRegexes => SimpleCasesRegexes;

        Regex ICJKDatePeriodExtractorConfiguration.TillRegex => TillRegex;

        Regex ICJKDatePeriodExtractorConfiguration.FutureRegex => FutureRegex;

        Regex ICJKDatePeriodExtractorConfiguration.PastRegex => PastRegex;

        Regex ICJKDatePeriodExtractorConfiguration.NumberCombinedWithUnit => NumberCombinedWithUnit;

        Regex ICJKDatePeriodExtractorConfiguration.FollowedUnit => FollowedUnit;

        Regex ICJKDatePeriodExtractorConfiguration.RangePrefixRegex => RangePrefixRegex;

        Regex ICJKDatePeriodExtractorConfiguration.RangeSuffixRegex => RangeSuffixRegex;
    }
}