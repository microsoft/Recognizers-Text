using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.Choice
{
    public interface IChoiceParserConfiguration<T>
    {
        IDictionary<string, T> Resolutions { get; }
    }
}
