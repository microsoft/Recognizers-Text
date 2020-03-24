using System.Globalization;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.German;

namespace Microsoft.Recognizers.Text.NumberWithUnit.German
{
    public class GermanNumberWithUnitParserConfiguration : BaseNumberWithUnitParserConfiguration
    {
        public GermanNumberWithUnitParserConfiguration(CultureInfo ci)
            : base(ci)
        {

            var numConfig = new BaseNumberOptionsConfiguration(Culture.German, NumberOptions.None);

            this.InternalNumberExtractor = NumberExtractor.GetInstance();
            this.InternalNumberParser = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number,
                                                                              new GermanNumberParserConfiguration(numConfig));
            this.ConnectorToken = string.Empty;
        }

        public override IParser InternalNumberParser { get; }

        public override IExtractor InternalNumberExtractor { get; }

        public override string ConnectorToken { get; }
    }
}
