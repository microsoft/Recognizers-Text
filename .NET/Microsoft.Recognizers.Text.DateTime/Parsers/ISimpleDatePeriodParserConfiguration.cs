using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface ISimpleDatePeriodParserConfiguration
    {
        Regex YearRegex { get; }

        Regex RelativeRegex { get; }

        IDateExtractor DateExtractor { get; }
    }
}
