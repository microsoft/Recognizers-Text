using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IDateTimeParserConfiguration : IOptionsConfiguration
    {
        string TokenBeforeDate { get; }

        string TokenBeforeTime { get; }

        IDateExtractor DateExtractor { get; }

        IDateTimeExtractor TimeExtractor { get; }

        IDateTimeParser DateParser { get; }

        IDateTimeParser TimeParser { get; }

        IExtractor CardinalExtractor { get; }

        IExtractor IntegerExtractor { get; }

        IParser NumberParser { get; }

        IDateTimeExtractor DurationExtractor { get; }

        IDateTimeParser DurationParser { get; }

        Regex NowRegex { get; }

        Regex AMTimeRegex { get; }

        Regex PMTimeRegex { get; }

        Regex SimpleTimeOfTodayAfterRegex { get; }

        Regex SimpleTimeOfTodayBeforeRegex { get; }

        Regex SpecificTimeOfDayRegex { get; }

        Regex SpecificEndOfRegex { get; }

        Regex UnspecificEndOfRegex { get; }

        Regex UnitRegex { get; }

        Regex DateNumberConnectorRegex { get; }

        Regex YearRegex { get; }

        IImmutableDictionary<string, string> UnitMap { get; }

        IImmutableDictionary<string, int> Numbers { get; }

        IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        bool ContainsAmbiguousToken(string text, string matchedText);

        bool GetMatchedNowTimex(string text, out string timex);

        int GetSwiftDay(string text);

        int GetHour(string text, int hour);
    }
}
