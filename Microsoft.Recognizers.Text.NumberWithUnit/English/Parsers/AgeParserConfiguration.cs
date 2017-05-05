using Microsoft.Recognizers.Text.NumberWithUnit.English.Extractors;
using Microsoft.Recognizers.Text.Number.Utilities;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.English.Parsers
{
    public class AgeParserConfiguration : EnglishNumberWithUnitParserConfiguration
    {
        public AgeParserConfiguration() : this(new CultureInfo(Culture.English)) { }

        public AgeParserConfiguration(CultureInfo ci) : base(ci)
        {
            this.BindDictionary(AgeExtractorConfiguration.AgeSuffixList);
        }
    }
}
