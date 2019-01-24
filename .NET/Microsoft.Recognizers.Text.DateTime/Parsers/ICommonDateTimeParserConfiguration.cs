using System.Collections.Immutable;

using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface ICommonDateTimeParserConfiguration : IOptionsConfiguration
    {
        IExtractor CardinalExtractor { get; }

        IExtractor IntegerExtractor { get; }

        IExtractor OrdinalExtractor { get; }

        IParser NumberParser { get; }

        IDateExtractor DateExtractor { get; }

        IDateTimeExtractor TimeExtractor { get; }

        IDateTimeExtractor DateTimeExtractor { get; }

        IDateTimeExtractor DurationExtractor { get; }

        IDateTimeExtractor DatePeriodExtractor { get; }

        IDateTimeExtractor TimePeriodExtractor { get; }

        IDateTimeExtractor DateTimePeriodExtractor { get; }

        IDateTimeParser DateParser { get; }

        IDateTimeParser TimeParser { get; }

        IDateTimeParser DateTimeParser { get; }

        IDateTimeParser DurationParser { get; }

        IDateTimeParser DatePeriodParser { get; }

        IDateTimeParser TimePeriodParser { get; }

        IDateTimeParser DateTimePeriodParser { get; }

        IDateTimeParser DateTimeAltParser { get; }

        IDateTimeParser TimeZoneParser { get; }

        IImmutableDictionary<string, int> MonthOfYear { get; }

        IImmutableDictionary<string, int> Numbers { get; }

        IImmutableDictionary<string, long> UnitValueMap { get; }

        IImmutableDictionary<string, string> SeasonMap { get; }

        IImmutableDictionary<string, string> UnitMap { get; }

        IImmutableDictionary<string, int> CardinalMap { get; }

        IImmutableDictionary<string, int> DayOfMonth { get; }

        IImmutableDictionary<string, int> DayOfWeek { get; }

        IImmutableDictionary<string, double> DoubleNumbers { get; }

        IImmutableDictionary<string, int> WrittenDecades { get; }

        IImmutableDictionary<string, int> SpecialDecadeCases { get; }

        IDateTimeUtilityConfiguration UtilityConfiguration { get; }
    }
}
