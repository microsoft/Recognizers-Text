namespace Microsoft.Recognizers.Text.NumberWithUnit
{
    public class DimensionModel : AbstractNumberWithUnitModel
    {
        public DimensionModel(IParser parser, IExtractor extractor) : base(parser, extractor)
        {
        }

        public override string ModelTypeName => "dimension";
    }
}