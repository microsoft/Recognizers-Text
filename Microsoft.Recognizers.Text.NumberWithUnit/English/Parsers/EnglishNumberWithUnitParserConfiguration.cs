using Microsoft.Recognizers.Text.Number.English.Extractors;
using Microsoft.Recognizers.Text.Number.English.Parsers;
using Microsoft.Recognizers.Text.Number.Extractors;
using Microsoft.Recognizers.Text.Number.Parsers;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.English.Parsers
{
    public class EnglishNumberWithUnitParserConfiguration : BaseNumberWithUnitParserConfiguration
    {
        public EnglishNumberWithUnitParserConfiguration(CultureInfo ci) : base(ci)
        {
            this.InternalNumberExtractor = new NumberExtractor(NumberMode.Default);
            this.InternalNumberParser = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new EnglishNumberParserConfiguration());
            this.ConnectorToken = string.Empty;
        }

        public override IParser InternalNumberParser { get; }

        public override IExtractor InternalNumberExtractor { get; }

        public override string ConnectorToken { get; }
    }
}
