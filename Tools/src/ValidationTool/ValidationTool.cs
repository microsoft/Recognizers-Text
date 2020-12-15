using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.Recognizers.Text.DataDrivenTests;

using Newtonsoft.Json;

namespace Microsoft.Recognizers.Text.Validation
{
    public class ValidationTool
    {

        public static void Main(string[] args)
        {
            // Enabling different encodings (e.g. to support chars like '€' in .NET Core
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            // Validate parameters
            if (args == null || args.Length == 0)
            {
                Console.WriteLine(@"No arguments passed. Please specify a file path or a directory containing specs.");
                Console.WriteLine(@"Usages: ValidationTool <specs_path>");
                return;
            }

            string inputPath = args[0];

            string[] specPaths = ListSpecFiles(inputPath);

            foreach (string path in specPaths)
            {
                int status = RunPipeline(path);
                if (status == 3)
                {
                    Console.WriteLine($"File {path} passes all validation stages.");
                }
                else
                {
                    Console.WriteLine($"File {path} fails stages {status + 1}.");
                }
            }

            Console.WriteLine();
        }

        private static string[] ListSpecFiles(string specsPath)
        {

            string directory;
            string filename;

            if (Directory.Exists(specsPath))
            {
                directory = specsPath;
                filename = "*";
            }
            else
            {
                var pathSplit = specsPath.Split(Path.DirectorySeparatorChar);
                directory = pathSplit.Length > 1 ? string.Join(Path.DirectorySeparatorChar, pathSplit.Take(pathSplit.Length - 1)) : $".{Path.DirectorySeparatorChar}";
                filename = pathSplit[pathSplit.Length - 1];
            }

            if (!Directory.Exists(directory))
            {
                Console.WriteLine($"Specs directory {directory} does not exist.");
                return Array.Empty<string>();
            }

            string[] paths = Directory.GetFiles(directory, filename);

            Console.WriteLine($"Loading {paths.Length} spec files...");
            Console.WriteLine();

            return paths;
        }

        private static int RunPipeline(string filePath)
        {
            Console.WriteLine(@"-------------------------");
            Console.WriteLine($"Begin to check file: {filePath}");

            BasicStageResult data = LoadJsonFile(filePath);

            if (!data.IsSuccess)
            {
                return 0;
            }

            bool stage2 = CheckBasicContent(data.Specs);
            if (!stage2)
            {
                return 1;
            }

            bool stage3 = CheckModelResult(data.Specs);

            Console.WriteLine($"Check concluded for file: {filePath}");
            Console.WriteLine(@"-------------------------");

            return Convert.ToInt32(data.IsSuccess) + Convert.ToInt32(stage2) + Convert.ToInt32(stage3);
        }

        private static BasicStageResult LoadJsonFile(string filePath)
        {
            List<TestModel> specs = new List<TestModel> { };
            string logStr = string.Empty;

            if (!File.Exists(filePath))
            {
                logStr = $"File {filePath} does not exist!";
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
                        logStr = $"JSON File is empty.";
                    }
                }
                catch (JsonReaderException jre)
                {
                    logStr = $"JSON Reader exception! \nException Type: {jre.GetType()}, \nException Message: {jre.Message}, \nPosition: Line{jre.LineNumber}/{jre.LinePosition}.";
                }
                catch (JsonSerializationException jse)
                {
                    logStr = $"JSON Serialzation Exception! \nException Type: {jse.GetType()}, \nException Message: {jse.Message}, \nPosition: Line{jse.LineNumber}/{jse.LinePosition}.";
                }
                catch (Exception e)
                {
                    logStr = $"Other error! \nException Type: {e.GetType()}, \nException Message: {e.Message}.";
                }
            }

            return new BasicStageResult(ConsoleStageResult("JSON is valid", logStr), specs);
        }

        private static bool CheckBasicContent(List<TestModel> data)
        {
            List<string> logStrs = new List<string> { };
            int errorNum = 0;

            foreach (var item in data.Select((value, i) => new { i, value }))
            {
                string logStr = GetSpecBasicContent(item.value);
                if (!string.Empty.Equals(logStr))
                {
                    if (errorNum == 0)
                    {
                        Console.WriteLine();
                    }

                    Console.WriteLine($"\tSpec error in item #{item.i}, \n\tText: {item.value.Input}, \n\tError Message: {logStr}\n");
                    ++errorNum;
                }

                logStrs.Add(logStr);
            }

            return ConsoleStageResult("Content valid", CountLogLines(logStrs), logStrs.Count - errorNum);
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

            if (string.Empty.Equals(spec.Input))
            {
                logStr = "Json file is empty";
            }
            else
            {
                var dynamicResults = spec.CastResults<dynamic>();
                foreach (dynamic result in dynamicResults)
                {
                    string resultStr = CheckSpecAttributes(spec, result);
                    if (!string.Empty.Equals(resultStr))
                    {
                        logStr = resultStr;
                        break;
                    }
                }
            }

            return logStr;
        }

        private static string CheckSpecAttributes(TestModel spec, dynamic result)
        {
            // Check the content in one Spec["Result"]
            if (result["Text"] == null || result["Start"] == null || (result["End"] == null && result["Length"] == null))
            {
                return "Spec[\"Result\"] Loss Some Parameter.";
            }

            int start = result["Start"];
            int end = result["End"] != null ? result["End"] : result["Length"] + start - 1;
            if (end > spec.Input.Length || end < 0)
            {
                return "Spec[\"End\"] Value Out of Bounds.";
            }

            if (start > spec.Input.Length || end < 0)
            {
                return "Spec[\"Start\"] Value Out of Bounds.";
            }

            var startEndStr = spec.Input.Substring(start, end - start + 1);
            if (result["Text"].Equals(startEndStr))
            {
                return "Spec[\"Result\"] Index Error";
            }

            return string.Empty;
        }

        private static string CountLogLines(List<string> logStrs)
        {
            Dictionary<string, int> logStrCounter = new Dictionary<string, int>();
            foreach (string f in logStrs)
            {
                int oldValue;
                logStrCounter.TryGetValue(f, out oldValue);
                logStrCounter[f] = ++oldValue;
            }

            string needLogStr = string.Empty;
            var logStrLogs = logStrCounter.Keys.Where(f => !string.Empty.Equals(f));
            foreach (string logStrLog in logStrLogs)
            {
                needLogStr += $"{logStrCounter[logStrLog]} specs {logStrLog}, ";
            }

            return needLogStr;
        }

        private static bool ConsoleStageResult(string stage, string logStr, int succeedNum = -1)
        {
            // Log stage result, and return success/error bool flag.
            bool res = string.Empty.Equals(logStr);

            string statusLogStr = res ? "Success" : $"Error: {logStr}";
            if (succeedNum != -1)
            {
                statusLogStr += $", {succeedNum} specs checked successfully";
            }

            Console.WriteLine($"{stage} check {statusLogStr}.");
            return res;
        }

        internal class BasicStageResult
        {
            public bool IsSuccess { get; set; }

            public List<TestModel> Specs { get; set; }

            public BasicStageResult(bool isSucceed, List<TestModel> specs)
            {
                IsSuccess = isSucceed;
                Specs = specs;
            }
        }
    }

}
