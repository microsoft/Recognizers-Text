using Microsoft.Recognizers.Text.Number.Extractors;
using Microsoft.Recognizers.Text.Number.Parsers;
using Microsoft.Recognizers.Text.Number.Spanish.Extractors;
using Microsoft.Recognizers.Text.Number.Spanish.Parsers;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Spanish.Parsers
{
    public class SpanishNumberWithUnitParserConfiguration : BaseNumberWithUnitParserConfiguration
    {
        public SpanishNumberWithUnitParserConfiguration(CultureInfo ci) : base(ci)
        {
            this.InternalNumberExtractor = new NumberExtractor(NumberMode.Default);
            this.InternalNumberParser = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new SpanishNumberParserConfiguration());
            this.ConnectorToken = "de";
        }

        public override IParser InternalNumberParser { get; }

        public override IExtractor InternalNumberExtractor { get; }

        public override string ConnectorToken { get; }
    }
}
