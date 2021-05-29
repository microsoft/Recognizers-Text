namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IDateTimeOptionsConfiguration : IConfiguration
    {
        DateTimeOptions Options { get; }

        bool DmyDateFormat { get; }

        string LanguageMarker { get; }
    }
}
