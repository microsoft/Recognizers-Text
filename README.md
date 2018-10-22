# Microsoft Recognizers Text Overview

![Build Status](https://msrasia.visualstudio.com/_apis/public/build/definitions/310c848f-b260-4305-9255-b97bfb69974b/116/badge)
![Build Status](https://ci.appveyor.com/api/projects/status/github/Microsoft/Recognizers-Text?branch=master&svg=true&passingText=all%20plats%20-%20OK)

Microsoft.Recognizers.Text provides robust recognition and resolution of entities like numbers, units, and date/time; expressed in multiple languages. Full support for Chinese, English, French, Spanish, and Portuguese. Partial support for German, Japanese, Korean, and Dutch. More on the way.

# Utilizing the Project

Microsoft.Recognizers.Text powers pre-built entities in both [**LUIS: Language Understanding Intelligent Service**](https://www.luis.ai/home) and [**Microsoft Bot Framework**](https://dev.botframework.com/); and is also available as standalone packages (for the base classes and the different entity recognizers).

The Microsoft.Recognizers.Text packages currently target four platforms:
* [C#/.NET](https://github.com/Microsoft/Recognizers-Text/tree/master/.NET) - **NuGet packages** available at: https://www.nuget.org/profiles/Recognizers.Text
* [JavaScript/TypeScript](https://github.com/Microsoft/Recognizers-Text/tree/master/JavaScript/packages/recognizers-text-suite) - **NPM packages** available at: https://www.npmjs.com/~recognizers.text
* [Python](https://github.com/Microsoft/Recognizers-Text/tree/master/Python) (in progress)
* [Java](https://github.com/Microsoft/Recognizers-Text/tree/master/Java) (in progress)

Contributions are greatly welcome! Both for fixes and extensions in the currently supported languages and for expansion to new ones.
Especially for Japanese, Italian, Korean, and Dutch. More info below.

# Contributing

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

Good stating points for contribution are:
* the list of [open issues](https://github.com/Microsoft/Recognizers-Text/issues) (especially those marked as ```help wanted```); 
* the json spec cases temporarily marked as ```NotSupported``` ([Specs](./Specs)); and
* translating json test spec cases that work in English, but don't yet exist in a target language.

The links below decribe the project structure and provide both an overview and tips on how to contribute. Thank you!

* [Overview and language resources](https://blog.botframework.com/2018/01/24/contributing-luis-microsoft-recognizers-text-part-1/)
* [Implementing language specific behaviour](https://blog.botframework.com/2018/02/01/contributing-luis-microsoft-recognizers-text-part-2/)
* [Test specs and testing in general](https://blog.botframework.com/2018/02/12/contributing-luis-microsoft-recognizers-text-part-3/)
