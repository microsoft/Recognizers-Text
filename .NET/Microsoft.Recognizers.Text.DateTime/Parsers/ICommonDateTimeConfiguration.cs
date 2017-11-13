using System.Collections.Immutable;

using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface ICommonDateTimeParserConfiguration
    {
        IExtractor CardinalExtractor { get; }

        IExtractor IntegerExtractor { get; }

        IExtractor OrdinalExtractor { get; }

        IParser NumberParser { get; }

        IDateTimeExtractor DateExtractor { get; }

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
        
        IImmutableDictionary<string, int> MonthOfYear { get; }

        IImmutableDictionary<string, int> Numbers { get; }

        IImmutableDictionary<string, long> UnitValueMap { get; }

        IImmutableDictionary<string, string> SeasonMap { get; }

        IImmutableDictionary<string, string> UnitMap { get; }

        IImmutableDictionary<string, int> CardinalMap { get; }

        IImmutableDictionary<string, int> DayOfMonth { get; }

        IImmutableDictionary<string, int> DayOfWeek { get; }

        IImmutableDictionary<string, double> DoubleNumbers { get; }

        IDateTimeUtilityConfiguration UtilityConfiguration { get; }
    }
}
