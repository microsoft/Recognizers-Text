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
        private static readonly string SplitLineStr = @"-------------------------";
        private static readonly string[] StageStrs = new string[3]
        {
            "JSON vaild",
            "content vaild",
            "model result",
        };

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
                Console.WriteLine();
                string logStr = $"Check concluded for file: {path}. ";
                if (status == StageStrs.Length)
                {
                    logStr += "Passed all validation stages.";
                }
                else
                {
                    logStr += $"Failed to check {StageStrs[status]}.";
                }

                Console.WriteLine(logStr);
                Console.WriteLine(SplitLineStr);
            }

            Console.WriteLine();
        }

        private static string[] ListSpecFiles(string specsPath)
        {

            string directory;
            string filename;

            if (specsPath.EndsWith(Path.DirectorySeparatorChar))
            {
                specsPath = specsPath[0..^1];
            }

            if (Directory.Exists(specsPath))
            {
                directory = specsPath;
                filename = "*";
            }
            else
            {
                directory = Path.GetDirectoryName(specsPath);
                filename = Path.GetFileName(specsPath);
            }

            if (!Directory.Exists(directory))
            {
                Console.WriteLine($"Specs directory {directory} does not exist.");
                return Array.Empty<string>();
            }

            string[] paths = Directory.GetFiles(directory, filename);
            if (paths.Length == 0)
            {
                Console.WriteLine($"No specs file in path {specsPath}.");
                return paths;
            }

            Console.WriteLine($"Loading {paths.Length} spec files...");
            Console.WriteLine();

            return paths;
        }

        private static int RunPipeline(string filePath)
        {
            Console.WriteLine(SplitLineStr);
            Console.WriteLine($"Begin to check file: {filePath}");
            Console.WriteLine();

            BasicStageResult stage1 = LoadJsonFile(filePath);

            if (!stage1.IsSuccess)
            {
                return 0;
            }

            BasicStageResult stage2 = CheckBasicContent(stage1.Specs);
            if (!stage2.IsSuccess)
            {
                return 1;
            }

            BasicStageResult stage3 = CheckModelResult(stage2.Specs);
            return Convert.ToInt32(stage1.IsSuccess) + Convert.ToInt32(stage2.IsSuccess) + Convert.ToInt32(stage3.IsSuccess);
        }

        private static BasicStageResult LoadJsonFile(string filePath)
        {
            List<TestModel> specs = new List<TestModel> { };
            string logStr = string.Empty;

            if (!Path.GetExtension(filePath).Equals(".json", StringComparison.OrdinalIgnoreCase))
            {
                logStr = $"File path {filePath} extension should be \".json\"";
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

            return new BasicStageResult("JSON is valid", logStr, specs);
        }

        private static BasicStageResult CheckBasicContent(List<TestModel> data)
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

                    Console.WriteLine($"\tSpec error in item #{item.i}, \n\tText: {item.value.Input}, \n\tContext: {JsonConvert.SerializeObject(item.value.Context)},\n\tError Message: {logStr}\n");
                    ++errorNum;
                }

                logStrs.Add(RemoveLogStrDetailInformation(logStr));
            }

            return new BasicStageResult("Content valid", CountLogLines(logStrs), data, logStrs.Count - errorNum);
        }

        private static BasicStageResult CheckModelResult(List<TestModel> data)
        {
            // ToDo: Compare the Recoginzer model result with specs file result.
            string logStr = string.Empty;
            return new BasicStageResult("Model Result", logStr, data);
        }

        // Function waiting to be realized
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
            if (result["Text"] == null)
            {
                return "Spec lost parameter [\"Text\"] .";
            }

            if (result["Start"] == null)
            {
                return "Spec lost parameter [\"Start\"] .";
            }

            if (result["End"] == null && result["Length"] == null)
            {
                return "Spec lost parameter [\"End\"] or [\"Length\"]. ";
            }

            int start = result["Start"];
            int end = result["End"] != null ? result["End"] : result["Length"] + start - 1;
            if (start > spec.Input.Length || start < 0)
            {
                return $"Spec[\"Start\"] value {result["Start"]} out of bounds.";
            }

            if (end > spec.Input.Length || end < start)
            {
                return result["End"] != null ? $"Spec[\"End\"] value {result["End"]} out of bounds." : $"Spec[\"Length\"] value {result["Length"]} out of bounds.";
            }

            var startEndStr = spec.Input.Substring(start, end - start + 1);
            var textStr = result["Text"].Value;
            if (!textStr.Equals(startEndStr, StringComparison.OrdinalIgnoreCase))
            {
                return $"Spec[\"Result\"] content \"{textStr}\" mismatched input text \"{startEndStr}\".";
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
                statusLogStr += res ? ": " : string.Empty;
                statusLogStr += $"{succeedNum} specs checked successfully";
            }

            Console.WriteLine($"{stage} check {statusLogStr}.");
            return res;
        }

        private static string RemoveLogStrDetailInformation(string logStr)
        {
            if (logStr.Contains("out of bounds."))
            {
                int valueStart = logStr.IndexOf("value");
                logStr = $"{logStr.Substring(0, valueStart)} value out of bounds.";
            }
            else if (logStr.Contains("mismatched input text"))
            {
                logStr = "Spec[\"Result\"] content mismatched input text";
            }

            return logStr;
        }

        internal class BasicStageResult
        {
            public bool IsSuccess { get; set; }

            public List<TestModel> Specs { get; set; }

            public BasicStageResult(string stage, string logStr, List<TestModel> specs, int succeedNum = -1)
            {
                IsSuccess = ConsoleStageResult(stage, logStr, succeedNum);
                Specs = specs;
            }
        }
    }

}
