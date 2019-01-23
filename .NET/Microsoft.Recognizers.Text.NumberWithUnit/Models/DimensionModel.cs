using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.NumberWithUnit
{
    public class DimensionModel : AbstractNumberWithUnitModel
    {
        public DimensionModel(Dictionary<IExtractor, IParser> extractorParserDic)
            : base(extractorParserDic)
        {
        }

        public override string ModelTypeName => "dimension";
    }
}