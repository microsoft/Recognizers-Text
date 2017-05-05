namespace Microsoft.Recognizers.Text.NumberWithUnit
{
    public class TemperatureModel : AbstractNumberWithUnitModel
    {
        public TemperatureModel(IParser parser, IExtractor extractor) : base(parser, extractor)
        {
        }

        public override string ModelTypeName => "temperature";
    }
}