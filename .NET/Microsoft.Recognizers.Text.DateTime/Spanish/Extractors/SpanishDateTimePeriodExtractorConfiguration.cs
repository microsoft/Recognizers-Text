using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Spanish;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public class SpanishDateTimePeriodExtractorConfiguration : BaseDateTimeOptionsConfiguration,
        IDateTimePeriodExtractorConfiguration
    {
        public static readonly Regex NumberCombinedWithUnit =
            new Regex(DateTimeDefinitions.DateTimePeriodNumberCombinedWithUnit, RegexFlags);

        public static readonly Regex WeekDayRegex =
            new Regex(DateTimeDefinitions.WeekDayRegex, RegexFlags);

        public static readonly Regex RestOfDateTimeRegex =
            new Regex(DateTimeDefinitions.RestOfDateTimeRegex, RegexFlags);

        public static readonly Regex PeriodTimeOfDayWithDateRegex =
            new Regex(DateTimeDefinitions.PeriodTimeOfDayWithDateRegex, RegexFlags);

        public static readonly Regex RelativeTimeUnitRegex =
            new Regex(DateTimeDefinitions.RelativeTimeUnitRegex, RegexFlags);

        public static readonly Regex GeneralEndingRegex =
            new Regex(DateTimeDefinitions.GeneralEndingRegex, RegexFlags);

        public static readonly Regex MiddlePauseRegex =
            new Regex(DateTimeDefinitions.MiddlePauseRegex, RegexFlags);

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

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly Regex FromRegex =
            new Regex(DateTimeDefinitions.FromRegex, RegexFlags);

        private static readonly Regex RangeConnectorRegex =
            new Regex(DateTimeDefinitions.RangeConnectorRegex, RegexFlags);

        private static readonly Regex BetweenRegex =
            new Regex(DateTimeDefinitions.BetweenRegex, RegexFlags);

        public SpanishDateTimePeriodExtractorConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            TokenBeforeDate = DateTimeDefinitions.TokenBeforeDate;

            var numOptions = NumberOptions.None;
            if ((config.Options & DateTimeOptions.NoProtoCache) != 0)
            {
                numOptions = NumberOptions.NoProtoCache;
            }

            var numConfig = new BaseNumberOptionsConfiguration(config.Culture, numOptions);

            CardinalExtractor = Number.Spanish.CardinalExtractor.GetInstance(numConfig);

            SingleDateExtractor = new BaseDateExtractor(new SpanishDateExtractorConfiguration(this));
            SingleTimeExtractor = new BaseTimeExtractor(new SpanishTimeExtractorConfiguration(this));
            SingleDateTimeExtractor = new BaseDateTimeExtractor(new SpanishDateTimeExtractorConfiguration(this));
            DurationExtractor = new BaseDurationExtractor(new SpanishDurationExtractorConfiguration(this));
            TimePeriodExtractor = new BaseTimePeriodExtractor(new SpanishTimePeriodExtractorConfiguration(this));
            TimeZoneExtractor = new BaseTimeZoneExtractor(new SpanishTimeZoneExtractorConfiguration(this));
        }

        public string TokenBeforeDate { get; }

        public IExtractor CardinalExtractor { get; }

        public IDateTimeExtractor SingleDateExtractor { get; }

        public IDateTimeExtractor SingleTimeExtractor { get; }

        public IDateTimeExtractor SingleDateTimeExtractor { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateTimeExtractor TimePeriodExtractor { get; }

        public IDateTimeExtractor TimeZoneExtractor { get; }

        public IEnumerable<Regex> SimpleCasesRegex => new[]
        {
            SpanishTimePeriodExtractorConfiguration.PureNumFromTo,
            SpanishTimePeriodExtractorConfiguration.PureNumBetweenAnd,
        };

        public Regex PrepositionRegex => SpanishDateTimeExtractorConfiguration.PrepositionRegex;

        public Regex TillRegex => SpanishDatePeriodExtractorConfiguration.TillRegex;

        public Regex SpecificTimeOfDayRegex => SpanishDateTimeExtractorConfiguration.SpecificTimeOfDayRegex;

        public Regex TimeOfDayRegex => SpanishDateTimeExtractorConfiguration.TimeOfDayRegex;

        public Regex FollowedUnit => SpanishTimePeriodExtractorConfiguration.FollowedUnit;

        public Regex TimeUnitRegex => SpanishTimePeriodExtractorConfiguration.UnitRegex;

        public Regex PreviousPrefixRegex => SpanishDatePeriodExtractorConfiguration.PastRegex;

        public Regex NextPrefixRegex => SpanishDateTimeAltExtractorConfiguration.NextPrefixRegex;

        public Regex FutureSuffixRegex => SpanishDatePeriodExtractorConfiguration.FutureSuffixRegex;

        bool IDateTimePeriodExtractorConfiguration.CheckBothBeforeAfter => DateTimeDefinitions.CheckBothBeforeAfter;

        Regex IDateTimePeriodExtractorConfiguration.PrefixDayRegex => PrefixDayRegex;

        Regex IDateTimePeriodExtractorConfiguration.DateUnitRegex => DateUnitRegex;

        Regex IDateTimePeriodExtractorConfiguration.NumberCombinedWithUnit => NumberCombinedWithUnit;

        Regex IDateTimePeriodExtractorConfiguration.WeekDayRegex => WeekDayRegex;

        Regex IDateTimePeriodExtractorConfiguration.PeriodTimeOfDayWithDateRegex => PeriodTimeOfDayWithDateRegex;

        Regex IDateTimePeriodExtractorConfiguration.RelativeTimeUnitRegex => RelativeTimeUnitRegex;

        Regex IDateTimePeriodExtractorConfiguration.RestOfDateTimeRegex => RestOfDateTimeRegex;

        Regex IDateTimePeriodExtractorConfiguration.GeneralEndingRegex => GeneralEndingRegex;

        Regex IDateTimePeriodExtractorConfiguration.MiddlePauseRegex => MiddlePauseRegex;

        Regex IDateTimePeriodExtractorConfiguration.AmDescRegex => AmDescRegex;

        Regex IDateTimePeriodExtractorConfiguration.PmDescRegex => PmDescRegex;

        Regex IDateTimePeriodExtractorConfiguration.WithinNextPrefixRegex => WithinNextPrefixRegex;

        Regex IDateTimePeriodExtractorConfiguration.SuffixRegex => SuffixRegex;

        Regex IDateTimePeriodExtractorConfiguration.BeforeRegex => BeforeRegex;

        Regex IDateTimePeriodExtractorConfiguration.AfterRegex => AfterRegex;

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