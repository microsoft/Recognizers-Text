using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.Matcher;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IMergedParserConfiguration : ICommonDateTimeParserConfiguration
    {
        Regex BeforeRegex { get; }

        Regex AfterRegex { get; }

        Regex SinceRegex { get; }

        Regex AroundRegex { get; }

        Regex SuffixAfter { get; }

        Regex YearRegex { get; }

        IDateTimeParser SetParser { get; }

        IDateTimeParser HolidayParser { get; }

        StringMatcher SuperfluousWordMatcher { get; }
    }
}
