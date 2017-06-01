using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.Extractors
{
    public interface IDateTimeExtractorConfiguration
    {
        Regex NowRegex { get; }
        Regex SuffixRegex { get; }
        Regex TimeOfTodayAfterRegex { get; }
        Regex SimpleTimeOfTodayAfterRegex { get; }
        Regex TimeOfTodayBeforeRegex { get; }
        Regex SimpleTimeOfTodayBeforeRegex { get; }
        Regex NightRegex { get; }
        Regex TheEndOfRegex { get; }

        IExtractor DatePointExtractor { get; }
        IExtractor TimePointExtractor { get; }

        bool IsConnector(string text);
    }
}