using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.French;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime.French
{
    public class FrenchDateTimePeriodExtractorConfiguration : IDateTimePeriodExtractorConfiguration
    {
        public FrenchDateTimePeriodExtractorConfiguration()
        {
            CardinalExtractor = Number.English.CardinalExtractor.GetInstance();
            SingleDateExtractor = new BaseDateExtractor(new FrenchDateExtractorConfiguration());
            SingleTimeExtractor = new BaseTimeExtractor(new FrenchTimeExtractorConfiguration());
            SingleDateTimeExtractor = new BaseDateTimeExtractor(new FrenchDateTimeExtractorConfiguration());
            DurationExtractor = new BaseDurationExtractor(new FrenchDurationExtractorConfiguration());
            TimePeriodExtractor = new BaseTimePeriodExtractor(new FrenchTimePeriodExtractorConfiguration());
        }

        private static readonly Regex[] SimpleCases =
        {
            FrenchTimePeriodExtractorConfiguration.PureNumFromTo,
            FrenchTimePeriodExtractorConfiguration.PureNumBetweenAnd,
            FrenchTimePeriodExtractorConfiguration.SpecificTimeOfDayRegex
        };

        private static readonly Regex FromRegex = new Regex(DateTimeDefinitions.FromRegex2, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly Regex ConnectorAndRegex = new Regex(DateTimeDefinitions.ConnectorAndRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly Regex BeforeRegex = new Regex(DateTimeDefinitions.BeforeRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public IEnumerable<Regex> SimpleCasesRegex => SimpleCases;

        public Regex PrepositionRegex => FrenchTimePeriodExtractorConfiguration.PrepositionRegex;

        public Regex TillRegex => FrenchTimePeriodExtractorConfiguration.TillRegex;

        private static readonly Regex PeriodTimeOfDayRegex =
            new Regex(DateTimeDefinitions.PeriodTimeOfDayRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private static readonly Regex PeriodSpecificTimeOfDayRegex =
            new Regex(DateTimeDefinitions.PeriodSpecificTimeOfDayRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public Regex TimeOfDayRegex => PeriodTimeOfDayRegex;

        public Regex SpecificTimeOfDayRegex => PeriodSpecificTimeOfDayRegex;

        private static readonly Regex TimeTimeUnitRegex =
            new Regex(DateTimeDefinitions.TimeUnitRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private static readonly Regex TimeFollowedUnit =
            new Regex(DateTimeDefinitions.TimeFollowedUnit, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex TimeNumberCombinedWithUnit =
            new Regex(DateTimeDefinitions.TimeNumberCombinedWithUnit, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex PeriodTimeOfDayWithDateRegex =
            new Regex(DateTimeDefinitions.PeriodTimeOfDayWithDateRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex RelativeTimeUnitRegex =
            new Regex(DateTimeDefinitions.RelativeTimeUnitRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex RestOfDateTimeRegex =
            new Regex(DateTimeDefinitions.RestOfDateTimeRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex GeneralEndingRegex =
            new Regex(DateTimeDefinitions.GeneralEndingRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MiddlePauseRegex =
            new Regex(DateTimeDefinitions.MiddlePauseRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public Regex FollowedUnit => TimeFollowedUnit;

        Regex IDateTimePeriodExtractorConfiguration.NumberCombinedWithUnit => TimeNumberCombinedWithUnit;

        Regex IDateTimePeriodExtractorConfiguration.TimeUnitRegex => TimeTimeUnitRegex;

        Regex IDateTimePeriodExtractorConfiguration.RelativeTimeUnitRegex => RelativeTimeUnitRegex;

        Regex IDateTimePeriodExtractorConfiguration.RestOfDateTimeRegex => RestOfDateTimeRegex;

        public Regex PastPrefixRegex => FrenchDatePeriodExtractorConfiguration.PastPrefixRegex; // Note: FR 'past' i.e 'dernier' is a suffix following after, however interface enforces 'prefix' nomenclature

        public Regex NextPrefixRegex => FrenchDatePeriodExtractorConfiguration.NextPrefixRegex; // Note: FR 'next' i.e 'prochain' is a suffix following after, i.e 'lundi prochain', however 'prefix' is enforced by interface

        public Regex WeekDayRegex => new Regex(DateTimeDefinitions.WeekDayRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        Regex IDateTimePeriodExtractorConfiguration.PeriodTimeOfDayWithDateRegex => PeriodTimeOfDayWithDateRegex;

        Regex IDateTimePeriodExtractorConfiguration.GeneralEndingRegex => GeneralEndingRegex;

        Regex IDateTimePeriodExtractorConfiguration.MiddlePauseRegex => MiddlePauseRegex;

        public IExtractor CardinalExtractor { get; }

        public IDateTimeExtractor SingleDateExtractor { get; }

        public IDateTimeExtractor SingleTimeExtractor { get; }

        public IDateTimeExtractor SingleDateTimeExtractor { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateTimeExtractor TimePeriodExtractor { get; }

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
            var beforeMatch = BeforeRegex.Match(text);
            if (beforeMatch.Success)
            {
                index = beforeMatch.Index;
            }
            return beforeMatch.Success;
        }

        public bool HasConnectorToken(string text)
        {
            return ConnectorAndRegex.IsMatch(text);
        }
    }
}
