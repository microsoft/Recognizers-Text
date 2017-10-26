# Microsoft.Recognizers.Text for JavaScript

## Getting Started

Recognizer's are organized into groups and designed to be used in C# and Node.js to help you build great applications! To use the samples clone our GitHub repository using Git.

    git clone https://github.com/southworkscom/Recognizers-Text.git
    cd Recognizers-Text

## Setup

You can choose between build the solution manually or through an automatized build.cmd file.

### Manual Build
Open terminal and launch

    npm run prebuild-all
    npm run build
    npm run test

### Automatized Build
Launch `Build.cmd` file.

## Installation

Install Recognizer's by launching the following commands:

* Get numbers Recognizer's features:
`npm install recognizers-text-number`

* Get numbers with units Recognizer's features:
`npm install recognizers-text-number-with-unit`

* Get datetime Recognizer's features:
`npm install recognizers-text-date-time`

## API Documentation

### Microsoft.Recognizers.Text.Number

* [NumberModel](/recognizers-number/src/number/numberRecognizer.ts)

    This recognizer will find any number from the input. E.g. "I have two apples" will return "2".

    `Recognizers.NumberRecognizer.instance.getNumberModel(Culture.English)`

* [OrdinalModel](/recognizers-number/src/number/numberRecognizer.ts)

    This recognizer will find any ordinal number. E.g. "eleventh" will return "11".

    `Recognizers.NumberRecognizer.instance.getOrdinalModel(Culture.English)`

* [PercentageModel](/recognizers-number/src/number/numberRecognizer.ts)

    This recognizer will find any number presented as percentage. E.g. "one hundred percents" will return "100%".

    `Recognizers.NumberRecognizer.instance.getPercentageModel(Culture.English)`

### Microsoft.Recognizers.Text.NumberWithUnit

* [AgeModel](/recognizers-number-with-unit/src/numberWithUnit/numberWithUnitRecognizer.ts)

    This recognizer will find any age number presented. E.g. "After ninety five years of age, perspectives change" will return "95 Year".

    `Recognizers.NumberWithUnitRecognizer.instance.getAgeModel(Culture.English)`

* [CurrencyModel](/recognizers-number-with-unit/src/numberWithUnit/numberWithUnitRecognizer.ts)

    This recognizer will find any currency presented. E.g. "Interest expense in the 1988 third quarter was $ 75.3 million" will return "75300000 Dollar".

    `Recognizers.NumberWithUnitRecognizer.instance.getCurrencyModel(Culture.English)`

* [DimensionModel](/recognizers-number-with-unit/src/numberWithUnit/numberWithUnitRecognizer.ts)

    This recognizer will find any dimension presented. E.g. "The six-mile trip to my airport hotel that had taken 20 minutes earlier in the day took more than three hours." will return "6 Mile".

    `Recognizers.NumberWithUnitRecognizer.instance.getDimensionModel(Culture.English)`

* [TemperatureModel](/recognizers-number-with-unit/src/numberWithUnit/numberWithUnitRecognizer.ts)

    This recognizer will find any temperature presented. E.g. "Set the temperature to 30 degrees celsius" will return "30 C".

    `Recognizers.NumberWithUnitRecognizer.instance.getTemperatureModel(Culture.English)`

### Microsoft.Recognizers.Text.DateTime

* [DateTimeModel](/recognizers-date-time/src/dateTime/dateTimeRecognizer.ts)

    This model will find any date, time, duration and date/time ranges, even if its write in coloquial language. E.g. "I'll go back 8pm today" will return "2017-10-04 20:00:00".

    `Recognizers.DateTimeRecognizer.instance.getDateTimeModel(Culture.English)`

## Samples

[Start using recognizers!](/samples)

## Integration tips

The recognizers were designed to disjoint language's logic from the recognizer's core in order to grow without the obligation of change the supported platforms.

To achieve this, the recognizers contains the following folders:

* [Specs](..\Specs) - Contains all the necessary tests that should be run on any improvements to the recognizers. It's divided by recognizer and supported language.
* [Patterns](..\Patterns)  - Contains all the regular expresions that fulfill the recognizers logic. It's divided by supported language.