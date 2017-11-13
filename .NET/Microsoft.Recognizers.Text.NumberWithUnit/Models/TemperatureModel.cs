using System.Collections.Generic;

using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.NumberWithUnit
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