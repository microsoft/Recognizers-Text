using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Korean;

using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Korean
{
    public class KoreanSetExtractorConfiguration : BaseDateTimeOptionsConfiguration, ICJKSetExtractorConfiguration
    {
        public static readonly Regex UnitRegex = new Regex(DateTimeDefinitions.SetUnitRegex, RegexFlags);

        public static readonly Regex EachUnitRegex = new Regex(DateTimeDefinitions.SetEachUnitRegex, RegexFlags);

        public static readonly Regex EachPrefixRegex = new Regex(DateTimeDefinitions.SetEachPrefixRegex, RegexFlags);

        public static readonly Regex LastRegex = new Regex(DateTimeDefinitions.SetLastRegex, RegexFlags);

        public static readonly Regex EachDayRegex = new Regex(DateTimeDefinitions.SetEachDayRegex, RegexFlags);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public KoreanSetExtractorConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            DurationExtractor = new BaseCJKDurationExtractor(new KoreanDurationExtractorConfiguration(this));
            TimeExtractor = new BaseCJKTimeExtractor(new KoreanTimeExtractorConfiguration(this));
            DateExtractor = new BaseCJKDateExtractor(new KoreanDateExtractorConfiguration(this));
            DateTimeExtractor = new BaseCJKDateTimeExtractor(new KoreanDateTimeExtractorConfiguration(this));
            DatePeriodExtractor = new BaseCJKDatePeriodExtractor(new KoreanDatePeriodExtractorConfiguration(this));
            TimePeriodExtractor = new BaseCJKTimePeriodExtractor(new KoreanTimePeriodExtractorConfiguration(this));
            DateTimePeriodExtractor = new BaseCJKDateTimePeriodExtractor(new KoreanDateTimePeriodExtractorConfiguration(this));
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