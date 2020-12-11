using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.Choice;
using Microsoft.Recognizers.Text.DateTime;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.NumberWithUnit;
using Microsoft.Recognizers.Text.Sequence;
using Newtonsoft.Json;
using Microsoft.Recognizers.Text.DataDrivenTests;

namespace Microsoft.Recognizers.Text.Validation
{
    class ValidationTool
    {

        static void Main(string[] args)
        {
            // Encoding for 'exotic' characters e.g. '€'
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            string[] specsPaths = LoadSpecsPaths(args);

            foreach (string path in specsPaths)
            {
                RunPipeline(path);
            }

            Console.WriteLine();
        }

        private static string[] LoadSpecsPaths(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please enter a string argument.");
                Console.WriteLine("Usage: SpecsPath <string>");
                return Array.Empty<string>();
            }

            string specsPath = args[0];
            string basicDir;
            string basicPath;

            if (Directory.Exists(specsPath))
            {
                basicDir = specsPath;
                basicPath = "*";
            }
            else
            {
                var pathSplit = specsPath.Split(Path.DirectorySeparatorChar);
                basicDir = pathSplit.Length > 1 ? string.Join(Path.DirectorySeparatorChar, pathSplit.Take(pathSplit.Length - 1)) : $".{Path.DirectorySeparatorChar}";
                basicPath = pathSplit[pathSplit.Length - 1];
            }

            if (!Directory.Exists(basicDir))
            {
                Console.WriteLine($"Specs Path Dir {basicDir} Not Exist.");
                return Array.Empty<string>();
            }

            string[] paths = Directory.GetFiles(basicDir, basicPath);
            Console.WriteLine($"Loading {paths.Length} Spec Files...");
            Console.WriteLine();
            return paths;
        }

        private static void RunPipeline(string filePath)
        {
            Console.WriteLine("-------------------------");
            Console.WriteLine($"Begin to check file {filePath}.");
            BasicStageResult data = LoadJsonFile(filePath);
            if (!data.IsSucceed)
            {
                return;
            }

            bool stage2 = CheckBasicContent(data.Specs);
            if (!stage2)
            {
                return;
            }

            bool stage3 = CheckModelResult(data.Specs);
        }

        private static BasicStageResult LoadJsonFile(string filePath)
        {
            List<TestModel> specs = new List<TestModel> { };
            string logStr = string.Empty;
            if (!File.Exists(filePath))
            {
                logStr = $"File {filePath} Dose Not Exist!";
            }
            else
            {
                var rawData = File.ReadAllText(filePath);

                // Try to load Specs JSON file, and catch exception.
                try
                {
                    specs = (List<TestModel>)JsonConvert.DeserializeObject<IList<TestModel>>(rawData);
                    if (!specs.Any())
                    {
                        logStr = $"JSON File is Empty.";
                    }
                }
                catch (JsonReaderException jre)
                {
                    logStr = $"JSON Reader Exception! \nException Type: {jre.GetType()}, \nException Message: {jre.Message}, \nPosition: Line{jre.LineNumber}/{jre.LinePosition}.";
                }
                catch (JsonSerializationException jse)
                {
                    logStr = $"JSON SerialzationException! \nException Type: {jse.GetType()}, \nException Message: {jse.Message}, \nPosition: Line{jse.LineNumber}/{jse.LinePosition}.";
                }
                catch (Exception e)
                {
                    logStr = $"Other File Error! \nException Type: {e.GetType()}, \nException Message: {e.Message}.";
                }
            }

            return new BasicStageResult(ConsoleStageResult("JSON Valid", logStr), specs);
        }

        private static bool CheckBasicContent(List<TestModel> data)
        {
            List<string> res = new List<string> { };
            foreach (TestModel spec in data)
            {
                res.Add(CheckOneBasicContentSpec(spec));
            }

            return ConsoleStageResult("Content Valid", string.Join("|", res), true);
        }

        private static bool CheckModelResult(List<TestModel> data)
        {
            string logStr = string.Empty;
            return ConsoleStageResult("Model Result", logStr);
        }

        private static string CheckOneBasicContentSpec(TestModel spec)
        {
            string logStr = string.Empty;
            if (spec.Input.Equals(string.Empty))
            {
                logStr = "No Input";
            }
            else
            {
                foreach (object result in spec.Results)
                {
                    string resultStr = CheckOneBasicContentSpecResult(spec, result);
                    if (!resultStr.Equals(string.Empty))
                    {
                        logStr = resultStr;
                        break;
                    }
                }
            }

            return logStr;
        }

        private static string CheckOneBasicContentSpecResult(TestModel spec, object result)
        {
            var serializeResult = JsonConvert.SerializeObject(result);
            var dynamicResult = JsonConvert.DeserializeObject<dynamic>(serializeResult);
            if (dynamicResult["Text"] == null || dynamicResult["Start"] == null || (dynamicResult["End"] == null && dynamicResult["Length"] == null))
            {
                return "Result Parameter Loss";
            }

            int start = dynamicResult["Start"];
            int end = dynamicResult["End"] != null ? dynamicResult["End"] : dynamicResult["Length"] + start - 1;
            var startEndStr = spec.Input.Substring(start, end - start + 1);
            if (dynamicResult["Text"] != startEndStr)
            {
                return "Index Error";
            }

            return string.Empty;
        }

        private static bool ConsoleStageResult(string stage, string logStr, bool isList = false)
        {
            // Log stage result, and return success/error bool flag.
            bool res;
            string needLogStr = string.Empty;
            if (isList)
            {
                // Counter the number of 
                string[] logStrs = logStr.Split("|");
                Dictionary<string, int> logStrCounter = new Dictionary<string, int>();
                foreach (string f in logStrs)
                {
                    int oldValue;
                    logStrCounter.TryGetValue(f, out oldValue);
                    logStrCounter[f] = ++oldValue;
                }

                int emptyValue;
                logStrCounter.TryGetValue(string.Empty, out emptyValue);
                res = emptyValue == logStrs.Length;
                var logStrLogs = logStrCounter.Keys.Where(f => !f.Equals(string.Empty));
                needLogStr = res ? $"{logStrs.Length} specs Success" : "Error. ";
                foreach (string logStrLog in logStrLogs)
                {
                    needLogStr += $"{logStrCounter[logStrLog]} specs {logStrLog}, ";
                }
            }
            else
            {
                res = logStr.Equals(string.Empty);
                needLogStr = res ? "Success" : $"Error. {logStr}";
            }

            Console.WriteLine($"{stage} Check {needLogStr}.");
            return res;
        }

        internal class BasicStageResult
        {
            public bool IsSucceed { get; set; }

            public List<TestModel> Specs { get; set; }

            public BasicStageResult(bool isSucceed, List<TestModel> specs)
            {
                IsSucceed = isSucceed;
                Specs = specs;
            }
        }
    }

}
