using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Hindi
{
    public class DimensionParserConfiguration : HindiNumberWithUnitParserConfiguration
    {
        public DimensionParserConfiguration()
               : this(new CultureInfo(Culture.Hindi))
        {
        }

        public DimensionParserConfiguration(CultureInfo ci)
               : base(ci)
        {
            this.BindDictionary(DimensionExtractorConfiguration.DimensionSuffixList);
        }
    }
}
