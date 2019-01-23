using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Dutch
{
    public class AreaParserConfiguration : DutchNumberWithUnitParserConfiguration
    {
        public AreaParserConfiguration()
            : this(new CultureInfo(Culture.Dutch))
        {
        }

        public AreaParserConfiguration(CultureInfo ci)
            : base(ci)
        {
            this.BindDictionary(AreaExtractorConfiguration.AreaSuffixList);
        }
    }
}
