# Microsoft.Recognizers.Text for .NET

## Getting Started

Recognizer's are organized into groups and designed to be used in C# and Node.js to help you build great applications! To use the samples clone our GitHub repository using Git.

    git clone https://github.com/Microsoft/Recognizers-Tex.git
    cd Recognizers-Text

## Setup

You can choose between build the solution manually or through an automatized build.cmd file.

### Manual Build
* Open `Microsoft.Recognizers.Text.sln` and build solution.
* Launch all available tests by context menu _Test/Run/All Tests_.

### Automatized Build
* Launch `Build.cmd` file.

### Installation

Install Recognizer's by launching the following commands:

* Get core Recognizer's features:
`nuget install Microsoft.Recognizers.Text`

* Get numbers Recognizer's features:
`nuget install Microsoft.Recognizers.Text.Number`

* Get numbers with units Recognizer's features:
`nuget install Microsoft.Recognizers.Text.NumberWithUnit`

* Get datetime Recognizer's features:
`nuget install Microsoft.Recognizers.Text.DateTime`

## API Documentation

### Microsoft.Recognizers.Text.Number

* [NumberModel](/.NET/Microsoft.Recognizers.Text.Number/Models/NumberModel.cs)

    This recognizer will find any number from the input. E.g "I have two apples" will return "2".

    `new NumberRecognizer(Culture.English).GetNumberModel()`

    Optionally, you can use the `recognizeNumber` method to parse your input with the same result.

    `NumberRecognizer.RecognizeNumber("I have two apples", Culture.English)`

* [OrdinalModel](/.NET/Microsoft.Recognizers.Text.Number/Models/OrdinalModel.cs)

    This recognizer will find any ordinal number. E.g "eleventh" will return "11".

    `new NumberRecognizer(Culture.English).GetOrdinalModel()`

    Optionally, you can use the `recognizeOrdinal` method to parse your input with the same result.

    `NumberRecognizer.RecognizeOrdinal("eleventh", Culture.English)`

* [PercentageModel](/.NET/Microsoft.Recognizers.Text.Number/Models/PercentModel.cs)

    This recognizer will find any number presented as percentage. E.g "one hundred percents" will return "100%".

    `new NumberRecognizer(Culture.English).GetPercentageModel()`

    Optionally, you can use the `recognizePercentage` method to parse your input with the same result.

    `NumberRecognizer.RecognizePercentage("one hundred percents", Culture.English)`

### Microsoft.Recognizers.Text.NumberWithUnit

* [AgeModel](/.NET/Microsoft.Recognizers.Text.NumberWithUnit/Models/AgeModel.cs)

    This recognizer will find any age number presented. E.g "After ninety five years of age, perspectives change" will return "95 Year".

    `new NumberWithUnitRecognizer(Culture.English).GetAgeModel()`

    Optionally, you can use the `recognizeAge` method to parse your input with the same result.

    `NumberWithUnitRecognizer.RecognizeAge("After ninety five years of age, perspectives change", Culture.English)`

* [CurrencyModel](/.NET/Microsoft.Recognizers.Text.NumberWithUnit/Models/CurrencyModel.cs)

    This recognizer will find any currency presented. E.g "Interest expense in the 1988 third quarter was $ 75.3 million" will return "75300000 Dollar".

    `new NumberWithUnitRecognizer(Culture.English).GetCurrencyModel()`

    Optionally, you can use the `recognizeCurrency` method to parse your input with the same result.

    `NumberWithUnitRecognizer.RecognizeCurrency("Interest expense in the 1988 third quarter was $ 75.3 million", Culture.English)`

* [DimensionModel](/.NET/Microsoft.Recognizers.Text.NumberWithUnit/Models/DimensionModel.cs)

    This recognizer will find any dimension presented. E.g "The six-mile trip to my airport hotel that had taken 20 minutes earlier in the day took more than three hours." will return "6 Mile".

    `new NumberWithUnitRecognizer(Culture.English).GetDimensionModel()`

    Optionally, you can use the `recognizeDimension` method to parse your input with the same result.

    `NumberWithUnitRecognizer.RecognizeDimension("The six-mile trip to my airport hotel that had taken 20 minutes earlier in the day took more than three hours.", Culture.English)`

* [TemperatureModel](/.NET/Microsoft.Recognizers.Text.NumberWithUnit/Models/TemperatureModel.cs)

    This recognizer will find any temperature presented. E.g "Set the temperature to 30 degrees celsius" will return "30 C".

    `new NumberWithUnitRecognizer(Culture.English).GetTemperatureModel()`

    Optionally, you can use the `recognizeTemperature` method to parse your input with the same result.

    `NumberWithUnitRecognizer.RecognizeTemperature("Set the temperature to 30 degrees celsius", Culture.English)`

### Microsoft.Recognizers.Text.DateTime

* [DateTimeModel](/.NET/Microsoft.Recognizers.Text.DateTime/Models/DateTimeModel.cs)

    This model will find any date, time, duration and date/time ranges, even if its write in coloquial language. E.g. "I'll go back 8pm today" will return "2017-10-04 20:00:00".

    `new DateTimeRecognizer(Culture.English).GetDateTimeModel()`

    Optionally, you can use the `recognizeDateTime` method to parse your input with the same result.

    `DateTimeRecognizer.RecognizeDateTime("I'll go back 8pm today", Culture.English)`

### Microsoft.Recognizers.Text.Sequence

* [PhoneNumberModel](/.NET/Microsoft.Recognizers.Text.Sequence/Models/PhoneNumberModel.cs)

    This model will find any patter of symbols detected as a phone number, even if its write in coloquial language. E.g. "My phone number is 1 (877) 609-2233." will return "1 (877) 609-2233".

    `new SequenceRecognizer(Culture.English).GetPhoneNumberModel()`

    Optionally, you can use the `RecognizePhoneNumber` method to parse your input with the same result.

    `DateTimeRecognizer.RecognizePhoneNumber("My phone number is 1 (877) 609-2233.", Culture.English)`

## Samples

[Start using recognizers!](/.NET/Samples)

## Integration tips

The recognizers were designed to disjoint language's logic from the recognizer's core in order to grow without the obligation of change the supported platforms.

To achieve this, the recognizers contains the following folders:

* [Specs](/Specs) - Contains all the necessary tests that should be run on any improvements to the recognizers. It's divided by recognizer and supported language.
* [Patterns](/Patterns)  - Contains all the regular expresions that fulfill the recognizers logic. It's divided by supported language.
