using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.English
{
    public class DimensionParserConfiguration : EnglishNumberWithUnitParserConfiguration
    {
        public DimensionParserConfiguration()
               : this(new CultureInfo(Culture.English))
        {
        }

        public DimensionParserConfiguration(CultureInfo ci)
               : base(ci)
        {
            this.BindDictionary(DimensionExtractorConfiguration.DimensionSuffixList);
        }
    }
}
