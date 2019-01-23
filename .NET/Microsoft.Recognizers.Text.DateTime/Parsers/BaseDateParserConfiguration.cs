using System.Collections.Immutable;

using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime
{
    public abstract class BaseDateParserConfiguration : BaseOptionsConfiguration, ICommonDateTimeParserConfiguration
    {
        protected BaseDateParserConfiguration(IOptionsConfiguration config)
            : base(config)
        {
        }

        public virtual IExtractor CardinalExtractor { get; protected set; }

        public virtual IExtractor IntegerExtractor { get; protected set; }

        public virtual IExtractor OrdinalExtractor { get; protected set; }

        public virtual IParser NumberParser { get; protected set; }

        public virtual IDateExtractor DateExtractor { get; protected set; }

        public virtual IDateTimeExtractor TimeExtractor { get; protected set; }

        public virtual IDateTimeExtractor DateTimeExtractor { get; protected set; }

        public virtual IDateTimeExtractor DurationExtractor { get; protected set; }

        public virtual IDateTimeExtractor DatePeriodExtractor { get; protected set; }

        public virtual IDateTimeExtractor TimePeriodExtractor { get; protected set; }

        public virtual IDateTimeExtractor DateTimePeriodExtractor { get; protected set; }

        public virtual IDateTimeParser DateParser { get; protected set; }

        public virtual IDateTimeParser TimeParser { get; protected set; }

        public virtual IDateTimeParser DateTimeParser { get; protected set; }

        public virtual IDateTimeParser DurationParser { get; protected set; }

        public virtual IDateTimeParser DatePeriodParser { get; protected set; }

        public virtual IDateTimeParser TimePeriodParser { get; protected set; }

        public virtual IDateTimeParser DateTimePeriodParser { get; protected set; }

        public virtual IDateTimeParser DateTimeAltParser { get; protected set; }

        public virtual IDateTimeParser TimeZoneParser { get; protected set; }

        public virtual IImmutableDictionary<string, int> MonthOfYear { get; protected set; }

        public virtual IImmutableDictionary<string, int> Numbers { get; protected set; }

        public virtual IImmutableDictionary<string, double> DoubleNumbers { get; protected set; }

        public virtual IImmutableDictionary<string, long> UnitValueMap { get; protected set; }

        public virtual IImmutableDictionary<string, string> SeasonMap { get; protected set; }

        public virtual IImmutableDictionary<string, string> UnitMap { get; protected set; }

        public virtual IImmutableDictionary<string, int> CardinalMap { get; protected set; }

        public virtual IImmutableDictionary<string, int> DayOfWeek { get; protected set; }

        public virtual IImmutableDictionary<string, int> DayOfMonth => BaseDateTime.DayOfMonthDictionary.ToImmutableDictionary();

        public virtual IImmutableDictionary<string, int> WrittenDecades { get; protected set; }

        public virtual IImmutableDictionary<string, int> SpecialDecadeCases { get; protected set; }

        public virtual IDateTimeUtilityConfiguration UtilityConfiguration { get; protected set; }
    }
}