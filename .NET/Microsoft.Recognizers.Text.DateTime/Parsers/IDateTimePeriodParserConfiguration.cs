using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IDateTimePeriodParserConfiguration
    {
        IExtractor DateExtractor { get; }

        IExtractor TimeExtractor { get; }

        IExtractor DateTimeExtractor { get; }

        IExtractor DurationExtractor { get; }

        IExtractor CardinalExtractor { get; }

        IParser NumberParser { get; }

        IDateTimeParser DateParser { get; }

        IDateTimeParser TimeParser { get; }

        IDateTimeParser DateTimeParser { get; }

        IDateTimeParser DurationParser { get; }

        Regex PureNumberFromToRegex { get; }

        Regex PureNumberBetweenAndRegex { get; }

        Regex SpecificTimeOfDayRegex { get; }

        Regex TimeOfDayRegex { get; }

        Regex PastRegex { get; }

        Regex FutureRegex { get; }

        Regex NumberCombinedWithUnitRegex { get; }

        Regex UnitRegex { get; }

        Regex PeriodTimeOfDayWithDateRegex { get; }

        Regex RelativeTimeUnitRegex { get; }

        Regex RestOfDateTimeRegex { get; }

        IImmutableDictionary<string, string> UnitMap { get; }

        IImmutableDictionary<string, int> Numbers { get; }

        bool GetMatchedTimeRange(string text, out string timeStr, out int beginHour, out int endHour, out int endMin);

        int GetSwiftPrefix(string text);
    }
}
