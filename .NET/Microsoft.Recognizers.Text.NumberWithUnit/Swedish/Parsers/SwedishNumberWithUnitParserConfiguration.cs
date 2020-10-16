using System.Globalization;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Swedish;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Swedish
{
    public class SwedishNumberWithUnitParserConfiguration : BaseNumberWithUnitParserConfiguration
    {
        public SwedishNumberWithUnitParserConfiguration(CultureInfo ci)
            : base(ci)
        {

            var config = new BaseNumberOptionsConfiguration(Culture.Swedish, NumberOptions.None);

            this.InternalNumberExtractor = NumberExtractor.GetInstance(config);
            this.InternalNumberParser = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number,
                                                                              new SwedishNumberParserConfiguration(config));
            this.ConnectorToken = string.Empty;
        }

        public override IParser InternalNumberParser { get; }

        public override IExtractor InternalNumberExtractor { get; }

        public override string ConnectorToken { get; }
    }
}
