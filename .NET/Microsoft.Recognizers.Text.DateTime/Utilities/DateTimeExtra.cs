using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.Utilities
{
    public class DateTimeExtra<T>
    {
        public GroupCollection NamedEntity { get; set; }

        public T Type { get; set; }
    }
}
