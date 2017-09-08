using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using System.IO;

namespace Microsoft.Recognizers.Text
{
    public static class TestWriter
    {
        private const string separator = "\t";
        public static void Write(IEnumerable<string> values)
        {
            Trace.WriteLine(string.Join(separator, values));
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
            Write(new string[] { lang, fileName, callerMemberName, modelStr, source, count.ToString() });
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
            Write(new string[] { lang, fileName, callerMemberName, modelStr, source, "0"});
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
