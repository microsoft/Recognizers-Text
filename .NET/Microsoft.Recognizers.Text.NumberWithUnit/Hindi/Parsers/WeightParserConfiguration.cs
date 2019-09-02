using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Hindi
{
    public class WeightParserConfiguration : HindiNumberWithUnitParserConfiguration
    {
        public WeightParserConfiguration()
               : this(new CultureInfo(Culture.Hindi))
        {
        }

        public WeightParserConfiguration(CultureInfo ci)
               : base(ci)
        {
            this.BindDictionary(WeightExtractorConfiguration.WeightSuffixList);
        }
    }
}
