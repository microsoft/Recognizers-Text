using Microsoft.Recognizers.Text.NumberWithUnit.English.Extractors;
using Microsoft.Recognizers.Text.Number.Utilities;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.English.Parsers
{
    public class WeightParserConfiguration : EnglishNumberWithUnitParserConfiguration
    {
        public WeightParserConfiguration() : this(new CultureInfo(Culture.English)) { }

        public WeightParserConfiguration(CultureInfo ci) : base(ci)
        {
            this.BindDictionary(WeightExtractorConfiguration.WeightSuffixList);
        }
    }
}
