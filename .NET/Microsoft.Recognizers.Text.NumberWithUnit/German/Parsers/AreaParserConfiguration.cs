using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.German
{
    public class AreaParserConfiguration : GermanNumberWithUnitParserConfiguration
    {
        public AreaParserConfiguration()
            : this(new CultureInfo(Culture.German))
        {
        }

        public AreaParserConfiguration(CultureInfo ci)
            : base(ci)
        {
            this.BindDictionary(AreaExtractorConfiguration.AreaSuffixList);
        }
    }
}
