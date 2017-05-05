namespace Microsoft.Recognizers.Text.NumberWithUnit
{
    public class CurrencyModel : AbstractNumberWithUnitModel
    {
        public CurrencyModel(IParser parser, IExtractor extractor) : base(parser, extractor)
        {
        }

        public override string ModelTypeName => "currency";
    }
}