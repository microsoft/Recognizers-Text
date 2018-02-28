using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Choice.Extractors
{
    public interface IBooleanExtractorConfiguration : IChoiceExtractorConfiguration
    {
        Regex TrueRegex { get; }

        Regex FalseRegex { get; }
    }
}
