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
                int status = RunPipeline(path);
                if (status == 3)
                {
                    Console.WriteLine($"File {path} Pass All Validation Stages.");
                }
                else
                {
                    Console.WriteLine($"File {path} Exception In Stages {status + 1}.");
                }
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

        private static int RunPipeline(string filePath)
        {
            Console.WriteLine("-------------------------");
            Console.WriteLine($"Begin to check file {filePath}.");
            BasicStageResult data = LoadJsonFile(filePath);
            if (!data.IsSucceed)
            {
                return 0;
            }

            bool stage2 = CheckBasicContent(data.Specs);
            if (!stage2)
            {
                return 1;
            }

            bool stage3 = CheckModelResult(data.Specs);

            return Convert.ToInt32(data.IsSucceed) + Convert.ToInt32(stage2) + Convert.ToInt32(stage3);
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
            List<string> logStrs = new List<string> { };
            int errorNum = 0;
            foreach (var item in data.Select((value, i) => new { i, value })) {
                string logStr = GetSpecBasicContent(item.value);
                if (!logStr.Equals(string.Empty))
                {
                    if (errorNum == 0)
                    {
                        Console.WriteLine();
                    }

                    Console.WriteLine($"\tNo.{item.i} spec error, \n\tText: {item.value.Input}, \n\tError Message: {logStr}\n");
                    ++errorNum;
                }

                logStrs.Add(logStr);
            }

            return ConsoleStageResult("Content Valid", CounterLogStrs(logStrs), logStrs.Count - errorNum);
        }

        private static bool CheckModelResult(List<TestModel> data)
        {
            // ToDo: Compare the Recoginzer model result with specs file result.
            string logStr = string.Empty;
            return ConsoleStageResult("Model Result", logStr);
        }

        private static string GetSpecBasicContent(TestModel spec)
        {
            // Check the content in one Spec file
            string logStr = string.Empty;
            if (spec.Input.Equals(string.Empty))
            {
                logStr = "Json File Empty";
            }
            else
            {
                var dynamicResults = spec.CastResults<dynamic>();
                foreach (dynamic result in dynamicResults)
                {
                    string resultStr = GetSpecBasicResult(spec, result);
                    if (!resultStr.Equals(string.Empty))
                    {
                        logStr = resultStr;
                        break;
                    }
                }
            }

            return logStr;
        }

        private static string GetSpecBasicResult(TestModel spec, dynamic result)
        {
            // Check the content in one Spec["Result"]
            if (result["Text"] == null || result["Start"] == null || (result["End"] == null && result["Length"] == null))
            {
                return "Spec[\"Result\"] Loss Some Parameter.";
            }

            int start = result["Start"];
            int end = result["End"] != null ? result["End"] : result["Length"] + start - 1;
            var startEndStr = spec.Input.Substring(start, end - start + 1);
            if (result["Text"] != startEndStr)
            {
                return "Spec[\"Result\"] Index Error";
            }

            return string.Empty;
        }

        private static string CounterLogStrs(List<string> logStrs)
        {
            Dictionary<string, int> logStrCounter = new Dictionary<string, int>();
            foreach (string f in logStrs)
            {
                int oldValue;
                logStrCounter.TryGetValue(f, out oldValue);
                logStrCounter[f] = ++oldValue;
            }

            string needLogStr = string.Empty;
            var logStrLogs = logStrCounter.Keys.Where(f => !f.Equals(string.Empty));
            foreach (string logStrLog in logStrLogs)
            {
                needLogStr += $"{logStrCounter[logStrLog]} specs {logStrLog}, ";
            }

            return needLogStr;
        }

        private static bool ConsoleStageResult(string stage, string logStr, int succeedNum = -1)
        {
            // Log stage result, and return success/error bool flag.
            bool res = logStr.Equals(string.Empty);
            string statusLogStr = res ? "Success" : $"Error. {logStr}";
            if (succeedNum != -1)
            {
                statusLogStr += $", {succeedNum} Specs Check Succeed";
            }

            Console.WriteLine($"{stage} Check {statusLogStr}.");
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
