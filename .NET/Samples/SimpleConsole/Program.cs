﻿using System;
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
        // Use English for the Recognizers culture
        private const string defaultCulture = Culture.English;

        static void Main(string[] args)
        {
            ShowIntro();

            while (true)
            {
                // Read the text to recognize
                Console.WriteLine("Enter the text to recognize:");
                string input = Console.ReadLine().Trim();
                Console.WriteLine();

                if (input.ToLower() == "exit")
                {
                    // Close application if user types "exit"
                    break;
                }
                else
                {
                    // Validate input 
                    if (input.Length > 0)
                    {
                        // Retrieve all the parsers and call 'Parse' to recognize all the values from the user input
                        var results = GetParsers()
                            .Select(parser => parser.Parse(input))
                            .SelectMany(a => a);

                        // Write output
                        Console.WriteLine(results.Count() > 0 ? (string.Format("I found the following entities ({0:d}):", results.Count())) : "I found no entities.");
                        results.ToList().ForEach(result => Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented)));
                        Console.WriteLine();
                    }
                }
            }
        }
        
        /// <summary>
        /// Introduction
        /// </summary>
        private static void ShowIntro()
        {
            Console.WriteLine("Welcome to the Recognizer's Sample console application!");
            Console.WriteLine("To try the recognizers enter a phrase and let us show you the different outputs for each recognizer or just type 'exit' to leave the application.");
            Console.WriteLine();
        }
        
        /// <summary>
        /// Get all recognizers model instances.
        /// </summary>
        /// <returns>A list of all the existing recognizer's models</returns>
        private static IEnumerable<IModel> GetParsers()
        {
            return new IModel[]
            {
                // Add Number recognizer - This recognizer will find any number from the input
                // E.g "I have two apples" will return "2".
                NumberRecognizer.Instance.GetNumberModel(defaultCulture),
                
                // Add Ordinal number recognizer - This recognizer will find any ordinal number
                // E.g "eleventh" will return "11".
                NumberRecognizer.Instance.GetOrdinalModel(defaultCulture),

                // Add Percentage recognizer - This recognizer will find any number presented as percentage
                // E.g "one hundred percents" will return "100%"
                NumberRecognizer.Instance.GetPercentageModel(defaultCulture),

                // Add Age recognizer - This recognizer will find any age number presented
                // E.g "After ninety five years of age, perspectives change" will return "95 Year"
                NumberWithUnitRecognizer.Instance.GetAgeModel(defaultCulture),

                // Add Currency recognizer - This recognizer will find any currency presented
                // E.g "Interest expense in the 1988 third quarter was $ 75.3 million" will return "75300000 Dollar"
                NumberWithUnitRecognizer.Instance.GetCurrencyModel(defaultCulture),

                // Add Dimension recognizer - This recognizer will find any dimension presented
                // E.g "The six-mile trip to my airport hotel that had taken 20 minutes earlier in the day took more than three hours." will return "6 Mile"
                NumberWithUnitRecognizer.Instance.GetDimensionModel(defaultCulture),

                // Add Temperature recognizer - This recognizer will find any temperature presented
                // E.g "Set the temperature to 30 degrees celsius" will return "30 C"
                NumberWithUnitRecognizer.Instance.GetTemperatureModel(defaultCulture),

                // Add Datetime recognizer - This model will find any Date even if its write in coloquial language - 
                // E.g "I'll go back 8pm today" will return "2017-10-04 20:00:00"
                DateTimeRecognizer.GetInstance().GetDateTimeModel(defaultCulture)
            };
        }
    }
}
