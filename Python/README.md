# Microsoft.Recognizers.Text for Python

## Getting Started

Recognizer's are organized into groups and designed to be used in C#, Node.js, and Python to help you build great applications! To use the samples clone our GitHub repository using Git.

## Cloning and building the Repository

    git clone https://github.com/Microsoft/Recognizers-Text.git
    cd Recognizers-Text

### Manual Build

Open a terminal and run the following commands:

    cd python/libraries/resource-generator
    pip install -r .\requirements.txt
    python index.py ..\recognizers-number\resource-definitions.json
    python index.py ..\recognizers-number-with-unit\resource-definitions.json

### Automatized Build

Launch `Build.cmd` file.

## Installation

Install Recognizer's by launching the following commands:

* Get the numbers Recognizer's features:
`pip install recognizers-text-number`

## API Documentation

Once the proper package is installed, you'll need to reference the package:

````Python
from recognizers_text import Culture, ModelResult
from recognizers_number import NumberRecognizer
````

### Recognizer's Models

This is the preferred way if you need to parse multiple inputs based on the same context (e.g.: language and options):

```Python
recognizer = NumberRecognizer(Culture.English)
model = recognizer.get_number_model()
result = model.parse('Twelve')
```

Or, for less verbosity, you use the helper methods:

`result = NumberRecognizer.recognize_number("Twelve", Culture.English);`

Internally, both methods will cache the instance models to avoid extra costs.

### Microsoft.Recognizers.Text.Number

* **Numbers**

    This recognizer will find any number from the input. E.g. _"I have two apples"_ will return _"2"_.

    `NumberRecognizer.recognize_number('I have two apples', Culture.English)`

    Or you can obtain a model instance using:

    `NumberRecognizer(Culture.English).get_number_model()`


* **Ordinal Numbers**

    This recognizer will find any ordinal number. E.g. _"eleventh"_ will return _"11"_.

    `NumberRecognizer.recognize_ordinal('eleventh', Culture.English)`

    Or you can obtain a model instance using:

    `NumberRecognizer(Culture.English).get_ordinal_model()`


* **Percentages**

    This recognizer will find any number presented as percentage. E.g. _"one hundred percents"_ will return _"100%"_.

    `NumberRecognizer.recognize_percentage('one hundred percents', Culture.English)`

    Or you can obtain a model instance using:

    `NumberRecognizer(Culture.English).get_percentage_model()`

### Microsoft.Recognizers.Text.NumberWithUnit

* **Ages**

    This recognizer will find any age number presented. E.g. _"After ninety five years of age, perspectives change"_ will return _"95 Year"_.

    `NumberWithUnitRecognizers.recognize_age('After ninety five years of age, perspectives change', Culture.English)`

    Or you can obtain a model instance using:

    `NumberWithUnitRecognizer(Culture.English).get_age_model()`


* **Currencies**

    This recognizer will find any currency presented. E.g. _"Interest expense in the 1988 third quarter was $ 75.3 million"_ will return _"75300000 Dollar"_.

    `NumberWithUnitRecognizers.recognize_currency('Interest expense in the 1988 third quarter was $ 75.3 million', Culture.English)`

    Or you can obtain a model instance using:

    `NumberWithUnitRecognizer(Culture.English).get_currency_model()`


* **Dimensions**

    This recognizer will find any dimension presented. E.g. _"The six-mile trip to my airport hotel that had taken 20 minutes earlier in the day took more than three hours."_ will return _"6 Mile"_.

    `NumberWithUnitRecognizers.recognize_dimension('The six-mile trip to my airport hotel that had taken 20 minutes earlier in the day took more than three hours.', Culture.English)`

    Or you can obtain a model instance using:

    `NumberWithUnitRecognizer(Culture.English).get_dimension_model()`


* **Temperatures**

    This recognizer will find any temperature presented. E.g. _"Set the temperature to 30 degrees celsius"_ will return _"30 C"_.

    `NumberWithUnitRecognizers.recognize_temperature('Set the temperature to 30 degrees celsius', Culture.English)`

    Or you can obtain a model instance using:

    `NumberWithUnitRecognizer(Culture.English).get_temperature_model()`


## Samples

[Start using recognizers!](https://github.com/Microsoft/Recognizers-Text/tree/master/Python/samples)

## Integration tips

The recognizers were designed to disjoint language's logic from the recognizer's core in order to grow without the obligation of change the supported platforms.

To achieve this, the recognizers contains the following folders:

* [Specs](https://github.com/Microsoft/Recognizers-Text/tree/master/Specs) - Contains all the necessary tests that should be run on any improvements to the recognizers. It's divided by recognizer and supported language.
* [Patterns](https://github.com/Microsoft/Recognizers-Text/tree/master/Patterns)  - Contains all the regular expressions that fulfill the recognizers logic. It's divided by supported language.
