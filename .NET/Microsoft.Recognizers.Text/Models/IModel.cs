using System.Collections.Generic;

namespace Microsoft.Recognizers.Text
{
    public interface IModel
    {
        string ModelTypeName { get; }

        string Culture { get; }

        string RequestedCulture { get; }

        List<ModelResult> Parse(string query);

        void SetCultureInfo(string culture, string requestedCulture = null);
    }
}