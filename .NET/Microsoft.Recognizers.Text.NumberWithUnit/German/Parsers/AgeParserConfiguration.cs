using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.German
{
    public class AgeParserConfiguration : GermanNumberWithUnitParserConfiguration
    {
        public AgeParserConfiguration()
            : this(new CultureInfo(Culture.German))
        {
        }

        public AgeParserConfiguration(CultureInfo ci)
            : base(ci)
        {
            this.BindDictionary(AgeExtractorConfiguration.AgeSuffixList);
        }
    }
}
