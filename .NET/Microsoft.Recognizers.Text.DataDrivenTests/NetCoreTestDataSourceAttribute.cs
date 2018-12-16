using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DateObject = System.DateTime;

using Microsoft.Recognizers.Text.Choice;
using Microsoft.Recognizers.Text.DateTime;
using Microsoft.Recognizers.Text.DateTime.English;
using Microsoft.Recognizers.Text.DateTime.French;
using Microsoft.Recognizers.Text.DateTime.German;
using Microsoft.Recognizers.Text.DateTime.Italian;
using Microsoft.Recognizers.Text.DateTime.Spanish;
using Microsoft.Recognizers.Text.DateTime.Portuguese;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.NumberWithUnit;
using Microsoft.Recognizers.Text.Sequence;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;
using System.Reflection;

namespace Microsoft.Recognizers.Text.DataDrivenTests
{
    /// <summary>
    /// This class replaces the DataSourceAttribute which is not available in .Net Core
    /// See https://github.com/Microsoft/testfx/issues/233
    /// </summary>
    public class NetCoreTestDataSourceAttribute : Attribute, ITestDataSource
    {
        public IEnumerable<object[]> GetData(MethodInfo methodInfo)
        {
            var fullyQualifiedTestClassName = methodInfo.DeclaringType.FullName;
            var testName = methodInfo.Name;

            var classNameIndex = fullyQualifiedTestClassName.LastIndexOf('.');
            var className = fullyQualifiedTestClassName.Substring(classNameIndex + 1).Replace("Test", "");
            var recognizerLanguage = className.Split('_');

            var directorySpecs = Path.Combine("..", "..", "..", "..", "..", "Specs", recognizerLanguage[0], recognizerLanguage[1]);

            var specsFile = Path.Combine(directorySpecs, $"{testName}.json");
            var fileName = Path.GetFileNameWithoutExtension(specsFile) + "-" + recognizerLanguage[1];
            var rawData = File.ReadAllText(specsFile);
            var specs = JsonConvert.DeserializeObject<IList<TestModel>>(rawData);
            
            var data = specs.Select(spec => new[] { spec });
            return data;
        }

        public string GetDisplayName(MethodInfo methodInfo, object[] data)
        {
            return $"{methodInfo.DeclaringType.FullName} {methodInfo.Name}";
        }
    }

}