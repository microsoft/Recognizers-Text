using Microsoft.Recognizers.Text.Config;

namespace Microsoft.Recognizers.Text.Number
{
    public interface INumberOptionsConfiguration : IConfiguration
    {
        NumberOptions Options { get; }

        NumberMode Mode { get; }

        string Placeholder { get; }
    }
}
