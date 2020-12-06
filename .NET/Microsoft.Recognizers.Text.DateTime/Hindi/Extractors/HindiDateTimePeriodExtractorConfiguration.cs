using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Definitions.Hindi;
using Microsoft.Recognizers.Text.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Hindi
{
    public class HindiDateTimePeriodExtractorConfiguration : BaseDateTimeOptionsConfiguration, IDateTimePeriodExtractorConfiguration
    {
        public static readonly Regex TimeNumberCombinedWithUnit =
            RegexCache.Get(DateTimeDefinitions.TimeNumberCombinedWithUnit, RegexFlags);

        public static readonly Regex HyphenDateRegex =
            RegexCache.Get(BaseDateTime.HyphenDateRegex, RegexFlags);

        public static readonly Regex PeriodTimeOfDayWithDateRegex =
            RegexCache.Get(DateTimeDefinitions.PeriodTimeOfDayWithDateRegex, RegexFlags);

        public static readonly Regex RelativeTimeUnitRegex =
            RegexCache.Get(DateTimeDefinitions.RelativeTimeUnitRegex, RegexFlags);

        public static readonly Regex RestOfDateTimeRegex =
            RegexCache.Get(DateTimeDefinitions.RestOfDateTimeRegex, RegexFlags);

        public static readonly Regex AmDescRegex =
            RegexCache.Get(DateTimeDefinitions.AmDescRegex, RegexFlags);

        public static readonly Regex PmDescRegex =
            RegexCache.Get(DateTimeDefinitions.PmDescRegex, RegexFlags);

        public static readonly Regex WithinNextPrefixRegex =
            RegexCache.Get(DateTimeDefinitions.WithinNextPrefixRegex, RegexFlags);

        public static readonly Regex DateUnitRegex =
            RegexCache.Get(DateTimeDefinitions.DateUnitRegex, RegexFlags);

        public static readonly Regex PrefixDayRegex =
            RegexCache.Get(DateTimeDefinitions.PrefixDayRegex, RegexFlags | RegexOptions.RightToLeft);

        public static readonly Regex SuffixRegex =
            RegexCache.Get(DateTimeDefinitions.SuffixRegex, RegexFlags);

        public static readonly Regex BeforeRegex =
            RegexCache.Get(DateTimeDefinitions.BeforeRegex, RegexFlags);

        public static readonly Regex AfterRegex =
            RegexCache.Get(DateTimeDefinitions.AfterRegex, RegexFlags);

        public static readonly Regex WeekDaysRegex =
            RegexCache.Get(DateTimeDefinitions.WeekDayRegex, RegexFlags);

        public static readonly Regex PeriodSpecificTimeOfDayRegex =
            RegexCache.Get(DateTimeDefinitions.PeriodSpecificTimeOfDayRegex, RegexFlags);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly Regex[] SimpleCases =
        {
           HindiTimePeriodExtractorConfiguration.PureNumFromTo,
           HindiTimePeriodExtractorConfiguration.PureNumBetweenAnd,
        };

        private static readonly Regex PeriodTimeOfDayRegex =
            RegexCache.Get(DateTimeDefinitions.PeriodTimeOfDayRegex, RegexFlags);

        private static readonly Regex TimeUnitRegex =
            RegexCache.Get(DateTimeDefinitions.TimeUnitRegex, RegexFlags);

        private static readonly Regex TimeFollowedUnit =
            RegexCache.Get(DateTimeDefinitions.TimeFollowedUnit, RegexFlags);

        private static readonly Regex GeneralEndingRegex =
            RegexCache.Get(DateTimeDefinitions.GeneralEndingRegex, RegexFlags);

        private static readonly Regex MiddlePauseRegex =
            RegexCache.Get(DateTimeDefinitions.MiddlePauseRegex, RegexFlags);

        private static readonly Regex FromRegex =
            RegexCache.Get(DateTimeDefinitions.FromTokenRegex, RegexFlags);

        private static readonly Regex BetweenRegex =
            RegexCache.Get(DateTimeDefinitions.BetweenTokenRegex, RegexFlags);

        public HindiDateTimePeriodExtractorConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            TokenBeforeDate = DateTimeDefinitions.TokenBeforeDate;

            CardinalExtractor = Number.Hindi.CardinalExtractor.GetInstance();
            SingleDateExtractor = new BaseDateExtractor(new HindiDateExtractorConfiguration(this));
            SingleTimeExtractor = new BaseTimeExtractor(new HindiTimeExtractorConfiguration(this));
            SingleDateTimeExtractor = new BaseDateTimeExtractor(new HindiDateTimeExtractorConfiguration(this));
            DurationExtractor = new BaseDurationExtractor(new HindiDurationExtractorConfiguration(this));
            TimePeriodExtractor = new BaseTimePeriodExtractor(new HindiTimePeriodExtractorConfiguration(this));
        }

        public IEnumerable<Regex> SimpleCasesRegex => SimpleCases;

        public Regex PrepositionRegex => HindiTimePeriodExtractorConfiguration.PrepositionRegex;

        public Regex TillRegex => HindiTimePeriodExtractorConfiguration.TillRegex;

        public Regex TimeOfDayRegex => PeriodTimeOfDayRegex;

        public Regex SpecificTimeOfDayRegex => PeriodSpecificTimeOfDayRegex;

        public Regex PreviousPrefixRegex => HindiDatePeriodExtractorConfiguration.PreviousPrefixRegex;

        public Regex NextPrefixRegex => HindiDatePeriodExtractorConfiguration.NextPrefixRegex;

        public Regex FutureSuffixRegex => HindiDatePeriodExtractorConfiguration.FutureSuffixRegex;

        public Regex WeekDayRegex => WeekDaysRegex;

        public Regex FollowedUnit => TimeFollowedUnit;

        bool IDateTimePeriodExtractorConfiguration.CheckBothBeforeAfter => DateTimeDefinitions.CheckBothBeforeAfter;

        Regex IDateTimePeriodExtractorConfiguration.PrefixDayRegex => PrefixDayRegex;

        Regex IDateTimePeriodExtractorConfiguration.DateUnitRegex => DateUnitRegex;

        Regex IDateTimePeriodExtractorConfiguration.NumberCombinedWithUnit => TimeNumberCombinedWithUnit;

        Regex IDateTimePeriodExtractorConfiguration.TimeUnitRegex => TimeUnitRegex;

        Regex IDateTimePeriodExtractorConfiguration.RelativeTimeUnitRegex => RelativeTimeUnitRegex;

        Regex IDateTimePeriodExtractorConfiguration.RestOfDateTimeRegex => RestOfDateTimeRegex;

        Regex IDateTimePeriodExtractorConfiguration.GeneralEndingRegex => GeneralEndingRegex;

        Regex IDateTimePeriodExtractorConfiguration.MiddlePauseRegex => MiddlePauseRegex;

        Regex IDateTimePeriodExtractorConfiguration.PeriodTimeOfDayWithDateRegex => PeriodTimeOfDayWithDateRegex;

        Regex IDateTimePeriodExtractorConfiguration.AmDescRegex => AmDescRegex;

        Regex IDateTimePeriodExtractorConfiguration.PmDescRegex => PmDescRegex;

        Regex IDateTimePeriodExtractorConfiguration.WithinNextPrefixRegex => WithinNextPrefixRegex;

        Regex IDateTimePeriodExtractorConfiguration.SuffixRegex => SuffixRegex;

        Regex IDateTimePeriodExtractorConfiguration.BeforeRegex => BeforeRegex;

        Regex IDateTimePeriodExtractorConfiguration.AfterRegex => AfterRegex;

        public string TokenBeforeDate { get; }

        public IExtractor CardinalExtractor { get; }

        public IDateTimeExtractor SingleDateExtractor { get; }

        public IDateTimeExtractor SingleTimeExtractor { get; }

        public IDateTimeExtractor SingleDateTimeExtractor { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateTimeExtractor TimePeriodExtractor { get; }

        public IDateTimeExtractor TimeZoneExtractor { get; }

        // TODO: these three methods are the same in DatePeriod, should be abstracted
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
            var betweenMatch = BetweenRegex.Match(text);
            if (betweenMatch.Success)
            {
                index = betweenMatch.Index;
            }

            return betweenMatch.Success;
        }

        public bool HasConnectorToken(string text)
        {
            var rangeConnetorRegex = RegexCache.Get(DateTimeDefinitions.RangeConnectorRegex);

            return rangeConnetorRegex.IsExactMatch(text, trim: true);
        }
    }
}
