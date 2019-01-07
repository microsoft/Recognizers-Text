namespace Microsoft.Recognizers.Text.Number
{
    public class OrdinalModel : AbstractNumberModel
    {
        public OrdinalModel(IParser parser, IExtractor extractor)
            : base(parser, extractor)
        {
        }

        public override string ModelTypeName => Constants.MODEL_ORDINAL;
    }
}