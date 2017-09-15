using Microsoft.Recognizers.Text.DateTime;
using Microsoft.Recognizers.Text.DateTime.English;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Chinese;
using Microsoft.Recognizers.Text.NumberWithUnit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DataDrivenTests
{
    public class TestResources : Dictionary<string, IList<TestModel>> { }

    public static class TestResourcesExtensions
    {
        public static void InitFromTestContext(this TestResources resources, TestContext context)
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
                case "AgeModel":
                    return NumberWithUnitRecognizer.Instance.GetAgeModel(language);
                case "CurrencyModel":
                    return NumberWithUnitRecognizer.Instance.GetCurrencyModel(language);
                case "DimensionModel":
                    return NumberWithUnitRecognizer.Instance.GetDimensionModel(language);
                case "TemperatureModel":
                    return NumberWithUnitRecognizer.Instance.GetTemperatureModel(language);
                case "CustomNumberModel":
                    return GetCustomModelFor(language);
            }
            return null;
        }

        public static IExtractor GetExtractor(this TestContext context)
        {
            var language = TestUtils.GetCulture(context.FullyQualifiedTestClassName);
            var extractorName = TestUtils.GetExtractorName(context.TestName);
            switch (language)
            {
                case Culture.English:
                    return GetEnglishExtractor(extractorName);
            }
            return null;
        }

        public static IDateTimeParser GetDateTimeParser(this TestContext context)
        {
            var language = TestUtils.GetCulture(context.FullyQualifiedTestClassName);
            var parserName = TestUtils.GetParserName(context.TestName);
            switch (language)
            {
                case Culture.English:
                    return GetEnglishParser(parserName);
            }
            return null;
        }

        public static IExtractor GetEnglishExtractor(string extractorName)
        {
            switch (extractorName)
            {
                case "Date":
                    return new BaseDateExtractor(new EnglishDateExtractorConfiguration());
                case "Time":
                    return new BaseTimeExtractor(new EnglishTimeExtractorConfiguration());
                case "DatePeriod":
                    return new BaseDatePeriodExtractor(new EnglishDatePeriodExtractorConfiguration());
                case "TimePeriod":
                    return new BaseTimePeriodExtractor(new EnglishTimePeriodExtractorConfiguration());
                case "DateTime":
                    return new BaseDateTimeExtractor(new EnglishDateTimeExtractorConfiguration());
                case "DateTimePeriod":
                    return new BaseDateTimePeriodExtractor(new EnglishDateTimePeriodExtractorConfiguration());
                case "Duration":
                    return new BaseDurationExtractor(new EnglishDurationExtractorConfiguration());
                case "Holiday":
                    return new BaseHolidayExtractor(new EnglishHolidayExtractorConfiguration());
                case "Set":
                    return new BaseSetExtractor(new EnglishSetExtractorConfiguration());
                case "Merged":
                    return new BaseMergedExtractor(new EnglishMergedExtractorConfiguration(), DateTimeOptions.None);
                case "MergedSkipFromTo":
                    return new BaseMergedExtractor(new EnglishMergedExtractorConfiguration(), DateTimeOptions.SkipFromToMerge);
            }
            return null;
        }

        public static IDateTimeParser GetEnglishParser(string parserName)
        {
            var commonConfiguration = new EnglishCommonDateTimeParserConfiguration();
            switch (parserName)
            {
                case "Date":
                    return new BaseDateParser(new EnglishDateParserConfiguration(commonConfiguration));
                case "Time":
                    return new TimeParser(new EnglishTimeParserConfiguration(commonConfiguration));
                case "DatePeriod":
                    return new BaseDatePeriodParser(new EnglishDatePeriodParserConfiguration(commonConfiguration));
                case "TimePeriod":
                    return new BaseTimePeriodParser(new EnglishTimePeriodParserConfiguration(commonConfiguration));
                case "DateTime":
                    return new BaseDateTimeParser(new EnglishDateTimeParserConfiguration(commonConfiguration));
                case "DateTimePeriod":
                    return new BaseDateTimePeriodParser(new EnglishDateTimePeriodParserConfiguration(commonConfiguration));
                case "Duration":
                    return new BaseDurationParser(new EnglishDurationParserConfiguration(commonConfiguration));
                case "Holiday":
                    return new BaseHolidayParser(new EnglishHolidayParserConfiguration());
                case "Set":
                    return new BaseSetParser(new EnglishSetParserConfiguration(commonConfiguration));
                case "Merged":
                    return new BaseMergedParser(new EnglishMergedParserConfiguration());
            }
            return null;
        }

        private static IModel GetCustomModelFor(string language)
        {
            switch (language)
            {
                case Culture.Chinese:
                    return new NumberModel(
                        AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new ChineseNumberParserConfiguration()),
                        new NumberExtractor(ChineseNumberMode.ExtractAll));
            }
            return null;
        }
    }

    public static class TestModelExtensions
    {
        public static bool IsNotSupported(this TestModel testSpec)
        {
            return testSpec.NotSupported.HasFlag(Platform.dotNet);
        }

        public static bool IsNotSupportedByDesign(this TestModel testSpec)
        {
            return testSpec.NotSupportedByDesign.HasFlag(Platform.dotNet);
        }

        public static DateObject GetReferenceDateTime(this TestModel testSpec)
        {
            object dateTimeObject;
            if (testSpec.Context.TryGetValue("ReferenceDateTime", out dateTimeObject))
            {
                return (DateObject)dateTimeObject;
            }
            return DateObject.Now;
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

        public static bool EvaluateSpec(TestModel spec, out string message)
        {
            if (spec.IsNotSupported())
            {
                message = $"input '{spec.Input}' not supported";
                return true;
            }
            if (spec.IsNotSupportedByDesign())
            {
                message = $"input '{spec.Input}' not supported by design";
                return true;
            }
            message = string.Empty;
            return false;
        }

        public static string GetModelName(string source)
        {
            return source;
        }

        public static string GetExtractorName(string source)
        {
            return source.Replace("Base", "").Replace("Extractor", "").Replace("Parser", "");
        }

        public static string GetParserName(string source)
        {
            return source.Replace("Base", "").Replace("Parser", "");
        }
    }
}
