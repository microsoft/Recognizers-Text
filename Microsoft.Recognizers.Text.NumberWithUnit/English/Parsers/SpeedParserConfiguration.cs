using Microsoft.Recognizers.Text.NumberWithUnit.English.Extractors;
using Microsoft.Recognizers.Text.Number.Utilities;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.English.Parsers
{
    public class SpeedParserConfiguration : EnglishNumberWithUnitParserConfiguration
    {
        public SpeedParserConfiguration() : this(new CultureInfo(Culture.English)) { }

        public SpeedParserConfiguration(CultureInfo ci) : base(ci)
        {
            this.BindDictionary(SpeedExtractorConfiguration.SpeedSuffixList);
        }
    }
}
