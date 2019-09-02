using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Hindi
{
    public class AreaParserConfiguration : HindiNumberWithUnitParserConfiguration
    {
        public AreaParserConfiguration()
               : this(new CultureInfo(Culture.Hindi))
        {
        }

        public AreaParserConfiguration(CultureInfo ci)
               : base(ci)
        {
            this.BindDictionary(AreaExtractorConfiguration.AreaSuffixList);
        }
    }
}
