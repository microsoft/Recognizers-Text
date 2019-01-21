using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Dutch
{
    public class SpeedParserConfiguration : DutchNumberWithUnitParserConfiguration
    {
        public SpeedParserConfiguration()
            : this(new CultureInfo(Culture.Dutch))
        {
        }

        public SpeedParserConfiguration(CultureInfo ci)
            : base(ci)
        {
            this.BindDictionary(SpeedExtractorConfiguration.SpeedSuffixList);
        }
    }
}
