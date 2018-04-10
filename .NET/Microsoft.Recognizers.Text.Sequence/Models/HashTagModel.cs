namespace Microsoft.Recognizers.Text.Sequence
{
    public class HashTagModel : AbstractSequenceModel
    {
        public HashTagModel(IParser parser, IExtractor extractor) : base(parser, extractor)
        {
        }

        public override string ModelTypeName => Constants.MODEL_HASHTAG;
    }
}