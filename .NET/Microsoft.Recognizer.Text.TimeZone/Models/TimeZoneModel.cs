namespace Microsoft.Recognizers.Text.TimeZone
{
    public class TimeZoneModel : AbstractTimeZoneModel
    {
        public TimeZoneModel(IParser parser, IExtractor extractor) : base(parser, extractor)
        {
        }

        public override string ModelTypeName => "timezone";
    }
}