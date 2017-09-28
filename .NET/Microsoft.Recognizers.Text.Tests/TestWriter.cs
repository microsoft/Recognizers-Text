using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System;
using System.Linq;
using DateObject = System.DateTime;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Microsoft.Recognizers.Text.DateTime;

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
        public string Input { get; set; }

        public IDictionary<string, object> Context { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Platform? NotSupported { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Platform? NotSupportedByDesign { get; set; }

        public IEnumerable<object> Results { get; set; }

        public SingleTestModel()
        {
            this.NotSupported = null;
            this.NotSupportedByDesign = Platform.python;
        }
    }

    public class TestModelResult
    {
        public string Text { get; set; }
        public string TypeName { get; set; }
        public IDictionary<string, object> Resolution { get; set; }

        public TestModelResult(ModelResult result)
        {
            this.Text = result.Text;
            this.TypeName = result.TypeName;
            Resolution = new Dictionary<string, object>();
            object value;

            if (result.Resolution.TryGetValue("value", out value))
            {
                Resolution.Add("value", value);
            }

            if (result.Resolution.TryGetValue("unit", out value))
            {
                Resolution.Add("unit", value);
            }

            if (result.Resolution.TryGetValue("values", out value))
            {
                Resolution.Add("values", value);
            }
        }
    }

    public class TestExtractorResult
    {
        public string Text { get; set; }
        public string Type { get; set; }

        public TestExtractorResult(ExtractResult result)
        {
            this.Text = result.Text;
            this.Type = result.Type;
        }
    }

    public class TestParserResultValue
    {
        public string Timex { get; set; }
        public bool? IsLunar { get; set; }
        public IDictionary<string, string> FutureResolution { get; set; }
        public IDictionary<string, string> PastResolution { get; set; }

        public TestParserResultValue(DateTimeResolutionResult result)
        {
            this.Timex = result.Timex;
            this.FutureResolution = result.FutureResolution;
            this.PastResolution = result.PastResolution;
        }
    }

    public class TestParseResult
    {
        public string Text { get; set; }
        public string Type { get; set; }
        public object Value { get; set; }

        public TestParseResult(ParseResult result)
        {
            this.Text = result.Text;
            this.Type = result.Type;

            if (result.Value is DateTimeResolutionResult)
            {
                this.Value = new TestParserResultValue((DateTimeResolutionResult)result.Value);
            }
            else
            {
                this.Value = result.Value;
            }
        }
    }

    public class TestWriter
    {
        
        private static readonly IList<string> LogList = new List<string>();

        public static readonly TestWriter Instance = new TestWriter();

        private TestWriter()
        {
            Trace.Listeners.Clear();
            Trace.AutoFlush = true;
            LogList.Clear();
        }

        public static void Write(TestModel testModel)
        {
            if (LogList.Contains($"{testModel.Language}-{testModel.Input}-{GetJson(testModel.Results)}"))
            {
                return;
            }

            LogList.Add($"{testModel.Language}-{testModel.Input}-{GetJson(testModel.Results)}");

            if (Trace.Listeners[string.Join("-", testModel.Language, testModel.Recognizer, testModel.Model)] == null)
            {
                Trace.Listeners.Add(new TestTextWriterTraceListener(testModel.Language, testModel.Recognizer, testModel.Model));
            }

            Trace.WriteLine(GetJson(testModel));
        }

        public static string GetJson<T>(T obj, Formatting formatting = Formatting.None)
        {
            return JsonConvert.SerializeObject(obj, formatting,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    PreserveReferencesHandling = PreserveReferencesHandling.None,
                    NullValueHandling = NullValueHandling.Ignore,
                    Formatting = formatting
                });
        }

        public static void Write(string lang, string model, string source, IEnumerable<object> results, 
                                 [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "")
        {
            Write(lang, model, null, source, results, callerFilePath, callerMemberName);
        }
        
        public static void Write(string lang, IModel model, DateObject datetime, string source, IEnumerable<ModelResult> results, 
                                 [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "")
        {
            var modelStr = GetName(model);
            Write(lang, modelStr, datetime, source, results.Select(o => new TestModelResult(o)), callerFilePath, callerMemberName);
        }

        public static void Write(string lang, IModel model, string source, IEnumerable<ModelResult> results, 
                                 [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "")
        {
            var modelStr = GetName(model);
            Write(lang, modelStr, null, source, results.Select(o => new TestModelResult(o)), callerFilePath, callerMemberName);
        }

        public static void Write(string lang, IModel model, string source, [CallerFilePath] string callerFilePath = "", 
                                 [CallerMemberName] string callerMemberName = "")
        {
            Write(lang, model, source, Enumerable.Empty<ModelResult>(), callerFilePath, callerMemberName);
        }


        public static void Write(string lang, IParser parser, string source, ParseResult result, [CallerFilePath] string callerFilePath = "", 
                                 [CallerMemberName] string callerMemberName = "")
        {
            var modelStr = GetName(parser);
            Write(lang, modelStr, null, source, new TestParseResult[] { new TestParseResult(result) }, callerFilePath, callerMemberName);
        }

        public static void Write(string lang, IParser parser, DateObject datetime, string source, ParseResult result, 
                                 [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "")
        {
            var modelStr = GetName(parser);
            Write(lang, modelStr, datetime, source, new TestParseResult[] { new TestParseResult(result) }, callerFilePath, callerMemberName);
        }

        public static void Write(string lang, IParser parser, DateObject datetime, string source, IEnumerable<ParseResult> results, 
                                 [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "")
        {
            var modelStr = GetName(parser);
            Write(lang, modelStr, datetime, source, results.Select(o => new TestParseResult(o)), callerFilePath, callerMemberName);
        }

        public static void Write(string lang, IParser parser, string source, [CallerFilePath] string callerFilePath = "", 
                                 [CallerMemberName] string callerMemberName = "")
        {
            Write(lang, parser, source, null, callerFilePath, callerMemberName);
        }
        

        public static void Write(string lang, IExtractor extractor, string source, IEnumerable<ExtractResult> results, 
                                 [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "")
        {
            var modelStr = GetName(extractor);
            Write(lang, modelStr, null, source, results.Select(o => new TestExtractorResult(o)), callerFilePath, callerMemberName);
        }

        public static void Write(string lang, IExtractor extractor, string source, [CallerFilePath] string callerFilePath = "", 
                                 [CallerMemberName] string callerMemberName = "")
        {
            Write(lang, extractor, source, Enumerable.Empty<ExtractResult>(), callerFilePath, callerMemberName);
        }

        public static void Close(string lang, Type type, [CallerFilePath] string callerFilePath = "")
        {
            var model = GetName(type);
            var recognizer = GetProjectName(callerFilePath);

            var trace = Trace.Listeners[string.Join("-", lang, recognizer, model)];
            if (trace != null)
            {
                trace.Write("]");
                trace.Flush();
            }
        }

        public static void Close(string lang, string model, [CallerFilePath] string callerFilePath = "")
        {
            var recognizer = GetProjectName(callerFilePath);

            var trace = Trace.Listeners[string.Join("-", lang, recognizer, GetName(model))];
            if (trace != null)
            {
                trace.Write("]");
                trace.Flush();
            }
        }

        private static string GetProjectName(string path)
        {

            var prefix = "Microsoft.Recognizers.Text.";
            var start = path.IndexOf(prefix, StringComparison.Ordinal) + prefix.Length;
            var end = path.IndexOf('.', start);
            return path.Substring(start, end - start);
        }

        private static string GetName(object obj)
        {
            return GetName(obj.GetType());
        }

        private static string GetName(Type type)
        {
            return GetName(type.Name);
        }

        private static string GetName(string typeName)
        {
            return typeName.Replace("Chs", "").Replace("Base", "");
        }

        private static void Write(string lang, string model, DateObject? datetime, string source, IEnumerable<object> results, 
                                  [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "")
        {
            var recognizer = GetProjectName(callerFilePath);
            var context = new Dictionary<string, object>();
            if (datetime != null)
            {
                context.Add("ReferenceDateTime", datetime);
            }

            Write(new TestModel
            {
                Language = lang,
                Recognizer = recognizer,
                Model = GetName(model),
                TestType = callerMemberName,
                Context = context.Count > 0 ? context : null,
                Input = source,
                Results = results
            });
        }
    }
}
