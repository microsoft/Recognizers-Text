using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.English
{
    public class AgeParserConfiguration : EnglishNumberWithUnitParserConfiguration
    {
        public AgeParserConfiguration()
               : this(new CultureInfo(Culture.English))
        {
        }

        public AgeParserConfiguration(CultureInfo ci)
               : base(ci)
        {
            this.BindDictionary(AgeExtractorConfiguration.AgeSuffixList);
        }
    }
}
