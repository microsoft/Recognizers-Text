using System.Globalization;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.English;

namespace Microsoft.Recognizers.Text.NumberWithUnit.English
{
    public class EnglishNumberWithUnitParserConfiguration : BaseNumberWithUnitParserConfiguration
    {
        public EnglishNumberWithUnitParserConfiguration(CultureInfo ci)
               : base(ci)
        {
            this.InternalNumberExtractor = NumberExtractor.GetInstance();
            this.InternalNumberParser = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new EnglishNumberParserConfiguration());
            this.ConnectorToken = string.Empty;
        }

        public override IParser InternalNumberParser { get; }

        public override IExtractor InternalNumberExtractor { get; }

        public override string ConnectorToken { get; }
    }
}
