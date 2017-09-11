using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using System.IO;
using System;

namespace Microsoft.Recognizers.Text
{
    public class TestModel
    {
        public string Language { get; set; }
        public string Recognizer { get; set; }
        public string Model { get; set; }
        public string TestType { get; set; }
        public string Input { get; set; }
        public object Results { get; set; }
    }

    public class SingleTestModel
    {
        public string TestType { get; set; }
        public string Input { get; set; }
        public object Results { get; set; }
    }
    
    public class TestFilter : TraceFilter
    {
        private readonly string language;
        private readonly string recognizer;
        private readonly string model;

        public TestFilter(string language, string recognizer, string model)
        {
            this.language = language;
            this.recognizer = recognizer;
            this.model = model;
        }

        public override bool ShouldTrace(TraceEventCache cache, string source, TraceEventType eventType, int id, string formatOrMessage, object[] args, object data1, object[] data)
        {
            var testCase = JsonConvert.DeserializeObject<TestModel>(formatOrMessage);
            return testCase.Language.Equals(language) && testCase.Recognizer.Equals(recognizer) && testCase.Model.Equals(model);
        }
    }

    public class TestTextWriterTraceListener : TextWriterTraceListener
    {
        private bool isAppend = false;
        public TestTextWriterTraceListener(string language, string recognizer, string model) : base(string.Join("-", "results", language, recognizer, model) + ".json", string.Join("-", language, recognizer, model))
        {
            base.Writer = new StreamWriter(string.Join("-", "results", language, recognizer, model) + ".json", false);
            base.Write("[");
            base.Filter = new TestFilter(language, recognizer, model);
        }

        public override void Close()
        {
            base.WriteLine("]");
            base.Flush();
            base.Close();
        }
        
        public override void WriteLine(string message)
        {
            if (Filter.ShouldTrace(null, "", TraceEventType.Verbose, 0, message, null, null, null))
            {
                var model = JsonConvert.DeserializeObject<SingleTestModel>(message);
                if (isAppend)
                {
                    base.WriteLine("," + JsonConvert.SerializeObject(model));
                } else
                {
                    isAppend = true;
                    base.WriteLine(JsonConvert.SerializeObject(model));
                }
            }
        }
    }

    public class TestWriter
    {
        private const string separator = "\t";

        public static readonly TestWriter Instance = new TestWriter();

        private TestWriter()
        {
            Trace.Listeners.Clear();
        }

        public static void Write(TestModel testModel)
        {
            if (Trace.Listeners[string.Join("-", testModel.Language, testModel.Recognizer, testModel.Model)] == null)
            {
                Trace.Listeners.Add(new TestTextWriterTraceListener(testModel.Language, testModel.Recognizer, testModel.Model));
            }
            Trace.WriteLine(getJson(testModel));
        }
        
        public static void Write(string[] values)
        {
            var testModel = new TestModel
            {
                Language = values[0],
                Recognizer = values[1],
                TestType = values[2],
                Model = values[3],
                Input = values[4],
                Results = JsonConvert.DeserializeObject(values[6])
            };
            Write(testModel);
        }

        private static string getFileName(string path)
        {
            return Path.GetFileNameWithoutExtension(path).Replace("Chs", "").Replace("Test", "");
        }
        
        private static string getName(object obj)
        {
            return obj.GetType().Name.Replace("Chs", "");
        }

        private static string getJson<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.None, 
                new JsonSerializerSettings {
                    ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                });
        }

        public static void Write(string lang, IModel model, string source, ModelResult result, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "")
        {
            var fileName = getFileName(callerFilePath);
            var modelStr = getName(model);
            var resultStr = getJson(result);
            Write(new string[] { lang, fileName, callerMemberName, modelStr, source, "1", resultStr });
        }

        public static void Write(string lang, IModel model, string source, int count, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "")
        {
            var fileName = getFileName(callerFilePath);
            var modelStr = getName(model);
            Write(new string[] { lang, fileName, callerMemberName, modelStr, source, count.ToString(), "" });
        }

        public static void Write(string lang, IModel model, string source, ModelResult result, int count, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "")
        {
            var fileName = getFileName(callerFilePath);
            var modelStr = getName(model);
            var resultStr = getJson(result);
            Write(new string[] { lang, fileName, callerMemberName, modelStr, source, count.ToString(), resultStr });
        }

        public static void Write(string lang, IModel model, string source, IEnumerable<ModelResult> results, int count, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "")
        {
            var fileName = getFileName(callerFilePath);
            var modelStr = getName(model);
            var resultStr = getJson(results);
            Write(new string[] { lang, fileName, callerMemberName, modelStr, source, count.ToString(), resultStr });
        }

        public static void Write(string lang, IExtractor extractor, string source, ExtractResult result, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "")
        {
            var fileName = getFileName(callerFilePath);
            var modelStr = getName(extractor);
            var resultStr = getJson(result);
            Write(new string[] { lang, fileName, callerMemberName, modelStr, source, "1", resultStr });
        }

        public static void Write(string lang, IExtractor extractor, string source, IEnumerable<ExtractResult> results, int count, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "")
        {
            var fileName = getFileName(callerFilePath);
            var modelStr = getName(extractor);
            var resultStr = getJson(results);
            Write(new string[] { lang, fileName, callerMemberName, modelStr, source, count.ToString(), resultStr });
        }

        public static void Write(string lang, IExtractor extractor, string source, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "")
        {
            var fileName = getFileName(callerFilePath);
            var modelStr = getName(extractor);
            Write(new string[] { lang, fileName, callerMemberName, modelStr, source, "0", ""});
        }

        public static void Write(string lang, IParser parser, string source, ParseResult result, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "")
        {
            var fileName = getFileName(callerFilePath);
            var modelStr = getName(parser);
            var resultStr = getJson(result);
            Write(new string[] { lang, fileName, callerMemberName, modelStr, source, "1", resultStr });
        }
    }
}
