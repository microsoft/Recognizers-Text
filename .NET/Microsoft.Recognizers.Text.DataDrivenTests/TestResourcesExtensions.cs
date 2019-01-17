using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Microsoft.Recognizers.Text.DataDrivenTests
{
    public static class TestResourcesExtensions
    {
        public static void InitFromTestContext(this TestResources resources, TestContext context)
        {
            var classNameIndex = context.FullyQualifiedTestClassName.LastIndexOf('.');
            var className = context.FullyQualifiedTestClassName.Substring(classNameIndex + 1).Replace("Test", string.Empty);
            var recognizerLanguage = className.Split('_');

            var directorySpecs = Path.Combine("..", "..", "..", "..", "Specs", recognizerLanguage[0], recognizerLanguage[1]);

            var specsFiles = Directory.GetFiles(directorySpecs, "*.json");
            foreach (var specsFile in specsFiles)
            {
                var fileName = Path.GetFileNameWithoutExtension(specsFile) + "-" + recognizerLanguage[1];
                var rawData = File.ReadAllText(specsFile);
                var specs = JsonConvert.DeserializeObject<IList<TestModel>>(rawData);
                File.WriteAllText(fileName + ".csv", "Index" + Environment.NewLine + string.Join(Environment.NewLine, Enumerable.Range(0, specs.Count).Select(o => o.ToString())));
                resources.Add(Path.GetFileNameWithoutExtension(specsFile), specs);
            }
        }

        public static TestModel GetSpecForContext(this TestResources resources, TestContext context)
        {
            var index = Convert.ToInt32(context.DataRow[0]);
            return resources[context.TestName][index];
        }
    }
}