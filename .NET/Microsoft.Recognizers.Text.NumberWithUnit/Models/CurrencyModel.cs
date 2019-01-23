using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.NumberWithUnit
{
    public class CurrencyModel : AbstractNumberWithUnitModel
    {
        public CurrencyModel(Dictionary<IExtractor, IParser> extractorParserDic)
            : base(extractorParserDic)
        {
        }

        public override string ModelTypeName => "currency";
    }
}