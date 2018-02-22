using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Recognizers.Text.Options
{
    public interface IOptionParserConfiguration<T>
    {
        IDictionary<string, T> Resolutions { get; }
    }
}
