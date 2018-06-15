# Python Samples

The best way to learn how to use something is through examples. The Python project contains one simple sample to get you started.

## Simple Console ([source](./simple_console))

This sample demonstrate the combination of all Recognizers to extract possible values from the user's input.

First, install its dependencies:

```
cd .\samples\simple_console\
pip install -r .\requirements.txt
```

And then start the sample:

```python sample.py```

The important piece is the `Recognizers` module, which you'll need to import using:

```Python
import recognizers_suite as Recognizers
```

Then, the sample gets a model reference of each available Recognizer. We need to do so by passing the Culture code we'll want to detect. E.g.: `en-us`.

```Python
from recognizers_suite import Culture, ModelResult
```

So far, the available models are:

```Python
# Use English for the Recognizers culture
DEFAULT_CULTURE = Culture.English

# Number recognizer - This function will find any number from the input
# E.g "I have two apples" will return "2".
Recognizers.recognize_number(user_input, culture),

# Ordinal number recognizer - This function will find any ordinal number
# E.g "eleventh" will return "11".
Recognizers.recognize_ordinal(user_input, culture),

# Percentage recognizer - This function will find any number presented as percentage
# E.g "one hundred percents" will return "100%"
Recognizers.recognize_percentage(user_input, culture),

# Age recognizer - This function will find any age number presented
# E.g "After ninety five years of age, perspectives change" will return "95 Year"
Recognizers.recognize_age(user_input, culture),

# Currency recognizer - This function will find any currency presented
# E.g "Interest expense in the 1988 third quarter was $ 75.3 million" will return "75300000 Dollar"
Recognizers.recognize_currency(user_input, culture),

# Dimension recognizer - This function will find any dimension presented
# E.g "The six-mile trip to my airport hotel that had taken 20 minutes earlier in the day took more than three hours." will return "6 Mile"
Recognizers.recognize_dimension(user_input, culture),

# Temperature recognizer - This function will find any temperature presented
# E.g "Set the temperature to 30 degrees celsius" will return "30 C"
Recognizers.recognize_temperature(user_input, culture),

# DateTime recognizer - This function will find any Date even if its write in colloquial language
# E.g "I'll go back 8pm today" will return "2017-10-04 20:00:00"
Recognizers.recognize_datetime(user_input, culture)
````

All these models accept `user_input: str` as a string and returns a **List** of [ModelResult](../libraries/recognizers-text/recognizers_text/model.py#L10-L16):

````Python
result = Recognizers.recognize_number("I have twenty apples")

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