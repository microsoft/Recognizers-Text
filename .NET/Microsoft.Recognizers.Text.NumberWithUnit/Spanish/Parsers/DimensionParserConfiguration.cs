using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Spanish
{
    public class DimensionParserConfiguration : SpanishNumberWithUnitParserConfiguration
    {
        public DimensionParserConfiguration()
               : this(new CultureInfo(Culture.Spanish))
        {
        }

        public DimensionParserConfiguration(CultureInfo ci)
               : base(ci)
        {
            this.BindDictionary(DimensionExtractorConfiguration.DimensionSuffixList);
        }
    }
}
