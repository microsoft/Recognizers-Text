using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.French;

namespace Microsoft.Recognizers.Text.DateTime.French
{
    public class FrenchDateTimePeriodExtractorConfiguration : BaseOptionsConfiguration,
        IDateTimePeriodExtractorConfiguration
    {
        public string TokenBeforeDate { get; }

        public FrenchDateTimePeriodExtractorConfiguration() : base(DateTimeOptions.None)
        {
            TokenBeforeDate = DateTimeDefinitions.TokenBeforeDate;

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

        private static readonly Regex FromRegex = new Regex(DateTimeDefinitions.FromRegex2,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private static readonly Regex ConnectorAndRegex = new Regex(DateTimeDefinitions.ConnectorAndRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private static readonly Regex BeforeRegex = new Regex(DateTimeDefinitions.BeforeRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private static readonly Regex PeriodTimeOfDayRegex =
            new Regex(DateTimeDefinitions.PeriodTimeOfDayRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private static readonly Regex PeriodSpecificTimeOfDayRegex =
            new Regex(DateTimeDefinitions.PeriodSpecificTimeOfDayRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private static readonly Regex TimeTimeUnitRegex =
            new Regex(DateTimeDefinitions.TimeUnitRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private static readonly Regex TimeFollowedUnit =
            new Regex(DateTimeDefinitions.TimeFollowedUnit, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex TimeNumberCombinedWithUnit =
            new Regex(DateTimeDefinitions.TimeNumberCombinedWithUnit,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex PeriodTimeOfDayWithDateRegex =
            new Regex(DateTimeDefinitions.PeriodTimeOfDayWithDateRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex RelativeTimeUnitRegex =
            new Regex(DateTimeDefinitions.RelativeTimeUnitRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex RestOfDateTimeRegex =
            new Regex(DateTimeDefinitions.RestOfDateTimeRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex GeneralEndingRegex =
            new Regex(DateTimeDefinitions.GeneralEndingRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MiddlePauseRegex =
            new Regex(DateTimeDefinitions.MiddlePauseRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex AmDescRegex =
            new Regex(DateTimeDefinitions.AmDescRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex PmDescRegex =
            new Regex(DateTimeDefinitions.PmDescRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex WithinNextPrefixRegex =
            new Regex(DateTimeDefinitions.WithinNextPrefixRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex DateUnitRegex =
            new Regex(DateTimeDefinitions.DateUnitRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex PrefixDayRegex =
            new Regex(DateTimeDefinitions.PrefixDayRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.RightToLeft);

        public IEnumerable<Regex> SimpleCasesRegex => SimpleCases;

        public Regex PrepositionRegex => FrenchTimePeriodExtractorConfiguration.PrepositionRegex;

        public Regex TillRegex => FrenchTimePeriodExtractorConfiguration.TillRegex;

        public Regex TimeOfDayRegex => PeriodTimeOfDayRegex;

        public Regex SpecificTimeOfDayRegex => PeriodSpecificTimeOfDayRegex;

        public Regex FollowedUnit => TimeFollowedUnit;

        // Note: FR 'past' i.e 'dernier' is a suffix following after, however interface enforces 'prefix' nomenclature
        public Regex PastPrefixRegex =>
            FrenchDatePeriodExtractorConfiguration
                .PastPrefixRegex;

        // Note: FR 'next' i.e 'prochain' is a suffix following after, i.e 'lundi prochain', however 'prefix' is enforced by interface
        public Regex NextPrefixRegex =>
            FrenchDatePeriodExtractorConfiguration
                .NextPrefixRegex; 

        public Regex FutureSuffixRegex => FrenchDatePeriodExtractorConfiguration.FutureSuffixRegex;

        public Regex WeekDayRegex => new Regex(DateTimeDefinitions.WeekDayRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public IExtractor CardinalExtractor { get; }

        public IDateTimeExtractor SingleDateExtractor { get; }

        public IDateTimeExtractor SingleTimeExtractor { get; }

        public IDateTimeExtractor SingleDateTimeExtractor { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateTimeExtractor TimePeriodExtractor { get; }

        Regex IDateTimePeriodExtractorConfiguration.PrefixDayRegex => PrefixDayRegex;

        Regex IDateTimePeriodExtractorConfiguration.DateUnitRegex => DateUnitRegex;

        Regex IDateTimePeriodExtractorConfiguration.NumberCombinedWithUnit => TimeNumberCombinedWithUnit;

        Regex IDateTimePeriodExtractorConfiguration.TimeUnitRegex => TimeTimeUnitRegex;

        Regex IDateTimePeriodExtractorConfiguration.RelativeTimeUnitRegex => RelativeTimeUnitRegex;

        Regex IDateTimePeriodExtractorConfiguration.RestOfDateTimeRegex => RestOfDateTimeRegex;

        Regex IDateTimePeriodExtractorConfiguration.PeriodTimeOfDayWithDateRegex => PeriodTimeOfDayWithDateRegex;

        Regex IDateTimePeriodExtractorConfiguration.GeneralEndingRegex => GeneralEndingRegex;

        Regex IDateTimePeriodExtractorConfiguration.MiddlePauseRegex => MiddlePauseRegex;

        Regex IDateTimePeriodExtractorConfiguration.AmDescRegex => AmDescRegex;

        Regex IDateTimePeriodExtractorConfiguration.PmDescRegex => PmDescRegex;

        Regex IDateTimePeriodExtractorConfiguration.WithinNextPrefixRegex => WithinNextPrefixRegex;

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