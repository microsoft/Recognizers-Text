using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Swedish
{
    public class AgeParserConfiguration : SwedishNumberWithUnitParserConfiguration
    {
        public AgeParserConfiguration()
            : this(new CultureInfo(Culture.Swedish))
        {
        }

        public AgeParserConfiguration(CultureInfo ci)
            : base(ci)
        {
            this.BindDictionary(AgeExtractorConfiguration.AgeSuffixList);
        }
    }
}
