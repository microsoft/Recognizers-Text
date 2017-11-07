using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.Models
{
    public interface IModel
    {
        string ModelTypeName { get; }

        List<ModelResult> Parse(string query);
    }
}