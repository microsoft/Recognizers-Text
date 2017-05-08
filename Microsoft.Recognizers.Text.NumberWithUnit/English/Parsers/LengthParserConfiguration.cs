using Microsoft.Recognizers.Text.NumberWithUnit.English.Extractors;
using Microsoft.Recognizers.Text.Number.Utilities;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.English.Parsers
{
    public class LengthParserConfiguration : EnglishNumberWithUnitParserConfiguration
    {
        public LengthParserConfiguration() : this(new CultureInfo(Culture.English)) { }

        public LengthParserConfiguration(CultureInfo ci) : base(ci)
        {
            this.BindDictionary(LengthExtractorConfiguration.LenghtSuffixList);
        }
    }
}
