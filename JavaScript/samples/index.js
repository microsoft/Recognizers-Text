var recognizersDateTime = require('recognizers-text-date-time')
var recognizersNumberWithUnit = require('recognizers-text-number-with-unit')
var recognizersNumber = require('recognizers-text-number')
var helpers = require('./helpers');

// Use English for the Recognizers culture
const defaultCulture = recognizersNumber.Culture.English;

// Show Introduction
function ShowIntro() {
    console.log("Welcome to the Recognizer's Sample console application!");
    console.log("To try the recognizers enter a phrase and let us show you the different outputs for each recognizer or just type 'exit' to leave the application.");
    console.log('');
    console.log("Here are some examples you could try:");
    console.log('');
    console.log("\" I want twenty meters of cable for tomorrow \"");
    console.log("\" I'll be available tomorrow from 11am to 2pm to receive up to 5kg of sugar\"");
    console.log("\" I'll be out between 4 and 22 this month \"");
    console.log("\" I was the fifth person to finish the 10 km race \"");
    console.log("\" The temperature this night will be of 40 deg celsius \"");
    console.log("\" The american stock exchange said a seat was sold for down $ 5,000 from the previous sale last friday \"");
    console.log("\" It happened when the baby was only ten months old \"");
    console.log('');
}

// Get all recognizers model instances.
function GetModels() {
    return [
        // Add Number recognizer - This recognizer will find any number from the input
        // E.g "I have two apples" will return "2".
        recognizersNumber.NumberRecognizer.instance.getNumberModel(defaultCulture),

        // Add Ordinal number recognizer - This recognizer will find any ordinal number
        // E.g "eleventh" will return "11".
        recognizersNumber.NumberRecognizer.instance.getOrdinalModel(defaultCulture),

        // Add Percentage recognizer - This recognizer will find any number presented as percentage
        // E.g "one hundred percents" will return "100%"
        recognizersNumber.NumberRecognizer.instance.getPercentageModel(defaultCulture),

        // Add Age recognizer - This recognizer will find any age number presented
        // E.g "After ninety five years of age, perspectives change" will return "95 Year"
        recognizersNumberWithUnit.NumberWithUnitRecognizer.instance.getAgeModel(defaultCulture),

        // Add Currency recognizer - This recognizer will find any currency presented
        // E.g "Interest expense in the 1988 third quarter was $ 75.3 million" will return "75300000 Dollar"
        recognizersNumberWithUnit.NumberWithUnitRecognizer.instance.getCurrencyModel(defaultCulture),

        // Add Dimension recognizer - This recognizer will find any dimension presented
        // E.g "The six-mile trip to my airport hotel that had taken 20 minutes earlier in the day took more than three hours." will return "6 Mile"
        recognizersNumberWithUnit.NumberWithUnitRecognizer.instance.getDimensionModel(defaultCulture),

        // Add Temperature recognizer - This recognizer will find any temperature presented
        // E.g "Set the temperature to 30 degrees celsius" will return "30 C"
        recognizersNumberWithUnit.NumberWithUnitRecognizer.instance.getTemperatureModel(defaultCulture),

        // Add Datetime recognizer - This model will find any Date even if its write in coloquial language - 
        // E.g "I'll go back 8pm today" will return "2017-10-04 20:00:00"
        recognizersDateTime.DateTimeRecognizer.instance.getDateTimeModel(defaultCulture)
    ]
}

// Read and write from Console
function RunRecognition() {
    var stdin = process.openStdin();
    // Read the text to recognize
    process.stdout.write('Enter the text to recognize: ');

    stdin.addListener('data', function (e) {
        var input = e.toString().trim();

        // Validate input 
        if (input && input.length > 0) {
            // exit
            if (input.toLowerCase() === 'exit') {
                return process.exit();
            } else {
                // Retrieve all the parsers and call 'parse' to recognize all the values from the user input
                var results = GetModels().map(function (model) {
                        // Recognize values from the user input
                        var result = model.parse(input);

                        // Format resolution for DateTimes
                        result.forEach(r => {
                            if (!r.resolution || !r.resolution.get) return;
                            r.resolution = r.resolution.get('values').map(helpers.toObject);
                        });

                        return result;
                    })
                    .filter(
                        ele => ele.length > 0
                    );

                // Write results on console
                console.log('');
                console.log(results.length > 0 ? "I found the following entities (" + results.length + ")" : "I found no entities");
                console.log('');
                [].concat.apply([], results).forEach(function (result) {
                    process.stdout.write(JSON.stringify(result, null, "\t"));
                });
                console.log('');
            }

        }

        // Read the text to recognize
        process.stdout.write('\nEnter the text to recognize: ');
    });
}

// Start Sample

ShowIntro();

RunRecognition();