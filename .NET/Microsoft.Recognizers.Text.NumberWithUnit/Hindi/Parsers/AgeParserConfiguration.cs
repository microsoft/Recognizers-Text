using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Hindi
{
    public class AgeParserConfiguration : HindiNumberWithUnitParserConfiguration
    {
        public AgeParserConfiguration()
               : this(new CultureInfo(Culture.Hindi))
        {
        }

        public AgeParserConfiguration(CultureInfo ci)
               : base(ci)
        {
            this.BindDictionary(AgeExtractorConfiguration.AgeSuffixList);
        }
    }
}
