using Microsoft.Recognizers.Text.NumberWithUnit.Spanish.Extractors;
using Microsoft.Recognizers.Text.Number.Utilities;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Spanish.Parsers
{
    public class SpeedParserConfiguration : SpanishNumberWithUnitParserConfiguration
    {
        public SpeedParserConfiguration() : this(new CultureInfo(Culture.Spanish)) { }

        public SpeedParserConfiguration(CultureInfo ci) : base(ci)
        {
            this.BindDictionary(SpeedExtractorConfiguration.SpeedSuffixList);
        }
    }
}
