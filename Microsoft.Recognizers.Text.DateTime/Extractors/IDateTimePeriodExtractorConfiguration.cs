using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IDateTimePeriodExtractorConfiguration
    {
        IEnumerable<Regex> SimpleCasesRegex { get; }
        Regex PrepositionRegex { get; }
        Regex TillRegex { get; }
        Regex SpecificNightRegex { get; }
        Regex NightRegex { get; }
        Regex FollowedUnit { get; }
        Regex NumberCombinedWithUnit { get; }
        Regex UnitRegex { get; }
        Regex PastRegex { get; }
        Regex FutureRegex { get; }

        IExtractor CardinalExtractor { get; }
        IExtractor SingleDateExtractor { get; }
        IExtractor SingleTimeExtractor { get; }
        IExtractor SingleDateTimeExtractor { get; }
        
        bool GetFromTokenIndex(string text, out int index);
        bool HasConnectorToken(string text);
        bool GetBetweenTokenIndex(string text, out int index);
    }
}