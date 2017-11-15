using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.English;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime.English
{
    public class EnglishDatePeriodExtractorConfiguration : IDatePeriodExtractorConfiguration
    {
        // base regexes
        public static readonly Regex TillRegex = 
            new Regex(DateTimeDefinitions.TillRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex AndRegex = 
            new Regex(DateTimeDefinitions.RangeConnectorRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex DayRegex =
            new Regex(DateTimeDefinitions.DayRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthNumRegex =
            new Regex(DateTimeDefinitions.MonthNumRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex YearRegex = 
            new Regex(DateTimeDefinitions.YearRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex WeekDayRegex =
            new Regex(DateTimeDefinitions.WeekDayRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex RelativeMonthRegex = 
            new Regex(DateTimeDefinitions.RelativeMonthRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex EngMonthRegex =
            new Regex(DateTimeDefinitions.EngMonthRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthSuffixRegex =
            new Regex(DateTimeDefinitions.MonthSuffixRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex DateUnitRegex = 
            new Regex(DateTimeDefinitions.DateUnitRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex PastPrefixRegex = 
            new Regex(DateTimeDefinitions.PastPrefixRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex NextPrefixRegex = 
            new Regex(DateTimeDefinitions.NextPrefixRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // composite regexes
        public static readonly Regex SimpleCasesRegex =
            new Regex(DateTimeDefinitions.SimpleCasesRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthFrontSimpleCasesRegex =
            new Regex(DateTimeDefinitions.MonthFrontSimpleCasesRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthFrontBetweenRegex =
            new Regex(DateTimeDefinitions.MonthFrontBetweenRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex BetweenRegex =
            new Regex(DateTimeDefinitions.BetweenRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthWithYear =
            new Regex(DateTimeDefinitions.MonthWithYear, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex OneWordPeriodRegex =
            new Regex(DateTimeDefinitions.OneWordPeriodRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthNumWithYear =
            new Regex(DateTimeDefinitions.MonthNumWithYear, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex WeekOfMonthRegex =
            new Regex(DateTimeDefinitions.WeekOfMonthRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex WeekOfYearRegex =
            new Regex(DateTimeDefinitions.WeekOfYearRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex FollowedDateUnit = 
            new Regex(DateTimeDefinitions.FollowedDateUnit, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex NumberCombinedWithDateUnit =
            new Regex(DateTimeDefinitions.NumberCombinedWithDateUnit, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex QuarterRegex =
            new Regex(DateTimeDefinitions.QuarterRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex QuarterRegexYearFront =
            new Regex(DateTimeDefinitions.QuarterRegexYearFront, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex SeasonRegex =
            new Regex(DateTimeDefinitions.SeasonRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex WhichWeekRegex =
            new Regex(DateTimeDefinitions.WhichWeekRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex WeekOfRegex =
            new Regex(DateTimeDefinitions.WeekOfRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthOfRegex =
            new Regex(DateTimeDefinitions.MonthOfRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex RangeUnitRegex =
            new Regex(DateTimeDefinitions.RangeUnitRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex InConnectorRegex =
            new Regex(DateTimeDefinitions.InConnectorRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex RestOfDateRegex =
            new Regex(DateTimeDefinitions.RestOfDateRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex LaterEarlyPeriodRegex =
            new Regex(DateTimeDefinitions.LaterEarlyPeriodRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex WeekWithWeekDayRangeRegex =
            new Regex(DateTimeDefinitions.WeekWithWeekDayRangeRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private static readonly Regex[] SimpleCasesRegexes =
        {
            SimpleCasesRegex,
            BetweenRegex,
            OneWordPeriodRegex,
            MonthWithYear,
            MonthNumWithYear,
            YearRegex,
            WeekOfMonthRegex,
            WeekOfYearRegex,
            MonthFrontBetweenRegex,
            MonthFrontSimpleCasesRegex,
            QuarterRegex,
            QuarterRegexYearFront,
            SeasonRegex,
            WhichWeekRegex,
            RestOfDateRegex,
            LaterEarlyPeriodRegex,
            WeekWithWeekDayRangeRegex,
        };

        public EnglishDatePeriodExtractorConfiguration()
        {
            DatePointExtractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration());
            CardinalExtractor = Number.English.CardinalExtractor.GetInstance();
            DurationExtractor = new BaseDurationExtractor(new EnglishDurationExtractorConfiguration());
        }

        public IDateTimeExtractor DatePointExtractor { get; }

        public IExtractor CardinalExtractor { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        IEnumerable<Regex> IDatePeriodExtractorConfiguration.SimpleCasesRegexes => SimpleCasesRegexes;

        Regex IDatePeriodExtractorConfiguration.TillRegex => TillRegex;

        Regex IDatePeriodExtractorConfiguration.FollowedDateUnit => FollowedDateUnit;

        Regex IDatePeriodExtractorConfiguration.DateUnitRegex => DateUnitRegex;

        Regex IDatePeriodExtractorConfiguration.NumberCombinedWithDateUnit => NumberCombinedWithDateUnit;

        Regex IDatePeriodExtractorConfiguration.PastRegex => PastPrefixRegex;

        Regex IDatePeriodExtractorConfiguration.FutureRegex => NextPrefixRegex;

        Regex IDatePeriodExtractorConfiguration.WeekOfRegex => WeekOfRegex;

        Regex IDatePeriodExtractorConfiguration.MonthOfRegex => MonthOfRegex;

        Regex IDatePeriodExtractorConfiguration.RangeUnitRegex => RangeUnitRegex;

        Regex IDatePeriodExtractorConfiguration.InConnectorRegex => InConnectorRegex;

        public bool GetFromTokenIndex(string text, out int index)
        {
            index = -1;
            if (text.EndsWith("from"))
            {
                index = text.LastIndexOf("from", StringComparison.Ordinal);
                return true;
            }

            return false;
        }

        public bool GetBetweenTokenIndex(string text, out int index)
        {
            index = -1;
            if (text.EndsWith("between"))
            {
                index = text.LastIndexOf("between", StringComparison.Ordinal);
                return true;
            }

            return false;
        }

        public bool HasConnectorToken(string text)
        {
            var match = Regex.Match(text, DateTimeDefinitions.RangeConnectorRegex);
            return match.Success && match.Length == text.Trim().Length;
        }
    }
}