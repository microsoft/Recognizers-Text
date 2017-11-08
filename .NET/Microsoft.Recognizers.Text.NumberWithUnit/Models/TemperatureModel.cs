using System.Collections.Generic;

using Microsoft.Recognizers.Text.Number.Extractors;
using Microsoft.Recognizers.Text.Number.Parsers;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Models
{
    public class TemperatureModel : AbstractNumberWithUnitModel
    {
        public TemperatureModel(Dictionary<IExtractor, IParser> extractorParserDic)
            : base(extractorParserDic)
        {
        }

        public override string ModelTypeName => "temperature";
    }
}