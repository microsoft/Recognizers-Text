using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Portuguese
{
    public class SpeedParserConfiguration : PortugueseNumberWithUnitParserConfiguration
    {
        public SpeedParserConfiguration()
               : this(new CultureInfo(Culture.Portuguese))
        {
        }

        public SpeedParserConfiguration(CultureInfo ci)
               : base(ci)
        {
            this.BindDictionary(SpeedExtractorConfiguration.SpeedSuffixList);
        }
    }
}
