namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseOptionsConfiguration : IOptionsConfiguration
    {
        public BaseOptionsConfiguration(DateTimeOptions options = DateTimeOptions.None, bool enableDmy = false)
        {
            Options = options;
            EnableDmy = enableDmy;
        }

        public BaseOptionsConfiguration(IOptionsConfiguration config)
        {
            Options = config.Options;
            EnableDmy = config.EnableDmy;
        }

        public DateTimeOptions Options { get; }

        public bool EnableDmy { get; }
    }
}
