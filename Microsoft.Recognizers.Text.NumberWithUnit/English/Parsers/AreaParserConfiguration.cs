using Microsoft.Recognizers.Text.NumberWithUnit.English.Extractors;
using Microsoft.Recognizers.Text.Number.Utilities;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.English.Parsers
{
    public class AreaParserConfiguration : EnglishNumberWithUnitParserConfiguration
    {
        public AreaParserConfiguration() : this(new CultureInfo(Culture.English)) { }

        public AreaParserConfiguration(CultureInfo ci) : base(ci)
        {
            this.BindDictionary(AreaExtractorConfiguration.AreaSuffixList);
        }
    }
}
