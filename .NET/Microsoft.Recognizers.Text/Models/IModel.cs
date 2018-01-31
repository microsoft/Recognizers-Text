using System.Collections.Generic;

namespace Microsoft.Recognizers.Text
{
    public interface IModel
    {
        string ModelTypeName { get; }

        List<object> Parse(string query);
    }
}