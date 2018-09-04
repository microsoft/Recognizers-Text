namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IOptionsConfiguration
    {
        DateTimeOptions Options { get; }

        bool DmyDateFormat { get; }
    }
}
