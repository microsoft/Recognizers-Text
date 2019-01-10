namespace Microsoft.Recognizers.Text.Sequence
{
    public class MentionModel : AbstractSequenceModel
    {
        public MentionModel(IParser parser, IExtractor extractor)
            : base(parser, extractor)
        {
        }

        public override string ModelTypeName => Constants.MODEL_MENTION;
    }
}