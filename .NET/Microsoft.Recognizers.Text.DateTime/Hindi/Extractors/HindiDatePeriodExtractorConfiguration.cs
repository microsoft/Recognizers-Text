using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Definitions.Hindi;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Hindi;
using Microsoft.Recognizers.Text.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Hindi
{
    public class HindiDatePeriodExtractorConfiguration : BaseDateTimeOptionsConfiguration, IDatePeriodExtractorConfiguration
    {
        // Base regexes
        public static readonly Regex TillRegex =
            RegexCache.Get(DateTimeDefinitions.TillRegex, RegexFlags);

        public static readonly Regex RangeConnectorRegex =
            RegexCache.Get(DateTimeDefinitions.RangeConnectorRegex, RegexFlags);

        public static readonly Regex DayRegex =
            RegexCache.Get(DateTimeDefinitions.DayRegex, RegexFlags);

        public static readonly Regex MonthNumRegex =
            RegexCache.Get(DateTimeDefinitions.MonthNumRegex, RegexFlags);

        public static readonly Regex IllegalYearRegex =
            RegexCache.Get(BaseDateTime.IllegalYearRegex, RegexFlags);

        public static readonly Regex YearRegex =
            RegexCache.Get(DateTimeDefinitions.YearRegex, RegexFlags);

        public static readonly Regex WeekDayRegex =
            RegexCache.Get(DateTimeDefinitions.WeekDayRegex, RegexFlags);

        public static readonly Regex RelativeMonthRegex =
            RegexCache.Get(DateTimeDefinitions.RelativeMonthRegex, RegexFlags);

        public static readonly Regex WrittenMonthRegex =
            RegexCache.Get(DateTimeDefinitions.WrittenMonthRegex, RegexFlags);

        public static readonly Regex MonthSuffixRegex =
            RegexCache.Get(DateTimeDefinitions.MonthSuffixRegex, RegexFlags);

        public static readonly Regex DateUnitRegex =
            RegexCache.Get(DateTimeDefinitions.DateUnitRegex, RegexFlags);

        public static readonly Regex TimeUnitRegex =
            RegexCache.Get(DateTimeDefinitions.TimeUnitRegex, RegexFlags);

        public static readonly Regex PreviousPrefixRegex =
            RegexCache.Get(DateTimeDefinitions.PreviousPrefixRegex, RegexFlags);

        public static readonly Regex NextPrefixRegex =
            RegexCache.Get(DateTimeDefinitions.NextPrefixRegex, RegexFlags);

        public static readonly Regex FutureSuffixRegex =
            RegexCache.Get(DateTimeDefinitions.FutureSuffixRegex, RegexFlags);

        public static readonly Regex NowRegex =
             RegexCache.Get(DateTimeDefinitions.NowRegex, RegexFlags);

        // composite regexes
        public static readonly Regex SimpleCasesRegex =
            RegexCache.Get(DateTimeDefinitions.SimpleCasesRegex, RegexFlags);

        public static readonly Regex NumberBeforeWeekRegex =
            RegexCache.Get(DateTimeDefinitions.NumberBeforeWeekRegex, RegexFlags);

        public static readonly Regex MonthFrontSimpleCasesRegex =
            RegexCache.Get(DateTimeDefinitions.MonthFrontSimpleCasesRegex, RegexFlags);

        public static readonly Regex MonthFrontBetweenRegex =
            RegexCache.Get(DateTimeDefinitions.MonthFrontBetweenRegex, RegexFlags);

        public static readonly Regex BetweenRegex =
            RegexCache.Get(DateTimeDefinitions.BetweenRegex, RegexFlags);

        public static readonly Regex MonthWithYear =
            RegexCache.Get(DateTimeDefinitions.MonthWithYear, RegexFlags);

        public static readonly Regex OneWordPeriodRegex =
            RegexCache.Get(DateTimeDefinitions.OneWordPeriodRegex, RegexFlags);

        public static readonly Regex MonthNumWithYear =
            RegexCache.Get(DateTimeDefinitions.MonthNumWithYear, RegexFlags);

        public static readonly Regex WeekOfMonthRegex =
            RegexCache.Get(DateTimeDefinitions.WeekOfMonthRegex, RegexFlags);

        public static readonly Regex WeekOfYearRegex =
            RegexCache.Get(DateTimeDefinitions.WeekOfYearRegex, RegexFlags);

        public static readonly Regex FollowedDateUnit =
            RegexCache.Get(DateTimeDefinitions.FollowedDateUnit, RegexFlags);

        public static readonly Regex NumberCombinedWithDateUnit =
            RegexCache.Get(DateTimeDefinitions.NumberCombinedWithDateUnit, RegexFlags);

        public static readonly Regex QuarterRegex =
            RegexCache.Get(DateTimeDefinitions.QuarterRegex, RegexFlags);

        public static readonly Regex QuarterRegexYearFront =
            RegexCache.Get(DateTimeDefinitions.QuarterRegexYearFront, RegexFlags);

        public static readonly Regex AllHalfYearRegex =
            RegexCache.Get(DateTimeDefinitions.AllHalfYearRegex, RegexFlags);

        public static readonly Regex SeasonRegex =
            RegexCache.Get(DateTimeDefinitions.SeasonRegex, RegexFlags);

        public static readonly Regex WhichWeekRegex =
            RegexCache.Get(DateTimeDefinitions.WhichWeekRegex, RegexFlags);

        public static readonly Regex WeekOfRegex =
            RegexCache.Get(DateTimeDefinitions.WeekOfRegex, RegexFlags);

        public static readonly Regex MonthOfRegex =
            RegexCache.Get(DateTimeDefinitions.MonthOfRegex, RegexFlags);

        public static readonly Regex RangeUnitRegex =
            RegexCache.Get(DateTimeDefinitions.RangeUnitRegex, RegexFlags);

        public static readonly Regex InConnectorRegex =
            RegexCache.Get(DateTimeDefinitions.InConnectorRegex, RegexFlags);

        public static readonly Regex WithinNextPrefixRegex =
            RegexCache.Get(DateTimeDefinitions.WithinNextPrefixRegex, RegexFlags);

        public static readonly Regex RestOfDateRegex =
            RegexCache.Get(DateTimeDefinitions.RestOfDateRegex, RegexFlags);

        public static readonly Regex LaterEarlyPeriodRegex =
            RegexCache.Get(DateTimeDefinitions.LaterEarlyPeriodRegex, RegexFlags);

        public static readonly Regex WeekWithWeekDayRangeRegex =
            RegexCache.Get(DateTimeDefinitions.WeekWithWeekDayRangeRegex, RegexFlags);

        public static readonly Regex YearPlusNumberRegex =
            RegexCache.Get(DateTimeDefinitions.YearPlusNumberRegex, RegexFlags);

        public static readonly Regex DecadeWithCenturyRegex =
            RegexCache.Get(DateTimeDefinitions.DecadeWithCenturyRegex, RegexFlags);

        public static readonly Regex YearPeriodRegex =
            RegexCache.Get(DateTimeDefinitions.YearPeriodRegex, RegexFlags);

        public static readonly Regex ComplexDatePeriodRegex =
            RegexCache.Get(DateTimeDefinitions.ComplexDatePeriodRegex, RegexFlags);

        public static readonly Regex RelativeDecadeRegex =
            RegexCache.Get(DateTimeDefinitions.RelativeDecadeRegex, RegexFlags);

        public static readonly Regex ReferenceDatePeriodRegex =
            RegexCache.Get(DateTimeDefinitions.ReferenceDatePeriodRegex, RegexFlags);

        public static readonly Regex AgoRegex =
            RegexCache.Get(DateTimeDefinitions.AgoRegex, RegexFlags);

        public static readonly Regex LaterRegex =
            RegexCache.Get(DateTimeDefinitions.LaterRegex, RegexFlags);

        public static readonly Regex LessThanRegex =
            RegexCache.Get(DateTimeDefinitions.LessThanRegex, RegexFlags);

        public static readonly Regex MoreThanRegex =
            RegexCache.Get(DateTimeDefinitions.MoreThanRegex, RegexFlags);

        public static readonly Regex CenturySuffixRegex =
            RegexCache.Get(DateTimeDefinitions.CenturySuffixRegex, RegexFlags);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly Regex FromTokenRegex =
            RegexCache.Get(DateTimeDefinitions.FromTokenRegex, RegexFlags);

        private static readonly Regex RangePrefixRegex =
            RegexCache.Get(DateTimeDefinitions.RangePrefixRegex, RegexFlags);

        private static readonly Regex[] SimpleCasesRegexes =
        {
            // "3-5 Jan, 2018",
            SimpleCasesRegex,

            // "18 तारीख वाली सप्ताह"
            NumberBeforeWeekRegex,

            // "between 3 and 5 Jan, 2018"
            BetweenRegex,

            // "next april", "year to date", "previous year"
            OneWordPeriodRegex,

            // "January, 2018", "this year Feb"
            MonthWithYear,

            // "2018-3", "2018.3", "5-2015", only FourDigitYear is allow in this Regex
            MonthNumWithYear,

            // "2018", "two thousand and ten"
            YearRegex,

            // "4th week of Feb"
            WeekOfMonthRegex,

            // "3rd week of 2018", "4th week last year"
            WeekOfYearRegex,

            // "Jan between 8-10"
            MonthFrontBetweenRegex,

            // "from Jan 5th-10th", "Feb from 5-10"
            MonthFrontSimpleCasesRegex,

            // "Q1 2018", "2nd quarter"
            QuarterRegex,

            // "2016 Q1", "last year the 4th quarter"
            QuarterRegexYearFront,

            // "2015 the H1", "H2 of 2016", "1st half 2018", "2nd half this year"
            AllHalfYearRegex,

            // "last summer", "fall of 2018", "early this summer"
            SeasonRegex,

            // "week 25", "week 06"
            WhichWeekRegex,

            // "rest of this week", "rest of current year"
            RestOfDateRegex,

            // "early this year", "late next April"
            LaterEarlyPeriodRegex,

            // "this week between Mon and Wed", "next week from Tuesday to Wednesday"
            WeekWithWeekDayRangeRegex,

            // "year 834", "two thousand and nine"
            YearPlusNumberRegex,

            // "21st century 30's"
            DecadeWithCenturyRegex,

            // "next five decades", "previous 2 decades"
            RelativeDecadeRegex,

            // "this week", "same year"
            ReferenceDatePeriodRegex,
        };

        public HindiDatePeriodExtractorConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            DatePointExtractor = new BaseDateExtractor(new HindiDateExtractorConfiguration(this));
            DurationExtractor = new BaseDurationExtractor(new HindiDurationExtractorConfiguration(this));

            var numOptions = NumberOptions.None;
            if ((config.Options & DateTimeOptions.NoProtoCache) != 0)
            {
                numOptions = NumberOptions.NoProtoCache;
            }

            var numConfig = new BaseNumberOptionsConfiguration(config.Culture, numOptions);

            CardinalExtractor = Number.Hindi.CardinalExtractor.GetInstance();
            OrdinalExtractor = Number.Hindi.OrdinalExtractor.GetInstance();

            NumberParser = new BaseNumberParser(new HindiNumberParserConfiguration(numConfig));
        }

        public IDateExtractor DatePointExtractor { get; }

        public IExtractor CardinalExtractor { get; }

        public IExtractor OrdinalExtractor { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        public IParser NumberParser { get; }

        IEnumerable<Regex> IDatePeriodExtractorConfiguration.SimpleCasesRegexes => SimpleCasesRegexes;

        Regex IDatePeriodExtractorConfiguration.IllegalYearRegex => IllegalYearRegex;

        Regex IDatePeriodExtractorConfiguration.YearRegex => YearRegex;

        Regex IDatePeriodExtractorConfiguration.TillRegex => TillRegex;

        Regex IDatePeriodExtractorConfiguration.FollowedDateUnit => FollowedDateUnit;

        Regex IDatePeriodExtractorConfiguration.DateUnitRegex => DateUnitRegex;

        Regex IDatePeriodExtractorConfiguration.TimeUnitRegex => TimeUnitRegex;

        Regex IDatePeriodExtractorConfiguration.NumberCombinedWithDateUnit => NumberCombinedWithDateUnit;

        Regex IDatePeriodExtractorConfiguration.PreviousPrefixRegex => PreviousPrefixRegex;

        Regex IDatePeriodExtractorConfiguration.FutureRegex => NextPrefixRegex;

        Regex IDatePeriodExtractorConfiguration.FutureSuffixRegex => FutureSuffixRegex;

        Regex IDatePeriodExtractorConfiguration.WeekOfRegex => WeekOfRegex;

        Regex IDatePeriodExtractorConfiguration.MonthOfRegex => MonthOfRegex;

        Regex IDatePeriodExtractorConfiguration.RangeUnitRegex => RangeUnitRegex;

        Regex IDatePeriodExtractorConfiguration.InConnectorRegex => InConnectorRegex;

        Regex IDatePeriodExtractorConfiguration.WithinNextPrefixRegex => WithinNextPrefixRegex;

        Regex IDatePeriodExtractorConfiguration.YearPeriodRegex => YearPeriodRegex;

        Regex IDatePeriodExtractorConfiguration.ComplexDatePeriodRegex => ComplexDatePeriodRegex;

        Regex IDatePeriodExtractorConfiguration.RelativeDecadeRegex => RelativeDecadeRegex;

        Regex IDatePeriodExtractorConfiguration.ReferenceDatePeriodRegex => ReferenceDatePeriodRegex;

        Regex IDatePeriodExtractorConfiguration.AgoRegex => AgoRegex;

        Regex IDatePeriodExtractorConfiguration.LaterRegex => LaterRegex;

        Regex IDatePeriodExtractorConfiguration.LessThanRegex => LessThanRegex;

        Regex IDatePeriodExtractorConfiguration.MoreThanRegex => MoreThanRegex;

        Regex IDatePeriodExtractorConfiguration.CenturySuffixRegex => CenturySuffixRegex;

        Regex IDatePeriodExtractorConfiguration.MonthNumRegex => MonthNumRegex;

        Regex IDatePeriodExtractorConfiguration.NowRegex => NowRegex;

        bool IDatePeriodExtractorConfiguration.CheckBothBeforeAfter => DateTimeDefinitions.CheckBothBeforeAfter;

        string[] IDatePeriodExtractorConfiguration.DurationDateRestrictions => DateTimeDefinitions.DurationDateRestrictions;

        public bool GetFromTokenIndex(string text, out int index)
        {
            index = -1;
            var fromMatch = FromTokenRegex.Match(text);
            if (fromMatch.Success)
            {
                index = fromMatch.Index;
            }

            return fromMatch.Success;
        }

        public bool GetBetweenTokenIndex(string text, out int index)
        {
            index = -1;
            var betweenMatch = RangePrefixRegex.MatchBegin(text, trim: true);
            if (betweenMatch.Success)
            {
                index = betweenMatch.Index + betweenMatch.Length;
            }

            return betweenMatch.Success;
        }

        public bool HasConnectorToken(string text)
        {
            return RangeConnectorRegex.IsExactMatch(text, trim: true);
        }
    }
}
