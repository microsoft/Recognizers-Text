using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Turkish
{
    public class SpeedParserConfiguration : TurkishNumberWithUnitParserConfiguration
    {
        public SpeedParserConfiguration()
               : this(new CultureInfo(Culture.Turkish))
        {
        }

        public SpeedParserConfiguration(CultureInfo ci)
               : base(ci)
        {
            this.BindDictionary(SpeedExtractorConfiguration.SpeedSuffixList);
        }
    }
}
