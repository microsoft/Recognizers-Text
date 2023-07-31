# Microsoft.Recognizers.Text for JavaScript

## Getting Started

Recognizers are organized into groups and designed to be used in C#, Node.js, Python and Java to help you build great applications! To use the samples, install the `recognizers-text-suite` package, or  clone our GitHub repository using Git.

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

* Get **only** the sequence Recognizer's features:
`npm install @microsoft/recognizers-text-sequence`

* Get **only** the choice Recognizer's features:
`npm install @microsoft/recognizers-text-choice`

## API Documentation

Once the proper package is installed, you'll need to reference the package:

````JavaScript
var Recognizers = require('@microsoft/recognizers-text-suite');
var NumberRecognizers = require('@microsoft/recognizers-text-number');
var NumberWithUnitRecognizers = require('@microsoft/recognizers-text-number-with-unit');
var DateTimeRecognizers = require('@microsoft/recognizers-text-date-time');
var SequenceRecognizers = require('@microsoft/recognizers-text-sequence');
var ChoiceRecognizers = require('@microsoft/recognizers-text-choice');
````

### Recognizer's Models

This is the preferred way if you need to parse multiple inputs based on the same context (e.g.: language and options):

```JavaScript
var recognizer = new NumberRecognizers.NumberRecognizer(Recognizers.Culture.English);
var model = recognizer.getNumberModel();
var result = model.parse('Twelve');
```

Or, for less verbosity, you use the helper methods:

`var result = Recognizers.recognizeNumber("Twelve", Recognizers.Culture.English);`

Internally, both methods will cache the instance models to avoid extra costs.

### Microsoft.Recognizers.Text.Number

* **Numbers**

    This recognizer will find any number from the input. E.g. _"I have two apples"_ will return _"2"_.

    `Recognizers.recognizeNumber('I have two apples', Recognizers.Culture.English)`

    Or you can obtain a model instance using:

    `new NumberRecognizers.NumberRecognizer(Recognizers.Culture.English).getNumberModel()`


* **Ordinal Numbers**

    This recognizer will find any ordinal number. E.g. _"eleventh"_ will return _"11"_.

    `Recognizers.recognizeOrdinal('eleventh', Recognizers.Culture.English)`

    Or you can obtain a model instance using:

    `new NumberRecognizers.NumberRecognizer(Recognizers.Culture.English).getOrdinalModel()`


* **Percentages**

    This recognizer will find any number presented as percentage. E.g. _"one hundred percents"_ will return _"100%"_.

    `Recognizers.recognizePercentage('one hundred percents', Recognizers.Culture.English)`

    Or you can obtain a model instance using:

    `new NumberRecognizers.NumberRecognizer(Recognizers.Culture.English).getPercentageModel()`

### Microsoft.Recognizers.Text.NumberWithUnit

* **Ages**

    This recognizer will find any age number presented. E.g. _"After ninety five years of age, perspectives change"_ will return _"95 Year"_.

    `Recognizers.recognizeAge('After ninety five years of age, perspectives change', Recognizers.Culture.English)`

    Or you can obtain a model instance using:

    `new NumberWithUnitRecognizers.NumberWithUnitRecognizer(Recognizers.Culture.English).getAgeModel()`


* **Currencies**

    This recognizer will find any currency presented. E.g. _"Interest expense in the 1988 third quarter was $ 75.3 million"_ will return _"75300000 Dollar"_.

    `Recognizers.recognizeCurrency('Interest expense in the 1988 third quarter was $ 75.3 million', Recognizers.Culture.English)`

    Or you can obtain a model instance using:

    `new NumberWithUnitRecognizers.NumberWithUnitRecognizer(Recognizers.Culture.English).getCurrencyModel()`


* **Dimensions**

    This recognizer will find any dimension presented. E.g. _"The six-mile trip to my airport hotel that had taken 20 minutes earlier in the day took more than three hours."_ will return _"6 Mile"_.

    `Recognizers.recognizeDimension('The six-mile trip to my airport hotel that had taken 20 minutes earlier in the day took more than three hours.', Recognizers.Culture.English)`

    Or you can obtain a model instance using:

    `new NumberWithUnitRecognizers.NumberWithUnitRecognizer(Recognizers.Culture.English).getDimensionModel()`


* **Temperatures**

    This recognizer will find any temperature presented. E.g. _"Set the temperature to 30 degrees celsius"_ will return _"30 C"_.

    `Recognizers.recognizeTemperature('Set the temperature to 30 degrees celsius', Recognizers.Culture.English)`

    Or you can obtain a model instance using:

    `new NumberWithUnitRecognizers.NumberWithUnitRecognizer(Recognizers.Culture.English).getTemperatureModel()`


### Microsoft.Recognizers.Text.DateTime

* **DateTime**

    This recognizer will find any date, time, duration and date/time ranges, even if its write in colloquial language. E.g. _"I'll go back 8pm today"_ will return _"2017-10-04 20:00:00"_.

    `Recognizers.recognizeDateTime("I'll go back 8pm today", Recognizers.Culture.English)`

    Or you can obtain a model instance using:

    `new DateTimeRecognizers.DateTimeRecognizer(Recognizers.Culture.English).getDateTimeModel()`

### Microsoft.Recognizers.Text.Sequence

* **Phone Numbers**

    This model will find any patter of symbols detected as a phone number, even if its write in coloquial language. E.g. "My phone number is 1 (877) 609-2233." will return "1 (877) 609-2233".

    `Recognizers.recognizePhoneNumber("My phone number is 1 (877) 609-2233.", Culture.English)`

    Or you can obtain a model instance using:

    `new SequenceRecognizers.SequenceRecognizer(Culture.English).GetPhoneNumberModel()`

* **IP Address**

    This model will find any Ipv4/Ipv6 presented. 
    E.g. "My Ip is 8.8.8.8".

    `Recognizers.recognizeIpAddress("My Ip is 8.8.8.8")`

    Or you can obtain a model instance using:

    `new SequenceRecognizers.SequenceRecognizer(Culture.English).IpAddressModel()`

### Microsoft.Recognizers.Text.Choice

* **Booleans**

    This recognizer will find any boolean value, even if its write with emoji. 
    E.g. _"👌 It's ok"_ will return _"true"_.

    `Recognizers.recognizeBoolean("👌 It's ok", Recognizers.Culture.English)`

    Or you can obtain a model instance using:

    `new ChoiceRecognizers.ChoiceRecognizer(Recognizers.Culture.English).getBooleanModel()`

## Samples

[Start using recognizers!](https://github.com/Microsoft/Recognizers-Text/tree/master/JavaScript/samples)

## Integration tips

The recognizers were designed to disjoint language's logic from the recognizer's core in order to grow without the obligation of change the supported platforms.

To achieve this, the recognizers contains the following folders:

* [Specs](https://github.com/Microsoft/Recognizers-Text/tree/master/Specs) - Contains all the necessary tests that should be run on any improvements to the recognizers. It's divided by recognizer and supported language.
* [Patterns](https://github.com/Microsoft/Recognizers-Text/tree/master/Patterns)  - Contains all the regular expressions that fulfill the recognizers logic. It's divided by supported language.
