using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.Matcher;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface ICJKMergedParserConfiguration : ICJKCommonDateTimeParserConfiguration
    {
        Regex BeforeRegex { get; }

        Regex AfterRegex { get; }

        Regex SincePrefixRegex { get; }

        Regex SinceSuffixRegex { get; }

        Regex UntilRegex { get; }

        Regex EqualRegex { get; }
    }
}
