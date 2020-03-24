using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Definitions.German;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.German;
using Microsoft.Recognizers.Text.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.German
{
    public class GermanDatePeriodExtractorConfiguration : BaseDateTimeOptionsConfiguration, IDatePeriodExtractorConfiguration
    {
        // base regexes
        public static readonly Regex TillRegex =
            new Regex(DateTimeDefinitions.TillRegex, RegexFlags);

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

        public static readonly Regex RelativeMonthRegex =
            new Regex(DateTimeDefinitions.RelativeMonthRegex, RegexFlags);

        public static readonly Regex WrittenMonthRegex =
            new Regex(DateTimeDefinitions.WrittenMonthRegex, RegexFlags);

        public static readonly Regex MonthSuffixRegex =
            new Regex(DateTimeDefinitions.MonthSuffixRegex, RegexFlags);

        public static readonly Regex DateUnitRegex =
            new Regex(DateTimeDefinitions.DateUnitRegex, RegexFlags);

        public static readonly Regex TimeUnitRegex =
            new Regex(DateTimeDefinitions.TimeUnitRegex, RegexFlags);

        public static readonly Regex PreviousPrefixRegex =
            new Regex(DateTimeDefinitions.PreviousPrefixRegex, RegexFlags);

        public static readonly Regex NextPrefixRegex =
            new Regex(DateTimeDefinitions.NextPrefixRegex, RegexFlags);

        public static readonly Regex FutureSuffixRegex =
            new Regex(DateTimeDefinitions.FutureSuffixRegex, RegexFlags);

        // composite regexes
        public static readonly Regex SimpleCasesRegex =
            new Regex(DateTimeDefinitions.SimpleCasesRegex, RegexFlags);

        public static readonly Regex MonthFrontSimpleCasesRegex =
            new Regex(DateTimeDefinitions.MonthFrontSimpleCasesRegex, RegexFlags);

        public static readonly Regex MonthFrontBetweenRegex =
            new Regex(DateTimeDefinitions.MonthFrontBetweenRegex, RegexFlags);

        public static readonly Regex BetweenRegex =
            new Regex(DateTimeDefinitions.BetweenRegex, RegexFlags);

        public static readonly Regex MonthWithYear =
            new Regex(DateTimeDefinitions.MonthWithYear, RegexFlags);

        public static readonly Regex OneWordPeriodRegex =
            new Regex(DateTimeDefinitions.OneWordPeriodRegex, RegexFlags);

        public static readonly Regex MonthNumWithYear =
            new Regex(DateTimeDefinitions.MonthNumWithYear, RegexFlags);

        public static readonly Regex WeekOfMonthRegex =
            new Regex(DateTimeDefinitions.WeekOfMonthRegex, RegexFlags);

        public static readonly Regex WeekOfYearRegex =
            new Regex(DateTimeDefinitions.WeekOfYearRegex, RegexFlags);

        public static readonly Regex FollowedDateUnit =
            new Regex(DateTimeDefinitions.FollowedDateUnit, RegexFlags);

        public static readonly Regex NumberCombinedWithDateUnit =
            new Regex(DateTimeDefinitions.NumberCombinedWithDateUnit, RegexFlags);

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
            WhichWeekRegex,
            RestOfDateRegex,
            LaterEarlyPeriodRegex,
            WeekWithWeekDayRangeRegex,
            YearPlusNumberRegex,
            DecadeWithCenturyRegex,
            RelativeDecadeRegex,
            ReferenceDatePeriodRegex,
        };

        public GermanDatePeriodExtractorConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            DatePointExtractor = new BaseDateExtractor(new GermanDateExtractorConfiguration(this));
            DurationExtractor = new BaseDurationExtractor(new GermanDurationExtractorConfiguration(this));

            var numOptions = NumberOptions.None;
            if ((config.Options & DateTimeOptions.NoProtoCache) != 0)
            {
                numOptions = NumberOptions.NoProtoCache;
            }

            var numConfig = new BaseNumberOptionsConfiguration(config.Culture, numOptions);

            CardinalExtractor = Number.German.CardinalExtractor.GetInstance();
            OrdinalExtractor = Number.German.OrdinalExtractor.GetInstance();

            NumberParser = new BaseNumberParser(new GermanNumberParserConfiguration(numConfig));
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
            if (text.EndsWith("vom"))
            {
                index = text.LastIndexOf("vom", StringComparison.Ordinal);
                return true;
            }

            return false;
        }

        public bool GetBetweenTokenIndex(string text, out int index)
        {
            index = -1;
            if (text.EndsWith("zwischen"))
            {
                index = text.LastIndexOf("zwischen", StringComparison.Ordinal);
                return true;
            }

            return false;
        }

        public bool HasConnectorToken(string text)
        {
            return RangeConnectorRegex.IsExactMatch(text, trim: true);
        }
    }
}