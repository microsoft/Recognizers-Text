namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IDateTimeOptionsConfiguration
    {
        DateTimeOptions Options { get; }

        bool DmyDateFormat { get; }
    }
}
