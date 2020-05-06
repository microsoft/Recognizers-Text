namespace Microsoft.Recognizers.Text.Number
{
    public class OrdinalModel : AbstractNumberModel
    {
        public OrdinalModel(IParser parser, IExtractor extractor)
            : base(parser, extractor)
        {
        }

        public OrdinalModel(IParser parser, IExtractor extractor, bool recode)
            : base(parser, extractor, recode)
        {
        }

        public override string ModelTypeName => Constants.MODEL_ORDINAL;
    }
}