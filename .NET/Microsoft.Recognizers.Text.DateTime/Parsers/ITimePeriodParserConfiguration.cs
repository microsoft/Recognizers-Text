using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface ITimePeriodParserConfiguration : IOptionsConfiguration
    {
        IDateTimeExtractor TimeExtractor { get; }

        IDateTimeParser TimeParser { get; }

        IExtractor IntegerExtractor { get; }

        Regex PureNumberFromToRegex { get; }

        Regex PureNumberBetweenAndRegex { get; }

        Regex SpecificTimeFromToRegex { get; }

        Regex SpecificTimeBetweenAndRegex { get; }
        
        Regex TimeOfDayRegex { get; }

        Regex GeneralEndingRegex { get; }

        Regex TillRegex { get; }

        IImmutableDictionary<string, int> Numbers { get; }

        IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        bool GetMatchedTimexRange(string text, out string timex, out int beginHour, out int endHour, out int endMin);
    }
}
