using System.Globalization;

using Microsoft.Recognizers.Text.Number.Chinese;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Chinese
{
    public class ChineseNumberWithUnitParserConfiguration : BaseNumberWithUnitParserConfiguration
    {
        public ChineseNumberWithUnitParserConfiguration(CultureInfo ci) : base(ci)
        {
            this.InternalNumberExtractor = new NumberExtractor(ChineseNumberMode.Default);
            this.InternalNumberParser = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new ChineseNumberParserConfiguration());
            this.ConnectorToken = string.Empty;
        }

        public override IParser InternalNumberParser { get; }

        public override IExtractor InternalNumberExtractor { get; }

        public override string ConnectorToken { get; }
    }
}
