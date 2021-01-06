using Microsoft.Recognizers.Definitions;

namespace Microsoft.Recognizers.Text.Number
{
    public class BaseNumberOptionsConfiguration : INumberOptionsConfiguration
    {

        public BaseNumberOptionsConfiguration(string culture, NumberOptions options = NumberOptions.None,
                                              NumberMode mode = NumberMode.Default, string placeholder = BaseNumbers.PlaceHolderDefault)
        {
            Culture = culture;
            Options = options;
            Mode = mode;
            Placeholder = placeholder;
        }

        public BaseNumberOptionsConfiguration(INumberOptionsConfiguration config)
        {
            Culture = config.Culture;
            Options = config.Options;
            Mode = config.Mode;
            Placeholder = config.Placeholder;
        }

        public NumberOptions Options { get; }

        public NumberMode Mode { get; }

        public string Placeholder { get; }

        public string Culture { get; }

    }
}
