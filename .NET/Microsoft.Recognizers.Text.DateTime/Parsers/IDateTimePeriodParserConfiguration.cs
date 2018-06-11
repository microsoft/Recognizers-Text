using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IDateTimePeriodParserConfiguration : IOptionsConfiguration
    {
        string TokenBeforeDate { get; }

        IDateTimeExtractor DateExtractor { get; }

        IDateTimeExtractor TimeExtractor { get; }

        IDateTimeExtractor DateTimeExtractor { get; }

        IDateTimeExtractor TimePeriodExtractor { get; }

        IDateTimeExtractor DurationExtractor { get; }

        IExtractor CardinalExtractor { get; }

        IParser NumberParser { get; }

        IDateTimeParser DateParser { get; }

        IDateTimeParser TimeParser { get; }

        IDateTimeParser DateTimeParser { get; }

        IDateTimeParser TimePeriodParser { get; }

        IDateTimeParser DurationParser { get; }

        Regex PureNumberFromToRegex { get; }

        Regex PureNumberBetweenAndRegex { get; }

        Regex SpecificTimeOfDayRegex { get; }

        Regex TimeOfDayRegex { get; }

        Regex PastRegex { get; }

        Regex FutureRegex { get; }

        Regex FutureSuffixRegex { get; }

        Regex NumberCombinedWithUnitRegex { get; }

        Regex UnitRegex { get; }

        Regex PeriodTimeOfDayWithDateRegex { get; }

        Regex RelativeTimeUnitRegex { get; }

        Regex RestOfDateTimeRegex { get; }

        Regex AmDescRegex { get; }

        Regex PmDescRegex { get; }

        Regex WithinNextPrefixRegex { get; }

        Regex PrefixDayRegex { get; }

        IImmutableDictionary<string, string> UnitMap { get; }

        IImmutableDictionary<string, int> Numbers { get; }

        bool GetMatchedTimeRange(string text, out string timeStr, out int beginHour, out int endHour, out int endMin);

        int GetSwiftPrefix(string text);
    }
}
