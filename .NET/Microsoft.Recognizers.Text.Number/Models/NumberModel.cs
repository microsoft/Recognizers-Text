namespace Microsoft.Recognizers.Text.Number
{
    public class NumberModel : AbstractNumberModel
    {
        public NumberModel(IParser parser, IExtractor extractor)
            : base(parser, extractor)
        {
        }

        public NumberModel(IParser parser, IExtractor extractor, bool recode)
            : base(parser, extractor, recode)
        {
        }

        public override string ModelTypeName => Constants.MODEL_NUMBER;
    }
}