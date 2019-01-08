using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.French
{
    public class SpeedParserConfiguration : FrenchNumberWithUnitParserConfiguration
    {
        public SpeedParserConfiguration()
            : this(new CultureInfo(Culture.French))
        {
        }

        public SpeedParserConfiguration(CultureInfo ci)
            : base(ci)
        {
            this.BindDictionary(SpeedExtractorConfiguration.SpeedSuffixList);
        }
    }
}
