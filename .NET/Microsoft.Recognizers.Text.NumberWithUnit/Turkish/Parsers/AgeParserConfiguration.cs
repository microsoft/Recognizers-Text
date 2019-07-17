using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Turkish
{
    public class AgeParserConfiguration : TurkishNumberWithUnitParserConfiguration
    {
        public AgeParserConfiguration()
               : this(new CultureInfo(Culture.Turkish))
        {
        }

        public AgeParserConfiguration(CultureInfo ci)
               : base(ci)
        {
            this.BindDictionary(AgeExtractorConfiguration.AgeSuffixList);
        }
    }
}
