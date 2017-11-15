using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.French;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime.French
{
    public class FrenchDatePeriodExtractorConfiguration : IDatePeriodExtractorConfiguration
    {
        // base regexes
        public static readonly Regex TillRegex = new Regex(
            DateTimeDefinitions.TillRegex, // until 
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex AndRegex = new Regex(
            DateTimeDefinitions.RangeConnectorRegex, // and 
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex DayRegex =
            new Regex(
                DateTimeDefinitions.DayRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthNumRegex =
            new Regex(
                DateTimeDefinitions.MonthNumRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex YearRegex = 
            new Regex(
                DateTimeDefinitions.YearRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex WeekDayRegex =
            new Regex(
                DateTimeDefinitions.WeekDayRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex RelativeMonthRegex = 
            new Regex(
                DateTimeDefinitions.RelativeMonthRegex, // this month, next month, last month
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex EngMonthRegex =
            new Regex(
                DateTimeDefinitions.EngMonthRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthSuffixRegex =
            new Regex(
                DateTimeDefinitions.MonthSuffixRegex, // in, of, no "on"...
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex DateUnitRegex = new Regex(
            DateTimeDefinitions.DateUnitRegex, // year, month, week, day
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex PastPrefixRegex = // **In French, Past/Next is suffix, but interface enforces this 
            new Regex(
                DateTimeDefinitions.PastSuffixRegex, // past, last, previous
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex NextPrefixRegex = // **In French, Past/Next is suffix, but interface enforces this 
            new Regex(
                DateTimeDefinitions.NextSuffixRegex, // next, in
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex ThisPrefexRegex =
            new Regex(
                DateTimeDefinitions.ThisPrefixRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // composite regexes
        public static readonly Regex SimpleCasesRegex =
            new Regex(
                DateTimeDefinitions.SimpleCasesRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthFrontSimpleCasesRegex = 
            new Regex(
                DateTimeDefinitions.MonthFrontSimpleCasesRegex, // between 'x' until 'y', from 'x' until 'y'
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthFrontBetweenRegex =
            new Regex(
                DateTimeDefinitions.MonthFrontBetweenRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex BetweenRegex =
            new Regex(
                DateTimeDefinitions.BetweenRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthWithYear =
            new Regex(
                DateTimeDefinitions.MonthWithYear,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex OneWordPeriodRegex =
            new Regex(
               DateTimeDefinitions.OneWordPeriodRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline); // a cote de - 'next to', cette - 'this', dernier - 'last' (always after the noun, i.e annee dernier - 'last year'  

        public static readonly Regex MonthNumWithYear =
            new Regex(
                DateTimeDefinitions.MonthNumWithYear,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex WeekOfMonthRegex =
            new Regex(
                DateTimeDefinitions.WeekDayOfMonthRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline); // le/la - masc/fem 'the'

        public static readonly Regex WeekOfYearRegex =
            new Regex(
                DateTimeDefinitions.WeekOfYearRegex, 
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex FollowedDateUnit = 
            new Regex(
                DateTimeDefinitions.FollowedDateUnit,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex NumberCombinedWithDateUnit =
            new Regex(
                DateTimeDefinitions.NumberCombinedWithDateUnit, 
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex QuarterRegex =
            new Regex(
                DateTimeDefinitions.QuarterRegex, // 1st quarter of this year, 2nd quarter of next/last year, etc 
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex QuarterRegexYearFront =
            new Regex(
                DateTimeDefinitions.QuarterRegexYearFront,
                RegexOptions.IgnoreCase | RegexOptions.Singleline); 

        public static readonly Regex SeasonRegex =
            new Regex(
                DateTimeDefinitions.SeasonRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex WhichWeekRegex =
            new Regex(
                DateTimeDefinitions.WhichWeekRegex, 
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex WeekOfRegex =
            new Regex(
                DateTimeDefinitions.WeekOfRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthOfRegex =
            new Regex(
                DateTimeDefinitions.MonthOfRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        //TODO: add regexs below
        public static readonly Regex RangeUnitRegex =
            new Regex(
                DateTimeDefinitions.RangeUnitRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex InConnectorRegex =
            new Regex(
                DateTimeDefinitions.InConnectorRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex RestOfDateRegex =
            new Regex(DateTimeDefinitions.RestOfDateRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex LaterEarlyPeriodRegex =
            new Regex(DateTimeDefinitions.LaterEarlyPeriodRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex WeekWithWeekDayRangeRegex =
            new Regex(DateTimeDefinitions.WeekWithWeekDayRangeRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private static readonly Regex FromRegex = new Regex(DateTimeDefinitions.FromRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly Regex ConnectorAndRegex = new Regex(DateTimeDefinitions.ConnectorAndRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly Regex BeforeRegex = new Regex(DateTimeDefinitions.BeforeRegex2, RegexOptions.IgnoreCase | RegexOptions.Singleline);


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
            PastPrefixRegex,
            NextPrefixRegex,
            ThisPrefexRegex,
            LaterEarlyPeriodRegex,
            WeekWithWeekDayRangeRegex,
        };

        public FrenchDatePeriodExtractorConfiguration()
        {
            DatePointExtractor = new BaseDateExtractor(new FrenchDateExtractorConfiguration());
            CardinalExtractor = Number.French.CardinalExtractor.GetInstance();
            DurationExtractor = new BaseDurationExtractor(new FrenchDurationExtractorConfiguration());
        }

        public IDateTimeExtractor DatePointExtractor { get; }

        public IExtractor CardinalExtractor { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        IEnumerable<Regex> IDatePeriodExtractorConfiguration.SimpleCasesRegexes => SimpleCasesRegexes;

        Regex IDatePeriodExtractorConfiguration.TillRegex => TillRegex;

        Regex IDatePeriodExtractorConfiguration.DateUnitRegex => DateUnitRegex;

        Regex IDatePeriodExtractorConfiguration.FollowedDateUnit => FollowedDateUnit;

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
            var fromMatch = FromRegex.Match(text);
            if (fromMatch.Success)
            {
                index = fromMatch.Index;
            }
            return fromMatch.Success;
        }

        public bool GetBetweenTokenIndex(string text, out int index)
        {
            index = -1;
            var beforeMatch = BeforeRegex.Match(text);
            if (beforeMatch.Success)
            {
                index = beforeMatch.Index;
            }
            return beforeMatch.Success;
        }

        public bool HasConnectorToken(string text)
        {
            return ConnectorAndRegex.IsMatch(text);
        }
    }
}
