# Microsoft.Recognizers.Text for Java

## Getting Started

Recognizer's are organized into groups and designed to be used in C#, Node.js, Python and Java to help you build great applications! To use the samples clone our GitHub repository using Git.

## Cloning and building the Repository

    git clone https://github.com/Microsoft/Recognizers-Text.git
    cd Recognizers-Text

You can between build the solution manually using Maven.

### Manual Build and installation on local Maven folder (.m2)

Open a terminal and run the following commands:

    cd Java
    mvn clean install

### Installation
Install Recognizer's by adding the following dependencies in your `pom.xml`:
- Get core Recognizer's features:
    ````xml
    <dependency>
        <groupId>com.microsoft.recognizers.text</groupId>
        <artifactId>recognizers-text</artifactId>
        <version>1.0-SNAPSHOT</version>
    </dependency>
    ````
- Get numbers Recognizer's features:
    ````xml
    <dependency>
        <groupId>com.microsoft.recognizers.text.number</groupId>
        <artifactId>recognizers-text-number</artifactId>
        <version>1.0-SNAPSHOT</version>
    </dependency>
    ````

- Get numbers with units Recognizer's features:
    ````xml
    <dependency>
        <groupId>com.microsoft.recognizers.text.numberwithunit</groupId>
        <artifactId>recognizers-text-number-with-unit</artifactId>
        <version>1.0-SNAPSHOT</version>
    </dependency>
    ````

- Get datetime Recognizer's features:
    ````xml
    <dependency>
        <groupId>com.microsoft.recognizers.text.datetime</groupId>
        <artifactId>recognizers-text-date-time</artifactId>
        <version>1.0-SNAPSHOT</version>
    </dependency>
    ````

- Get sequence Recognizer's features:
    ````xml
    <dependency>
        <groupId>com.microsoft.recognizers.text.sequence</groupId>
        <artifactId>recognizers-text-sequence</artifactId>
        <version>1.0-SNAPSHOT</version>
    </dependency>
    ````

- Get choice Recognizer's features:
    ````xml
    <dependency>
        <groupId>com.microsoft.recognizers.text.choice</groupId>
        <artifactId>recognizers-text-choice</artifactId>
        <version>1.0-SNAPSHOT</version>
    </dependency>
    ````

## API Documentation

Once the proper modules are installed, you'll need to import the modules:

````Java
import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.IModel;
import com.microsoft.recognizers.text.ModelResult;
import com.microsoft.recognizers.text.number.NumberRecognizer;
import com.microsoft.recognizers.text.numberwithunit.NumberWithUnitRecognizer;
````

### Recognizer's Models

This is the preferred way if you need to parse multiple inputs based on the same context (e.g.: language and options):

```Java
NumberRecognizer recognizer = new NumberRecognizer(Culture.English);
IModel model = recognizer.getNumberModel();
List<ModelResult> result = model.parse("Twelve");
```

Or, for less verbosity, you use the helper methods:

`List<ModelResult> result = NumberRecognizer.recognizeNumber("Twelve", Culture.English);`

Internally, both methods will cache the instance models to avoid extra costs.

### com.microsoft.recognizers.text.number

* **Numbers**

    This recognizer will find any number from the input. E.g. _"I have two apples"_ will return _"2"_.

    `NumberRecognizer.recognizeNumber("I have two apples", Culture.English)`

    Or you can obtain a model instance using:

    `new NumberRecognizer(Culture.English).getNumberModel()`


* **Ordinal Numbers**

    This recognizer will find any ordinal number. E.g. _"eleventh"_ will return _"11"_.

    `NumberRecognizer.recognizeOrdinal("eleventh", Culture.English)`

    Or you can obtain a model instance using:

    `new NumberRecognizer(Culture.English).getOrdinalModel()`


* **Percentages**

    This recognizer will find any number presented as percentage. E.g. _"one hundred percents"_ will return _"100%"_.

    `NumberRecognizer.recognizePercentage("one hundred percents", Culture.English)`

    Or you can obtain a model instance using:

    `new NumberRecognizer(Culture.English).getPercentageModel()`

### com.microsoft.recognizers.text.numberwithunit

* **Ages**

    This recognizer will find any age number presented. E.g "After ninety five years of age, perspectives change" will return "95 Year".

    `NumberWithUnitRecognizer.recognizeAge("After ninety five years of age, perspectives change", Culture.English)`

    Or you can obtain a model instance using:

    `new NumberWithUnitRecognizer(Culture.English).getAgeModel()`


* **Currencies**

    This recognizer will find any currency presented. E.g "Interest expense in the 1988 third quarter was $ 75.3 million" will return "75300000 Dollar".

    `NumberWithUnitRecognizer.recognizeCurrency("Interest expense in the 1988 third quarter was $ 75.3 million", Culture.English)`

    Or you can obtain a model instance using:

    `new NumberWithUnitRecognizer(Culture.English).getCurrencyModel()`


* **Dimensions**

    This recognizer will find any dimension presented. E.g "The six-mile trip to my airport hotel that had taken 20 minutes earlier in the day took more than three hours." will return "6 Mile".

    `NumberWithUnitRecognizer.recognizeDimension("The six-mile trip to my airport hotel that had taken 20 minutes earlier in the day took more than three hours.", Culture.English)`

    Or you can obtain a model instance using:

    `new NumberWithUnitRecognizer(Culture.English).getDimensionModel()`


* **Temperatures**

    This recognizer will find any temperature presented. E.g "Set the temperature to 30 degrees celsius" will return "30 C".

    `NumberWithUnitRecognizer.recognizeTemperature("Set the temperature to 30 degrees celsius", Culture.English)`

    Or you can obtain a model instance using:

    `new NumberWithUnitRecognizer(Culture.English).getTemperatureModel()`

## Samples

[Start using recognizers!](https://github.com/Microsoft/Recognizers-Text/tree/master/Java/samples)

## Integration tips

The recognizers were designed to disjoint language's logic from the recognizer's core in order to grow without the obligation of change the supported platforms.

To achieve this, the recognizers contains the following folders:

* [Specs](https://github.com/Microsoft/Recognizers-Text/tree/master/Specs) - Contains all the necessary tests that should be run on any improvements to the recognizers. It's divided by recognizer and supported language.
* [Patterns](https://github.com/Microsoft/Recognizers-Text/tree/master/Patterns)  - Contains all the regular expressions that fulfill the recognizers logic. It's divided by supported language.

## Linting rules

This project uses linting rules to enforce code standardization. These rules are specified in the file [linting-rules.xml](linting-rules.xml) with [CheckStyle](https://checkstyle.org/) and are hooked to Maven's build cycle.

**INFO**: Since the CheckStyle plugin is hook to the build cycle, this makes the build **fail** in case there are linting warnings in the project files so be sure to check that the code doesn't break any rule.

CheckStyle is available in different flavours:
- [Visual Studio Code plugin](https://marketplace.visualstudio.com/items?itemName=shengchen.vscode-checkstyle)
- [IntelliJ IDEA plugin](https://plugins.jetbrains.com/plugin/1065-checkstyle-idea)
- [Eclipse plugin](https://checkstyle.org/eclipse-cs)
- [CLI Tool](https://checkstyle.org/cmdline.html)

**INFO**: Be sure to configure your IDE to use the file [linting-rules.xml](linting-rules.xml) instead of the default rules.