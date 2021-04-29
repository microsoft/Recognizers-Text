using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Japanese;

using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Japanese
{
    public class JapaneseSetExtractorConfiguration : BaseDateTimeOptionsConfiguration, ICJKSetExtractorConfiguration
    {
        public static readonly Regex UnitRegex = new Regex(DateTimeDefinitions.SetUnitRegex, RegexFlags);

        public static readonly Regex EachUnitRegex = new Regex(DateTimeDefinitions.SetEachUnitRegex, RegexFlags);

        public static readonly Regex EachPrefixRegex = new Regex(DateTimeDefinitions.SetEachPrefixRegex, RegexFlags);

        public static readonly Regex LastRegex = new Regex(DateTimeDefinitions.SetLastRegex, RegexFlags);

        public static readonly Regex EachDayRegex = new Regex(DateTimeDefinitions.SetEachDayRegex, RegexFlags);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public JapaneseSetExtractorConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            DurationExtractor = new BaseCJKDurationExtractor(new JapaneseDurationExtractorConfiguration(this));
            TimeExtractor = new BaseCJKTimeExtractor(new JapaneseTimeExtractorConfiguration(this));
            DateExtractor = new BaseCJKDateExtractor(new JapaneseDateExtractorConfiguration(this));
            DateTimeExtractor = new BaseCJKDateTimeExtractor(new JapaneseDateTimeExtractorConfiguration(this));
            DatePeriodExtractor = new BaseCJKDatePeriodExtractor(new JapaneseDatePeriodExtractorConfiguration(this));
            TimePeriodExtractor = new BaseCJKTimePeriodExtractor(new JapaneseTimePeriodExtractorConfiguration(this));
            DateTimePeriodExtractor = new BaseCJKDateTimePeriodExtractor(new JapaneseDateTimePeriodExtractorConfiguration(this));
        }

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateTimeExtractor TimeExtractor { get; }

        public IDateTimeExtractor DateExtractor { get; }

        public IDateTimeExtractor DateTimeExtractor { get; }

        public IDateTimeExtractor DatePeriodExtractor { get; }

        public IDateTimeExtractor TimePeriodExtractor { get; }

        public IDateTimeExtractor DateTimePeriodExtractor { get; }

        Regex ICJKSetExtractorConfiguration.LastRegex => LastRegex;

        Regex ICJKSetExtractorConfiguration.EachPrefixRegex => EachPrefixRegex;

        Regex ICJKSetExtractorConfiguration.EachUnitRegex => EachUnitRegex;

        Regex ICJKSetExtractorConfiguration.UnitRegex => UnitRegex;

        Regex ICJKSetExtractorConfiguration.EachDayRegex => EachDayRegex;
    }
}