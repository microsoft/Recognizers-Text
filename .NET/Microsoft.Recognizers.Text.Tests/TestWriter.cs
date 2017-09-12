using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using System;
using System.Linq;

namespace Microsoft.Recognizers.Text
{
    public class TestModel
    {
        public string Language { get; set; }
        public string Recognizer { get; set; }
        public string Model { get; set; }
        public string TestType { get; set; }
        public string Input { get; set; }
        public int ResultsLength { get; set; }
        public object Results { get; set; }
    }

    public class SingleTestModel
    {
        public string TestType { get; set; }
        public string Input { get; set; }
        public int ResultsLength { get; set; }
        public object Results { get; set; }
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
                ResultsLength = Int32.Parse(values[5]),
                Results = JsonConvert.DeserializeObject(values[6])
            };
            Write(testModel);
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

        private static string getJson<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.None, 
                new JsonSerializerSettings {
                    ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                });
        }

        public static void Write(string lang, IModel model, string source, IEnumerable<ModelResult> result, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "")
        {
            var fileName = getProjectName(callerFilePath);
            var modelStr = getName(model);
            var resultStr = getJson(result);
            Write(new string[] { lang, fileName, callerMemberName, modelStr, source, "1", resultStr });
        }

        public static void Write(string lang, IModel model, string source, int count, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "")
        {
            var fileName = getProjectName(callerFilePath);
            var modelStr = getName(model);
            Write(new string[] { lang, fileName, callerMemberName, modelStr, source, count.ToString(), "" });
        }

        public static void Write(string lang, IModel model, string source, IEnumerable<ModelResult> result, int count, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "")
        {
            var fileName = getProjectName(callerFilePath);
            var modelStr = getName(model);
            var resultStr = getJson(result);
            Write(new string[] { lang, fileName, callerMemberName, modelStr, source, count.ToString(), resultStr });
        }

        public static void Write(string lang, IExtractor extractor, string source, IEnumerable<ExtractResult> result, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "")
        {
            var fileName = getProjectName(callerFilePath);
            var modelStr = getName(extractor);
            var resultStr = getJson(result);
            Write(new string[] { lang, fileName, callerMemberName, modelStr, source, "1", resultStr });
        }

        public static void Write(string lang, IExtractor extractor, string source, IEnumerable<ExtractResult> results, int count, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "")
        {
            var fileName = getProjectName(callerFilePath);
            var modelStr = getName(extractor);
            var resultStr = getJson(results);
            Write(new string[] { lang, fileName, callerMemberName, modelStr, source, count.ToString(), resultStr });
        }

        public static void Write(string lang, IExtractor extractor, string source, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "")
        {
            var fileName = getProjectName(callerFilePath);
            var modelStr = getName(extractor);
            Write(new string[] { lang, fileName, callerMemberName, modelStr, source, "0", ""});
        }

        public static void Write(string lang, IParser parser, string source, ParseResult result, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "")
        {
            var fileName = getProjectName(callerFilePath);
            var modelStr = getName(parser);
            var resultStr = getJson(new ParseResult[] { result });
            Write(new string[] { lang, fileName, callerMemberName, modelStr, source, "1", resultStr });
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
    }
}
