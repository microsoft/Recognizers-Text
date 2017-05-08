namespace Microsoft.Recognizers.Text.Number.Models
{
    public class OrdinalModel : AbstractNumberModel
    {
        public OrdinalModel(IParser parser, IExtractor extractor) : base(parser, extractor)
        {
        }

        public override string ModelTypeName => "ordinal";
    }
}