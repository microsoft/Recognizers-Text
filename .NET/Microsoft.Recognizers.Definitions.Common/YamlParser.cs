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
}
