// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Definitions.Italian;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Italian;
using Microsoft.Recognizers.Text.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Italian
{
    public class ItalianDatePeriodExtractorConfiguration : BaseDateTimeOptionsConfiguration, IDatePeriodExtractorConfiguration
    {
        // base regexes

        // until
        public static readonly Regex TillRegex =
            new Regex(DateTimeDefinitions.RestrictedTillRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex FullTillRegex =
            new Regex(DateTimeDefinitions.TillRegex, RegexFlags, RegexTimeOut);

        // and
        public static readonly Regex RangeConnectorRegex =
            new Regex(DateTimeDefinitions.RangeConnectorRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex DayRegex =
            new Regex(DateTimeDefinitions.DayRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex MonthNumRegex =
            new Regex(DateTimeDefinitions.MonthNumRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex IllegalYearRegex =
            new Regex(BaseDateTime.IllegalYearRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex YearRegex =
            new Regex(DateTimeDefinitions.YearRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex WeekDayRegex =
            new Regex(DateTimeDefinitions.WeekDayRegex, RegexFlags, RegexTimeOut);

        // this month, next month, last month
        public static readonly Regex RelativeMonthRegex =
            new Regex(DateTimeDefinitions.RelativeMonthRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex TimeUnitRegex =
           new Regex(DateTimeDefinitions.TimeUnitRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex EngMonthRegex =
            new Regex(DateTimeDefinitions.EngMonthRegex, RegexFlags, RegexTimeOut);

        // in, of, no "on"...
        public static readonly Regex MonthSuffixRegex =
            new Regex(DateTimeDefinitions.MonthSuffixRegex, RegexFlags, RegexTimeOut);

        // year, month, week, day
        public static readonly Regex DateUnitRegex =
            new Regex(DateTimeDefinitions.DateUnitRegex, RegexFlags, RegexTimeOut);

        // **In Italian, Past/Next is suffix, but interface enforces this
        // past, last, previous
        public static readonly Regex PastPrefixRegex =
            new Regex(DateTimeDefinitions.PastSuffixRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex PreviousPrefixRegex =
            new Regex(DateTimeDefinitions.PreviousPrefixRegex, RegexFlags, RegexTimeOut);

        // **In Italian, Past/Next is suffix, but interface enforces this
        // next, in
        public static readonly Regex NextPrefixRegex =
            new Regex(DateTimeDefinitions.NextPrefixRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex FutureSuffixRegex =
            new Regex(DateTimeDefinitions.FutureSuffixRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex ThisPrefixRegex =
            new Regex(DateTimeDefinitions.ThisPrefixRegex, RegexFlags, RegexTimeOut);

        // composite regexes
        public static readonly Regex SimpleCasesRegex =
            new Regex(DateTimeDefinitions.SimpleCasesRegex, RegexFlags, RegexTimeOut);

        // between 'x' until 'y', from 'x' until 'y'
        public static readonly Regex MonthFrontSimpleCasesRegex =
            new Regex(DateTimeDefinitions.MonthFrontSimpleCasesRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex MonthFrontBetweenRegex =
            new Regex(DateTimeDefinitions.MonthFrontBetweenRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex BetweenRegex =
            new Regex(DateTimeDefinitions.BetweenRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex MonthWithYear =
            new Regex(DateTimeDefinitions.MonthWithYear, RegexFlags, RegexTimeOut);

        // @TODO localize comment?
        // a cote de - 'next to', cette - 'this', dernier - 'last' (always after the noun, i.e annee dernier - 'last year'
        public static readonly Regex OneWordPeriodRegex =
            new Regex(DateTimeDefinitions.OneWordPeriodRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex MonthNumWithYear =
            new Regex(DateTimeDefinitions.MonthNumWithYear, RegexFlags, RegexTimeOut);

        // le/la - masc/fem 'the'
        public static readonly Regex WeekOfMonthRegex =
            new Regex(DateTimeDefinitions.WeekOfMonthRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex WeekOfYearRegex =
            new Regex(DateTimeDefinitions.WeekOfYearRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex FollowedDateUnit =
            new Regex(DateTimeDefinitions.FollowedDateUnit, RegexFlags, RegexTimeOut);

        public static readonly Regex NumberCombinedWithDateUnit =
            new Regex(DateTimeDefinitions.NumberCombinedWithDateUnit, RegexFlags, RegexTimeOut);

        // 1st quarter of this year, 2nd quarter of next/last year, etc
        public static readonly Regex QuarterRegex =
            new Regex(DateTimeDefinitions.QuarterRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex QuarterRegexYearFront =
            new Regex(DateTimeDefinitions.QuarterRegexYearFront, RegexFlags, RegexTimeOut);

        public static readonly Regex AllHalfYearRegex =
            new Regex(DateTimeDefinitions.AllHalfYearRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex SeasonRegex =
            new Regex(DateTimeDefinitions.SeasonRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex WhichWeekRegex =
            new Regex(DateTimeDefinitions.WhichWeekRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex WeekOfRegex =
            new Regex(DateTimeDefinitions.WeekOfRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex MonthOfRegex =
            new Regex(DateTimeDefinitions.MonthOfRegex, RegexFlags, RegexTimeOut);

        // TODO: add regexs below
        public static readonly Regex RangeUnitRegex =
            new Regex(DateTimeDefinitions.RangeUnitRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex InConnectorRegex =
            new Regex(DateTimeDefinitions.InConnectorRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex WithinNextPrefixRegex =
          new Regex(DateTimeDefinitions.WithinNextPrefixRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex RestOfDateRegex =
            new Regex(DateTimeDefinitions.RestOfDateRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex LaterEarlyPeriodRegex =
            new Regex(DateTimeDefinitions.LaterEarlyPeriodRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex WeekWithWeekDayRangeRegex =
            new Regex(DateTimeDefinitions.WeekWithWeekDayRangeRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex YearPlusNumberRegex =
           new Regex(DateTimeDefinitions.YearPlusNumberRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex DecadeWithCenturyRegex =
            new Regex(DateTimeDefinitions.DecadeWithCenturyRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex YearPeriodRegex =
            new Regex(DateTimeDefinitions.YearPeriodRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex ComplexDatePeriodRegex =
            new Regex(DateTimeDefinitions.ComplexDatePeriodRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex RelativeDecadeRegex =
            new Regex(DateTimeDefinitions.RelativeDecadeRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex ReferenceDatePeriodRegex =
            new Regex(DateTimeDefinitions.ReferenceDatePeriodRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex AgoRegex =
            new Regex(DateTimeDefinitions.AgoRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex LaterRegex =
            new Regex(DateTimeDefinitions.LaterRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex LessThanRegex =
            new Regex(DateTimeDefinitions.LessThanRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex MoreThanRegex =
            new Regex(DateTimeDefinitions.MoreThanRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex CenturySuffixRegex =
            new Regex(DateTimeDefinitions.CenturySuffixRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex NowRegex =
            new Regex(DateTimeDefinitions.NowRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex FirstLastRegex =
            new Regex(DateTimeDefinitions.FirstLastRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex OfYearRegex =
            new Regex(DateTimeDefinitions.OfYearRegex, RegexFlags, RegexTimeOut);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly Regex FromRegex =
            new Regex(DateTimeDefinitions.FromRegex, RegexFlags, RegexTimeOut);

        private static readonly Regex ConnectorAndRegex =
            new Regex(DateTimeDefinitions.ConnectorAndRegex, RegexFlags, RegexTimeOut);

        private static readonly Regex RangePrefixRegex =
            new Regex(DateTimeDefinitions.RangePrefixRegex, RegexFlags, RegexTimeOut);

        private static readonly Regex BeforeRegex =
            new Regex(DateTimeDefinitions.BeforeRegex, RegexFlags, RegexTimeOut);

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
            AllHalfYearRegex,
            SeasonRegex,
            WhichWeekRegex,
            RestOfDateRegex,
            LaterEarlyPeriodRegex,
            WeekWithWeekDayRangeRegex,
            YearPlusNumberRegex,
            DecadeWithCenturyRegex,
            RelativeDecadeRegex,
            ReferenceDatePeriodRegex,
        };

        public ItalianDatePeriodExtractorConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            DatePointExtractor = new BaseDateExtractor(new ItalianDateExtractorConfiguration(this));
            DurationExtractor = new BaseDurationExtractor(new ItalianDurationExtractorConfiguration(this));

            var numOptions = NumberOptions.None;
            if ((config.Options & DateTimeOptions.NoProtoCache) != 0)
            {
                numOptions = NumberOptions.NoProtoCache;
            }

            var numConfig = new BaseNumberOptionsConfiguration(config.Culture, numOptions);

            CardinalExtractor = Number.Italian.CardinalExtractor.GetInstance();
            OrdinalExtractor = Number.Italian.OrdinalExtractor.GetInstance(numConfig);

            NumberParser = new BaseNumberParser(new ItalianNumberParserConfiguration(numConfig));
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

        Regex IDatePeriodExtractorConfiguration.FirstLastRegex => FirstLastRegex;

        Regex IDatePeriodExtractorConfiguration.OfYearRegex => OfYearRegex;

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
            var beforeMatch = BeforeRegex.MatchEnd(text, false);
            var fromMatch = RangePrefixRegex.MatchEnd(text, false);

            if (beforeMatch.Success)
            {
                index = beforeMatch.Index;
            }
            else if (fromMatch.Success)
            {
                index = fromMatch.Index;

                return fromMatch.Success;
            }

            return beforeMatch.Success;
        }

        public bool HasConnectorToken(string text)
        {
            return ConnectorAndRegex.IsMatch(text) || FullTillRegex.IsMatch(text);
        }
    }
}
