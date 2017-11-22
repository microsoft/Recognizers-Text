using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.English;

namespace Microsoft.Recognizers.Text.DateTime.English
{
    public class EnglishDateTimeALTExtractorConfiguration : IDateTimeALTExtractorConfiguration
    {
        public EnglishDateTimeALTExtractorConfiguration()
        {
            DateExtractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration());
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