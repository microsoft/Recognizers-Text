using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface ICJKDateTimePeriodExtractorConfiguration : IDateTimeOptionsConfiguration
    {
        Regex PrepositionRegex { get; }

        Regex TillRegex { get; }

        Regex SpecificTimeOfDayRegex { get; }

        Regex TimeOfDayRegex { get; }

        Regex FollowedUnit { get; }

        Regex UnitRegex { get; }

        Regex PastRegex { get; }

        Regex FutureRegex { get; }

        IExtractor CardinalExtractor { get; }

        IDateTimeExtractor SingleDateExtractor { get; }

        IDateTimeExtractor SingleTimeExtractor { get; }

        IDateTimeExtractor SingleDateTimeExtractor { get; }

        IDateTimeExtractor DurationExtractor { get; }

        IDateTimeExtractor TimePeriodExtractor { get; }

        bool GetFromTokenIndex(string text, out int index);

        bool HasConnectorToken(string text);

        bool GetBetweenTokenIndex(string text, out int index);
    }
}