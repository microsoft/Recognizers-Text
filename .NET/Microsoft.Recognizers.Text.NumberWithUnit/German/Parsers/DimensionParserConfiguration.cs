using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.German
{
    public class DimensionParserConfiguration : GermanNumberWithUnitParserConfiguration
    {
        public DimensionParserConfiguration()
            : this(new CultureInfo(Culture.German))
        {
        }

        public DimensionParserConfiguration(CultureInfo ci)
            : base(ci)
        {
            this.BindDictionary(DimensionExtractorConfiguration.DimensionSuffixList);
        }
    }
}
