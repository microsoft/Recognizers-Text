# Microsoft.Recognizers.Text for .NET

## Getting Started

Recognizer's are organized into groups and designed to be used in C#, Node.js, Python and Java to help you build great applications! To use the samples clone our GitHub repository using Git.

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

* Get sequence Recognizer's features:
`nuget install Microsoft.Recognizers.Text.Sequence`

* Get choice Recognizer's features:
`nuget install Microsoft.Recognizers.Text.Choice`

## API Documentation

Resolution of values can be achieved in two ways, using the Recognizer's models or using the helper methods:

### Recognizer's Models

This is the preferred way if you need to parse multiple inputs based on the same context (e.g.: language and options):

```C#
var recognizer = new NumberRecognizer(Culture.English);
var model = recognizer.GetNumberModel();
var result = model.Parse("Twelve");
```

Or, for less verbosity, you use the helper methods:

`var result = NumberRecognizer.RecognizeNumber("Twelve", Culture.English);`

Internally, both methods will cache the instance models to avoid extra costs.

### Microsoft.Recognizers.Text.Number

* **Numbers**

    This recognizer will find any number from the input. E.g "I have two apples" will return "2".

    `NumberRecognizer.RecognizeNumber("I have two apples", Culture.English)`

    Or you can obtain a model instance using:

    `var model = new NumberRecognizer(Culture.English).GetNumberModel()`

* **Ordinal Numbers**

    This recognizer will find any ordinal number. E.g "eleventh" will return "11".

    `NumberRecognizer.RecognizeOrdinal("eleventh", Culture.English)`

    Or you can obtain a model instance using:

    `var model = new NumberRecognizer(Culture.English).GetOrdinalModel()`


* **Percentages**

    This recognizer will find any number presented as percentage. E.g "one hundred percents" will return "100%".

    `NumberRecognizer.RecognizePercentage("one hundred percents", Culture.English)`

    Or you can obtain a model instance using:

    `var model = new NumberRecognizer(Culture.English).GetPercentageModel()`


### Microsoft.Recognizers.Text.NumberWithUnit

* **Ages**

    This recognizer will find any age number presented. E.g "After ninety five years of age, perspectives change" will return "95 Year".

    `NumberWithUnitRecognizer.RecognizeAge("After ninety five years of age, perspectives change", Culture.English)`

    Or you can obtain a model instance using:

    `var model = new NumberWithUnitRecognizer(Culture.English).GetAgeModel()`


* **Currencies**

    This recognizer will find any currency presented. E.g "Interest expense in the 1988 third quarter was $ 75.3 million" will return "75300000 Dollar".

    `NumberWithUnitRecognizer.RecognizeCurrency("Interest expense in the 1988 third quarter was $ 75.3 million", Culture.English)`

    Or you can obtain a model instance using:

    `var model = new NumberWithUnitRecognizer(Culture.English).GetCurrencyModel()`


* **Dimensions**

    This recognizer will find any dimension presented. E.g "The six-mile trip to my airport hotel that had taken 20 minutes earlier in the day took more than three hours." will return "6 Mile".

    `NumberWithUnitRecognizer.RecognizeDimension("The six-mile trip to my airport hotel that had taken 20 minutes earlier in the day took more than three hours.", Culture.English)`

    Or you can obtain a model instance using:

    `new NumberWithUnitRecognizer(Culture.English).GetDimensionModel()`


* **Temperatures**

    This recognizer will find any temperature presented. E.g "Set the temperature to 30 degrees celsius" will return "30 C".

    `NumberWithUnitRecognizer.RecognizeTemperature("Set the temperature to 30 degrees celsius", Culture.English)`

    Or you can obtain a model instance using:

    `new NumberWithUnitRecognizer(Culture.English).GetTemperatureModel()`

### Microsoft.Recognizers.Text.DateTime

* **Date, time, duration and date/time ranges**

    This model will find any date, time, duration and date/time ranges, even if its write in coloquial language. E.g. "I'll go back 8pm today" will return "2017-10-04 20:00:00".

    `DateTimeRecognizer.RecognizeDateTime("I'll go back 8pm today", Culture.English)`

    Or you can obtain a model instance using:

    `new DateTimeRecognizer(Culture.English).GetDateTimeModel()`


### Microsoft.Recognizers.Text.Sequence

* **Phone Numbers**

    This model will find any patter of symbols detected as a phone number, even if its write in coloquial language. E.g. "My phone number is 1 (877) 609-2233." will return "1 (877) 609-2233".

    `SequenceRecognizer.RecognizePhoneNumber("My phone number is 1 (877) 609-2233.", Culture.English)`

    Or you can obtain a model instance using:

    `new SequenceRecognizer(Culture.English).GetPhoneNumberModel()`

    * **IP Addresses**

    This model will find any Ipv4/Ipv6 presented. 
    E.g. "My Ip is 8.8.8.8".

    `SequenceRecognizer.RecognizeIpAddress"My Ip is 8.8.8.8", Culture.English)`

    Or you can obtain a model instance using:

    `new SequenceRecognizer(Culture.English).GetIpAddressModel()`

### Microsoft.Recognizers.Text.Choice

* **Booleans**

    This recognizer will find any boolean value, even if its write with emoji. 
    E.g. _"👌 It's ok"_ will return `True`.

    `ChoiceRecognizer.RecognizeBoolean("👌 It's ok", Culture.English)`

    Or you can obtain a model instance using:

    `new ChoiceRecognizer(Culture.English).GetBooleanModel()`


## Samples

[Start using recognizers!](/.NET/Samples)

## Integration tips

The recognizers were designed to disjoint language's logic from the recognizer's core in order to grow without the obligation of change the supported platforms.

To achieve this, the recognizers contains the following folders:

* [Specs](/Specs) - Contains all the necessary tests that should be run on any improvements to the recognizers. It's divided by recognizer and supported language.
* [Patterns](/Patterns)  - Contains all the regular expresions that fulfill the recognizers logic. It's divided by supported language.
