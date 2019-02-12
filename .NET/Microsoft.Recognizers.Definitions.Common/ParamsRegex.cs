using System.Collections.Generic;
using System.IO;

using YamlDotNet.Serialization;

namespace Microsoft.Recognizers.Definitions.Common
{
    public class ParamsRegex : SimpleRegex
    {
        [YamlMember(Alias = "params", ApplyNamingConventions = false)]
        public IEnumerable<string> Params { get; set; }

        public string SanitizedDefinition
        {
            get
            {
                var result = this.Definition.Replace("{", "{{").Replace("}", "}}");
                foreach (var token in this.Params)
                {
                    result = result.Replace("{" + token + "}", token);
                }

                return result;
            }
        }
    }
}
