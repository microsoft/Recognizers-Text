namespace Microsoft.Recognizers.Text.Sequence
{
    public class IpAddressModel : AbstractSequenceModel
    {
        public IpAddressModel(IParser parser, IExtractor extractor) : base(parser, extractor)
        {
        }

        public override string ModelTypeName => Constants.MODEL_IP;

    }
}