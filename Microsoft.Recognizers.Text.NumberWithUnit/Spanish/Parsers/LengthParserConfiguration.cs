using Microsoft.Recognizers.Text.NumberWithUnit.Spanish.Extractors;
using Microsoft.Recognizers.Text.Number.Utilities;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Spanish.Parsers
{
    public class LengthParserConfiguration : SpanishNumberWithUnitParserConfiguration
    {
        public LengthParserConfiguration() : this(new CultureInfo(Culture.Spanish)) { }

        public LengthParserConfiguration(CultureInfo ci) : base(ci)
        {
            this.BindDictionary(LengthExtractorConfiguration.LenghtSuffixList);
        }
    }
}
