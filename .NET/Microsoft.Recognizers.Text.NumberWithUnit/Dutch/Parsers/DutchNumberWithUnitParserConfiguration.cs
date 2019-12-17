using System.Globalization;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Dutch;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Dutch
{
    public class DutchNumberWithUnitParserConfiguration : BaseNumberWithUnitParserConfiguration
    {
        public DutchNumberWithUnitParserConfiguration(CultureInfo ci)
            : base(ci)
        {

            var config = new BaseNumberOptionsConfiguration(Culture.Dutch, NumberOptions.None);

            this.InternalNumberExtractor = NumberExtractor.GetInstance(config);
            this.InternalNumberParser = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number,
                                                                              new DutchNumberParserConfiguration(config));
            this.ConnectorToken = string.Empty;
        }

        public override IParser InternalNumberParser { get; }

        public override IExtractor InternalNumberExtractor { get; }

        public override string ConnectorToken { get; }
    }
}
