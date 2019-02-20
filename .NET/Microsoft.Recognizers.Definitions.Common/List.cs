using System.Collections.Generic;
using System.IO;

using YamlDotNet.Serialization;

namespace Microsoft.Recognizers.Definitions.Common
{
    public class List
    {
        [YamlMember(Alias = "types", ApplyNamingConventions = false)]
        public IList<string> Types { get; set; }

        [YamlMember(Alias = "entries", ApplyNamingConventions = false)]
        public IEnumerable<object> Entries { get; set; }
    }
}
