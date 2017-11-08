using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface ITimePeriodParserConfiguration
    {
        IDateTimeExtractor TimeExtractor { get; }

        IDateTimeParser TimeParser { get; }

        Regex PureNumberFromToRegex { get; }

        Regex PureNumberBetweenAndRegex { get; }
        
        Regex TimeOfDayRegex { get; }

        IImmutableDictionary<string, int> Numbers { get; }

        IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        bool GetMatchedTimexRange(string text, out string timex, out int beginHour, out int endHour, out int endMin);
    }
}
