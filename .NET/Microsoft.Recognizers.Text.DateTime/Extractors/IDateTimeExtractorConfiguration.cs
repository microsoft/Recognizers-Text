using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IDateTimeExtractorConfiguration : IOptionsConfiguration
    {
        Regex NowRegex { get; }

        Regex SuffixRegex { get; }

        Regex TimeOfTodayAfterRegex { get; }

        Regex SimpleTimeOfTodayAfterRegex { get; }

        Regex TimeOfTodayBeforeRegex { get; }

        Regex SimpleTimeOfTodayBeforeRegex { get; }

        Regex TimeOfDayRegex { get; }

        Regex SpecificEndOfRegex { get; }

        Regex UnspecificEndOfRegex { get; }

        Regex UnitRegex { get; }

        Regex NumberAsTimeRegex { get; }

        Regex DateNumberConnectorRegex { get; }

        IDateTimeExtractor DurationExtractor { get; }

        IDateExtractor DatePointExtractor { get; }

        IDateTimeExtractor TimePointExtractor { get; }

        IExtractor IntegerExtractor { get; }

        bool IsConnector(string text);

        IDateTimeUtilityConfiguration UtilityConfiguration { get; }
    }
}