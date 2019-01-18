using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Italian
{
    public class LengthParserConfiguration : ItalianNumberWithUnitParserConfiguration
    {
        public LengthParserConfiguration()
            : this(new CultureInfo(Culture.Italian))
        {
        }

        public LengthParserConfiguration(CultureInfo ci)
            : base(ci)
        {
            this.BindDictionary(LengthExtractorConfiguration.LengthSuffixList);
        }
    }
}
