using System.Globalization;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Chinese;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Chinese
{
    public class ChineseNumberWithUnitParserConfiguration : BaseNumberWithUnitParserConfiguration
    {
        public ChineseNumberWithUnitParserConfiguration(CultureInfo ci)
            : base(ci)
        {

            var numConfig = new BaseNumberOptionsConfiguration(ci.Name, NumberOptions.None);

            this.InternalNumberExtractor = new NumberExtractor(numConfig);
            this.InternalNumberParser = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number,
                                                                              new ChineseNumberParserConfiguration(numConfig));
            this.ConnectorToken = string.Empty;
        }

        public override IParser InternalNumberParser { get; }

        public override IExtractor InternalNumberExtractor { get; }

        public override string ConnectorToken { get; }
    }
}
