using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Definitions.French;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.French;

namespace Microsoft.Recognizers.Text.DateTime.French
{
    public class FrenchDatePeriodExtractorConfiguration : BaseOptionsConfiguration, IDatePeriodExtractorConfiguration
    {
        // base regexes

        // until
        public static readonly Regex TillRegex =
            new Regex(DateTimeDefinitions.TillRegex, RegexFlags);

        // and
        public static readonly Regex RangeConnectorRegex =
            new Regex(DateTimeDefinitions.RangeConnectorRegex, RegexFlags);

        public static readonly Regex DayRegex =
            new Regex(DateTimeDefinitions.DayRegex, RegexFlags);

        public static readonly Regex MonthNumRegex =
            new Regex(DateTimeDefinitions.MonthNumRegex, RegexFlags);

        public static readonly Regex IllegalYearRegex =
            new Regex(BaseDateTime.IllegalYearRegex, RegexFlags);

        public static readonly Regex YearRegex =
            new Regex(DateTimeDefinitions.YearRegex, RegexFlags);

        public static readonly Regex WeekDayRegex =
            new Regex(DateTimeDefinitions.WeekDayRegex, RegexFlags);

        // this month, next month, last month
        public static readonly Regex RelativeMonthRegex =
            new Regex(DateTimeDefinitions.RelativeMonthRegex, RegexFlags);

        public static readonly Regex WrittenMonthRegex =
            new Regex(DateTimeDefinitions.WrittenMonthRegex, RegexFlags);

        // in, of, no "on"...
        public static readonly Regex MonthSuffixRegex =
            new Regex(DateTimeDefinitions.MonthSuffixRegex, RegexFlags);

        // year, month, week, day
        public static readonly Regex DateUnitRegex = new Regex(
            DateTimeDefinitions.DateUnitRegex, RegexFlags);

        public static readonly Regex TimeUnitRegex =
            new Regex(DateTimeDefinitions.TimeUnitRegex, RegexFlags);

        // **In French, Past/Next is suffix, but interface enforces this
        // past, last, previous
        public static readonly Regex PastPrefixRegex =
            new Regex(DateTimeDefinitions.PastSuffixRegex, RegexFlags);

        // **In French, Past/Next is suffix, but interface enforces this
        // next, in
        public static readonly Regex NextPrefixRegex =
            new Regex(DateTimeDefinitions.NextSuffixRegex, RegexFlags);

        public static readonly Regex FutureSuffixRegex =
            new Regex(DateTimeDefinitions.FutureSuffixRegex, RegexFlags);

        public static readonly Regex ThisPrefixRegex =
            new Regex(DateTimeDefinitions.ThisPrefixRegex, RegexFlags);

        // composite regexes
        public static readonly Regex SimpleCasesRegex =
            new Regex(DateTimeDefinitions.SimpleCasesRegex, RegexFlags);

        // between 'x' until 'y', from 'x' until 'y'
        public static readonly Regex MonthFrontSimpleCasesRegex =
            new Regex(DateTimeDefinitions.MonthFrontSimpleCasesRegex, RegexFlags);

        public static readonly Regex MonthFrontBetweenRegex =
            new Regex(DateTimeDefinitions.MonthFrontBetweenRegex, RegexFlags);

        public static readonly Regex BetweenRegex =
            new Regex(DateTimeDefinitions.BetweenRegex, RegexFlags);

        public static readonly Regex MonthWithYear =
            new Regex(DateTimeDefinitions.MonthWithYear, RegexFlags);

        // a cote de - 'next to', cette - 'this', dernier - 'last' (always after the noun, i.e annee dernier - 'last year'
        public static readonly Regex OneWordPeriodRegex =
            new Regex(DateTimeDefinitions.OneWordPeriodRegex, RegexFlags);

        public static readonly Regex MonthNumWithYear =
            new Regex(DateTimeDefinitions.MonthNumWithYear, RegexFlags);

        // le/la - masc/fem 'the'
        public static readonly Regex WeekOfMonthRegex =
            new Regex(DateTimeDefinitions.WeekDayOfMonthRegex, RegexFlags);

        public static readonly Regex WeekOfYearRegex =
            new Regex(DateTimeDefinitions.WeekOfYearRegex, RegexFlags);

        public static readonly Regex FollowedDateUnit =
            new Regex(DateTimeDefinitions.FollowedDateUnit, RegexFlags);

        public static readonly Regex NumberCombinedWithDateUnit =
            new Regex(DateTimeDefinitions.NumberCombinedWithDateUnit, RegexFlags);

        // 1st quarter of this year, 2nd quarter of next/last year, etc
        public static readonly Regex QuarterRegex =
            new Regex(DateTimeDefinitions.QuarterRegex, RegexFlags);

        public static readonly Regex QuarterRegexYearFront =
            new Regex(DateTimeDefinitions.QuarterRegexYearFront, RegexFlags);

        public static readonly Regex AllHalfYearRegex =
            new Regex(DateTimeDefinitions.AllHalfYearRegex, RegexFlags);

        public static readonly Regex SeasonRegex =
            new Regex(DateTimeDefinitions.SeasonRegex, RegexFlags);

        public static readonly Regex WhichWeekRegex =
            new Regex(DateTimeDefinitions.WhichWeekRegex, RegexFlags);

        public static readonly Regex WeekOfRegex =
            new Regex(DateTimeDefinitions.WeekOfRegex, RegexFlags);

        public static readonly Regex MonthOfRegex =
            new Regex(DateTimeDefinitions.MonthOfRegex, RegexFlags);

        // TODO: add regexs below
        public static readonly Regex RangeUnitRegex =
            new Regex(DateTimeDefinitions.RangeUnitRegex, RegexFlags);

        public static readonly Regex InConnectorRegex =
            new Regex(DateTimeDefinitions.InConnectorRegex, RegexFlags);

        public static readonly Regex WithinNextPrefixRegex =
            new Regex(DateTimeDefinitions.WithinNextPrefixRegex, RegexFlags);

        public static readonly Regex RestOfDateRegex =
            new Regex(DateTimeDefinitions.RestOfDateRegex, RegexFlags);

        public static readonly Regex LaterEarlyPeriodRegex =
            new Regex(DateTimeDefinitions.LaterEarlyPeriodRegex, RegexFlags);

        public static readonly Regex WeekWithWeekDayRangeRegex =
            new Regex(DateTimeDefinitions.WeekWithWeekDayRangeRegex, RegexFlags);

        public static readonly Regex YearPlusNumberRegex =
           new Regex(DateTimeDefinitions.YearPlusNumberRegex, RegexFlags);

        public static readonly Regex DecadeWithCenturyRegex =
            new Regex(DateTimeDefinitions.DecadeWithCenturyRegex, RegexFlags);

        public static readonly Regex YearPeriodRegex =
            new Regex(DateTimeDefinitions.YearPeriodRegex, RegexFlags);

        public static readonly Regex ComplexDatePeriodRegex =
            new Regex(DateTimeDefinitions.ComplexDatePeriodRegex, RegexFlags);

        public static readonly Regex RelativeDecadeRegex =
            new Regex(DateTimeDefinitions.RelativeDecadeRegex, RegexFlags);

        public static readonly Regex ReferenceDatePeriodRegex =
            new Regex(DateTimeDefinitions.ReferenceDatePeriodRegex, RegexFlags);

        public static readonly Regex AgoRegex =
                    new Regex(DateTimeDefinitions.AgoRegex, RegexFlags);

        public static readonly Regex LaterRegex =
            new Regex(DateTimeDefinitions.LaterRegex, RegexFlags);

        public static readonly Regex LessThanRegex =
            new Regex(DateTimeDefinitions.LessThanRegex, RegexFlags);

        public static readonly Regex MoreThanRegex =
            new Regex(DateTimeDefinitions.MoreThanRegex, RegexFlags);

        public static readonly Regex CenturySuffixRegex =
            new Regex(DateTimeDefinitions.CenturySuffixRegex, RegexFlags);

        public static readonly Regex NowRegex =
            new Regex(DateTimeDefinitions.NowRegex, RegexFlags);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly Regex FromRegex =
            new Regex(DateTimeDefinitions.FromRegex, RegexFlags);

        private static readonly Regex ConnectorAndRegex =
            new Regex(DateTimeDefinitions.ConnectorAndRegex, RegexFlags);

        private static readonly Regex BeforeRegex =
            new Regex(DateTimeDefinitions.BeforeRegex2, RegexFlags);

        private static readonly Regex[] SimpleCasesRegexes =
        {
            SimpleCasesRegex,
            BetweenRegex,
            OneWordPeriodRegex,
            MonthWithYear,
            MonthNumWithYear,
            YearRegex,
            YearPeriodRegex,
            WeekOfMonthRegex,
            WeekOfYearRegex,
            MonthFrontBetweenRegex,
            MonthFrontSimpleCasesRegex,
            QuarterRegex,
            QuarterRegexYearFront,
            SeasonRegex,
            LaterEarlyPeriodRegex,
            WeekWithWeekDayRangeRegex,
            YearPlusNumberRegex,
            DecadeWithCenturyRegex,
            RelativeDecadeRegex,
        };

        public FrenchDatePeriodExtractorConfiguration(IOptionsConfiguration config)
            : base(config)
        {
            DatePointExtractor = new BaseDateExtractor(new FrenchDateExtractorConfiguration(this));
            CardinalExtractor = Number.French.CardinalExtractor.GetInstance();
            OrdinalExtractor = Number.French.OrdinalExtractor.GetInstance();
            DurationExtractor = new BaseDurationExtractor(new FrenchDurationExtractorConfiguration(this));
            NumberParser = new BaseNumberParser(new FrenchNumberParserConfiguration());
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

        Regex IDatePeriodExtractorConfiguration.DateUnitRegex => DateUnitRegex;

        Regex IDatePeriodExtractorConfiguration.TimeUnitRegex => TimeUnitRegex;

        Regex IDatePeriodExtractorConfiguration.FollowedDateUnit => FollowedDateUnit;

        Regex IDatePeriodExtractorConfiguration.NumberCombinedWithDateUnit => NumberCombinedWithDateUnit;

        Regex IDatePeriodExtractorConfiguration.PreviousPrefixRegex => PastPrefixRegex;

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

        string[] IDatePeriodExtractorConfiguration.DurationDateRestrictions => DateTimeDefinitions.DurationDateRestrictions;

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
