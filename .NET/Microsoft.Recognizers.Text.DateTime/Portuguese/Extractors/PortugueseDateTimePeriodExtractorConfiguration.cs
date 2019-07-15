using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Portuguese;

namespace Microsoft.Recognizers.Text.DateTime.Portuguese
{
    public class PortugueseDateTimePeriodExtractorConfiguration : BaseOptionsConfiguration, IDateTimePeriodExtractorConfiguration
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

        private static readonly Regex ConnectorAndRegex =
            new Regex(DateTimeDefinitions.ConnectorAndRegex, RegexFlags);

        private static readonly Regex BetweenRegex =
            new Regex(DateTimeDefinitions.BetweenRegex, RegexFlags);

        public PortugueseDateTimePeriodExtractorConfiguration(IOptionsConfiguration config)
            : base(config)
        {
            TokenBeforeDate = DateTimeDefinitions.TokenBeforeDate;

            CardinalExtractor = Number.Portuguese.CardinalExtractor.GetInstance();

            SingleDateExtractor = new BaseDateExtractor(new PortugueseDateExtractorConfiguration(this));
            SingleTimeExtractor = new BaseTimeExtractor(new PortugueseTimeExtractorConfiguration(this));
            SingleDateTimeExtractor = new BaseDateTimeExtractor(new PortugueseDateTimeExtractorConfiguration(this));
            DurationExtractor = new BaseDurationExtractor(new PortugueseDurationExtractorConfiguration(this));
            TimePeriodExtractor = new BaseTimePeriodExtractor(new PortugueseTimePeriodExtractorConfiguration(this));
            TimeZoneExtractor = new BaseTimeZoneExtractor(new PortugueseTimeZoneExtractorConfiguration(this));
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
            PortugueseTimePeriodExtractorConfiguration.PureNumFromTo,
            PortugueseTimePeriodExtractorConfiguration.PureNumBetweenAnd,
        };

        public Regex PrepositionRegex => PortugueseDateTimeExtractorConfiguration.PrepositionRegex;

        public Regex TillRegex => PortugueseDatePeriodExtractorConfiguration.TillRegex;

        public Regex SpecificTimeOfDayRegex => PortugueseDateTimeExtractorConfiguration.SpecificTimeOfDayRegex;

        public Regex TimeOfDayRegex => PortugueseDateTimeExtractorConfiguration.TimeOfDayRegex;

        public Regex FollowedUnit => PortugueseTimePeriodExtractorConfiguration.FollowedUnit;

        public Regex TimeUnitRegex => PortugueseTimePeriodExtractorConfiguration.UnitRegex;

        public Regex PreviousPrefixRegex => PortugueseDatePeriodExtractorConfiguration.PastRegex;

        public Regex NextPrefixRegex => PortugueseDatePeriodExtractorConfiguration.FutureRegex;

        public Regex FutureSuffixRegex => PortugueseDatePeriodExtractorConfiguration.FutureSuffixRegex;

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
            return ConnectorAndRegex.IsMatch(text);
        }
    }
}
