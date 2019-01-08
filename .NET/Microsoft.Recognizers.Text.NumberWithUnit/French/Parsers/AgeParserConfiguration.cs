using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.French
{
    public class AgeParserConfiguration : FrenchNumberWithUnitParserConfiguration
    {
        public AgeParserConfiguration()
            : this(new CultureInfo(Culture.French))
        {
        }

        public AgeParserConfiguration(CultureInfo ci)
            : base(ci)
        {
            this.BindDictionary(AgeExtractorConfiguration.AgeSuffixList);
        }
    }
}
