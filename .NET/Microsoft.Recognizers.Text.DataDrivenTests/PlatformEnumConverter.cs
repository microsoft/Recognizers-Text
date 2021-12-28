using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Microsoft.Recognizers.Text.DataDrivenTests
{
    public class PlatformEnumConverter : StringEnumConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (string.IsNullOrEmpty(reader.Value.ToString()))
            {
                return 0;
            }

            return base.ReadJson(reader, objectType, existingValue, serializer);
        }
    }
}
