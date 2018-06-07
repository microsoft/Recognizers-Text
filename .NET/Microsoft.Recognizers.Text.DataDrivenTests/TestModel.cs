using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Recognizers.Text.DataDrivenTests
{
    public class TestModel
    {
        public string TestType { get; set; }
        public string Input { get; set; }
        public IDictionary<string, object> Context { get; set; }
        public bool Debug { get; set; }
        public Platform NotSupported { get; set; }
        public Platform NotSupportedByDesign { get; set; }
        public IEnumerable<object> Results { get; set; }

        public IEnumerable<T> CastResults<T>()
        {
            var results = JsonConvert.SerializeObject(this.Results);
            return JsonConvert.DeserializeObject<IEnumerable<T>>(results);
        }

        public TestModel()
        {
            Context = new Dictionary<string, object>();
            Results = Enumerable.Empty<object>();
            Debug = false;
        }
    }

    [Flags]
    public enum Platform
    {
        dotNet = 1,
        javascript = 2,
        python = 4,
        java = 8
    }
}
