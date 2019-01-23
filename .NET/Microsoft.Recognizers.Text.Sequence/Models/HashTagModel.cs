namespace Microsoft.Recognizers.Text.Sequence
{
    public class HashtagModel : AbstractSequenceModel
    {
        public HashtagModel(IParser parser, IExtractor extractor)
            : base(parser, extractor)
        {
        }

        public override string ModelTypeName => Constants.MODEL_HASHTAG;
    }
}