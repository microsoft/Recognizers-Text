using System.Globalization;

using Microsoft.Recognizers.Definitions.Chinese;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Chinese
{
    public class AgeParserConfiguration : ChineseNumberWithUnitParserConfiguration
    {
        public AgeParserConfiguration()
            : this(new CultureInfo(Culture.Chinese))
        {
        }

        public AgeParserConfiguration(CultureInfo ci)
            : base(ci)
        {
            this.BindDictionary(NumbersWithUnitDefinitions.AgeSuffixList);
        }
    }
}
