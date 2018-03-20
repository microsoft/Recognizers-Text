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

### Automatized Build

Launch `Build.cmd` file.

## Installation

Install Recognizer's by launching the following commands:

* Get the numbers Recognizer's features:
`pip install recognizers-text-number`

## API Documentation

Once the proper package is installed, you'll need to reference the package:

````Python
from recognizers_number import Culture, ModelResult, NumberRecognizer
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

    This recognizer will find any number from the input. E.g. _"I have two apples"_ will output `Received "two", resolution is: {'value': '2'}`.

    ```
    result = NumberRecognizer.recognize_number('I have two apples', Culture.English)
    print(f'Received "{result[0].text}", resolution is: {result[0].resolution}')
    ```

    Or you can obtain a model instance using:

    `NumberRecognizer(Culture.English).get_number_model()`


* **Ordinal Numbers**

    This recognizer will find any ordinal number. E.g. _"eleventh"_ will output `Received "eleventh", resolution is: {'value': '11'}`.

    ```
    result = NumberRecognizer.recognize_ordinal('eleventh', Culture.English)
    print(f'Received "{result[0].text}", resolution is: {result[0].resolution}')
    ```

    Or you can obtain a model instance using:

    `NumberRecognizer(Culture.English).get_ordinal_model()`


* **Percentages**

    This recognizer will find any number presented as percentage. E.g. _"one hundred percents"_ will output `Received "one hundred percents", resolution is: {'value': '100%'}`.

    ```
    result = NumberRecognizer.recognize_percentage('one hundred percents', Culture.English)
    print(f'Received "{result[0].text}", resolution is: {result[0].resolution}')
    ```

    Or you can obtain a model instance using:

    `NumberRecognizer(Culture.English).get_percentage_model()`

## Samples

[Start using recognizers!](https://github.com/Microsoft/Recognizers-Text/tree/master/Python/samples)

## Integration tips

The Recognizers aim to bridge people's spoken language and machine's programming languages.
As such, Recognizers were designed to facilitate growing the number of supported _cultures_ (i.e. spoken languages) and _platforms_ (i.e. programming languages.)
 
With this goal in mind, they are designed to disjoint the specific culture's logic from the recognizer's core implementation. A shared set of tools are available at the heart of a *cross-culture & cross-platform* approach that will help extending the number and range of the recognizers.


To achieve this, the recognizers contains the following folders:

* [Specs](https://github.com/Microsoft/Recognizers-Text/tree/master/Specs) - Contains all the necessary tests that should be run on any improvements to the recognizers. It's divided by recognizer and supported language.
* [Patterns](https://github.com/Microsoft/Recognizers-Text/tree/master/Patterns)  - Contains all the regular expressions that fulfill the recognizers logic. It's divided by supported language.
