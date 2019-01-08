using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.German
{
    public class WeightParserConfiguration : GermanNumberWithUnitParserConfiguration
    {
        public WeightParserConfiguration()
            : this(new CultureInfo(Culture.German))
        {
        }

        public WeightParserConfiguration(CultureInfo ci)
            : base(ci)
        {
            this.BindDictionary(WeightExtractorConfiguration.WeightSuffixList);
        }
    }
}
