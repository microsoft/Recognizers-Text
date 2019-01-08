using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Dutch
{
    public class LengthParserConfiguration : DutchNumberWithUnitParserConfiguration
    {
        public LengthParserConfiguration()
            : this(new CultureInfo(Culture.Dutch))
        {
        }

        public LengthParserConfiguration(CultureInfo ci)
            : base(ci)
        {
            this.BindDictionary(LengthExtractorConfiguration.LengthSuffixList);
        }
    }
}
