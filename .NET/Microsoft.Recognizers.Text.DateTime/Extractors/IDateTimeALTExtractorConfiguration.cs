namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IDateTimeALTExtractorConfiguration
    {
        IDateTimeExtractor DateExtractor { get; }
    }
}