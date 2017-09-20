using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IDateTimePeriodExtractorConfiguration
    {
        IEnumerable<Regex> SimpleCasesRegex { get; }

        Regex PrepositionRegex { get; }

        Regex TillRegex { get; }

        Regex SpecificTimeOfDayRegex { get; }

        Regex TimeOfDayRegex { get; }

        Regex FollowedUnit { get; }

        Regex NumberCombinedWithUnit { get; }

        Regex TimeUnitRegex { get; }

        Regex PastPrefixRegex { get; }

        Regex NextPrefixRegex { get; }

        Regex WeekDayRegex { get; }

        Regex PeriodTimeOfDayWithDateRegex { get; }

        Regex RelativeTimeUnitRegex { get; }

        Regex RestOfDateTimeRegex { get; }

        IExtractor CardinalExtractor { get; }

        IExtractor SingleDateExtractor { get; }

        IExtractor SingleTimeExtractor { get; }

        IExtractor SingleDateTimeExtractor { get; }

        IExtractor DurationExtractor { get; }

        bool GetFromTokenIndex(string text, out int index);

        bool HasConnectorToken(string text);

        bool GetBetweenTokenIndex(string text, out int index);
    }
}