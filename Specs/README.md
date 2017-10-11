# Test Cases Specs

Microsoft.Recognizers.Text Recognizers are supported in multiple platforms and some feature parity must exist between all implementations.

In order to avoid duplicating unit-tests, written in different languages for each platforms, Microsoft.Recognizers.Text opts to use a shared format to define the assertions used to verify the recognizer's behavior.

These [Specs](../Specs) are defined as a list of test cases, each test case is represented by and input and a result. The result can either be a Parser's or Extractor's result to compare against.

## Definining a Test Case

This is a sample of a minimal test case, where at least the `Input` and `Results` attributes are required:

````JavaScript
{
  // The input text to extract or parse from
  "Input": "The Great Wall of China is more than 500 years old and extends for more than 5,000 miles.",
  // The expected ouput. This represents the return values from a Parser or Extractor result
  "Results": [
    {
      "Text": "500 years old",
      "TypeName": "age",
      "Resolution": {
        "value": "500",
        "unit": "Year"
      }
    }
  ]
}
````

Each test case can also have another set of optional attributes:

- **Debug**

  Indicates that the test case should trigger a Debugger Breakpoint before executing the extraction or parsing process. Usefull for debugging test cases and stepping-into the Extractors or Parsers.

  Sample usage:

  ````JavaScript
  {
    "Input": "in 1995 canon introduced the first commercially available slr lens with internal image stabilization , ef 75 -300mm f / 4 - 5 . 6 is usm.",

    // This test will trigger a Debugger Break just before invoking the Parser
    "Debug": true,

    "Results": [
      {
        "Text": "300mm",
        "TypeName": "dimension",
        "Resolution": {
          "value": "300",
          "unit": "Millimeter"
        }
      }
    ]
  }
  ````

- **NotSupported**

  Indicates that the test case is not (currently) supported on the specified platforms. It can be used when a feature is being implemented on certain platform and, in the meantime, 
  ignore temporary the backing test case on other platforms. This test case will either be skipped (.NET Test Runner) or ignored (AVA JS Test Runner), but a warning will be generated.

  Possible values include a list of platforms: dotNet, javascript, python. Each separated by a comma (,).

  Sample usage:

  ````JavaScript
  {
    "Input": "in 1995 canon introduced the first commercially available slr lens with internal image stabilization , ef 75 -300mm f / 4 - 5 . 6 is usm.",

    // This test will not be run for JavaScript, but a warning will be generated instead.
    "NotSupported": "javascript",

    "Results": [
      {
        "Text": "300mm",
        "TypeName": "dimension",
        "Resolution": {
          "value": "300",
          "unit": "Millimeter"
        }
      }
    ]
  }
  ````

- **NotSupportedByDesign**

  Similar to **NotSupported**, the test is ignored or skipped and no warning is generated. This can be used for disabling backing test cases for features not implemented on other platforms.

  Possible values include a list of platforms: dotNet, javascript, python. Each separated by a comma (,).

- **Context**

  Used to provide contextual arguments to the extractor or model. This attribute is used mostly on the DateTime recognizers to provide the 
  [`reference` date object](https://github.com/Microsoft/Recognizers-Text/blob/master/.NET/Microsoft.Recognizers.Text.DateTime/Parsers/BaseDateParser.cs#L26) (the relative date).

  Sample usage:

  ````JavaScript
  {
    "Input": "I'll go back today",
    // Add contextual arguments to test case
    "Context": {
      "ReferenceDateTime": "2016-11-07T00:00:00"
    },
    "Results": [
      {
        "Text": "today",
        "TypeName": "datetimeV2.date",
        "Resolution": {
          "values": [
            {
              "timex": "2016-11-07",
              "type": "date",
              "value": "2016-11-07"
            }
          ]
        }
      }
    ]
  }
  ````

## Organizing Test Suites

The Spec files containing the Test Suites and Test Cases are based on convention. The directory and file names follow the same naming convention as the classes used in the Extractor, Parsers and Models.

### .NET

In the .NET solution, the Test Suites are defined as Test Classes (using the `[TestClass]` attribute) and each may have several Test Methods (using the `[TestMethod]` attribute) to sub-group the test cases. 
Each Test Method should map to a single Spec file (JSON).

This is a sample structure of Test Classes:

````
Microsoft.Recognizers.Text.DataDrivenTests
│   Microsoft.Recognizers.Text.DataDrivenTests.csproj
│
├───DateTime
│       TestDateTime_Chinese.cs
│       TestDateTime_English.cs
│       TestDateTime_French.cs
│       TestDateTime_Spanish.cs
│
├───Number
│       TestNumber_Chinese.cs
│       TestNumber_English.cs
│       TestNumber_French.cs
│       TestNumber_Portuguese.cs
│       TestNumber_Spanish.cs
│
└───NumberWithUnit
        TestNumberWithUnit_Chinese.cs
        TestNumberWithUnit_English.cs
        TestNumberWithUnit_Portuguese.cs
        TestNumberWithUnit_Spanish.cs
````

The Test Classes names uses a convention to determine the Spec to run. The convention is:

`Test{Model}_{Language}`

And its Test Methods also have a convention in its name and attributes:

````C#
[DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "{Model}-{Language}.csv", "{Model}-{Language}#csv", DataAccessMethod.Sequential)]
[TestMethod]
public void {Model}()
{
    base.Test{ModelType}();
}
````

An example is the [`TestNumberWithUnit_Chinese`](../.NET/Microsoft.Recognizers.Text.DataDrivenTests/NumberWithUnit/TestNumberWithUnit_Chinese.cs) Class an its `AgeModel()` Test Method:

````C#
[DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "AgeModel-Chinese.csv", "AgeModel-Chinese#csv", DataAccessMethod.Sequential)]
[TestMethod]
public void AgeModel()
{
    base.TestNumberWithUnit();
}
````

### JavaScript

The JavaScript Test Runner will run the test cases and resolve the Extractor, Parser or Model to test based on the Spec file's path.

E.g:

 - [Specs/NumberWithUnit/English/TemperatureModel.json](../Specs/NumberWithUnit/English/TemperatureModel.json)

    Maps to the [NumberWithUnitRecognizer](../JavaScript/src/numberWithUnit/numberWithUnitRecognizer.ts) using the [English TemperatureModel](../JavaScript/src/numberWithUnit/english/dimension.ts).

 - [Specs/DateTime/English/DateTimeExtractor.json](../Specs/DateTime/English/DateTimeExtractor.json)

    Maps to the [BaseDateTimeExtractor](../JavaScript/src/dateTime/baseDateTime.ts#L28) and uses the [EnglishDateTimeExtractorConfiguration](../JavaScript/src/dateTime/english/dateTimeConfiguration.ts#L16)

 - [Specs/DateTime/English/DateTimeParser.json](../Specs/DateTime/English/DateTimeParser.json)

    Maps to the [BaseDateTimeParser](../JavaScript/src/dateTime/baseDateTime.ts#L204) and uses the [EnglishDateTimeParserConfiguration](../JavaScript/src/dateTime/english/dateTimeConfiguration.ts#L60)

## Running Tests

### .NET

Test Suites and Test Cases can be run from Visual Studio using the default Test Runner (from the Test menu \ Debug \ All Tests) or through the Test Explorer panel.

An alternative is to run the tests from the command line:

  1. Open the "Developer Command Prompt for Visual Studio" (from Windows' Start menu)
  2. CD to the location of the Microsoft.Recognizers.Text .NET solution
  3. Execute the VS Test console:

      ````
      > vstest.console.exe
      ````

### JavaScript

The JavaScript project uses [AVA Test Runner](https://github.com/avajs/ava). It is already included in the `devDependencies` and tests can be run with `npm run test`.

If AVA is globally installed, they can be triggered by invoking `ava`.

## Debugging Test Cases

It will come a moment were it is needed to debug a bunch of tests or a specific test case. This can be accomplished by adding the `Debug` attribute to the test case.

When launching the Test Runner with the Debugg attached, the test case will trigger a Debug Break before invoking the extraction or parsing process.

### .NET

By running the tests from Visual Studio with the Debug option (Test menu \ Debug \ All Tests), the debugger will break just before calling the extractor or parser. 
From there it should be straighforward to step-into the parser or extractor execution.

Using the Test Explorer it is possible to just trigger and debug a Test Suite.

### JavaScript

JavaScript tests can be debugged from within Visual Studio Code.

The project is already configured with a *Debug Tests* launch task, just press F5 (Start Debugging) and Visual Studio Code will run and attach to the AVA Test Runner. 
Once the test case with the `Debug` attribute is reached, it should be straighforward to step-into the parser or extractor execution.

## Troubleshooting

## Javascript build - Python version not supported by gyp

If you get an error like:

gyp ERR! configure error
gyp ERR! stack Error: Python executable "D:\Apps\Python\Python3\python.EXE" is v3.4.1, which is not supported by gyp.
gyp ERR! stack You can pass the --python switch to point to Python >= v2.5.0 & < 3.0.0.

Please make sure you have node v6.9.1+ and python 3.4.4+, this combination should work.
Remove the node_modules directories under Javacript and the sub-projects and build again.
