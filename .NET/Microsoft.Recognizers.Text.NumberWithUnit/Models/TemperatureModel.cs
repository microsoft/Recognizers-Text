using System.Collections.Generic;

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