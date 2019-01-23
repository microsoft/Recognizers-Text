using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.Utilities
{
    public class DateTimeExtra<T>
    {
        public GroupCollection NamedEntity { get; set; }

        public T Type { get; set; }
    }
}
