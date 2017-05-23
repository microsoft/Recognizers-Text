using Microsoft.Recognizers.Text.NumberWithUnit.Spanish.Extractors;
using Microsoft.Recognizers.Text.Number.Utilities;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Spanish.Parsers
{
    public class WeightParserConfiguration : SpanishNumberWithUnitParserConfiguration
    {
        public WeightParserConfiguration() : this(new CultureInfo(Culture.Spanish)) { }

        public WeightParserConfiguration(CultureInfo ci) : base(ci)
        {
            this.BindDictionary(WeightExtractorConfiguration.WeightSuffixList);
        }
    }
}
