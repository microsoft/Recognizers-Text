namespace Microsoft.Recognizers.Text.Number.Models
{
    public class PercentModel : AbstractNumberModel
    {
        public PercentModel(IParser parser, IExtractor extractor) : base(parser, extractor)
        {
        }

        public override string ModelTypeName => "percentage";
    }
}