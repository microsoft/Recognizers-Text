using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using Microsoft.Recognizers.Definitions.English;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.English;

namespace Microsoft.Recognizers.Text.NumberWithUnit.English
{
    public class EnglishNumberWithUnitParserConfiguration : BaseNumberWithUnitParserConfiguration
    {
        public EnglishNumberWithUnitParserConfiguration(CultureInfo ci)
               : base(ci)
        {
            var numConfig = new BaseNumberOptionsConfiguration(Culture.English, NumberOptions.None);

            this.InternalNumberExtractor = NumberExtractor.GetInstance(numConfig);
            this.InternalNumberParser = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number,
                                                                              new EnglishNumberParserConfiguration(numConfig));
            this.ConnectorToken = string.Empty;
        }

        public override IParser InternalNumberParser { get; }

        public override IExtractor InternalNumberExtractor { get; }

        public override string ConnectorToken { get; }

        public override ImmutableDictionary<string, IEnumerable<double>> DimensionUnitMultiplesMap =>
            NumbersWithUnitDefinitions.DimensionUnitMultiplesMap.ToImmutableDictionary(x => x.Key, x => x.Value);
    }
}
