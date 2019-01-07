namespace Microsoft.Recognizers.Text.Number
{
    public class NumberModel : AbstractNumberModel
    {
        public NumberModel(IParser parser, IExtractor extractor)
            : base(parser, extractor)
        {
        }

        public override string ModelTypeName => Constants.MODEL_NUMBER;
    }
}