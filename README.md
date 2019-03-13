# Microsoft Recognizers Text Overview

![Build Status](https://msrasia.visualstudio.com/_apis/public/build/definitions/310c848f-b260-4305-9255-b97bfb69974b/116/badge)
![Build Status](https://ci.appveyor.com/api/projects/status/github/Microsoft/Recognizers-Text?branch=master&svg=true&passingText=all%20plats%20-%20OK)

Microsoft.Recognizers.Text provides robust recognition and resolution of entities like numbers, units, and date/time; expressed in multiple languages. Full support for Chinese, English, French, Spanish, Portuguese, and German. Partial support for Dutch, Japanese, and Korean. More on the way.

# Utilizing the Project

Microsoft.Recognizers.Text powers pre-built entities in both [**LUIS: Language Understanding Intelligent Service**](https://www.luis.ai/home) and [**Microsoft Bot Framework**](https://dev.botframework.com/); base entity types in [**Text Analytics Cognitive Service**](https://docs.microsoft.com/en-us/azure/cognitive-services/text-analytics/how-tos/text-analytics-how-to-entity-linking); and it is also available as standalone packages (for the base classes and the different entity recognizers).

The Microsoft.Recognizers.Text packages currently target four platforms:
* [C#/.NET](https://github.com/Microsoft/Recognizers-Text/tree/master/.NET) - **NuGet packages** available at: https://www.nuget.org/profiles/Recognizers.Text
* [JavaScript/TypeScript](https://github.com/Microsoft/Recognizers-Text/tree/master/JavaScript/packages/recognizers-text-suite) - **NPM packages** available at: https://www.npmjs.com/~recognizers.text
* [Python](https://github.com/Microsoft/Recognizers-Text/tree/master/Python) (in progress)
* [Java](https://github.com/Microsoft/Recognizers-Text/tree/master/Java) (in progress)

Contributions are greatly welcome! Both for fixes and extensions in the currently supported languages and for expansion to new ones.
Especially for Dutch, Italian, Japanese, Korean, and Turkish. More info below.

# Help

If you have any questions, please go ahead and [open an issue](https://github.com/Microsoft/Recognizers-Text/issues/new/choose), even if it's not an actual bug. Issues are an acceptable discussion forum as well.

# Contributing

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

Good starting points for contribution are:
* the list of [open issues](https://github.com/Microsoft/Recognizers-Text/issues) (especially those marked as ```help wanted```); 
* the json spec cases temporarily marked as ```NotSupported``` ([Specs](./Specs)); and
* translating json test spec cases that work in English, but don't yet exist in a target language.

The links below describe the project structure and provide both an overview and tips on how to contribute (altough some steps may have become a little out-of-date). Thank you!

* [Overview and language resources](https://blog.botframework.com/2018/01/24/contributing-luis-microsoft-recognizers-text-part-1/)
* [Implementing language specific behaviour](https://blog.botframework.com/2018/02/01/contributing-luis-microsoft-recognizers-text-part-2/)
* [Test specs and testing in general](https://blog.botframework.com/2018/02/12/contributing-luis-microsoft-recognizers-text-part-3/)

# Supported Entities across Cultures

The table below summarizes the currently supported entities. Support for English is usually more complete than others. The primary platform is .NET (shown in table) and support should propagate to the others.

| Entity Type       | EN      | ZH-CN   | NL    | FR     | DE    | IT      | JA     | KO     | PT     | ES      |
|:-----------------:|:-------:|:-------:|:-----:|:------:|:-----:|:-------:|:------:|:------:|:------:|:-------:| 
| Number (cardinal)    | ✓    | ✓       | ✓    | ✓     | ✓     | **SO**  | ✓      | ✓      | ✓     | ✓       |
| Ordinal              | ✓    | ✓       | ✓    | ✓     | ✓     | **SO**  | ✓      | **SO** | ✓      | ✓      |
| Percentage           | ✓    | ✓       | ✓    | ✓     | ✓     | **SO**  | ✓      | **SO** | ✓      | ✓      |
| Unit - Age           | ✓    | ✓       | ✓    | ✓     | ✓     | **SO**  | ✓      | **SO** | ✓      | ✓      |
| Unit - Currency      | ✓    | ✓       | ✓    | ✓     | ✓     | **SO**  | ✓      | :x:    | ✓      | ✓      |
| Unit - Dimensions    | ✓    | ✓       | ✓    | ✓     | ✓     | **SO**  | :x:    | :x:     | ✓     | ✓       | 
| Unit - Temperature   | ✓    | ✓       | ✓    | ✓     | ✓     | **SO**  | :x:    | :x:     | ✓     | ✓       | 
| Choice - Boolean     | ✓    | ✓       | ✓    | ✓     | ✓     | **SO**  | ✓      | :x:    | ✓      | ✓      | 
| Seq. - E-mail        | G    | G*       | G    | G      | G     | G       | G      | G      | G      | G       |
| Seq. - GUID          | G    | G        | G    | G      | G     | G       | G      | G      | G      | G       |
| Seq. - Social        | G    | G        | G    | G      | G     | G       | G      | G      | G      | G       |
| Seq. - IP Address    | G    | G        | G    | G      | G     | G       | G      | G      | G      | G       |
| Seq. - Phone Number  | G    | G        | G    | G      | G     | G       | G      | G      | G      | G       |
| Seq. - URL           | G    | G*       | G    | G      | G     | G       | G      | G      | G      | G       |
| DateTime (+subtypes) | ✓    | ✓       | **SP**    | ✓     | ✓ | **SO** | **SI min** | **SI min** | ✓  | ✓      | 
* G: Generic entity, not language-specific (* unicode TLDs not-supported);
* SO: Specs-only;
* SP: Partial specs;
* SI: Very initial specs.
