using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Italian
{
    public class SpeedParserConfiguration : ItalianNumberWithUnitParserConfiguration
    {
        public SpeedParserConfiguration()
            : this(new CultureInfo(Culture.Italian))
        {
        }

        public SpeedParserConfiguration(CultureInfo ci)
            : base(ci)
        {
            this.BindDictionary(SpeedExtractorConfiguration.SpeedSuffixList);
        }
    }
}
