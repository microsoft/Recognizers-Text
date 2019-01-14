using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Portuguese
{
    public class AreaParserConfiguration : PortugueseNumberWithUnitParserConfiguration
    {
        public AreaParserConfiguration()
               : this(new CultureInfo(Culture.Portuguese))
        {
        }

        public AreaParserConfiguration(CultureInfo ci)
               : base(ci)
        {
            this.BindDictionary(AreaExtractorConfiguration.AreaSuffixList);
        }
    }
}
