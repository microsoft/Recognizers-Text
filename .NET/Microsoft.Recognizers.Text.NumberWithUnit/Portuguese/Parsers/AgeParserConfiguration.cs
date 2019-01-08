using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Portuguese
{
    public class AgeParserConfiguration : PortugueseNumberWithUnitParserConfiguration
    {
        public AgeParserConfiguration()
               : this(new CultureInfo(Culture.Portuguese))
        {
        }

        public AgeParserConfiguration(CultureInfo ci)
               : base(ci)
        {
            this.BindDictionary(AgeExtractorConfiguration.AgeSuffixList);
        }
    }
}