using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.German
{
    public class LengthParserConfiguration : GermanNumberWithUnitParserConfiguration
    {
        public LengthParserConfiguration()
            : this(new CultureInfo(Culture.German))
        {
        }

        public LengthParserConfiguration(CultureInfo ci)
            : base(ci)
        {
            this.BindDictionary(LengthExtractorConfiguration.LengthSuffixList);
        }
    }
}
