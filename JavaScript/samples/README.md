# JavaScript Samples

The best way to learn how to use something is through examples. The JavaScript project contains two simple samples to get you started.

## Simple Console ([source](./simple-console))

This sample demonstrate the combination of all Recognizers to extract possible values from the user's input.

First, install its dependencies:

````npm install````

And then start the sample:

````npm start````

The important piece is the `Recognizers` module, which you'll need to import using:

````JavaScript
var Recognizers = require('@microsoft/recognizers-text-suite');
````

Then, the sample gets a model reference of each available Recognizer. We need to do so by passing the Culture code we'll want to detect. E.g.: `en-us`.

So far, the available models are:

````JavaScript
// Use English for the Recognizers culture
var myCulture = Recognizers.Culture.English;

// Add Number recognizer - This recognizer will find any number from the input
// E.g "I have two apples" will return "2".
Recognizers.recognizeNumber(query, myCulture)

// Add Ordinal number recognizer - This recognizer will find any ordinal number
// E.g "eleventh" will return "11".
Recognizers.recognizeOrdinal(query, myCulture)

// Add Percentage recognizer - This recognizer will find any number presented as percentage
// E.g "one hundred percents" will return "100%"
Recognizers.recognizePercentage(query, myCulture)

// Add Age recognizer - This recognizer will find any age number presented
// E.g "After ninety five years of age, perspectives change" will return "95 Year"
Recognizers.recognizeAge(query, myCulture)

// Add Currency recognizer - This recognizer will find any currency presented
// E.g "Interest expense in the 1988 third quarter was $ 75.3 million" will return "75300000 Dollar"
Recognizers.recognizeCurrency(query, myCulture)

// Add Dimension recognizer - This recognizer will find any dimension presented
// E.g "The six-mile trip to my airport hotel that had taken 20 minutes earlier in the day took more than three hours." will return "6 Mile"
Recognizers.recognizeDimension(query, myCulture)

// Add Temperature recognizer - This recognizer will find any temperature presented
// E.g "Set the temperature to 30 degrees celsius" will return "30 C"
Recognizers.recognizeTemperature(query, myCulture)

// Add Datetime recognizer - This model will find any Date even if its write in coloquial language -
// E.g "I'll go back 8pm today" will return "2017-10-04 20:00:00"
Recognizers.recognizeDateTime(query, myCulture)

// Boolean recognizer - This function will find yes/no like responses, including emoji -
// E.g "yup, I need that" will return "True"
Recognizers.recognizeBoolean(query, myCulture)
````

All these models accept an input as a string and returns an **Array** of [ModelResult](../packages/recognizers-number/src/models.ts#L8-L14):

````JavaScript
var result = Recognizers.recognizeNumber("I have twenty apples");

// Returns:
// [
// 	{
// 		"start": 7,
// 		"end": 12,
// 		"resolution": {
// 			"value": "20"
// 		},
// 		"text": "twenty",
// 		"typeName": "number"
// 	}
// ]
````

## Browser Sample ([source](./browser))

The Recognizers package can also be used in a browser. So far, all major browsers are supported and IE11 can be supported using a Polyfill.

The Browser sample parses the user input and prints the results.
It allows to select the Recognizer type and also the culture.

To run the sample, first install its dependencies:

````npm install````

Run the sample's web server:

````npm start````

Open a browser to [http://localhost:8000/](http://localhost:8000/)

The sample works by referencing the browser bundle generated on build (located at `/dist/recognizers-text.browser.js`).

Recognizers Models can then be obtained from the global `RecognizersText` namespace:

````JavaScript
var result = Recognizers.recognizeDateTime("I need to leave ASAP", "en-us");

// Returns:
// [
// 	{
// 		"start": 16,
// 		"end": 19,
// 		"resolution": [
// 			{
// 				"timex": "FUTURE_REF",
// 				"type": "datetime",
// 				"value": "2017-10-12 18:13:44"
// 			}
// 		],
// 		"text": "asap",
// 		"typeName": "datetimeV2.datetime"
// 	}
// ]
````

> Alternativly, if you are using Browserify, Babel or Webpack, the UMD module can be used instead (located at `/dist/recognizers-text.umd.js`). This is the default when using `require('@microsoft/recognizers-text')`.

> NOTE for IE11: In order to support it, the [core-js shim](https://github.com/zloirock/core-js) needs to be added:

````HTML
<!-- IE11 Polyfill to support ES2015 features -->
<script type="text/javascript">
    if(/MSIE \d|Trident.*rv:/.test(navigator.userAgent))
        document.write('<script src="https://cdnjs.cloudflare.com/ajax/libs/core-js/2.5.1/shim.min.js"><\/script>');
</script>
````

## BotBuilder Sample ([source](./botbuilder))

This sample demonstrate how the Recognizers can be used with a BotBuilder Bot to parse user input. The bot provides a basic experience for ordering roses, it starts by asking the amount of roses and then asks for a delivery date and time.

First, install its dependencies:

````npm install````

Start the sample:

````npm start````

Then, launch the [BotFramework Emulator](https://github.com/Microsoft/BotFramework-Emulator/releases) and connect it to **http://127.0.0.1:3978/api/messages**.

Once connected, the bot will display a welcome message and ask for how many roses to deliver.

In order to validate user input, [Custom prompts](https://github.com/Microsoft/BotBuilder/issues/3129#issuecomment-315849557) are used, which can run custom validation logic using the [`onRecognizer` handler](https://docs.botframework.com/en-us/node/builder/chat-reference/classes/_botbuilder_d_.prompt.html#onrecognize). The following code creates a bot dialog that prompts the user for an integer number:

````JavaScript
// Ask for amount of roses and validate input
bot.dialog('ask-amount', new builder.Prompt().onRecognize((context, callback) => {
    var input = context.message.text || '';
    var results = Recognizers.recognizeNumber(input, Recognizers.Culture.English);

    // Care for the first result only
    if (results.length && results[0].typeName === 'number') {
        var first = results[0];
        var resolution = parseFloat(first.resolution.value);
        if (resolution % 1 === 0) {
            // no decimal part detected, good!
            return callback(null, 1, resolution);
        } else {
            // decimal part detected
            context.dialogData.options.prompt = 'I need to send whole roses, not fractions of them. How many would you like to send?';
        }
    } else {
        context.dialogData.options.prompt = 'I\'m sorry, that doesn\'t seem to be a valid quantity';
    }

    // return with score 0 to re-prompt
    callback(null, 0);
}));
````

Now, to call this dialog and later obtain its value, we need to invoke it throught a waterfall dialog, call the prompt in the first function and obtain the value in the next function:

````JavaScript
var bot = new builder.UniversalBot(connector, [
    function (session) {
        // Welcome message
        // ...

        // Prompt for amount of roses
        var promptMessage = [
            'How many roses do you want to send?',
            'Some valid options are:',
            ' - A dozen',
            ' - 22',
            ' - Just one rose'];

        session.beginDialog('ask-amount', { prompt: promptMessage.join('\n\n') });
    },

    function (session, results) {
        // results.response contains the prompt returned value
        var amount = results.response;

        var amountMsg = session.ngettext(`I'll send just one rose.`, `I'll send ${amount} roses.`, amount);
        session.send(`Great! ${amountMsg}`);

        // ...
        session.endDialog();
    }
]);
````

Asking the user for a specific delivery time may require special parsing, like extracting both date and time from the user input, or even obtain a range of dates and times.

The [`ask-date` dialog](./botbuilder/index.js#L118-L132) does exactly that. It will prompt the user for a possible delivery time, parse the user's input and extract, at least, one of these avaliable return values using the DateTime Recognizer:

 - date
 - daterange
 - datetime
 - datetimerange

(These are the DateTime Recognizer types that contains *date* information)

> NOTE: The DateTime Recognizer uses LUIS datetimeV2 subtypes. For a full list, please visit [LUIS prebuilt entities - Subtypes of datetimeV2](https://docs.microsoft.com/en-us/azure/cognitive-services/luis/pre-builtentities#subtypes-of-datetimev2).

This dialog uses a helper method to call the DateTime Recognizer, to validate the subtype and check the selected delivery moment is at least one hour from now:

````JavaScript
// Date Helpers
function validateAndExtract(input) {

    var results = Recognizers.recognizeDateTime(input, Recognizers.Culture.English);

    // Check there are valid results
    if (results.length && results[0].typeName.startsWith('datetimeV2')) {
        // The DateTime model can return several resolution types (https://github.com/Microsoft/Recognizers-Text/blob/master/JavaScript/recognizers-date-time/src/dateTime/constants.ts#L2-L9)
        // We only care for those with a date, date and time, or date time period:
        // date, daterange, datetime, datetimerange

        var first = results[0];
        var subType = first.typeName.split('.')[1];
        var resolutionValues = first.resolution && first.resolution.values;

        if (!resolutionValues) {
            // no resolution values
            return {
                valid: false
            }
        }

        if (subType.includes('date') && !subType.includes('range')) {
            // a date (or date & time) or multiple
            var moments = resolutionValues.map(m => new Date(m.value));
            var moment = moments.find(isFuture) || moments[0];              // Look for the first future moment; default to first resolution
            if (isFuture(moment)) {
                // a future moment, valid!
                return {
                    valid: true,
                    value: moment
                };
            }

            // a past moment
            return {
                valid: false,
                error: 'past_value',
                value: moment,
            }
        } else if (subType.includes('date') && subType.includes('range')) {
            // range
            var from = new Date(resolutionValues[0].start);
            var to = new Date(resolutionValues[0].end);
            if (!isNaN(from.getTime()) && !isNaN(to.getTime())) {
                if (isFuture(from) && isFuture(to)) {
                    // future
                    return {
                        valid: true,
                        value: [from, to]
                    };
                }

                // past
                return {
                    valid: false,
                    error: 'past_value',
                    value: [from, to]
                };
            }
        }
    }

    return {
        valid: false
    };
}

function isFuture(date) {
    // at least one hour
    var anHour = 1000 * 60 * 60;
    return date.getTime() > (Date.now() + anHour);
}
````

We use the helper function from the [`ask-date` custom prompt dialog](./botbuilder/index.js#L118-L132):

````JavaScript
var DateValidationErros = {
    'past_value': 'I\'m sorry, but I need at least an hour to deliver.\n\n $moment$ is no good for me.\n\nWhat other moment suits you best?',
    'default': 'I\'m sorry, that doesn\'t seem to be a valid delivery date and time'
};

// Ask for delivery date and validate input
bot.dialog('ask-date', new builder.Prompt().onRecognize((context, callback) => {

    var result = validateAndExtract(context.message.text || '');
    if (result.valid) {
        // return value to calling dialog
        return callback(null, 1, result.value);
    }

    // Set error message and re-prompt;
    var errorTemplate = DateValidationErros[result.error] || DateValidationErros.default;
    context.dialogData.options.prompt = errorTemplate.replace('$moment$', momentOrRangeToString(result.value, ''));
    callback(null, 0);
}));

function momentOrRangeToString(moment, momentPrefix) {
    momentPrefix = momentPrefix !== undefined ? momentPrefix : 'on ';
    if (_.isDate(moment)) {
        return momentPrefix + moment.toLocaleString('en-US');
    } else if (_.isArray(moment)) {
        return 'from ' + moment.map(m => momentOrRangeToString(m, '')).join(' to ');
    }

    return 'not supported';
}
````

Finally, this is how you call the dialog and obtain the date (or dates) back:

````JavaScript
var bot = new builder.UniversalBot(connector, [
    function (session) {
        // Prompt for delivery date
        var promptMessage = [
            'When do you want to receive the delivery?',
            'Some valid options are:',
            ' - Tomorrow morning',
            ' - 12/30/2017',
            ' - 9PM Tomorrow',
            ' - Five hours from now',
            ' - From 9AM to 10AM tomorrow'];
        session.beginDialog('ask-date', { prompt: promptMessage.join('\n\n') });
    },

    function (session, results) {
        // results.response contains the date (or array of dates) returned from the prompt
        var momentOrRange = results.response;

        session.send(`Thank you! I'll deliver ${momentOrRangeToString(momentOrRange)}.`);

        // TODO: It should continue to a checkout dialog or page
        session.send('Have a nice day!');
        session.endDialog();
    }
]);
````
