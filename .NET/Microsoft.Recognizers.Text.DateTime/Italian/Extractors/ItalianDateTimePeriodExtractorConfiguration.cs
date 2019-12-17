using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Italian;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Italian
{
    public class ItalianDateTimePeriodExtractorConfiguration : BaseDateTimeOptionsConfiguration, IDateTimePeriodExtractorConfiguration
    {
        public static readonly Regex SuffixRegex =
            new Regex(DateTimeDefinitions.SuffixRegex, RegexFlags);

        public static readonly Regex AfterRegex =
            new Regex(DateTimeDefinitions.AfterRegex, RegexFlags);

        public static readonly Regex BeforeRegex =
            new Regex(DateTimeDefinitions.BeforeRegex, RegexFlags);

        public static readonly Regex TimeNumberCombinedWithUnit =
            new Regex(DateTimeDefinitions.TimeNumberCombinedWithUnit, RegexFlags);

        public static readonly Regex PeriodTimeOfDayWithDateRegex =
            new Regex(DateTimeDefinitions.PeriodTimeOfDayWithDateRegex, RegexFlags);

        public static readonly Regex RelativeTimeUnitRegex =
            new Regex(DateTimeDefinitions.RelativeTimeUnitRegex, RegexFlags);

        public static readonly Regex RestOfDateTimeRegex =
            new Regex(DateTimeDefinitions.RestOfDateTimeRegex, RegexFlags);

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

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly Regex[] SimpleCases =
        {
            ItalianTimePeriodExtractorConfiguration.PureNumFromTo,
            ItalianTimePeriodExtractorConfiguration.PureNumBetweenAnd,
            ItalianTimePeriodExtractorConfiguration.SpecificTimeOfDayRegex,
        };

        private static readonly Regex FromRegex =
            new Regex(DateTimeDefinitions.FromRegex, RegexFlags);

        private static readonly Regex ConnectorAndRegex =
            new Regex(DateTimeDefinitions.ConnectorAndRegex, RegexFlags);

        private static readonly Regex RangePrefixRegex =
            new Regex(DateTimeDefinitions.RangePrefixRegex, RegexFlags);

        private static readonly Regex PeriodTimeOfDayRegex =
            new Regex(DateTimeDefinitions.PeriodTimeOfDayRegex, RegexFlags);

        private static readonly Regex PeriodSpecificTimeOfDayRegex =
            new Regex(DateTimeDefinitions.PeriodSpecificTimeOfDayRegex, RegexFlags);

        private static readonly Regex TimeUnitRegex =
            new Regex(DateTimeDefinitions.TimeUnitRegex, RegexFlags);

        private static readonly Regex TimeFollowedUnit =
            new Regex(DateTimeDefinitions.TimeFollowedUnit, RegexFlags);

        public ItalianDateTimePeriodExtractorConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            TokenBeforeDate = DateTimeDefinitions.TokenBeforeDate;

            var numOptions = NumberOptions.None;
            if ((config.Options & DateTimeOptions.NoProtoCache) != 0)
            {
                numOptions = NumberOptions.NoProtoCache;
            }

            var numConfig = new BaseNumberOptionsConfiguration(config.Culture, numOptions);

            CardinalExtractor = Number.English.CardinalExtractor.GetInstance(numConfig);

            SingleDateExtractor = new BaseDateExtractor(new ItalianDateExtractorConfiguration(this));
            SingleTimeExtractor = new BaseTimeExtractor(new ItalianTimeExtractorConfiguration(this));
            SingleDateTimeExtractor = new BaseDateTimeExtractor(new ItalianDateTimeExtractorConfiguration(this));
            DurationExtractor = new BaseDurationExtractor(new ItalianDurationExtractorConfiguration(this));
            TimePeriodExtractor = new BaseTimePeriodExtractor(new ItalianTimePeriodExtractorConfiguration(this));
            TimeZoneExtractor = new BaseTimeZoneExtractor(new ItalianTimeZoneExtractorConfiguration(this));
        }

        public IEnumerable<Regex> SimpleCasesRegex => SimpleCases;

        public Regex PrepositionRegex => ItalianTimePeriodExtractorConfiguration.PrepositionRegex;

        public Regex TillRegex => ItalianTimePeriodExtractorConfiguration.TillRegex;

        public Regex FullTillRegex => ItalianTimePeriodExtractorConfiguration.FullTillRegex;

        public Regex TimeOfDayRegex => PeriodTimeOfDayRegex;

        public Regex SpecificTimeOfDayRegex => PeriodSpecificTimeOfDayRegex;

        public Regex FollowedUnit => TimeFollowedUnit;

        bool IDateTimePeriodExtractorConfiguration.CheckBothBeforeAfter => DateTimeDefinitions.CheckBothBeforeAfter;

        Regex IDateTimePeriodExtractorConfiguration.NumberCombinedWithUnit => TimeNumberCombinedWithUnit;

        Regex IDateTimePeriodExtractorConfiguration.TimeUnitRegex => TimeUnitRegex;

        Regex IDateTimePeriodExtractorConfiguration.RelativeTimeUnitRegex => RelativeTimeUnitRegex;

        Regex IDateTimePeriodExtractorConfiguration.RestOfDateTimeRegex => RestOfDateTimeRegex;

        public Regex PreviousPrefixRegex => ItalianDatePeriodExtractorConfiguration.PreviousPrefixRegex; // Note: FR 'past' i.e 'dernier' is a suffix following after, however interface enforces 'prefix' nomenclature

        public Regex NextPrefixRegex => ItalianDatePeriodExtractorConfiguration.NextPrefixRegex; // Note: FR 'next' i.e 'prochain' is a suffix following after, i.e 'lundi prochain', however 'prefix' is enforced by interface

        public Regex FutureSuffixRegex => ItalianDatePeriodExtractorConfiguration.FutureSuffixRegex;

        public Regex WeekDayRegex => new Regex(DateTimeDefinitions.WeekDayRegex, RegexFlags);

        Regex IDateTimePeriodExtractorConfiguration.PeriodTimeOfDayWithDateRegex => PeriodTimeOfDayWithDateRegex;

        Regex IDateTimePeriodExtractorConfiguration.GeneralEndingRegex => GeneralEndingRegex;

        Regex IDateTimePeriodExtractorConfiguration.MiddlePauseRegex => MiddlePauseRegex;

        Regex IDateTimePeriodExtractorConfiguration.AmDescRegex => AmDescRegex;

        Regex IDateTimePeriodExtractorConfiguration.PmDescRegex => PmDescRegex;

        Regex IDateTimePeriodExtractorConfiguration.WithinNextPrefixRegex => WithinNextPrefixRegex;

        public string TokenBeforeDate { get; }

        public IExtractor CardinalExtractor { get; }

        public IDateTimeExtractor SingleDateExtractor { get; }

        public IDateTimeExtractor SingleTimeExtractor { get; }

        public IDateTimeExtractor SingleDateTimeExtractor { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateTimeExtractor TimePeriodExtractor { get; }

        public IDateTimeExtractor TimeZoneExtractor { get; }

        Regex IDateTimePeriodExtractorConfiguration.PrefixDayRegex => PrefixDayRegex;

        Regex IDateTimePeriodExtractorConfiguration.DateUnitRegex => DateUnitRegex;

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
