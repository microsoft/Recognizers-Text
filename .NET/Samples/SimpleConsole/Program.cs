using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.Number;
using Newtonsoft.Json;
using Microsoft.Recognizers.Text.NumberWithUnit;
using Microsoft.Recognizers.Text.DateTime;

namespace SimpleConsole
{
    class Program
    {
        private const string defaultCulture = "en-us";

        static void Main(string[] args)
        {
            // Show introduction
            Console.WriteLine("Welcome to the Recognizer's Sample console application!");
            Console.WriteLine("To try the recognizers enter a phrase and let us show you the different outputs for each recognizer or just type 'exit' to leave the application.");
            Console.WriteLine();

            while (true)
            {
                // Read the text to recognize
                Console.WriteLine("Enter the text to recognize:");
                string input = Console.ReadLine().Trim();
                Console.WriteLine();

                // Close application if user types "exit"
                if (input.ToLower() == "exit")
                {
                    Console.WriteLine("Goodbye!");
                    break;
                }
                else
                {
                    // Validate input 
                    if (input.Length > 0)
                    {
                        // Parse the input text and show the result of each recognizer as JSON.
                        GetParsers().Select(r =>
                            $"{r.GetType().Name}\t: {JsonConvert.SerializeObject(r.Parse(input))}")
                            .ToList()
                            .ForEach(x => Console.WriteLine(x.ToString()));
                        
                        //Leave a blank line
                        Console.WriteLine();
                    }
                }
            }
        }

        /// <summary>
        /// Get all recognizers model instances.
        /// </summary>
        /// <returns>A list of all the existing recognizer's models</returns>
        private static IEnumerable<IModel> GetParsers()
        {
            List<IModel> modelList = new List<IModel>();
            modelList.Add(NumberRecognizer.Instance.GetNumberModel(defaultCulture));
            modelList.Add(NumberRecognizer.Instance.GetOrdinalModel(defaultCulture));
            modelList.Add(NumberRecognizer.Instance.GetPercentageModel(defaultCulture));
            modelList.Add(NumberWithUnitRecognizer.Instance.GetAgeModel(defaultCulture));
            modelList.Add(NumberWithUnitRecognizer.Instance.GetCurrencyModel(defaultCulture));
            modelList.Add(NumberWithUnitRecognizer.Instance.GetDimensionModel(defaultCulture));
            modelList.Add(NumberWithUnitRecognizer.Instance.GetTemperatureModel(defaultCulture));
            modelList.Add(DateTimeRecognizer.GetInstance().GetDateTimeModel(defaultCulture));
            return modelList;
        }
    }
}
