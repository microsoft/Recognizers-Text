using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Recognizers.Text.Choice
{
    public interface IChoiceParserConfiguration<T>
    {
        IDictionary<string, T> Resolutions { get; }
    }
}
