using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Spanish
{
    public class WeightParserConfiguration : SpanishNumberWithUnitParserConfiguration
    {
        public WeightParserConfiguration()
               : this(new CultureInfo(Culture.Spanish))
        {
        }

        public WeightParserConfiguration(CultureInfo ci)
               : base(ci)
        {
            this.BindDictionary(WeightExtractorConfiguration.WeightSuffixList);
        }
    }
}
