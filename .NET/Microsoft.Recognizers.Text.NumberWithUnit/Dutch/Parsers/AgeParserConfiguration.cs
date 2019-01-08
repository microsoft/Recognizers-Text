using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Dutch
{
    public class AgeParserConfiguration : DutchNumberWithUnitParserConfiguration
    {
        public AgeParserConfiguration()
            : this(new CultureInfo(Culture.Dutch))
        {
        }

        public AgeParserConfiguration(CultureInfo ci)
            : base(ci)
        {
            this.BindDictionary(AgeExtractorConfiguration.AgeSuffixList);
        }
    }
}
