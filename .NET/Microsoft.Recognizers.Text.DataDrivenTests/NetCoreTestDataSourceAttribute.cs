using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Microsoft.Recognizers.Text.DataDrivenTests
{
    /// <summary>
    /// This class replaces the DataSourceAttribute which is not available in .Net Core
    /// See https://github.com/Microsoft/testfx/issues/233.
    /// </summary>
    public class NetCoreTestDataSourceAttribute : Attribute, ITestDataSource
    {
        public IEnumerable<object[]> GetData(MethodInfo methodInfo)
        {
            var fullyQualifiedTestClassName = methodInfo.DeclaringType.FullName;
            var testName = methodInfo.Name;

            var classNameIndex = fullyQualifiedTestClassName.LastIndexOf('.');
            var className = fullyQualifiedTestClassName.Substring(classNameIndex + 1).Replace("Test", string.Empty);
            var recognizerLanguage = className.Split('_');

            var directorySpecs = Path.Combine("..", "..", "..", "..", "..", "Specs", recognizerLanguage[0], recognizerLanguage[1]);

            var specsFile = Path.Combine(directorySpecs, $"{testName}.json");
            var fileName = Path.GetFileNameWithoutExtension(specsFile) + "-" + recognizerLanguage[1];
            var rawData = File.ReadAllText(specsFile);
            var specs = JsonConvert.DeserializeObject<IList<TestModel>>(rawData);

            // Adding a pseudo test case to avoid test failure causing by returning empty sequence.
            // https://github.com/microsoft/testfx-docs/blob/master/RFCs/005-Framework-Extensibility-Custom-DataSource.md#remarks
            if (!specs.Any())
            {
                var pseudoTest = new TestModel();
                pseudoTest.NotSupported = Platform.DotNet;
                specs.Add(pseudoTest);
            }

            var data = specs.Select(spec => new[] { spec });
            return data;
        }

        public string GetDisplayName(MethodInfo methodInfo, object[] data)
        {
            return $"{methodInfo.DeclaringType.FullName} {methodInfo.Name}";
        }
    }

}