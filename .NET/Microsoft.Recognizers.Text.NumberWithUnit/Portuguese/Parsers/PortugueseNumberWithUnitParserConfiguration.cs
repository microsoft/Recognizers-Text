using System.Globalization;

using Microsoft.Recognizers.Definitions.Portuguese;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Portuguese;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Portuguese
{
    public class PortugueseNumberWithUnitParserConfiguration : BaseNumberWithUnitParserConfiguration
    {
        public PortugueseNumberWithUnitParserConfiguration(CultureInfo ci)
               : base(ci)
        {
            this.InternalNumberExtractor = NumberExtractor.GetInstance();
            this.InternalNumberParser = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new PortugueseNumberParserConfiguration());
            this.ConnectorToken = NumbersWithUnitDefinitions.ConnectorToken;
        }

        public override IParser InternalNumberParser { get; }

        public override IExtractor InternalNumberExtractor { get; }

        public override string ConnectorToken { get; }
    }
}
