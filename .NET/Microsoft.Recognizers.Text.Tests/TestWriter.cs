using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using System.IO;

namespace Microsoft.Recognizers.Text
{
    public static class TestWriter
    {
        private const string separator = "|";
        public static void Write(IEnumerable<string> values)
        {
            Trace.WriteLine(string.Join(separator, values));
        }

        public static string getFileName(string path)
        {
            return Path.GetFileNameWithoutExtension(path).Replace("Chs", "");
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
            var modelStr = model.GetType().ToString();
            var resultStr = getJson(result);
            Write(new string[] { lang, fileName, callerMemberName, modelStr, source, resultStr });
        }

        public static void Write(string lang, IModel model, string source, int count, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "")
        {
            var fileName = getFileName(callerFilePath);
            var modelStr = model.GetType().ToString();
            Write(new string[] { lang, fileName, callerMemberName, modelStr, source, count.ToString() });
        }

        public static void Write(string lang, IModel model, string source, ModelResult result, int count, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "")
        {
            var fileName = getFileName(callerFilePath);
            var modelStr = model.GetType().ToString();
            var resultStr = getJson(result);
            Write(new string[] { lang, fileName, callerMemberName, modelStr, source, resultStr, count.ToString() });
        }

        public static void Write(string lang, IModel model, string source, IEnumerable<ModelResult> results, int count, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "")
        {
            var fileName = getFileName(callerFilePath);
            var modelStr = model.GetType().ToString();
            var resultStr = getJson(results);
            Write(new string[] { lang, fileName, callerMemberName, modelStr, source, resultStr, count.ToString() });
        }

        public static void Write(string lang, IExtractor extractor, string source, ExtractResult result, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "")
        {
            var fileName = getFileName(callerFilePath);
            var modelStr = extractor.GetType().ToString();
            var resultStr = getJson(result);
            Write(new string[] { lang, fileName, callerMemberName, modelStr, source, resultStr });
        }

        public static void Write(string lang, IExtractor extractor, string source, IEnumerable<ExtractResult> results, int count, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "")
        {
            var fileName = getFileName(callerFilePath);
            var modelStr = extractor.GetType().ToString();
            var resultStr = getJson(results);
            Write(new string[] { lang, fileName, callerMemberName, modelStr, source, resultStr, count.ToString() });
        }

        public static void Write(string lang, IExtractor extractor, string source, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "")
        {
            var fileName = getFileName(callerFilePath);
            var modelStr = extractor.GetType().ToString();
            Write(new string[] { lang, fileName, callerMemberName, modelStr, source, "0"});
        }

        public static void Write(string lang, IParser parser, string source, ParseResult result, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "")
        {
            var fileName = getFileName(callerFilePath);
            var modelStr = parser.GetType().ToString();
            var resultStr = getJson(result);
            Write(new string[] { lang, fileName, callerMemberName, modelStr, source, resultStr });
        }
    }
}
