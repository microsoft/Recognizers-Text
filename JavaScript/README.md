# Microsoft.Recognizers.Text for JavaScript

## Getting Started

Recognizer's are organized into groups and designed to be used in C# and Node.js to help you build great applications! To use the samples, install the `recognizers-text-suite` package, or  clone our GitHub repository using Git.

## Cloning and building the Repository

    git clone https://github.com/Microsoft/Recognizers-Text.git
    cd Recognizers-Text

You can choose between build the solution manually or through an automatized build.cmd file.

### Manual Build

Open a terminal and run the following commands:

    cd JavaScript
    npm install
    npm run build
    npm run test

### Automatized Build

Launch `Build.cmd` file.

## Installation from NPM

-- **Note:** the initial versions of the packages are available with the **@preview** tag. --

Install all the Recognizer's by launching the following command:

* Get **all** the Recognizers in a single module:
`npm install @microsoft/recognizers-text-suite`

Or, if you prefer to use a single type of recognizer:

* Get **only** the numbers Recognizer's features:
`npm install @microsoft/recognizers-text-number`

* Get **only** the numbers with units Recognizer's features:
`npm install @microsoft/recognizers-text-number-with-unit`

* Get **only** the date and time Recognizer's features:
`npm install @microsoft/recognizers-text-date-time`

* Get **only** the options (like boolean) Recognizer's features:
`npm install @microsoft/recognizers-text-options`

## API Documentation

Once the proper package is installed, you'll need to reference the package:

````JavaScript
var Recognizers = require('@microsoft/recognizers-text-suite');
var NumberRecognizers = require('@microsoft/recognizers-text-number');
var NumberWithUnitRecognizers = require('@microsoft/recognizers-text-number-with-unit');
var DateTimeRecognizers = require('@microsoft/recognizers-text-date-time');
var OptionsRecognizers = require('@microsoft/recognizers-text-options');
````

### Microsoft.Recognizers.Text.Number

* [NumberModel](https://github.com/Microsoft/Recognizers-Text/tree/master/JavaScript/packages/recognizers-number/src/number/numberRecognizer.ts)

    This recognizer will find any number from the input. E.g. _"I have two apples"_ will return _"2"_.

    `new NumberRecognizers(Recognizers.Culture.English).getNumberModel()`

    Optionally, you can use the `recognizeNumber` method to parse your input with the same result.

    `Recognizers.recognizeNumber('I have two apples', Recognizers.Culture.English)`

* [OrdinalModel](https://github.com/Microsoft/Recognizers-Text/tree/master/JavaScript/packages/recognizers-number/src/number/numberRecognizer.ts)

    This recognizer will find any ordinal number. E.g. _"eleventh"_ will return _"11"_.

    `new NumberRecognizers(Recognizers.Culture.English).getOrdinalModel()`

    Optionally, you can use the `recognizeOrdinal` method to parse your input with the same result.

    `Recognizers.recognizeOrdinal('eleventh', Recognizers.Culture.English)`

* [PercentageModel](https://github.com/Microsoft/Recognizers-Text/tree/master/JavaScript/packages/recognizers-number/src/number/numberRecognizer.ts)

    This recognizer will find any number presented as percentage. E.g. _"one hundred percents"_ will return _"100%"_.

    `new NumberRecognizers(Recognizers.Culture.English).getPercentageModel()`

    Optionally, you can use the `recognizePercentage` method to parse your input with the same result.

    `Recognizers.recognizePercentage('one hundred percents', Recognizers.Culture.English)`

### Microsoft.Recognizers.Text.NumberWithUnit

* [AgeModel](https://github.com/Microsoft/Recognizers-Text/tree/master/JavaScript/packages/recognizers-number-with-unit/src/numberWithUnit/numberWithUnitRecognizer.ts)

    This recognizer will find any age number presented. E.g. _"After ninety five years of age, perspectives change"_ will return _"95 Year"_.

    `new NumberWithUnitRecognizers(Recognizers.Culture.English).getAgeModel()`

    Optionally, you can use the `recognizeAge` method to parse your input with the same result.

    `Recognizers.recognizeAge('After ninety five years of age, perspectives change', Recognizers.Culture.English)`

* [CurrencyModel](https://github.com/Microsoft/Recognizers-Text/tree/master/JavaScript/packages/recognizers-number-with-unit/src/numberWithUnit/numberWithUnitRecognizer.ts)

    This recognizer will find any currency presented. E.g. _"Interest expense in the 1988 third quarter was $ 75.3 million"_ will return _"75300000 Dollar"_.

    `new NumberWithUnitRecognizers(Recognizers.Culture.English).getCurrencyModel()`

    Optionally, you can use the `recognizeCurrency` method to parse your input with the same result.

    `Recognizers.recognizeCurrency('Interest expense in the 1988 third quarter was $ 75.3 million', Recognizers.Culture.English)`

* [DimensionModel](https://github.com/Microsoft/Recognizers-Text/tree/master/JavaScript/packages/recognizers-number-with-unit/src/numberWithUnit/numberWithUnitRecognizer.ts)

    This recognizer will find any dimension presented. E.g. _"The six-mile trip to my airport hotel that had taken 20 minutes earlier in the day took more than three hours."_ will return _"6 Mile"_.

    `new NumberWithUnitRecognizers(Recognizers.Culture.English).getDimensionModel()`

    Optionally, you can use the `recognizeDimension` method to parse your input with the same result.

    `Recognizers.recognizeDimension('The six-mile trip to my airport hotel that had taken 20 minutes earlier in the day took more than three hours.', Recognizers.Culture.English)`

* [TemperatureModel](https://github.com/Microsoft/Recognizers-Text/tree/master/JavaScript/packages/recognizers-number-with-unit/src/numberWithUnit/numberWithUnitRecognizer.ts)

    This recognizer will find any temperature presented. E.g. _"Set the temperature to 30 degrees celsius"_ will return _"30 C"_.

    `new NumberWithUnitRecognizers(Recognizers.Culture.English).getTemperatureModel()`

    Optionally, you can use the `recognizeTemperature` method to parse your input with the same result.

    `Recognizers.recognizeTemperature('Set the temperature to 30 degrees celsius', Recognizers.Culture.English)`

### Microsoft.Recognizers.Text.DateTime

* [DateTimeModel](https://github.com/Microsoft/Recognizers-Text/tree/master/JavaScript/packages/recognizers-date-time/src/dateTime/dateTimeRecognizer.ts)

    This model will find any date, time, duration and date/time ranges, even if its write in colloquial language. E.g. _"I'll go back 8pm today"_ will return _"2017-10-04 20:00:00"_.

    `new DateTimeRecognizers(Recognizers.Culture.English).getDateTimeModel()`

    Optionally, you can use the `recognizeDateTime` method to parse your input with the same result.

    `Recognizers.recognizeDateTime("I'll go back 8pm today", Recognizers.Culture.English)`

### Microsoft.Recognizers.Text.Options

* [BooleanModel](https://github.com/Microsoft/Recognizers-Text/tree/master/JavaScript/packages/recognizers-options/src/options/optionsRecognizer.ts)

    This model will find any boolean value, even if its write with emoji. E.g. _"👌 It's ok"_ will return _"true"_.

    `new OptionsRecognizers(Recognizers.Culture.English).getBooleanModel()`

    Optionally, you can use the `recognizeBoolean` method to parse your input with the same result.

    `Recognizers.recognizeBoolean("👌 It's ok", Recognizers.Culture.English)`

## Samples

[Start using recognizers!](https://github.com/Microsoft/Recognizers-Text/tree/master/JavaScript/samples)

## Integration tips

The recognizers were designed to disjoint language's logic from the recognizer's core in order to grow without the obligation of change the supported platforms.

To achieve this, the recognizers contains the following folders:

* [Specs](https://github.com/Microsoft/Recognizers-Text/tree/master/Specs) - Contains all the necessary tests that should be run on any improvements to the recognizers. It's divided by recognizer and supported language.
* [Patterns](https://github.com/Microsoft/Recognizers-Text/tree/master/Patterns)  - Contains all the regular expressions that fulfill the recognizers logic. It's divided by supported language.
