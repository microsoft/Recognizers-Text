var RecognizersDateTime = require('recognizers-text-date-time');
var RecognizersNumberWithUnit = require('recognizers-text-number-with-unit');
var RecognizersNumber = require('recognizers-text-number');
var helpers = require('./helpers');

// Use English for the Recognizers culture
const defaultCulture = RecognizersNumber.Culture.English;

// Start Sample
showIntro();
runRecognition();

// Read from Console and recognize
function runRecognition() {
    var stdin = process.openStdin();

    // Read the text to recognize
    write('Enter the text to recognize: ');

    stdin.addListener('data', function (e) {
        var input = e.toString().trim();
        if (input) {
            // Exit
            if (input.toLowerCase() === 'exit') {
                return process.exit();
            } else {
                // Retrieve all the parsers and call 'parse' to recognize all the values from the user input
                var results = getModels().map(function (model) {
                        // Recognize values from the user input
                        var result = model.parse(input);

                        // Format resolution for DateTimes
                        result.forEach(r => {
                            if (!r.resolution || !r.resolution.get) return;
                            r.resolution = r.resolution.get('values').map(helpers.toObject);
                        });

                        return result;
                    });

                // Write results on console
                write();
                write(results.length > 0 ? "I found the following entities (" + results.length + "):" : "I found no entities.");
                write();
                [].concat.apply([], results).forEach(function (result) {
                    write(JSON.stringify(result, null, "\t"));
                    write();
                });
                write();
            }

        }

        // Read the text to recognize
        write('\nEnter the text to recognize: ');
    });
}

// Write on console
function write(message = ""){
    process.stdout.write(message + "\n");
}

// Get all recognizers model instances.
function getModels() {
    return [
        // Add Number recognizer - This recognizer will find any number from the input
        // E.g "I have two apples" will return "2".
        RecognizersNumber.NumberRecognizer.instance.getNumberModel(defaultCulture),

        // Add Ordinal number recognizer - This recognizer will find any ordinal number
        // E.g "eleventh" will return "11".
        RecognizersNumber.NumberRecognizer.instance.getOrdinalModel(defaultCulture),

        // Add Percentage recognizer - This recognizer will find any number presented as percentage
        // E.g "one hundred percents" will return "100%"
        RecognizersNumber.NumberRecognizer.instance.getPercentageModel(defaultCulture),

        // Add Age recognizer - This recognizer will find any age number presented
        // E.g "After ninety five years of age, perspectives change" will return "95 Year"
        RecognizersNumberWithUnit.NumberWithUnitRecognizer.instance.getAgeModel(defaultCulture),

        // Add Currency recognizer - This recognizer will find any currency presented
        // E.g "Interest expense in the 1988 third quarter was $ 75.3 million" will return "75300000 Dollar"
        RecognizersNumberWithUnit.NumberWithUnitRecognizer.instance.getCurrencyModel(defaultCulture),

        // Add Dimension recognizer - This recognizer will find any dimension presented
        // E.g "The six-mile trip to my airport hotel that had taken 20 minutes earlier in the day took more than three hours." will return "6 Mile"
        RecognizersNumberWithUnit.NumberWithUnitRecognizer.instance.getDimensionModel(defaultCulture),

        // Add Temperature recognizer - This recognizer will find any temperature presented
        // E.g "Set the temperature to 30 degrees celsius" will return "30 C"
        RecognizersNumberWithUnit.NumberWithUnitRecognizer.instance.getTemperatureModel(defaultCulture),

        // Add Datetime recognizer - This model will find any Date even if its write in coloquial language -
        // E.g "I'll go back 8pm today" will return "2017-10-04 20:00:00"
        RecognizersDateTime.DateTimeRecognizer.instance.getDateTimeModel(defaultCulture)
    ];
}

// Show Introduction
function showIntro() {
    write("Welcome to the Recognizer's Sample console application!");
    write("To try the recognizers enter a phrase and let us show you the different outputs for each recognizer or just type 'exit' to leave the application.");
    write();
    write("Here are some examples you could try:");
    write();
    write("\" I want twenty meters of cable for tomorrow\"");
    write("\" I'll be available tomorrow from 11am to 2pm to receive up to 5kg of sugar\"");
    write("\" I'll be out between 4 and 22 this month\"");
    write("\" I was the fifth person to finish the 10 km race\"");
    write("\" The temperature this night will be of 40 deg celsius\"");
    write("\" The american stock exchange said a seat was sold for down $ 5,000 from the previous sale last friday\"");
    write("\" It happened when the baby was only ten months old\"");
    write();
}