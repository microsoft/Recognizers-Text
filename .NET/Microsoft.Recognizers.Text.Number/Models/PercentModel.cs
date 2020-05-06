namespace Microsoft.Recognizers.Text.Number
{
    public class PercentModel : AbstractNumberModel
    {
        public PercentModel(IParser parser, IExtractor extractor)
            : base(parser, extractor)
        {
        }

        public PercentModel(IParser parser, IExtractor extractor, bool recode)
            : base(parser, extractor, recode)
        {
        }

        public override string ModelTypeName => Constants.MODEL_PERCENTAGE;
    }
}