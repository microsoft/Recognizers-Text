using System.Collections.Generic;
using System.Globalization;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Korean;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Korean
{
    public class KoreanNumberWithUnitParserConfiguration : BaseNumberWithUnitParserConfiguration
    {
        public KoreanNumberWithUnitParserConfiguration(CultureInfo ci)
            : base(ci)
        {

            var numConfig = new BaseNumberOptionsConfiguration(ci.Name, NumberOptions.None);

            this.InternalNumberExtractor = new NumberExtractor(numConfig);
            this.InternalNumberParser = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number,
                                                                              new KoreanNumberParserConfiguration(numConfig));
            this.ConnectorToken = string.Empty;
        }

        public override IParser InternalNumberParser { get; }

        public override IExtractor InternalNumberExtractor { get; }

        public override string ConnectorToken { get; }

        public override IDictionary<string, string> TypeList { get; set; } = null;
    }
}
