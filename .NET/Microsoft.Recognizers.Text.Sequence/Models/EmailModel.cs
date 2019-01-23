namespace Microsoft.Recognizers.Text.Sequence
{
    public class EmailModel : AbstractSequenceModel
    {
        public EmailModel(IParser parser, IExtractor extractor)
            : base(parser, extractor)
        {
        }

        public override string ModelTypeName => Constants.MODEL_EMAIL;
    }
}