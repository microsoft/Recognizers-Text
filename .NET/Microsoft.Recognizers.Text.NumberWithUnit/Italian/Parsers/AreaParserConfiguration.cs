using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Italian
{
    public class AreaParserConfiguration : ItalianNumberWithUnitParserConfiguration
    {
        public AreaParserConfiguration()
            : this(new CultureInfo(Culture.Italian))
        {
        }

        public AreaParserConfiguration(CultureInfo ci)
            : base(ci)
        {
            this.BindDictionary(AreaExtractorConfiguration.AreaSuffixList);
        }
    }
}
