# Python Samples

The best way to learn how to use something is through examples. The Python project contains one simple sample to get you started.

## Simple Console ([source](./simple_console))

This sample demonstrate the combination of all Recognizers to extract possible values from the user's input.

First, install its dependencies:

```pip install -r .\requirements.txt```

And then start the sample:

```python sample.py```

The important piece is the `Recognizers` module, which you'll need to import using:

```Python
from recognizers_number import NumberRecognizer
```

Then, the sample gets a model reference of each available Recognizer. We need to do so by passing the Culture code we'll want to detect. E.g.: `en-us`.

So far, the available models are:

```Python
# Use English for the Recognizers culture
DEFAULT_CULTURE = Culture.English

# Number recognizer - This function will find any number from the input
# E.g "I have two apples" will return "2".
NumberRecognizer.recognize_number(user_input, culture),
# Ordinal number recognizer - This function will find any ordinal number
# E.g "eleventh" will return "11".
NumberRecognizer.recognize_ordinal(user_input, culture),
# Percentage recognizer - This function will find any number presented as percentage
# E.g "one hundred percents" will return "100%"
NumberRecognizer.recognize_percentage(user_input, culture),
````

All these models accept `user_input: str` as a string and returns a **List** of [ModelResult](../libraries/recognizers-text/recognizers_text/model.py#L10-L16):

````Python
result = NumberRecognizer.recognize_number("I have twenty apples")

# Returns:
# [
# 	{
# 		"start": 7,
# 		"end": 12,
# 		"resolution": {
# 			"value": "20"
# 		},
# 		"text": "twenty",
# 		"typeName": "number"
# 	}
# ]
````