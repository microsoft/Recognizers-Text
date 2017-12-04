namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseOptionsConfiguration : IOptionsConfiguration
    {

        public BaseOptionsConfiguration(DateTimeOptions options)
        {
            Options = options;
        }

        public DateTimeOptions Options { get; }

    }
}
