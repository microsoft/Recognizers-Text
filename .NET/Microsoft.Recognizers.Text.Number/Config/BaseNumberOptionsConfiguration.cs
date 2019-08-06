namespace Microsoft.Recognizers.Text.Number
{
    public class BaseNumberOptionsConfiguration : INumberOptionsConfiguration
    {
        public BaseNumberOptionsConfiguration(string culture, NumberOptions options = NumberOptions.None, NumberMode mode = NumberMode.Default)
        {
            Culture = culture;
            Options = options;
            Mode = mode;
        }

        public BaseNumberOptionsConfiguration(INumberOptionsConfiguration config)
        {
            Culture = config.Culture;
            Options = config.Options;
            Mode = config.Mode;
        }

        public NumberOptions Options { get; }

        public NumberMode Mode { get; }

        public string Culture { get; }

    }
}
