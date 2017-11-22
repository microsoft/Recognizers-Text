using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.French;

namespace Microsoft.Recognizers.Text.DateTime.French
{
    public class FrenchDateTimeALTExtractorConfiguration : IDateTimeALTExtractorConfiguration
    {
        public FrenchDateTimeALTExtractorConfiguration()
        {
            DateExtractor = new BaseDateExtractor(new FrenchDateExtractorConfiguration());
        }

        public IDateTimeExtractor DateExtractor { get; }

        private static readonly Regex OrRegex =
            new Regex(DateTimeDefinitions.OrRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public bool IsConnector(string text)
        {
            text = text.Trim();
            var match = OrRegex.Match(text);
            return match.Success;
        }
    }
}