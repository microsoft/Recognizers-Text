using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface ICJKSetParserConfiguration : IDateTimeOptionsConfiguration
    {
        IDateTimeExtractor DurationExtractor { get; }

        IDateTimeParser DurationParser { get; }

        IDateTimeExtractor TimeExtractor { get; }

        IDateTimeParser TimeParser { get; }

        IDateTimeExtractor DateExtractor { get; }

        IDateTimeParser DateParser { get; }

        IDateTimeExtractor DateTimeExtractor { get; }

        IDateTimeParser DateTimeParser { get; }

        IImmutableDictionary<string, string> UnitMap { get; }

        Regex EachPrefixRegex { get; }

        Regex EachUnitRegex { get; }

        Regex EachDayRegex { get; }

        bool GetMatchedUnitTimex(string text, out string timex);
    }
}
