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
var Recognizers = require('recognizers-text');
````

Then, the sample gets a model reference of each available Recognizer. We need to do so by passing the Culture code we'll want to detect. E.g.: `en-us`.

So far, the available models are:

````JavaScript
// Use English for the Recognizers culture
var myCulture = Recognizers.Culture.English;

// Add Number recognizer - This recognizer will find any number from the input
// E.g "I have two apples" will return "2".
Recognizers.NumberRecognizer.instance.getNumberModel(myCulture)

// Add Ordinal number recognizer - This recognizer will find any ordinal number
// E.g "eleventh" will return "11".
Recognizers.NumberRecognizer.instance.getOrdinalModel(myCulture)

// Add Percentage recognizer - This recognizer will find any number presented as percentage
// E.g "one hundred percents" will return "100%"
Recognizers.NumberRecognizer.instance.getPercentageModel(myCulture)

// Add Age recognizer - This recognizer will find any age number presented
// E.g "After ninety five years of age, perspectives change" will return "95 Year"
Recognizers.NumberWithUnitRecognizer.instance.getAgeModel(myCulture)

// Add Currency recognizer - This recognizer will find any currency presented
// E.g "Interest expense in the 1988 third quarter was $ 75.3 million" will return "75300000 Dollar"
Recognizers.NumberWithUnitRecognizer.instance.getCurrencyModel(myCulture)

// Add Dimension recognizer - This recognizer will find any dimension presented
// E.g "The six-mile trip to my airport hotel that had taken 20 minutes earlier in the day took more than three hours." will return "6 Mile"
Recognizers.NumberWithUnitRecognizer.instance.getDimensionModel(myCulture)

// Add Temperature recognizer - This recognizer will find any temperature presented
// E.g "Set the temperature to 30 degrees celsius" will return "30 C"
Recognizers.NumberWithUnitRecognizer.instance.getTemperatureModel(myCulture)

// Add Datetime recognizer - This model will find any Date even if its write in coloquial language -
// E.g "I'll go back 8pm today" will return "2017-10-04 20:00:00"
Recognizers.DateTimeRecognizer.instance.getDateTimeModel(myCulture)
````

All these models accept an input as a string and returns an **Array** of [ModelResult](../recognizers-number/src/models.ts#L8-L14):

````JavaScript
// Number model
var model = Recognizers.NumberRecognizer.instance.getNumberModel("en-us");

// Parse input using Number model
var result = model.parse("I have twenty apples");

// result is:
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

The Recognizers package can also be used in the browser. So far, all mayor browsers are supported and IE11 can be supported using a Polyfill.

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
var model = RecognizersText.DateTimeRecognizer.instance.getDateTimeModel("en-us");
var result = model.parse("I need to leave ASAP");

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

> Alternativly, if you are using Browserify, Babel or Webpack, the UMD module can be used instead (located at `/dist/recognizers-text.umd.js`).

> NOTE for IE11: In order to support it, the [core-js shim](https://github.com/zloirock/core-js) needs to be added:

````HTML
<!-- IE11 Polyfill to support ES2015 features -->
<script type="text/javascript">
    if(/MSIE \d|Trident.*rv:/.test(navigator.userAgent))
        document.write('<script src="https://cdnjs.cloudflare.com/ajax/libs/core-js/2.5.1/shim.min.js"><\/script>');
</script>
````