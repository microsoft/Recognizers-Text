using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IDateTimeParserConfiguration
    {
        string TokenBeforeDate { get; }

        string TokenBeforeTime { get; }

        IDateTimeExtractor DateExtractor { get; }

        IDateTimeExtractor TimeExtractor { get; }

        IDateTimeParser DateParser { get; }

        IDateTimeParser TimeParser { get; }

        IExtractor CardinalExtractor { get; }

        IParser NumberParser { get; }

        IDateTimeExtractor DurationExtractor { get; }

        IDateTimeParser DurationParser { get; }

        Regex NowRegex { get; }

        Regex AMTimeRegex { get; }

        Regex PMTimeRegex { get; }

        Regex SimpleTimeOfTodayAfterRegex { get; }

        Regex SimpleTimeOfTodayBeforeRegex { get; }

        Regex SpecificTimeOfDayRegex { get; }

        Regex TheEndOfRegex { get; }

        Regex UnitRegex { get; }

        IImmutableDictionary<string, string> UnitMap { get; }

        IImmutableDictionary<string, int> Numbers { get; }

        bool HaveAmbiguousToken(string text, string matchedText);

        bool GetMatchedNowTimex(string text, out string timex);

        int GetSwiftDay(string text);

        int GetHour(string text, int hour);

        IDateTimeUtilityConfiguration UtilityConfiguration { get; }
    }
}
