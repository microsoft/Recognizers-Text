using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.English
{
    public class LengthParserConfiguration : EnglishNumberWithUnitParserConfiguration
    {
        public LengthParserConfiguration()
               : this(new CultureInfo(Culture.English))
        {
        }

        public LengthParserConfiguration(CultureInfo ci)
               : base(ci)
        {
            this.BindDictionary(LengthExtractorConfiguration.LengthSuffixList);
        }
    }
}
