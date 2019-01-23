using System.Collections.Generic;
using System.IO;

using YamlDotNet.Serialization;

namespace Microsoft.Recognizers.Definitions.Common
{
    public class YamlParser
    {
        private readonly Deserializer yamlDeserializer;

        public YamlParser()
        {
            yamlDeserializer = new DeserializerBuilder()
                .WithTagMapping("!simpleRegex", typeof(SimpleRegex))
                .WithTagMapping("!nestedRegex", typeof(NestedRegex))
                .WithTagMapping("!paramsRegex", typeof(ParamsRegex))
                .WithTagMapping("!dictionary", typeof(Dictionary))
                .WithTagMapping("!list", typeof(List))
                .WithTagMapping("!char", typeof(char))
                .Build();
        }

        public IDictionary<string, object> Deserialize(TextReader obj)
        {
            return yamlDeserializer.Deserialize<IDictionary<string, object>>(obj);
        }

        public IDictionary<string, object> Deserialize(string obj)
        {
            return yamlDeserializer.Deserialize<IDictionary<string, object>>(obj);
        }
    }

    public class SimpleRegex
    {
        [YamlMember(Alias = "def", ApplyNamingConventions = false)]
        public string Definition { get; set; }

        [YamlMember(Alias = "def_js", ApplyNamingConventions = false)]
        public string DefinitionJS { get; set; }
    }

    public class NestedRegex : SimpleRegex
    {
        [YamlMember(Alias = "references", ApplyNamingConventions = false)]
        public IEnumerable<string> References { get; set; }

        public string SanitizedDefinition {
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

    public class Dictionary
    {
        [YamlMember(Alias = "types", ApplyNamingConventions = false)]
        public IList<string> Types { get; set; }

        [YamlMember(Alias = "entries", ApplyNamingConventions = false)]
        public IDictionary<object, object> Entries { get; set; }
    }

    public class List
    {
        [YamlMember(Alias = "types", ApplyNamingConventions = false)]
        public IList<string> Types { get; set; }

        [YamlMember(Alias = "entries", ApplyNamingConventions = false)]
        public IEnumerable<object> Entries { get; set; }
    }
}
