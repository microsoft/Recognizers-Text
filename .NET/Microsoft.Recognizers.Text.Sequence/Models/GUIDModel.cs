namespace Microsoft.Recognizers.Text.Sequence
{
    public class GUIDModel : AbstractSequenceModel
    {
        public GUIDModel(IParser parser, IExtractor extractor) : base(parser, extractor)
        {
        }

        public override string ModelTypeName => Constants.MODEL_GUID;
    }
}