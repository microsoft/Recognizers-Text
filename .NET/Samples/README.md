# .NET Samples

The best way to learn how to use something is through examples. The .NET solution contains a simple sample to get you started.

## Simple Console ([source](./SimpleConsole))

This sample demonstrate the combination of all Recognizers to extract possible values from the user's input.

The important piece is the `Microsoft.Recognizers.Text` NuGet package, which you'll need to add using:

````
Install-Package Microsoft.Recognizers.Text
````

Then, the sample gets a model reference of each available Recognizer. We need to do so by passing the Culture code we'll want to detect. E.g.: `en-us`.

So far, the available models are:

````C#
using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.DateTime;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.NumberWithUnit;

// Use English for the Recognizers culture
var myCulture = Culture.English;

// Add Number recognizer - This recognizer will find any number from the input
// E.g "I have two apples" will return "2".
NumberRecognizer.Instance.GetNumberModel(myCulture);

// Add Ordinal number recognizer - This recognizer will find any ordinal number
// E.g "eleventh" will return "11".
NumberRecognizer.Instance.GetOrdinalModel(myCulture);

// Add Percentage recognizer - This recognizer will find any number presented as percentage
// E.g "one hundred percents" will return "100%"
NumberRecognizer.Instance.GetPercentageModel(myCulture);

// Add Age recognizer - This recognizer will find any age number presented
// E.g "After ninety five years of age, perspectives change" will return "95 Year"
NumberWithUnitRecognizer.Instance.GetAgeModel(myCulture);

// Add Currency recognizer - This recognizer will find any currency presented
// E.g "Interest expense in the 1988 third quarter was $ 75.3 million" will return "75300000 Dollar"
NumberWithUnitRecognizer.Instance.GetCurrencyModel(myCulture);

// Add Dimension recognizer - This recognizer will find any dimension presented
// E.g "The six-mile trip to my airport hotel that had taken 20 minutes earlier in the day took more than three hours." will return "6 Mile"
NumberWithUnitRecognizer.Instance.GetDimensionModel(myCulture);

// Add Temperature recognizer - This recognizer will find any temperature presented
// E.g "Set the temperature to 30 degrees celsius" will return "30 C"
NumberWithUnitRecognizer.Instance.GetTemperatureModel(myCulture);

// Add Datetime recognizer - This model will find any Date even if its write in coloquial language -
// E.g "I'll go back 8pm today" will return "2017-10-04 20:00:00"
DateTimeRecognizer.GetInstance().GetDateTimeModel(myCulture);
````

All these models accept an input as a string and returns an **IEnumerable** of [ModelResult](../Microsoft.Recognizers.Text/Models/ModelResult.cs):

````C#
// Number model
var model = NumberRecognizer.Instance.GetNumberModel("en-us");

// Parse input using Number model
var result = model.Parse("I have twenty apples");

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