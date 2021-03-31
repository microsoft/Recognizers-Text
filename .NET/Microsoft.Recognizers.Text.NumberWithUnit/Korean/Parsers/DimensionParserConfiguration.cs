using System.Globalization;

using Microsoft.Recognizers.Definitions.Korean;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Korean
{
    public class DimensionParserConfiguration : KoreanNumberWithUnitParserConfiguration
    {
        public DimensionParserConfiguration()
            : this(new CultureInfo(Culture.Korean))
        {
        }

        public DimensionParserConfiguration(CultureInfo ci)
            : base(ci)
        {
            this.BindDictionary(NumbersWithUnitDefinitions.DimensionSuffixList);
        }
    }
}