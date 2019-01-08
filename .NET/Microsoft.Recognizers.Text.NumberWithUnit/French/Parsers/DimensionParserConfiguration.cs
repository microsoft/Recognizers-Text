using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.French
{
    public class DimensionParserConfiguration : FrenchNumberWithUnitParserConfiguration
    {
        public DimensionParserConfiguration()
            : this(new CultureInfo(Culture.French))
        {
        }

        public DimensionParserConfiguration(CultureInfo ci)
            : base(ci)
        {
            this.BindDictionary(DimensionExtractorConfiguration.DimensionSuffixList);
        }
    }
}
