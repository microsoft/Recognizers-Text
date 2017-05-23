using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.NumberWithUnit
{
    public class AgeModel : AbstractNumberWithUnitModel
    {
        public AgeModel(Dictionary<IExtractor, IParser> extractorParserDic)
            : base(extractorParserDic)
        {
        }

        public override string ModelTypeName => "age";
    }
}