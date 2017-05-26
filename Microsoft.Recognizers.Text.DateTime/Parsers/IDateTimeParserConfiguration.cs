using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.Parsers
{
    public interface IDateTimeParserConfiguration
    {
        string TokenBeforeDate { get; }
        string TokenBeforeTime { get; }
        IExtractor DateExtractor { get; }
        IExtractor TimeExtractor { get; }
        IDateTimeParser DateParser { get; }
        IDateTimeParser TimeParser { get; }

        Regex NowRegex { get; }
        Regex AMTimeRegex { get; }
        Regex PMTimeRegex { get; }
        Regex SimpleTimeOfTodayAfterRegex { get; }
        Regex SimpleTimeOfTodayBeforeRegex { get; }
        Regex SpecificNightRegex { get; }
        Regex TheEndOfRegex { get; }


        IImmutableDictionary<string, int> Numbers { get; }

        bool HaveAmbiguousToken(string text, string matchedText);
        bool GetMatchedNowTimex(string text, out string timex);
        int GetSwiftDay(string text);
        int GetHour(string text, int hour);
    }
}
