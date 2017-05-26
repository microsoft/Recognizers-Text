using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.Parsers
{
    public interface ITimePeriodParserConfiguration
    {
        IExtractor TimeExtractor { get; }
        IDateTimeParser TimeParser { get; }

        Regex PureNumberFromToRegex { get; }
        Regex PureNumberBetweenAndRegex { get; }
        
        IImmutableDictionary<string, int> Numbers { get; }

        bool GetMatchedTimexRange(string text, out string timex, out int beginHour, out int endHour, out int endMin);
    }
}
