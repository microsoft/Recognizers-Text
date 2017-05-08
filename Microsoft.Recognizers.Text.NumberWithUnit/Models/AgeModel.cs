namespace Microsoft.Recognizers.Text.NumberWithUnit
{
    public class AgeModel : AbstractNumberWithUnitModel
    {
        public AgeModel(IParser parser, IExtractor extractor) : base(parser, extractor)
        {
        }

        public override string ModelTypeName => "age";
    }
}