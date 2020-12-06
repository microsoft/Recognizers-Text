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

namespace FormatCheckTool
{
    class FormatCheckTool
    {
        private static readonly String PleaseCheckStr = "Please Check it.";

        static void Main(string[] args)
        {
            // Encoding for 'exotic' characters e.g. '€'
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            ShowIntro();

            while (true)
            {
                Console.WriteLine("Enter the specs JSON file path to be checked:");
                var input = Console.ReadLine()?.Trim();
                Console.WriteLine();

                if (input?.ToLower(CultureInfo.InvariantCulture) == "exit")
                {
                    break;
                }

                // Validate input
                if (input?.Length > 0)
                {
                    var pathSplit = input.Split(Path.DirectorySeparatorChar);
                    string basicDir = pathSplit.Length > 1 ? string.Join(Path.DirectorySeparatorChar, pathSplit.Take(pathSplit.Length - 1)) : $".{Path.DirectorySeparatorChar}";
                    string basicPath = pathSplit[pathSplit.Length - 1];
                    if (!Directory.Exists(basicDir)) {
                        Console.WriteLine($"File Path Error. {PleaseCheckStr}");
                        continue;
                    }

                    string[] paths = Directory.GetFiles(basicDir, basicPath);
                    Console.WriteLine($"Loading {paths.Length} Spec Files...");
                    Console.WriteLine();

                    foreach (string path in paths)
                    {
                        CheckPipeline(path);
                    }

                    Console.WriteLine();
                }
            }
        }

        private static void CheckPipeline(string filePath)
        {
            Console.WriteLine("-------------------------");
            Console.WriteLine($"Begin to check file {filePath}.");
            Tuple<bool, List<TestModel>> data = LoadJsonFile(filePath);
            if (!data.Item1)
            {
                return;
            }

            bool stage2 = CheckBasicContent(data.Item2);
            if (!stage2)
            {
                return;
            }

            bool stage3 = CheckModelResult(data.Item2);
        }

        private static Tuple<bool, List<TestModel>> LoadJsonFile(string filePath)
        {
            List<TestModel> specs = new List<TestModel>{ };
            string flag = string.Empty;
            if (!File.Exists(filePath))
            {
                flag = $"File {filePath} Dose Not Exist! {PleaseCheckStr}";
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
                        flag = $"JSON File is Empty, {PleaseCheckStr}";
                    }
                }
                catch (JsonReaderException jre)
                {
                    flag = $"JSON Reader Exception! {PleaseCheckStr}";
                }
                catch (JsonSerializationException jse)
                {
                    flag = $"JSON SerialzationException! {PleaseCheckStr}";
                }
                catch (Exception e)
                {
                    flag = $"Other File Error, {PleaseCheckStr}";
                }
            }

            return new Tuple<bool, List<TestModel>>(ConsoleStageResult("JSON Valid", flag), specs);
        }

        private static bool CheckBasicContent(List<TestModel> data)
        {
            string flag = string.Empty;
            return ConsoleStageResult("Content Valid", flag);
        }

        private static bool CheckModelResult(List<TestModel> data)
        {
            string flag = string.Empty;
            return ConsoleStageResult("Model Result", flag);
        }

        private static bool ConsoleStageResult(string stage, string flag)
        {
            bool res = flag.Equals(string.Empty);
            string logStr = res ? "Success" : $"Error. {flag}";
            Console.WriteLine($"{stage} Check {logStr}.");
            return res;
        }

        /// <summary>
        /// Introduction.
        /// </summary>
        private static void ShowIntro()
        {
            Console.WriteLine("Welcome to the Recognizers' JSON Format Check Tool console application!");
            Console.WriteLine("To check the specs JSON format correctness. You should enter the specs file path which supports simple wildcard character and the tool will show the format check result.");
            Console.WriteLine();
            Console.WriteLine("Here are some path examples you could enter:");
            Console.WriteLine();
            Console.WriteLine("\" C:\\Users\\temp\\Desktop\\French\\DateTimeMode.json\"");
            Console.WriteLine("\" C:\\Users\\temp\\Desktop\\French\\*.json\"");
            Console.WriteLine();
            Console.WriteLine("(If you want to exit the format tool, please enter \"exit\")");
            Console.WriteLine();
        }
    }
}
