using Microsoft.Recognizers.Text.NumberWithUnit.Chinese.Extractors;
using Microsoft.Recognizers.Text.Number.Utilities;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Chinese.Parsers
{
    public class AgeParserConfiguration : ChineseNumberWithUnitParserConfiguration
    {
        public AgeParserConfiguration() : this(new CultureInfo(Culture.Chinese)) { }

        public AgeParserConfiguration(CultureInfo ci) : base(ci)
        {
            this.BindDictionary(AgeExtractorConfiguration.AgeSuffixList);
        }
    }
}
