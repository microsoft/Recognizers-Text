using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Dutch;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Dutch
{
    public class DutchDateTimePeriodExtractorConfiguration : BaseDateTimeOptionsConfiguration,
        IDateTimePeriodExtractorConfiguration
    {
        public static readonly Regex AmDescRegex =
            new Regex(DateTimeDefinitions.AmDescRegex, RegexFlags);

        public static readonly Regex PmDescRegex =
            new Regex(DateTimeDefinitions.PmDescRegex, RegexFlags);

        public static readonly Regex WithinNextPrefixRegex =
            new Regex(DateTimeDefinitions.WithinNextPrefixRegex, RegexFlags);

        public static readonly Regex DateUnitRegex =
            new Regex(DateTimeDefinitions.DateUnitRegex, RegexFlags);

        public static readonly Regex PrefixDayRegex =
            new Regex(DateTimeDefinitions.PrefixDayRegex, RegexFlags | RegexOptions.RightToLeft);

        public static readonly Regex SuffixRegex =
            new Regex(DateTimeDefinitions.SuffixRegex, RegexFlags);

        public static readonly Regex BeforeRegex =
            new Regex(DateTimeDefinitions.BeforeRegex, RegexFlags);

        public static readonly Regex AfterRegex =
            new Regex(DateTimeDefinitions.AfterRegex, RegexFlags);

        public static readonly Regex WeekDaysRegex =
            new Regex(DateTimeDefinitions.WeekDayRegex, RegexFlags);

        public static readonly Regex TimeNumberCombinedWithUnit =
            new Regex(DateTimeDefinitions.TimeNumberCombinedWithUnit, RegexFlags);

        public static readonly Regex PeriodTimeOfDayWithDateRegex =
            new Regex(DateTimeDefinitions.PeriodTimeOfDayWithDateRegex, RegexFlags);

        public static readonly Regex RelativeTimeUnitRegex =
            new Regex(DateTimeDefinitions.RelativeTimeUnitRegex, RegexFlags);

        public static readonly Regex RestOfDateTimeRegex =
            new Regex(DateTimeDefinitions.RestOfDateTimeRegex, RegexFlags);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly Regex GeneralEndingRegex =
            new Regex(DateTimeDefinitions.GeneralEndingRegex, RegexFlags);

        private static readonly Regex MiddlePauseRegex =
            new Regex(DateTimeDefinitions.MiddlePauseRegex, RegexFlags);

        private static readonly Regex PeriodTimeOfDayRegex =
            new Regex(DateTimeDefinitions.PeriodTimeOfDayRegex, RegexFlags);

        private static readonly Regex PeriodSpecificTimeOfDayRegex =
            new Regex(DateTimeDefinitions.PeriodSpecificTimeOfDayRegex, RegexFlags);

        private static readonly Regex TimeUnitRegex =
            new Regex(DateTimeDefinitions.TimeUnitRegex, RegexFlags);

        private static readonly Regex TimeFollowedUnit =
            new Regex(DateTimeDefinitions.TimeFollowedUnit, RegexFlags);

        private static readonly Regex[] SimpleCases =
{
            DutchTimePeriodExtractorConfiguration.PureNumFromTo,
            DutchTimePeriodExtractorConfiguration.PureNumBetweenAnd,
        };

        public DutchDateTimePeriodExtractorConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            TokenBeforeDate = DateTimeDefinitions.TokenBeforeDate;

            var numOptions = NumberOptions.None;
            if ((config.Options & DateTimeOptions.NoProtoCache) != 0)
            {
                numOptions = NumberOptions.NoProtoCache;
            }

            var numConfig = new BaseNumberOptionsConfiguration(config.Culture, numOptions);

            CardinalExtractor = Number.Dutch.CardinalExtractor.GetInstance(numConfig);

            SingleDateExtractor = new BaseDateExtractor(new DutchDateExtractorConfiguration(this));
            SingleTimeExtractor = new BaseTimeExtractor(new DutchTimeExtractorConfiguration(this));
            SingleDateTimeExtractor = new BaseDateTimeExtractor(new DutchDateTimeExtractorConfiguration(this));
            DurationExtractor = new BaseDurationExtractor(new DutchDurationExtractorConfiguration(this));
            TimePeriodExtractor = new BaseTimePeriodExtractor(new DutchTimePeriodExtractorConfiguration(this));
            TimeZoneExtractor = new BaseTimeZoneExtractor(new DutchTimeZoneExtractorConfiguration(this));
        }

        public string TokenBeforeDate { get; }

        public IEnumerable<Regex> SimpleCasesRegex => SimpleCases;

        public Regex PrepositionRegex => DutchTimePeriodExtractorConfiguration.PrepositionRegex;

        public Regex TillRegex => DutchTimePeriodExtractorConfiguration.TillRegex;

        public Regex TimeOfDayRegex => PeriodTimeOfDayRegex;

        public Regex SpecificTimeOfDayRegex => PeriodSpecificTimeOfDayRegex;

        public Regex PreviousPrefixRegex => DutchDatePeriodExtractorConfiguration.PreviousPrefixRegex;

        public Regex NextPrefixRegex => DutchDatePeriodExtractorConfiguration.NextPrefixRegex;

        public Regex FutureSuffixRegex => DutchDatePeriodExtractorConfiguration.FutureSuffixRegex;

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
            var rangeConnetorRegex = new Regex(DateTimeDefinitions.RangeConnectorRegex);

            return rangeConnetorRegex.IsExactMatch(text, trim: true);
        }
    }
}