using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Definitions.French;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.French;

namespace Microsoft.Recognizers.Text.DateTime.French
{
    public class FrenchDatePeriodExtractorConfiguration : BaseDateTimeOptionsConfiguration, IDatePeriodExtractorConfiguration
    {
        // base regexes

        // until
        public static readonly Regex TillRegex =
            RegexCache.Get(DateTimeDefinitions.TillRegex, RegexFlags);

        // and
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

        // this month, next month, last month
        public static readonly Regex RelativeMonthRegex =
            RegexCache.Get(DateTimeDefinitions.RelativeMonthRegex, RegexFlags);

        public static readonly Regex WrittenMonthRegex =
            RegexCache.Get(DateTimeDefinitions.WrittenMonthRegex, RegexFlags);

        // in, of, no "on"...
        public static readonly Regex MonthSuffixRegex =
            RegexCache.Get(DateTimeDefinitions.MonthSuffixRegex, RegexFlags);

        // year, month, week, day
        public static readonly Regex DateUnitRegex = RegexCache.Get(
            DateTimeDefinitions.DateUnitRegex, RegexFlags);

        public static readonly Regex TimeUnitRegex =
            RegexCache.Get(DateTimeDefinitions.TimeUnitRegex, RegexFlags);

        // **In French, Past/Next is suffix, but interface enforces this
        // past, last, previous
        public static readonly Regex PastPrefixRegex =
            RegexCache.Get(DateTimeDefinitions.PastSuffixRegex, RegexFlags);

        // **In French, Past/Next is suffix, but interface enforces this
        // next, in
        public static readonly Regex NextPrefixRegex =
            RegexCache.Get(DateTimeDefinitions.NextSuffixRegex, RegexFlags);

        public static readonly Regex FutureSuffixRegex =
            RegexCache.Get(DateTimeDefinitions.FutureSuffixRegex, RegexFlags);

        public static readonly Regex ThisPrefixRegex =
            RegexCache.Get(DateTimeDefinitions.ThisPrefixRegex, RegexFlags);

        // composite regexes
        public static readonly Regex SimpleCasesRegex =
            RegexCache.Get(DateTimeDefinitions.SimpleCasesRegex, RegexFlags);

        // between 'x' until 'y', from 'x' until 'y'
        public static readonly Regex MonthFrontSimpleCasesRegex =
            RegexCache.Get(DateTimeDefinitions.MonthFrontSimpleCasesRegex, RegexFlags);

        public static readonly Regex MonthFrontBetweenRegex =
            RegexCache.Get(DateTimeDefinitions.MonthFrontBetweenRegex, RegexFlags);

        public static readonly Regex BetweenRegex =
            RegexCache.Get(DateTimeDefinitions.BetweenRegex, RegexFlags);

        public static readonly Regex MonthWithYear =
            RegexCache.Get(DateTimeDefinitions.MonthWithYear, RegexFlags);

        // a cote de - 'next to', cette - 'this', dernier - 'last' (always after the noun, i.e annee dernier - 'last year'
        public static readonly Regex OneWordPeriodRegex =
            RegexCache.Get(DateTimeDefinitions.OneWordPeriodRegex, RegexFlags);

        public static readonly Regex MonthNumWithYear =
            RegexCache.Get(DateTimeDefinitions.MonthNumWithYear, RegexFlags);

        // le/la - masc/fem 'the'
        public static readonly Regex WeekOfMonthRegex =
            RegexCache.Get(DateTimeDefinitions.WeekDayOfMonthRegex, RegexFlags);

        public static readonly Regex WeekOfYearRegex =
            RegexCache.Get(DateTimeDefinitions.WeekOfYearRegex, RegexFlags);

        public static readonly Regex FollowedDateUnit =
            RegexCache.Get(DateTimeDefinitions.FollowedDateUnit, RegexFlags);

        public static readonly Regex NumberCombinedWithDateUnit =
            RegexCache.Get(DateTimeDefinitions.NumberCombinedWithDateUnit, RegexFlags);

        // 1st quarter of this year, 2nd quarter of next/last year, etc
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

        // TODO: add regexs below
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

        public static readonly Regex NowRegex =
            RegexCache.Get(DateTimeDefinitions.NowRegex, RegexFlags);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly Regex FromRegex =
            RegexCache.Get(DateTimeDefinitions.FromRegex, RegexFlags);

        private static readonly Regex ConnectorAndRegex =
            RegexCache.Get(DateTimeDefinitions.ConnectorAndRegex, RegexFlags);

        private static readonly Regex BeforeRegex =
            RegexCache.Get(DateTimeDefinitions.BeforeRegex2, RegexFlags);

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

        public FrenchDatePeriodExtractorConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            DatePointExtractor = new BaseDateExtractor(new FrenchDateExtractorConfiguration(this));
            DurationExtractor = new BaseDurationExtractor(new FrenchDurationExtractorConfiguration(this));

            var numOptions = NumberOptions.None;
            if ((config.Options & DateTimeOptions.NoProtoCache) != 0)
            {
                numOptions = NumberOptions.NoProtoCache;
            }

            var numConfig = new BaseNumberOptionsConfiguration(config.Culture, numOptions);

            CardinalExtractor = Number.French.CardinalExtractor.GetInstance();
            OrdinalExtractor = Number.French.OrdinalExtractor.GetInstance();

            NumberParser = new BaseNumberParser(new FrenchNumberParserConfiguration(numConfig));
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

        bool IDatePeriodExtractorConfiguration.CheckBothBeforeAfter => DateTimeDefinitions.CheckBothBeforeAfter;

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
            return ConnectorAndRegexCache.IsMatch(text);
        }
    }
}
