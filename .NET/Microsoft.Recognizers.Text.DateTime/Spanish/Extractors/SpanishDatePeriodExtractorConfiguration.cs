// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Definitions.Spanish;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Spanish;
using Microsoft.Recognizers.Text.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public class SpanishDatePeriodExtractorConfiguration : BaseDateTimeOptionsConfiguration, IDatePeriodExtractorConfiguration
    {
        // base regexes
        public static readonly Regex TillRegex =
            new Regex(DateTimeDefinitions.TillRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex DayRegex =
            new Regex(DateTimeDefinitions.DayRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex MonthNumRegex =
            new Regex(DateTimeDefinitions.MonthNumRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex IllegalYearRegex =
            new Regex(BaseDateTime.IllegalYearRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex YearRegex =
            new Regex(DateTimeDefinitions.YearRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex RelativeMonthRegex =
            new Regex(DateTimeDefinitions.RelativeMonthRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex MonthRegex =
            new Regex(DateTimeDefinitions.MonthRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex MonthSuffixRegex =
            new Regex(DateTimeDefinitions.MonthSuffixRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex DateUnitRegex =
            new Regex(DateTimeDefinitions.DateUnitRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex TimeUnitRegex =
            new Regex(DateTimeDefinitions.TimeUnitRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex PastRegex =
            new Regex(DateTimeDefinitions.PastRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex FutureRegex =
            new Regex(DateTimeDefinitions.FutureRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex FutureSuffixRegex =
            new Regex(DateTimeDefinitions.FutureSuffixRegex, RegexFlags, RegexTimeOut);

        // composite regexes
        public static readonly Regex SimpleCasesRegex =
            new Regex(DateTimeDefinitions.SimpleCasesRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex MonthFrontSimpleCasesRegex =
            new Regex(DateTimeDefinitions.MonthFrontSimpleCasesRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex MonthFrontBetweenRegex =
            new Regex(DateTimeDefinitions.MonthFrontBetweenRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex DayBetweenRegex =
            new Regex(DateTimeDefinitions.DayBetweenRegex, RegexFlags, RegexTimeOut);

        // TODO: modify it according to the related regex in English
        public static readonly Regex OneWordPeriodRegex =
            new Regex(DateTimeDefinitions.OneWordPeriodRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex MonthWithYearRegex =
            new Regex(DateTimeDefinitions.MonthWithYearRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex MonthNumWithYearRegex =
            new Regex(DateTimeDefinitions.MonthNumWithYearRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex WeekOfMonthRegex =
            new Regex(DateTimeDefinitions.WeekOfMonthRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex WeekOfYearRegex =
            new Regex(DateTimeDefinitions.WeekOfYearRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex FollowedDateUnit =
            new Regex(DateTimeDefinitions.FollowedDateUnit, RegexFlags, RegexTimeOut);

        public static readonly Regex NumberCombinedWithDateUnit =
            new Regex(DateTimeDefinitions.NumberCombinedWithDateUnit, RegexFlags, RegexTimeOut);

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

        public static readonly Regex RangeUnitRegex =
            new Regex(DateTimeDefinitions.RangeUnitRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex InConnectorRegex =
            new Regex(DateTimeDefinitions.InConnectorRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex WithinNextPrefixRegex =
            new Regex(DateTimeDefinitions.WithinNextPrefixRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex LaterEarlyPeriodRegex =
            new Regex(DateTimeDefinitions.LaterEarlyPeriodRegex, RegexFlags, RegexTimeOut);

        // TODO: add this regex, let it correspond to the one in English
        public static readonly Regex RestOfDateRegex =
            new Regex(DateTimeDefinitions.RestOfDateRegex, RegexFlags, RegexTimeOut);

        // TODO: add this regex, let it correspond to the one in English
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

        private static readonly Regex RangeConnectorRegex =
            new Regex(DateTimeDefinitions.RangeConnectorRegex, RegexFlags, RegexTimeOut);

        private static readonly Regex BetweenRegex =
            new Regex(DateTimeDefinitions.BetweenRegex, RegexFlags, RegexTimeOut);

        private static readonly Regex[] SimpleCasesRegexes =
        {
            SimpleCasesRegex,
            DayBetweenRegex,
            OneWordPeriodRegex,
            MonthWithYearRegex,
            MonthNumWithYearRegex,
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
            RestOfDateRegex,
            LaterEarlyPeriodRegex,
            WeekWithWeekDayRangeRegex,
            YearPlusNumberRegex,
            DecadeWithCenturyRegex,
            RelativeDecadeRegex,
            WhichWeekRegex,
            ReferenceDatePeriodRegex,
        };

        public SpanishDatePeriodExtractorConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            DatePointExtractor = new BaseDateExtractor(new SpanishDateExtractorConfiguration(this));
            DurationExtractor = new BaseDurationExtractor(new SpanishDurationExtractorConfiguration(this));

            var numOptions = NumberOptions.None;
            if ((config.Options & DateTimeOptions.NoProtoCache) != 0)
            {
                numOptions = NumberOptions.NoProtoCache;
            }

            var numConfig = new BaseNumberOptionsConfiguration(config.Culture, numOptions);

            CardinalExtractor = Number.Spanish.CardinalExtractor.GetInstance(numConfig);
            OrdinalExtractor = Number.Spanish.OrdinalExtractor.GetInstance(numConfig);

            NumberParser = new BaseNumberParser(new SpanishNumberParserConfiguration(numConfig));
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

        Regex IDatePeriodExtractorConfiguration.PreviousPrefixRegex => PastRegex;

        Regex IDatePeriodExtractorConfiguration.FutureRegex => FutureRegex;

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

            var match = BetweenRegex.Match(text);
            if (match.Success)
            {
                index = match.Index;
            }

            return match.Success;
        }

        public bool HasConnectorToken(string text)
        {
            return RangeConnectorRegex.IsExactMatch(text, true);
        }
    }
}
