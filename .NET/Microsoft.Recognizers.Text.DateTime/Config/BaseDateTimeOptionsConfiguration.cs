namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseDateTimeOptionsConfiguration : IDateTimeOptionsConfiguration
    {
        public BaseDateTimeOptionsConfiguration(DateTimeOptions options = DateTimeOptions.None, bool dmyDateFormat = false)
        {
            Options = options;
            DmyDateFormat = dmyDateFormat;
        }

        public BaseDateTimeOptionsConfiguration(IDateTimeOptionsConfiguration config)
        {
            Options = config.Options;
            DmyDateFormat = config.DmyDateFormat;
        }

        public DateTimeOptions Options { get; }

        public bool DmyDateFormat { get; }
    }
}
