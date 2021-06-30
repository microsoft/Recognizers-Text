using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface ICJKTimePeriodParserConfiguration : IDateTimeOptionsConfiguration
    {
        IDateTimeExtractor TimeExtractor { get; }

        IDateTimeParser TimeParser { get; }

        TimeFunctions TimeFunc { get; }

        bool GetMatchedTimexRange(string text, out string timex, out int beginHour, out int endHour, out int endMin);
    }
}
