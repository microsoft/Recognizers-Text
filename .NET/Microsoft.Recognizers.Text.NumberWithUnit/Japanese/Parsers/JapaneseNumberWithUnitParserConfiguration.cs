using System.Globalization;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Japanese;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Japanese
{
    public class JapaneseNumberWithUnitParserConfiguration : BaseNumberWithUnitParserConfiguration
    {
        public JapaneseNumberWithUnitParserConfiguration(CultureInfo ci)
            : base(ci)
        {
            this.InternalNumberExtractor = new NumberExtractor();
            this.InternalNumberParser = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new JapaneseNumberParserConfiguration());
            this.ConnectorToken = string.Empty;
        }

        public override IParser InternalNumberParser { get; }

        public override IExtractor InternalNumberExtractor { get; }

        public override string ConnectorToken { get; }
    }
}
