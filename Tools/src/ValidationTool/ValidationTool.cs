using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
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
        private const string AutoUpdatedSplit = "\n\t";

        private const string BackupFileExtension = ".bak";

        private static ToolOptions options;

        public static void Main(string[] args)
        {
            // Enabling different encodings (e.g. to support chars like '€' in .NET Core
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            if (!ParserArgs(args))
            {
                return;
            }

            if (options == null || string.IsNullOrWhiteSpace(options.SpecsPath))
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
            var command = new RootCommand(description: "Specs validation tool")
            {
                new Option<bool>(aliases: new string[] { "--save", "-s" }, () => false)
                {
                    Description = "Save auto-fixes in original spec files.",
                },
                new Argument<string>("specs-path")
                {
                    Description = "The specs path can be a spec file path or a directory containing spec files.",
                },
            };

            command.Handler = CommandHandler.Create(
                (string specsPath, bool save) =>
                {
                    options = new ToolOptions(specsPath, save);
                });

            return command.InvokeAsync(args).Result == 0 && options != null;
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

            string backupFilePath = path + BackupFileExtension;
            if (!File.Exists(backupFilePath))
            {
                File.Copy(path, backupFilePath);
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

            string rawResults = JsonConvert.SerializeObject(originalSpecs, Formatting.Indented) + "\n";

            // Write file in UTF-8, but omit BOM.
            var encoding = new UTF8Encoding(false);
            File.WriteAllText(path, rawResults, encoding);

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
            string logLines = $"Check concluded for file: {path}.";
            if (isSuccess)
            {
                logLines += "Passed all validation stages.";
            }
            else
            {
                logLines += $"Failed to check {errorStages}.";
            }

            Console.WriteLine(logLines);
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
            List<string> logLines = new List<string> { };
            bool isSuccess = true;

            if (!Path.GetExtension(filePath).Equals(".json", StringComparison.OrdinalIgnoreCase))
            {
                logLines.Add($"File path {filePath} extension should be \".json\"");
                isSuccess = false;
            }

            var rawData = File.ReadAllText(filePath);

            // Try to load specs JSON file, and catch exceptions.
            try
            {
                specs = (List<TestModel>)JsonConvert.DeserializeObject<IList<TestModel>>(rawData);
                if (!specs.Any())
                {
                    logLines.Add($"JSON File is empty.");
                    isSuccess = false;
                }
            }
            catch (JsonReaderException jre)
            {
                logLines.Add($"JSON Reader exception! \nException Type: {jre.GetType()}, \nException Message: {jre.Message}, \nPosition: Line{jre.LineNumber}/{jre.LinePosition}.");
                isSuccess = false;
            }
            catch (JsonSerializationException jse)
            {
                logLines.Add($"JSON Serialzation Exception! \nException Type: {jse.GetType()}, \nException Message: {jse.Message}, \nPosition: Line{jse.LineNumber}/{jse.LinePosition}.");
                isSuccess = false;
            }
            catch (Exception e)
            {
                logLines.Add($"Other error! \nException Type: {e.GetType()}, \nException Message: {e.Message}.");
                isSuccess = false;
            }

            return new BasicStageResult(filePath, "Json is valid", logLines, specs, isSuccess);
        }

        private static BasicStageResult CheckBasicContent(BasicStageResult stage)
        {
            List<TestModel> data = stage.Specs;
            List<TestModel> matchData = new List<TestModel> { };
            List<string> logLines = new List<string> { };
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
                        logLines.Add(string.Empty);
                    }

                    ++errorNum;

                    var msg = $"\tSpec error in item #{item.i + 1}, " +
                              $"{AutoUpdatedSplit}Text: {item.value.Input}, " +
                              $"{AutoUpdatedSplit}Context: {JsonConvert.SerializeObject(item.value.Context)}, " +
                              $"{AutoUpdatedSplit}Error Message: {specResult.logLine}\n";

                    logLines.Add(msg);
                    counterStrs.Add(RemoveLogDetailInformation(specResult.logLine));

                    if (options.Save && specResult.logLine.Contains(AutoUpdatedSplit))
                    {
                        autoUpdatedStrs.Add(specResult.logLine.Split(AutoUpdatedSplit)[1]);
                    }
                }

                matchData.Add(item.value);
            }

            if (options.Save)
            {
                logLines.Add(CountLogLines(autoUpdatedStrs));
            }

            logLines.Add(CountLogLines(counterStrs));

            return new BasicStageResult(stage.FilePath, "Content valid", logLines, matchData, errorNum == 0, matchData.Count - errorNum);
        }

        private static BasicStageResult CheckModelResult(BasicStageResult stage)
        {
            // @TODO: Compare the Recoginzer model results with results in specs file.
            List<TestModel> data = stage.Specs;
            List<string> logLines = new List<string> { };
            bool isSuccess = true;

            // To be implemented.

            return new BasicStageResult(stage.FilePath, "Model Result", logLines, data, isSuccess);
        }

        private static (string logLine, bool isSuccess) GetSpecBasicContent(TestModel spec)
        {
            // Check the contents in one spec file
            string logLine = string.Empty;
            bool isSuccess = true;

            if (string.Empty.Equals(spec.Input))
            {
                logLine = "Json file is empty";
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
                        logLine = resultStr;
                        isSuccess = false;
                        if (options.Save)
                        {
                            logLine += MatchIndex(spec, result);
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

            return (logLine, isSuccess);
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
                return $"Spec[\"Result\"] \"Start\" value {result["Start"]} out of bounds.";
            }

            if (end >= spec.Input.Length || end < start)
            {
                return result["End"] != null ? $"Spec[\"Result\"] \"End\" value {result["End"]} out of bounds." : $"Spec[\"Result\"] \"Length\" value {result["Length"]} out of bounds.";
            }

            var startEndStr = spec.Input.Substring(start, end - start + 1);
            var textStr = result["Text"].Value.ToString();
            if (!spec.Input.Contains(textStr, StringComparison.OrdinalIgnoreCase))
            {
                return $"Spec[\"Result\"] \"Text\" error, the text \"{textStr}\" is not in \"Input\".";
            }

            if (!textStr.Equals(startEndStr, StringComparison.OrdinalIgnoreCase))
            {
                return $"Spec[\"Result\"] index error, \"Start\": {start}, \"End\": {end}, \"Text\": \"{textStr}\".";
            }

            return string.Empty;
        }

        private static string CountLogLines(List<string> logLines)
        {
            Dictionary<string, int> logCollector = new Dictionary<string, int>();
            foreach (string f in logLines)
            {
                int oldValue;
                logCollector.TryGetValue(f, out oldValue);
                logCollector[f] = ++oldValue;
            }

            string feedLine = string.Empty;
            var validLines = logCollector.Keys.Where(f => !string.Empty.Equals(f));
            foreach (string line in validLines)
            {
                string logLine = line.Replace(".", string.Empty);
                feedLine += $"{logCollector[line]} specs {logLine}, ";
            }

            return feedLine;
        }

        private static string ConsoleStageResult(string stage, string logLine, bool isSuccess, int succeedNum = -1)
        {
            // Log stage result, and return success/error bool flag.
            string status = isSuccess ? "Success" : $"Error: {logLine}";
            if (succeedNum != -1)
            {
                status += isSuccess ? ": " : string.Empty;
                status += $"{succeedNum} specs checked successfully";
            }

            return $"{stage} check {status}.";
        }

        private static string RemoveLogDetailInformation(string logLine)
        {
            if (logLine.Contains("out of bounds."))
            {
                int valueStart = logLine.IndexOf("value", StringComparison.Ordinal);
                logLine = $"{logLine.Substring(0, valueStart)} value out of bounds.";
            }
            else if (logLine.Contains(","))
            {
                logLine = logLine.Split(",")[0];
            }
            else if (logLine.Contains(AutoUpdatedSplit))
            {
                logLine = logLine.Split(AutoUpdatedSplit)[0];
            }

            return logLine;
        }

        private static string MatchIndex(TestModel spec, dynamic result)
        {
            string logLine = string.Empty;
            if (spec.Input == null || result["Text"] == null || result["Text"].Equals(string.Empty))
            {
                return logLine;
            }

            string input = spec.Input;
            string text = result["Text"];
            int index;
            if (input.Contains(text, StringComparison.Ordinal))
            {
                index = input.IndexOf(text, StringComparison.Ordinal);
                if (input.Substring(index + 1).Contains(text, StringComparison.Ordinal))
                {
                    logLine = $"{AutoUpdatedSplit}Failed to auto update , text match is potentially ambiguous.";
                }
                else
                {
                    logLine = $"{AutoUpdatedSplit}Auto update success.";
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

            return logLine;
        }

        internal class BasicStageResult
        {
            public BasicStageResult(string path, string stage, List<string> logLines, List<TestModel> specs, bool isSuccess, int succeedNum = -1)
            {
                this.FilePath = path;
                this.StageName = stage;
                this.IsSuccess = isSuccess;
                this.Logs = logLines;
                this.Specs = specs;
                this.SucceedNum = succeedNum;
            }

            public string FilePath { get; set; }

            public string StageName { get; set; }

            public bool IsSuccess { get; set; }

            public int SucceedNum { get; set; }

            public List<string> Logs { get; set; }

            public List<TestModel> Specs { get; set; }
        }

        internal class ToolOptions
        {
            public ToolOptions(string specsPath, bool save)
            {
                this.SpecsPath = specsPath;
                this.Save = save;
            }

            public string SpecsPath { get; set; }

            public bool Save { get; set; }
        }
    }

}
