using System.Globalization;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Hindi;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Hindi
{
    public class HindiNumberWithUnitParserConfiguration : BaseNumberWithUnitParserConfiguration
    {
        public HindiNumberWithUnitParserConfiguration(CultureInfo ci)
               : base(ci)
        {
            this.InternalNumberExtractor = NumberExtractor.GetInstance();
            this.InternalNumberParser = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new HindiNumberParserConfiguration(
                                                                                  new BaseNumberOptionsConfiguration(ci.Name)));
            this.ConnectorToken = string.Empty;
        }

        public override IParser InternalNumberParser { get; }

        public override IExtractor InternalNumberExtractor { get; }

        public override string ConnectorToken { get; }
    }
}
