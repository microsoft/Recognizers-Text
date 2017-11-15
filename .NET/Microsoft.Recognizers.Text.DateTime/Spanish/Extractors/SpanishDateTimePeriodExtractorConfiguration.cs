using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Spanish;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public class SpanishDateTimePeriodExtractorConfiguration : IDateTimePeriodExtractorConfiguration
    {
        public static readonly Regex NumberCombinedWithUnit = new Regex(DateTimeDefinitions.DateTimePeriodNumberCombinedWithUnit, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private static readonly Regex FromRegex = new Regex(DateTimeDefinitions.FromRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly Regex ConnectorAndRegex = new Regex(DateTimeDefinitions.ConnectorAndRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly Regex BetweenRegex = new Regex(DateTimeDefinitions.BetweenRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public SpanishDateTimePeriodExtractorConfiguration()
        {
            CardinalExtractor = Number.Spanish.CardinalExtractor.GetInstance();

            SingleDateExtractor = new BaseDateExtractor(new SpanishDateExtractorConfiguration());
            SingleTimeExtractor = new BaseTimeExtractor(new SpanishTimeExtractorConfiguration());
            SingleDateTimeExtractor = new BaseDateTimeExtractor(new SpanishDateTimeExtractorConfiguration());
            DurationExtractor = new BaseDurationExtractor(new SpanishDurationExtractorConfiguration());
            TimePeriodExtractor = new BaseTimePeriodExtractor(new SpanishTimePeriodExtractorConfiguration());
        }

        public IExtractor CardinalExtractor { get; }

        public IDateTimeExtractor SingleDateExtractor { get; }

        public IDateTimeExtractor SingleTimeExtractor { get; }

        public IDateTimeExtractor SingleDateTimeExtractor { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateTimeExtractor TimePeriodExtractor { get; }

        public IEnumerable<Regex> SimpleCasesRegex => new[] 
        {
            SpanishTimePeriodExtractorConfiguration.PureNumFromTo,
            SpanishTimePeriodExtractorConfiguration.PureNumBetweenAnd
        };

        public Regex PrepositionRegex => SpanishDateTimeExtractorConfiguration.PrepositionRegex;

        public Regex TillRegex => SpanishDatePeriodExtractorConfiguration.TillRegex;

        public Regex SpecificTimeOfDayRegex => SpanishDateTimeExtractorConfiguration.SpecificTimeOfDayRegex;

        public Regex TimeOfDayRegex => SpanishDateTimeExtractorConfiguration.TimeOfDayRegex;

        public Regex FollowedUnit => SpanishTimePeriodExtractorConfiguration.FollowedUnit;

        public Regex TimeUnitRegex => SpanishTimePeriodExtractorConfiguration.UnitRegex;

        public Regex PastPrefixRegex => SpanishDatePeriodExtractorConfiguration.PastRegex;

        public Regex NextPrefixRegex => SpanishDatePeriodExtractorConfiguration.FutureRegex;

        //TODO: add this
        public static readonly Regex WeekDayRegex = new Regex(@"^[\.]", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        //TODO: add this according to the related part in English
        public static readonly Regex RestOfDateTimeRegex = new Regex(@"^[\.]", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex PeriodTimeOfDayWithDateRegex = new Regex(DateTimeDefinitions.PeriodTimeOfDayWithDateRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex RelativeTimeUnitRegex = new Regex(DateTimeDefinitions.RelativeTimeUnitRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex GeneralEndingRegex =
            new Regex(DateTimeDefinitions.GeneralEndingRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MiddlePauseRegex =
            new Regex(DateTimeDefinitions.MiddlePauseRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        Regex IDateTimePeriodExtractorConfiguration.NumberCombinedWithUnit => NumberCombinedWithUnit;

        Regex IDateTimePeriodExtractorConfiguration.WeekDayRegex => WeekDayRegex;

        Regex IDateTimePeriodExtractorConfiguration.PeriodTimeOfDayWithDateRegex => PeriodTimeOfDayWithDateRegex;

        Regex IDateTimePeriodExtractorConfiguration.RelativeTimeUnitRegex => RelativeTimeUnitRegex;

        Regex IDateTimePeriodExtractorConfiguration.RestOfDateTimeRegex => RestOfDateTimeRegex;

        Regex IDateTimePeriodExtractorConfiguration.GeneralEndingRegex => GeneralEndingRegex;

        Regex IDateTimePeriodExtractorConfiguration.MiddlePauseRegex => MiddlePauseRegex;

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
