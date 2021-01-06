using System;
using System.Collections.Immutable;
using Microsoft.Recognizers.Definitions.German;

namespace Microsoft.Recognizers.Text.DateTime.German
{
    public class GermanHolidayParserConfiguration : BaseHolidayParserConfiguration
    {
        public GermanHolidayParserConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            this.HolidayRegexList = GermanHolidayExtractorConfiguration.HolidayRegexList;
            this.HolidayNames = DateTimeDefinitions.HolidayNames.ToImmutableDictionary();
        }

        public override int GetSwiftYear(string text)
        {
            var trimmedText = text.Trim();
            var swift = -10;

            // @TODO move hardcoded terms to resource file
            if (trimmedText.StartsWith("nächster", StringComparison.Ordinal) ||
                trimmedText.StartsWith("nächstes", StringComparison.Ordinal) ||
                trimmedText.StartsWith("nächsten", StringComparison.Ordinal) ||
                trimmedText.StartsWith("nächste", StringComparison.Ordinal))
            {
                swift = 1;
            }
            else if (trimmedText.StartsWith("letzter", StringComparison.Ordinal) ||
                     trimmedText.StartsWith("letztes", StringComparison.Ordinal) ||
                     trimmedText.StartsWith("letzten", StringComparison.Ordinal) ||
                     trimmedText.StartsWith("letzte", StringComparison.Ordinal))
            {
                swift = -1;
            }
            else if (trimmedText.StartsWith("dieser", StringComparison.Ordinal) ||
                     trimmedText.StartsWith("dieses", StringComparison.Ordinal) ||
                     trimmedText.StartsWith("diesen", StringComparison.Ordinal) ||
                     trimmedText.StartsWith("diese", StringComparison.Ordinal))
            {
                swift = 0;
            }

            return swift;
        }

        public override string SanitizeHolidayToken(string holiday)
        {
            return holiday
                .Replace(" ", string.Empty)
                .Replace("'", string.Empty);
        }
    }
}
