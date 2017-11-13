using System.Collections.Generic;

using Microsoft.Recognizers.Text.Number;

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