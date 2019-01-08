using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.French
{
    public class WeightParserConfiguration : FrenchNumberWithUnitParserConfiguration
    {
        public WeightParserConfiguration()
            : this(new CultureInfo(Culture.French))
        {
        }

        public WeightParserConfiguration(CultureInfo ci)
            : base(ci)
        {
            this.BindDictionary(WeightExtractorConfiguration.WeightSuffixList);
        }
    }
}
