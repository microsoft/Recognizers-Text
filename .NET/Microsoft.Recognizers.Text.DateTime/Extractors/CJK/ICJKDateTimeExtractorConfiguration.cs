using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface ICJKDateTimeExtractorConfiguration : IDateTimeOptionsConfiguration
    {
        Regex NowRegex { get; }

        Regex PrepositionRegex { get; }

        Regex NightRegex { get; }

        Regex TimeOfTodayRegex { get; }

        Regex BeforeRegex { get; }

        Regex AfterRegex { get; }

        IDateTimeExtractor DurationExtractor { get; }

        IDateTimeExtractor DatePointExtractor { get; }

        IDateTimeExtractor TimePointExtractor { get; }
    }
}