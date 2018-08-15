package com.microsoft.recognizers.text.samples.simpleconsole;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.fasterxml.jackson.databind.SerializationFeature;
import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.ModelResult;
import com.microsoft.recognizers.text.number.NumberRecognizer;
import com.microsoft.recognizers.text.numberwithunit.NumberWithUnitRecognizer;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.util.Arrays;
import java.util.List;
import java.util.stream.Collectors;

public class Sample {

    private static final String defaultCulture = Culture.English;

    public static void main(String[] args) throws Exception {
        showIntro();

        while (true) {
            // Read the text to recognize
            System.out.println("Enter the text to recognize:");

            BufferedReader reader = new BufferedReader(new InputStreamReader(System.in));
            String input = reader.readLine().trim();
            System.out.println();

            // Prepare json serializer for printing results
            ObjectMapper mapper = new ObjectMapper();
            mapper.enable(SerializationFeature.INDENT_OUTPUT);

            if (input.toLowerCase() == "exit") {
                // Close application if user types "exit"
                break;
            } else {
                // Validate input 
                if (input.length() > 0) {
                    // Retrieve all the parsers and call 'Parse' to recognize all the values from the user input
                    List<ModelResult> results = parseAll(input, defaultCulture);

                    // Write output
                    System.out.println(results.size() > 0 ? (String.format("I found the following entities (%s):", results.size())) : "I found no entities.");
                    results.forEach(r -> {
                        try {
                            System.out.println(mapper.writeValueAsString(r));
                        } catch (JsonProcessingException e) {
                            e.printStackTrace();
                        }
                    });
                    System.out.println();
                }
            }
        }
    }

    private static List<ModelResult> parseAll(final String query, final String culture) {
        return mergeResults(
                // Number recognizer will find any number from the input
                // E.g "I have two apples" will return "2".
                NumberRecognizer.recognizeNumber(query, culture),

                // Ordinal number recognizer will find any ordinal number
                // E.g "eleventh" will return "11".
                NumberRecognizer.recognizeOrdinal(query, culture),

                // Percentage recognizer will find any number presented as percentage
                // E.g "one hundred percents" will return "100%"
                NumberRecognizer.recognizePercentage(query, culture),

                // Number Range recognizer will find any cardinal or ordinal number range
                // E.g. "between 2 and 5" will return "(2,5)"
                NumberRecognizer.recognizeNumberRange(query, culture),

                // Age recognizer will find any age number presented
                // E.g "After ninety five years of age, perspectives change" will return "95 Year"
                NumberWithUnitRecognizer.recognizeAge(query, culture),

                // Currency recognizer will find any currency presented
                // E.g "Interest expense in the 1988 third quarter was $ 75.3 million" will return "75300000 Dollar"
                NumberWithUnitRecognizer.recognizeCurrency(query, culture),

                // Dimension recognizer will find any dimension presented
                // E.g "The six-mile trip to my airport hotel that had taken 20 minutes earlier in the day took more than three hours." will return "6 Mile"
                NumberWithUnitRecognizer.recognizeDimension(query, culture),

                // Temperature recognizer will find any temperature presented
                // E.g "Set the temperature to 30 degrees celsius" will return "30 C"
                NumberWithUnitRecognizer.recognizeTemperature(query, culture)
        );
    }

    private static List<ModelResult> mergeResults(List<ModelResult>... results) {
        return Arrays.stream(results).flatMap(r -> r.stream()).collect(Collectors.toList());
    }

    private static void showIntro() {
        System.out.println("Welcome to the Recognizer's Sample console application!");
        System.out.println("To try the recognizers enter a phrase and let us show you the different outputs for each recognizer or just type 'exit' to leave the application.");
        System.out.println();
        System.out.println("Here are some examples you could try:");
        System.out.println();
        System.out.println("\" I want twenty meters of cable for tomorrow\"");
        System.out.println("\" I'll be available tomorrow from 11am to 2pm to receive up to 5kg of sugar\"");
        System.out.println("\" I'll be out between 4 and 22 this month\"");
        System.out.println("\" I was the fifth person to finish the 10 km race\"");
        System.out.println("\" The temperature this night will be of 40 deg celsius\"");
        System.out.println("\" The american stock exchange said a seat was sold for down $ 5,000 from the previous sale last friday\"");
        System.out.println("\" It happened when the baby was only ten months old\"");
        System.out.println();
    }
}
