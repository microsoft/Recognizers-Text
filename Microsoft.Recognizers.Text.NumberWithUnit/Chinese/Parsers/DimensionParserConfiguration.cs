using Microsoft.Recognizers.Text.NumberWithUnit.Chinese.Extractors;
using Microsoft.Recognizers.Text.Number.Utilities;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Chinese.Parsers
{
    public class DimensionParserConfiguration : ChineseNumberWithUnitParserConfiguration
    {
        public DimensionParserConfiguration() : this(new CultureInfo(Culture.Chinese)) { }

        public DimensionParserConfiguration(CultureInfo ci) : base(ci)
        {
            this.BindDictionary(DimensionExtractorConfiguration.DimensionSuffixList);
        }
    }
}
