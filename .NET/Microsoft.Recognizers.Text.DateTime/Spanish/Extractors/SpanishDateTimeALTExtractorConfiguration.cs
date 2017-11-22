using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Spanish;

namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public class SpanishDateTimeALTExtractorConfiguration : IDateTimeALTExtractorConfiguration
    {
        public SpanishDateTimeALTExtractorConfiguration()
        {
            DateExtractor = new BaseDateExtractor(new SpanishDateExtractorConfiguration());
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