using System.Collections.Generic;
using System.IO;

using YamlDotNet.Serialization;

namespace Microsoft.Recognizers.Definitions.Common
{
    public class NestedRegex : SimpleRegex
    {
        [YamlMember(Alias = "references", ApplyNamingConventions = false)]
        public IEnumerable<string> References { get; set; }

        public string SanitizedDefinition
        {
            get
            {
                var result = this.Definition.Replace("{", "{{").Replace("}", "}}");
                foreach (var token in this.References)
                {
                    result = result.Replace("{" + token + "}", token);
                }

                return result;
            }
        }
    }
}
