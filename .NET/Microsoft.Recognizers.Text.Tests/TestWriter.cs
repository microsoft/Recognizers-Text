using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using System;
using System.Linq;
using DateObject = System.DateTime;
using Newtonsoft.Json.Converters;

namespace Microsoft.Recognizers.Text
{
    public class TestModel
    {
        public string Language { get; set; }
        public string Recognizer { get; set; }
        public string Model { get; set; }
        public string TestType { get; set; }
        public string Input { get; set; }
        public IDictionary<string, object> Context { get; set; }
        public IEnumerable<object> Results { get; set; }
    }

    [Flags]
    public enum Platform
    {
        dotNet,
        javascript,
        python
    }

    public class SingleTestModel
    {
        public string TestType { get; set; }
        public string Input { get; set; }
        public IDictionary<string, object> Context { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Platform NotSupported { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Platform NotSupportedByDesign { get; set; }
        public IEnumerable<object> Results { get; set; }

        public SingleTestModel()
        {
            this.NotSupported = Platform.javascript | Platform.python;
            this.NotSupportedByDesign = Platform.python;
        }
    }
    
    public class TestWriter
    {
        private const string separator = "\t";

        public static readonly TestWriter Instance = new TestWriter();

        private TestWriter()
        {
            Trace.Listeners.Clear();
            Trace.AutoFlush = true;
        }

        public static void Write(TestModel testModel)
        {
            if (Trace.Listeners[string.Join("-", testModel.Language, testModel.Recognizer, testModel.Model)] == null)
            {
                Trace.Listeners.Add(new TestTextWriterTraceListener(testModel.Language, testModel.Recognizer, testModel.Model));
            }
            Trace.WriteLine(GetJson(testModel));
        }

        private static string getProjectName(string path)
        {
            var start = path.IndexOf("Microsoft.Recognizers.Text.") + 27;
            var end = path.IndexOf('.', start);
            return path.Substring(start, end - start);
        }

        private static string getName(object obj)
        {
            return getName(obj.GetType());
        }

        private static string getName(Type type)
        {
            return type.Name.Replace("Chs", "");
        }

        public static string GetJson<T>(T obj, Formatting formatting = Formatting.None)
        {
            return JsonConvert.SerializeObject(obj, formatting, 
                new JsonSerializerSettings {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    PreserveReferencesHandling = PreserveReferencesHandling.None,
                    NullValueHandling = NullValueHandling.Ignore,
                    Formatting = formatting
                });
        }

        private static void Write(string lang, string model, DateObject? datetime, string source, IEnumerable<object> results, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "")
        {
            var recognizer = getProjectName(callerFilePath);
            var context = new Dictionary<string, object>();
            if (datetime != null)
            {
                context.Add("ReferenceDateTime", datetime);
            }
            Write(new TestModel
            {
                Language = lang,
                Recognizer = recognizer,
                Model = model,
                TestType = callerMemberName,
                Context = context.Count > 0 ? context : null,
                Input = source,
                Results = results
            });
        }

        public static void Write(string lang, IModel model, DateObject datetime, string source, IEnumerable<object> results, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "")
        {
            var modelStr = getName(model);
            Write(lang, modelStr, datetime, source, results, callerFilePath, callerMemberName);
        }

        public static void Write(string lang, string model, string source, IEnumerable<object> results, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "")
        {
            Write(lang, model, null, source, results, callerFilePath, callerMemberName);
        }

        public static void Write(string lang, IModel model, string source, IEnumerable<ModelResult> results, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "")
        {
            var modelStr = getName(model);
            Write(lang, modelStr, null, source, results, callerFilePath, callerMemberName);
        }

        public static void Write(string lang, IModel model, string source, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "")
        {
            Write(lang, model, source, Enumerable.Empty<ModelResult>(), callerFilePath, callerMemberName);
        }

        public static void Write(string lang, IParser parser, string source, ParseResult result, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "")
        {
            var modelStr = getName(parser);
            Write(lang, modelStr, null, source, new ParseResult[] { result }, callerFilePath, callerMemberName);
        }

        public static void Write(string lang, IParser parser, DateObject datetime, string source, ParseResult result, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "")
        {
            var modelStr = getName(parser);
            Write(lang, modelStr, datetime, source, new ParseResult[] { result }, callerFilePath, callerMemberName);
        }

        public static void Write(string lang, IParser parser, string source, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "")
        {
            Write(lang, parser, source, null, callerFilePath, callerMemberName);
        }

        public static void Write(string lang, IExtractor extractor, string source, IEnumerable<ExtractResult> results, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "")
        {
            var modelStr = getName(extractor);
            Write(lang, modelStr, null, source, results, callerFilePath, callerMemberName);
        }

        public static void Write(string lang, IExtractor extractor, string source, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "")
        {
            Write(lang, extractor, source, Enumerable.Empty<ExtractResult>(), callerFilePath, callerMemberName);
        }

        public static void Close(string lang, Type type, [CallerFilePath] string callerFilePath = "")
        {
            var model = getName(type);
            var recognizer = getProjectName(callerFilePath);

            var trace = Trace.Listeners[string.Join("-", lang, recognizer, model)];
            if (trace != null)
            {
                trace.Write("]");
                trace.Flush();
            }
        }

        public static void Close(string lang, string model, [CallerFilePath] string callerFilePath = "")
        {
            var recognizer = getProjectName(callerFilePath);

            var trace = Trace.Listeners[string.Join("-", lang, recognizer, model)];
            if (trace != null)
            {
                trace.Write("]");
                trace.Flush();
            }
        }
    }
}
