namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseOptionsConfiguration : IOptionsConfiguration
    {
        public BaseOptionsConfiguration(DateTimeOptions options = DateTimeOptions.None, bool dmyDateFormat = false)
        {
            Options = options;
            DmyDateFormat = dmyDateFormat;
        }

        public BaseOptionsConfiguration(IOptionsConfiguration config)
        {
            Options = config.Options;
            DmyDateFormat = config.DmyDateFormat;
        }

        public DateTimeOptions Options { get; }

        public bool DmyDateFormat { get; }
    }
}
