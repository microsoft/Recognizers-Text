using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Options.Extractors
{
    public interface IBooleanExtractorConfiguration : IChoiceExtractorConfiguration
    {
        Regex TrueRegex { get; }

        Regex FalseRegex { get; }
    }
}
