namespace Microsoft.Recognizers.Text.Sequence
{
    public class IPModel : AbstractSequenceModel
    {
        public IPModel(IParser parser, IExtractor extractor) : base(parser, extractor)
        {
        }

        public override string ModelTypeName => Constants.MODEL_IP;

    }
}