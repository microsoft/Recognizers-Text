using Microsoft.Recognizers.Text.NumberWithUnit.English.Extractors;
using Microsoft.Recognizers.Text.Number.Utilities;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.English.Parsers
{
    public class DimensionParserConfiguration : EnglishNumberWithUnitParserConfiguration
    {
        public DimensionParserConfiguration() : this(new CultureInfo(Culture.English)) { }

        public DimensionParserConfiguration(CultureInfo ci) : base(ci)
        {
            this.BindDictionary(DimensionExtractorConfiguration.DimensionSuffixList);
        }
    }
}
