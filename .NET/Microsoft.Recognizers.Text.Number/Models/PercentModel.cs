namespace Microsoft.Recognizers.Text.Number
{
    public class PercentModel : AbstractNumberModel
    {
        public PercentModel(IParser parser, IExtractor extractor)
            : base(parser, extractor)
        {
        }

        public override string ModelTypeName => Constants.MODEL_PERCENTAGE;
    }
}