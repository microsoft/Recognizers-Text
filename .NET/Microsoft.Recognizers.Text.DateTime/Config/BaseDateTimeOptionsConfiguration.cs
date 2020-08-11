namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseDateTimeOptionsConfiguration : IDateTimeOptionsConfiguration
    {
        public BaseDateTimeOptionsConfiguration(string culture, DateTimeOptions options = DateTimeOptions.None, bool dmyDateFormat = false)
        {
            Culture = culture;
            Options = options;
            DmyDateFormat = dmyDateFormat;
        }

        public BaseDateTimeOptionsConfiguration(IDateTimeOptionsConfiguration config)
        {
            Culture = config.Culture;
            Options = config.Options;
            DmyDateFormat = config.DmyDateFormat;
        }

        public DateTimeOptions Options { get; }

        public bool DmyDateFormat { get; }

        public string LanguageMarker { get; set; }

        public string Culture { get; }

    }
}
