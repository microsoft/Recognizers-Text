using Microsoft.Recognizers.Text.NumberWithUnit.Spanish.Extractors;
using Microsoft.Recognizers.Text.Number.Utilities;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Spanish.Parsers
{
    public class AreaParserConfiguration : SpanishNumberWithUnitParserConfiguration
    {
        public AreaParserConfiguration() : this(new CultureInfo(Culture.Spanish)) { }

        public AreaParserConfiguration(CultureInfo ci) : base(ci)
        {
            this.BindDictionary(AreaExtractorConfiguration.AreaSuffixList);
        }
    }
}
