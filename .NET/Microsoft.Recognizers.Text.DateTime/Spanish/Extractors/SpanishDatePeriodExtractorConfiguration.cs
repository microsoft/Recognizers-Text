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
            RegexCache.Get(DateTimeDefinitions.TillRegex, RegexFlags);

        public static readonly Regex DayRegex =
            RegexCache.Get(DateTimeDefinitions.DayRegex, RegexFlags);

        public static readonly Regex MonthNumRegex =
            RegexCache.Get(DateTimeDefinitions.MonthNumRegex, RegexFlags);

        public static readonly Regex IllegalYearRegex =
            RegexCache.Get(BaseDateTime.IllegalYearRegex, RegexFlags);

        public static readonly Regex YearRegex =
            RegexCache.Get(DateTimeDefinitions.YearRegex, RegexFlags);

        public static readonly Regex RelativeMonthRegex =
            RegexCache.Get(DateTimeDefinitions.RelativeMonthRegex, RegexFlags);

        public static readonly Regex MonthRegex =
            RegexCache.Get(DateTimeDefinitions.MonthRegex, RegexFlags);

        public static readonly Regex MonthSuffixRegex =
            RegexCache.Get(DateTimeDefinitions.MonthSuffixRegex, RegexFlags);

        public static readonly Regex DateUnitRegex =
            RegexCache.Get(DateTimeDefinitions.DateUnitRegex, RegexFlags);

        public static readonly Regex TimeUnitRegex =
            RegexCache.Get(DateTimeDefinitions.TimeUnitRegex, RegexFlags);

        public static readonly Regex PastRegex =
            RegexCache.Get(DateTimeDefinitions.PastRegex, RegexFlags);

        public static readonly Regex FutureRegex =
            RegexCache.Get(DateTimeDefinitions.FutureRegex, RegexFlags);

        public static readonly Regex FutureSuffixRegex =
            RegexCache.Get(DateTimeDefinitions.FutureSuffixRegex, RegexFlags);

        // composite regexes
        public static readonly Regex SimpleCasesRegex =
            RegexCache.Get(DateTimeDefinitions.SimpleCasesRegex, RegexFlags);

        public static readonly Regex MonthFrontSimpleCasesRegex =
            RegexCache.Get(DateTimeDefinitions.MonthFrontSimpleCasesRegex, RegexFlags);

        public static readonly Regex MonthFrontBetweenRegex =
            RegexCache.Get(DateTimeDefinitions.MonthFrontBetweenRegex, RegexFlags);

        public static readonly Regex DayBetweenRegex =
            RegexCache.Get(DateTimeDefinitions.DayBetweenRegex, RegexFlags);

        // TODO: modify it according to the related regex in English
        public static readonly Regex OneWordPeriodRegex =
            RegexCache.Get(DateTimeDefinitions.OneWordPeriodRegex, RegexFlags);

        public static readonly Regex MonthWithYearRegex =
            RegexCache.Get(DateTimeDefinitions.MonthWithYearRegex, RegexFlags);

        public static readonly Regex MonthNumWithYearRegex =
            RegexCache.Get(DateTimeDefinitions.MonthNumWithYearRegex, RegexFlags);

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

        public static readonly Regex LaterEarlyPeriodRegex =
            RegexCache.Get(DateTimeDefinitions.LaterEarlyPeriodRegex, RegexFlags);

        // TODO: add this regex, let it correspond to the one in English
        public static readonly Regex RestOfDateRegex =
            RegexCache.Get(DateTimeDefinitions.RestOfDateRegex, RegexFlags);

        // TODO: add this regex, let it correspond to the one in English
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

        private static readonly Regex RangeConnectorRegex =
            RegexCache.Get(DateTimeDefinitions.RangeConnectorRegex, RegexFlags);

        private static readonly Regex BetweenRegex =
            RegexCache.Get(DateTimeDefinitions.BetweenRegex, RegexFlags);

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
