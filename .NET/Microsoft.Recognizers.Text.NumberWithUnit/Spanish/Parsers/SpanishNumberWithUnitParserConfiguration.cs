using System.Globalization;

using Microsoft.Recognizers.Definitions.Spanish;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Spanish;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Spanish
{
    public class SpanishNumberWithUnitParserConfiguration : BaseNumberWithUnitParserConfiguration
    {
        public SpanishNumberWithUnitParserConfiguration(CultureInfo ci)
               : base(ci)
        {

            var numConfig = new BaseNumberOptionsConfiguration(Culture.Spanish, NumberOptions.None);

            this.InternalNumberExtractor = NumberExtractor.GetInstance(numConfig);
            this.InternalNumberParser = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number,
                                                                              new SpanishNumberParserConfiguration(numConfig));
            this.ConnectorToken = NumbersWithUnitDefinitions.ConnectorToken;
        }

        public override IParser InternalNumberParser { get; }

        public override IExtractor InternalNumberExtractor { get; }

        public override string ConnectorToken { get; }
    }
}
