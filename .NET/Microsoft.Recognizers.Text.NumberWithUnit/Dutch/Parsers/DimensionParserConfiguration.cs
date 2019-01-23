using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Dutch
{
    public class DimensionParserConfiguration : DutchNumberWithUnitParserConfiguration
    {
        public DimensionParserConfiguration()
            : this(new CultureInfo(Culture.Dutch))
        {
        }

        public DimensionParserConfiguration(CultureInfo ci)
            : base(ci)
        {
            this.BindDictionary(DimensionExtractorConfiguration.DimensionSuffixList);
        }
    }
}
