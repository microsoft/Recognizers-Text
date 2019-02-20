using System.Collections.Generic;
using System.IO;

using YamlDotNet.Serialization;

namespace Microsoft.Recognizers.Definitions.Common
{
    public class SimpleRegex
    {
        [YamlMember(Alias = "def", ApplyNamingConventions = false)]
        public string Definition { get; set; }

        [YamlMember(Alias = "def_js", ApplyNamingConventions = false)]
        public string DefinitionJS { get; set; }
    }
}
