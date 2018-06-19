# Java Samples

The best way to learn how to use something is through examples. The Java project contains one simple sample to get you started.

## Simple Console ([source](./simple-console))

This sample demonstrate the combination of all Recognizers to extract possible values from the user's input.

To start the sample, execute from the sample's folder:

```mvn exec:java```

The important pieces are the Recognizers and Model classes, which you'll need to import using:

```Java
import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.ModelResult;
import com.microsoft.recognizers.text.number.NumberRecognizer;
```

Then, the sample gets a model reference of each available Recognizer. We need to do so by passing the Culture code we'll want to detect. E.g.: `en-us`.

So far, the available models are:

```Java
// Use English for the Recognizers culture
String culture = Culture.English;

// Number recognizer - This function will find any number from the input
// E.g "I have two apples" will return "2".
NumberRecognizer.recognizeNumber(userInput, culture),

// Ordinal number recognizer - This function will find any ordinal number
// E.g "eleventh" will return "11".
NumberRecognizer.recognizeOrdinal(userInput, culture),

// Percentage recognizer - This function will find any number presented as percentage
// E.g "one hundred percents" will return "100%"
NumberRecognizer.recognizePercentage(userInput, culture),

// Number Range recognizer will find any cardinal or ordinal number range
// E.g. "between 2 and 5" will return "(2,5)"
NumberRecognizer.recognizeNumberRange(userInput, culture)
````

All these models accept `userInput` as a string and returns a **List** of [ModelResult](../libraries/recognizers-text/src/main/java/com/microsoft/recognizers/text/ModelResult.java):

````Java
List<ModelResult> result = NumberRecognizer.recognizeNumber("I have twenty apples", Culture.English);

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