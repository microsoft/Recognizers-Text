using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Microsoft.Recognizers.Text.DataDrivenTests
{
    [Flags]
    public enum Platform
    {
        /// <summary>
        /// dotNet flag
        /// </summary>
        DotNet = 1,

        /// <summary>
        /// JavaScript flag
        /// </summary>
        Javascript = 2,

        /// <summary>
        /// Python flag
        /// </summary>
        Python = 4,

        /// <summary>
        /// Java flag
        /// </summary>
        Java = 8,
    }

    public class TestModel
    {
        public TestModel()
        {
            Context = new Dictionary<string, object>();
            Results = Enumerable.Empty<object>();
            Debug = false;
        }

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

        public bool IsSupportedDotNet()
        {
            return (NotSupported & Platform.DotNet) == 0 && (NotSupportedByDesign & Platform.DotNet) == 0;
        }
    }
}
