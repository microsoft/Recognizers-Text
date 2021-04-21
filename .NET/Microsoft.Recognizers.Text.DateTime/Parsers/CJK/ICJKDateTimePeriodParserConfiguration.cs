using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface ICJKDateTimePeriodParserConfiguration : IDateTimeOptionsConfiguration
    {
        IDateTimeExtractor DateExtractor { get; }

        IDateTimeExtractor TimeExtractor { get; }

        IDateTimeExtractor DateTimeExtractor { get; }

        IDateTimeExtractor TimePeriodExtractor { get; }

        IDateTimeExtractor DurationExtractor { get; }

        IExtractor CardinalExtractor { get; }

        IParser CardinalParser { get; }

        IDateTimeParser DateParser { get; }

        IDateTimeParser TimeParser { get; }

        IDateTimeParser DateTimeParser { get; }

        IDateTimeParser TimePeriodParser { get; }

        Regex SpecificTimeOfDayRegex { get; }

        Regex TimeOfDayRegex { get; }

        Regex NextRegex { get; }

        Regex LastRegex { get; }

        Regex PastRegex { get; }

        Regex FutureRegex { get; }

        Regex UnitRegex { get; }

        IImmutableDictionary<string, string> UnitMap { get; }

        bool GetMatchedTimeRange(string text, out string timeStr, out int beginHour, out int endHour, out int endMin);

        bool GetMatchedTimeRangeAndSwift(string text, out string timeStr, out int beginHour, out int endHour, out int endMin);
    }
}
