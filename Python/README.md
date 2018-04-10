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

You can then install each of the local packages:

    pip install -e .\libraries\recognizers-text\
    pip install -e .\libraries\recognizers-number\

### Automatized Build

Launch `Build.cmd` file to install requirements, generate resources, install local packages and run all tests.

## Installation

Install Recognizer's by launching the following commands:

* Get the numbers Recognizer's features:
`pip install recognizers-text-number`

## API Documentation

### [Microsoft.Recognizers.Text.Number](https://github.com/Microsoft/Recognizers-Text/tree/master/Python/libraries/recognizers-number)

## Samples

[Start using recognizers!](https://github.com/Microsoft/Recognizers-Text/tree/master/Python/samples)

## Integration tips

The Recognizers aim to bridge people's spoken language and machine's programming languages.
As such, Recognizers were designed to facilitate growing the number of supported _cultures_ (i.e. spoken languages) and _platforms_ (i.e. programming languages.)
 
With this goal in mind, they are designed to disjoint the specific culture's logic from the recognizer's core implementation. A shared set of tools are available at the heart of a *cross-culture & cross-platform* approach that will help with extending the number and range of the recognizers.


To achieve this, the recognizers contains the following folders:

* [Specs](https://github.com/Microsoft/Recognizers-Text/tree/master/Specs) - Contains all the necessary tests that should be run on any improvements to the recognizers. It's divided by recognizer and supported language.
* [Patterns](https://github.com/Microsoft/Recognizers-Text/tree/master/Patterns)  - Contains all the regular expressions that fulfill the recognizers logic. It's divided by supported language.
