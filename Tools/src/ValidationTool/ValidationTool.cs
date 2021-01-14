using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using Microsoft.Recognizers.Text.DataDrivenTests;
using Newtonsoft.Json;

namespace Microsoft.Recognizers.Text.Validation
{
    public class ValidationTool
    {
        private static readonly string[] StageStrs = new string[3]
        {
            "JSON vaild",
            "content vaild",
            "model result",
        };

        private static readonly string AutoUpdatedSplit = "\n\t";

        private static ToolOptions options;

        public static void Main(string[] args)
        {
            // Enabling different encodings (e.g. to support chars like '€' in .NET Core
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            if (!ParserArgs(args))
            {
                return;
            }

            string[] specsPaths = ListSpecFiles(options.SpecsPath);

            foreach (string path in specsPaths)
            {
                List<BasicStageResult> stageResults = RunPipeline(path);
                ShowLogs(stageResults);
                if (options.Save)
                {
                    SaveMatchResults(stageResults, path);
                }
            }

            Console.WriteLine();
        }

        // Parser args parameters
        private static bool ParserArgs(string[] args)
        {
            var command = new RootCommand(description: "Specs file validtion tool")
            {
                new Option<bool>(aliases: new string[] {"--save", "-s"}, () => false) {
                    Description = "Save match result in original file.",
                },
                new Argument<string>("specs-path") {
                    Description = "The specs file path, could be file path or a directory containing specs."
                },
            };

            command.Handler = CommandHandler.Create(
                (string specsPath, bool save) =>
                {
                    options = new ToolOptions(specsPath, save);
                });

            return command.InvokeAsync(args).Result == 0;
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

        private static void SaveMatchResults(List<BasicStageResult> stageResults, string path)
        {
            if (stageResults.Count <= 1)
            {
                return;
            }

            string newPath = path + ".bak";
            if (!File.Exists(newPath))
            {
                File.Copy(path, newPath);
            }

            // Convert match results and original specs to same data type Newtonsoft.Json.Linq.JArray.
            var rawSpecs = JsonConvert.SerializeObject(stageResults[stageResults.Count - 1].Specs);
            List<dynamic> jsonSpecs = (List<dynamic>)JsonConvert.DeserializeObject<IList<dynamic>>(rawSpecs);
            var rawData = File.ReadAllText(path);
            List<dynamic> originalSpecs = (List<dynamic>)JsonConvert.DeserializeObject<IList<dynamic>>(rawData);

            // Replace matching results.
            for (int i = 0; i < jsonSpecs.Count; ++i)
            {
                originalSpecs[i]["Results"] = jsonSpecs[i]["Results"];
            }

            string rawResults = JsonConvert.SerializeObject(originalSpecs, Formatting.Indented);
            File.WriteAllText(path, rawResults, Encoding.UTF8);
            Console.WriteLine($"Success save match results to path: {path}");
        }

        private static void ShowLogs(List<BasicStageResult> stageResults)
        {
            string path = stageResults[0].FilePath;
            bool isSuccess = true;
            string errorStages = string.Empty;

            Console.WriteLine(@"-------------------------");
            Console.WriteLine($"Begin to check file: {path}");
            Console.WriteLine();
            foreach (BasicStageResult stage in stageResults)
            {
                for (int index = 0; index < stage.Logs.Count - 1; ++index)
                {
                    Console.WriteLine(stage.Logs[index]);
                }

                Console.WriteLine(ConsoleStageResult(stage.StageName, !stage.IsSuccess ? stage.Logs[stage.Logs.Count - 1] : string.Empty, stage.IsSuccess, stage.SucceedNum));
                if (!stage.IsSuccess)
                {
                    if (!isSuccess)
                    {
                        errorStages += $",{stage.StageName}";
                    }

                    isSuccess = false;
                }
            }

            Console.WriteLine();
            string logStr = $"Check concluded for file: {path}.";
            if (isSuccess)
            {
                logStr += "Passed all validation stages.";
            }
            else
            {
                logStr += $"Failed to check {errorStages}.";
            }

            Console.WriteLine(logStr);
            Console.WriteLine(@"-------------------------");
        }

        private static List<BasicStageResult> RunPipeline(string filePath)
        {
            BasicStageResult stage1 = LoadJsonFile(filePath);
            List<BasicStageResult> results = new List<BasicStageResult> { stage1 };

            if (!stage1.IsSuccess)
            {
                return results;
            }

            BasicStageResult stage2 = CheckBasicContent(stage1);
            BasicStageResult stage3 = CheckModelResult(stage2);
            results.Add(stage2);
            results.Add(stage3);
            return results;
        }

        private static BasicStageResult LoadJsonFile(string filePath)
        {
            List<TestModel> specs = new List<TestModel> { };
            List<string> logStrs = new List<string> { };
            bool isSuccess = true;

            if (!Path.GetExtension(filePath).Equals(".json", StringComparison.OrdinalIgnoreCase))
            {
                logStrs.Add($"File path {filePath} extension should be \".json\"");
                isSuccess = false;
            }

            var rawData = File.ReadAllText(filePath);

            // Try to load Specs JSON file, and catch exception.
            try
            {
                specs = (List<TestModel>)JsonConvert.DeserializeObject<IList<TestModel>>(rawData);
                if (!specs.Any())
                {
                    logStrs.Add($"JSON File is empty.");
                    isSuccess = false;
                }
            }
            catch (JsonReaderException jre)
            {
                logStrs.Add($"JSON Reader exception! \nException Type: {jre.GetType()}, \nException Message: {jre.Message}, \nPosition: Line{jre.LineNumber}/{jre.LinePosition}.");
                isSuccess = false;
            }
            catch (JsonSerializationException jse)
            {
                logStrs.Add($"JSON Serialzation Exception! \nException Type: {jse.GetType()}, \nException Message: {jse.Message}, \nPosition: Line{jse.LineNumber}/{jse.LinePosition}.");
                isSuccess = false;
            }
            catch (Exception e)
            {
                logStrs.Add($"Other error! \nException Type: {e.GetType()}, \nException Message: {e.Message}.");
                isSuccess = false;
            }

            return new BasicStageResult(filePath, "Json is valid", logStrs, specs, isSuccess);
        }

        private static BasicStageResult CheckBasicContent(BasicStageResult stage)
        {
            List<TestModel> data = stage.Specs;
            List<TestModel> matchData = new List<TestModel> { };
            List<string> logStrs = new List<string> { };
            List<string> counterStrs = new List<string> { };
            List<string> autoUpdatedStrs = new List<string> { };
            int errorNum = 0;

            foreach (var item in data.Select((value, i) => new { i, value }))
            {
                var specResult = GetSpecBasicContent(item.value);
                if (!specResult.isSuccess)
                {
                    if (errorNum == 0)
                    {
                        logStrs.Add(string.Empty);
                    }

                    ++errorNum;
                    logStrs.Add($"\tSpec error in item #{item.i + 1}, \n\tText: {item.value.Input}, \n\tContext: {JsonConvert.SerializeObject(item.value.Context)},\n\tError Message: {specResult.logStr}\n");
                    counterStrs.Add(RemoveLogStrDetailInformation(specResult.logStr));
                    if (options.Save && specResult.logStr.Contains(AutoUpdatedSplit))
                    {
                        autoUpdatedStrs.Add(specResult.logStr.Split(AutoUpdatedSplit)[1]);
                    }
                }

                matchData.Add(item.value);
            }

            if (options.Save)
            {
                logStrs.Add(CountLogLines(autoUpdatedStrs));
            }

            logStrs.Add(CountLogLines(counterStrs));

            return new BasicStageResult(stage.FilePath, "Content valid", logStrs, matchData, errorNum == 0, logStrs.Count - errorNum);
        }

        private static BasicStageResult CheckModelResult(BasicStageResult stage)
        {
            // ToDo: Compare the Recoginzer model result with specs file result.
            List<TestModel> data = stage.Specs;
            List<string> logStrs = new List<string> { };
            bool isSuccess = true;
            string logStr = string.Empty;
            return new BasicStageResult(stage.FilePath, "Model Result", logStrs, data, isSuccess);
        }

        // Function waiting to be realized
        private static (string logStr, bool isSuccess) GetSpecBasicContent(TestModel spec)
        {
            // Check the content in one Spec file
            string logStr = string.Empty;
            bool isSuccess = true;

            if (string.Empty.Equals(spec.Input))
            {
                logStr = "Json file is empty";
                isSuccess = false;
            }
            else
            {
                var dynamicResults = spec.CastResults<dynamic>();
                var matchResults = new List<dynamic> { };
                foreach (dynamic result in dynamicResults)
                {
                    string resultStr = CheckSpecAttributes(spec, result);
                    if (!string.Empty.Equals(resultStr))
                    {
                        logStr = resultStr;
                        isSuccess = false;
                        if (options.Save)
                        {
                            logStr += MatchIndex(spec, result);
                        }
                    }

                    matchResults.Add(result);
                }

                if (!isSuccess)
                {
                    var resultsStr = JsonConvert.SerializeObject(matchResults);
                    spec.Results = JsonConvert.DeserializeObject<IEnumerable<object>>(resultsStr);
                }
            }

            return (logStr, isSuccess);
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

            if (end >= spec.Input.Length || end < start)
            {
                return result["End"] != null ? $"Spec[\"End\"] value {result["End"]} out of bounds." : $"Spec[\"Length\"] value {result["Length"]} out of bounds.";
            }

            var startEndStr = spec.Input.Substring(start, end - start + 1);
            var textStr = result["Text"].Value.ToString();
            if (!textStr.Equals(startEndStr, StringComparison.OrdinalIgnoreCase))
            {
                return $"Spec[\"Result\"] index error, \"Start\": {start}, \"End\": {end}, the text \"{textStr}\" mismatched input text \"{startEndStr}\".";
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
                string logStr = logStrLog.Replace(".", string.Empty);
                needLogStr += $"{logStrCounter[logStrLog]} specs {logStr}, ";
            }

            return needLogStr;
        }

        private static string ConsoleStageResult(string stage, string logStr, bool isSuccess, int succeedNum = -1)
        {
            // Log stage result, and return success/error bool flag.
            string statusLogStr = isSuccess ? "Success" : $"Error: {logStr}";
            if (succeedNum != -1)
            {
                statusLogStr += isSuccess ? ": " : string.Empty;
                statusLogStr += $"{succeedNum} specs checked successfully";
            }

            return $"{stage} check {statusLogStr}.";
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
            else if (logStr.Contains(AutoUpdatedSplit))
            {
                logStr = logStr.Split(AutoUpdatedSplit)[0];
            }

            return logStr;
        }

        private static string MatchIndex(TestModel spec, dynamic result) {
            string logStr = string.Empty;
            if (spec.Input == null || result["Text"] == null || result["Text"].Equals(string.Empty))
            {
                return logStr;
            }

            string input = spec.Input;
            string text = result["Text"];
            int index;
            if (input.Contains(text, StringComparison.Ordinal))
            {
                index = input.IndexOf(text, StringComparison.Ordinal);
                if (input.Substring(index + 1).Contains(text, StringComparison.Ordinal))
                {
                    logStr = $"{AutoUpdatedSplit}Failed to auto update , text match is potentially ambiguous.";
                }
                else
                {
                    logStr = $"{AutoUpdatedSplit}Auto update success.";
                    result["Start"] = index;
                    if (result["End"] == null)
                    {
                        result["Length"] = text.Length;
                    }
                    else
                    {
                        result["End"] = result["Start"] + text.Length;
                    }
                }
            }

            return logStr;
        }

        internal class BasicStageResult
        {
            public string FilePath { get; set; }

            public string StageName { get; set; }

            public bool IsSuccess { get; set; }

            public int SucceedNum { get; set; }

            public List<string> Logs { get; set; }

            public List<TestModel> Specs { get; set; }

            public BasicStageResult(string path, string stage, List<string> logStrs, List<TestModel> specs, bool isSuccess, int succeedNum = -1)
            {
                this.FilePath = path;
                this.StageName = stage;
                this.IsSuccess = isSuccess;
                this.Logs = logStrs;
                this.Specs = specs;
                this.SucceedNum = succeedNum;
            }
        }

        internal class ToolOptions
        {
            public string SpecsPath { get; set; }

            public bool Save { get; set; }

            public ToolOptions(string specsPath, bool save)
            {
                this.SpecsPath = specsPath;
                this.Save = save;
            }
        }
    }

}
