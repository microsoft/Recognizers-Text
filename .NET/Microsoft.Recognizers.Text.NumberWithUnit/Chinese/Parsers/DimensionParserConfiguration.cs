using System.Globalization;

using Microsoft.Recognizers.Definitions.Chinese;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Chinese
{
    public class DimensionParserConfiguration : ChineseNumberWithUnitParserConfiguration
    {
        public DimensionParserConfiguration()
            : this(new CultureInfo(Culture.Chinese))
        {
        }

        public DimensionParserConfiguration(CultureInfo ci)
            : base(ci)
        {
            this.BindDictionary(NumbersWithUnitDefinitions.DimensionSuffixList);
        }
    }
}