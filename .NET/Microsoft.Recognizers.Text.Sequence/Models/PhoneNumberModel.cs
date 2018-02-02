namespace Microsoft.Recognizers.Text.Sequence
{
    public class PhoneNumberModel : AbstractSequenceModel
    {
        public PhoneNumberModel(IParser parser, IExtractor extractor) : base(parser, extractor)
        {
        }

        public override string ModelTypeName => Constants.MODEL_PHONE_NUMBER;

    }
}