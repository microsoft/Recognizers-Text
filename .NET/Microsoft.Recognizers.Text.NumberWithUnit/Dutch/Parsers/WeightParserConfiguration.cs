using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Dutch
{
    public class WeightParserConfiguration : DutchNumberWithUnitParserConfiguration
    {
        public WeightParserConfiguration()
            : this(new CultureInfo(Culture.Dutch))
        {
        }

        public WeightParserConfiguration(CultureInfo ci)
            : base(ci)
        {
            this.BindDictionary(WeightExtractorConfiguration.WeightSuffixList);
        }
    }
}
