using Microsoft.Recognizers.Text.Number;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Recognizers.Text.DataDrivenTests
{
    public class TestResources : Dictionary<string, IList<TestModel>> { }

    public static class TestResourcesExtensions
    {
        public static void InitFromTextContext(this TestResources resources, TestContext context)
        {
            var classNameIndex = context.FullyQualifiedTestClassName.LastIndexOf('.');
            var className = context.FullyQualifiedTestClassName.Substring(classNameIndex + 1).Replace("Test", "");
            var recognizer_language = className.Split('_');
            var directorySpecs = Path.Combine("..", "..", "..", "..", "Specs", recognizer_language[0], recognizer_language[1]);
            var specsFiles = Directory.GetFiles(directorySpecs);
            foreach (var specsFile in specsFiles)
            {
                var fileName = Path.GetFileNameWithoutExtension(specsFile) + "-" + recognizer_language[1];
                var rawData = File.ReadAllText(specsFile);
                var specs = JsonConvert.DeserializeObject<IList<TestModel>>(rawData);
                File.WriteAllText(fileName + ".csv", string.Join(Environment.NewLine, Enumerable.Range(0, specs.Count).Select(o => o.ToString())));
                resources.Add(Path.GetFileNameWithoutExtension(specsFile), specs);
            }
        }

        public static TestModel GetSpecForContext(this TestResources resources, TestContext context)
        {
            var index = Convert.ToInt32(context.DataRow[0]);
            return resources[context.TestName][index];
        }
    }

    public static class TestContextExtensions
    {
        public static IModel GetModel(this TestContext context)
        {
            var language = TestUtils.GetCulture(context.FullyQualifiedTestClassName);
            var modelName = TestUtils.GetModelName(context.TestName);
            switch (modelName)
            {
                case "NumberModel":
                    return NumberRecognizer.Instance.GetNumberModel(language);
                case "OrdinalModel":
                    return NumberRecognizer.Instance.GetOrdinalModel(language);
                case "PercentModel":
                    return NumberRecognizer.Instance.GetPercentageModel(language);
            }
            return null;
        }
    }

    public static class TestUtils
    {
        public static string GetCulture(string source)
        {
            var langStr = source.Substring(source.LastIndexOf('_') + 1);
            switch (langStr)
            {
                case "Chs":
                    return Culture.Chinese;
                case "Spa":
                    return Culture.Spanish;
                case "Eng":
                    return Culture.English;
                case "Por":
                    return Culture.Portuguese;
                case "Fra":
                    return Culture.French;
                default:
                    return Culture.English;
            }
        }

        public static string GetModelName(string source)
        {
            return source;
        }
    }
}
