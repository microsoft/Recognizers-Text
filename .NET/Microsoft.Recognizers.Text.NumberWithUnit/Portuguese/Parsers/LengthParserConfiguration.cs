using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Portuguese
{
    public class LengthParserConfiguration : PortugueseNumberWithUnitParserConfiguration
    {
        public LengthParserConfiguration()
               : this(new CultureInfo(Culture.Portuguese))
        {
        }

        public LengthParserConfiguration(CultureInfo ci)
               : base(ci)
        {
            this.BindDictionary(LengthExtractorConfiguration.LengthSuffixList);
        }
    }
}
