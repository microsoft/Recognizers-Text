using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.German;

namespace Microsoft.Recognizers.Text.DateTime.German
{
    public class GermanDateTimePeriodExtractorConfiguration : BaseOptionsConfiguration,
        IDateTimePeriodExtractorConfiguration
    {
        public string TokenBeforeDate { get; }

        public GermanDateTimePeriodExtractorConfiguration() : base(DateTimeOptions.None)
        {
            TokenBeforeDate = DateTimeDefinitions.TokenBeforeDate;

            CardinalExtractor = Number.German.CardinalExtractor.GetInstance();
            SingleDateExtractor = new BaseDateExtractor(new GermanDateExtractorConfiguration());
            SingleTimeExtractor = new BaseTimeExtractor(new GermanTimeExtractorConfiguration());
            SingleDateTimeExtractor = new BaseDateTimeExtractor(new GermanDateTimeExtractorConfiguration());
            DurationExtractor = new BaseDurationExtractor(new GermanDurationExtractorConfiguration());
            TimePeriodExtractor = new BaseTimePeriodExtractor(new GermanTimePeriodExtractorConfiguration());
        }

        private static readonly Regex[] SimpleCases =
        {
            GermanTimePeriodExtractorConfiguration.PureNumFromTo,
            GermanTimePeriodExtractorConfiguration.PureNumBetweenAnd
        };

        private static readonly Regex PeriodTimeOfDayRegex =
            new Regex(DateTimeDefinitions.PeriodTimeOfDayRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private static readonly Regex PeriodSpecificTimeOfDayRegex =
            new Regex(DateTimeDefinitions.PeriodSpecificTimeOfDayRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private static readonly Regex TimeUnitRegex =
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

        private static readonly Regex GeneralEndingRegex =
            new Regex(DateTimeDefinitions.GeneralEndingRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private static readonly Regex MiddlePauseRegex =
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

        public static readonly Regex SuffixRegex =
            new Regex(DateTimeDefinitions.SuffixRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex BeforeRegex =
            new Regex(DateTimeDefinitions.BeforeRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex AfterRegex =
            new Regex(DateTimeDefinitions.AfterRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public IEnumerable<Regex> SimpleCasesRegex => SimpleCases;

        public Regex PrepositionRegex => GermanTimePeriodExtractorConfiguration.PrepositionRegex;

        public Regex TillRegex => GermanTimePeriodExtractorConfiguration.TillRegex;

        public Regex TimeOfDayRegex => PeriodTimeOfDayRegex;

        public Regex SpecificTimeOfDayRegex => PeriodSpecificTimeOfDayRegex;

        public Regex FollowedUnit => TimeFollowedUnit;

        public Regex PastPrefixRegex => GermanDatePeriodExtractorConfiguration.PastPrefixRegex;

        public Regex NextPrefixRegex => GermanDatePeriodExtractorConfiguration.NextPrefixRegex;

        public Regex FutureSuffixRegex => GermanDatePeriodExtractorConfiguration.FutureSuffixRegex;

        public Regex WeekDayRegex => new Regex(DateTimeDefinitions.WeekDayRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

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

        //TODO: these three methods are the same in DatePeriod, should be abstracted
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
            var match = Regex.Match(text, DateTimeDefinitions.RangeConnectorRegex);
            return match.Success && match.Length == text.Trim().Length;
        }
    }
}