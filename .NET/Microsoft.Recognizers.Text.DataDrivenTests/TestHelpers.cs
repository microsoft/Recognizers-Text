using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DateObject = System.DateTime;

using Microsoft.Recognizers.Text.DateTime;
using Microsoft.Recognizers.Text.DateTime.English;
using Microsoft.Recognizers.Text.DateTime.Spanish;
using Microsoft.Recognizers.Text.DateTime.Portuguese;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.NumberWithUnit;
using Microsoft.Recognizers.Text.DateTime.French;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Recognizers.Text.DateTime.German;
using Microsoft.Recognizers.Text.Sequence;
using Microsoft.Recognizers.Text.Choice;

using Newtonsoft.Json;

namespace Microsoft.Recognizers.Text.DataDrivenTests
{
    public class TestResources : Dictionary<string, IList<TestModel>> { }

    public static class TestResourcesExtensions
    {
        public static void InitFromTestContext(this TestResources resources, TestContext context)
        {
            var classNameIndex = context.FullyQualifiedTestClassName.LastIndexOf('.');
            var className = context.FullyQualifiedTestClassName.Substring(classNameIndex + 1).Replace("Test", "");
            var recognizerLanguage = className.Split('_');

            var directorySpecs = Path.Combine("..", "..", "..", "..", "Specs", recognizerLanguage[0], recognizerLanguage[1]);

            var specsFiles = Directory.GetFiles(directorySpecs);
            foreach (var specsFile in specsFiles)
            {
                var fileName = Path.GetFileNameWithoutExtension(specsFile) + "-" + recognizerLanguage[1];
                var rawData = File.ReadAllText(specsFile);
                var specs = JsonConvert.DeserializeObject<IList<TestModel>>(rawData);
                File.WriteAllText(fileName + ".csv", "Index" + Environment.NewLine + 
                                  string.Join(Environment.NewLine, Enumerable.Range(0, specs.Count).Select(o => o.ToString())));
                resources.Add(Path.GetFileNameWithoutExtension(specsFile), specs);
            }
        }

        public static TestModel GetSpecForContext(this TestResources resources, TestContext context)
        {
            var index = Convert.ToInt32(context.DataRow[0]);
            return resources[context.TestName][index];
        }
    }

    public enum Models
    {
        Number,
        NumberPercentMode,
        Ordinal,
        Percent,
        PercentPercentMode,
        NumberRange,
        CustomNumber,
        Age,
        Currency,
        Dimension,
        Temperature,
        DateTime,
        DateTimeSplitDateAndTime,
        DateTimeCalendarMode,
        DateTimeExtendedTypes,
        DateTimeComplexCalendar,
        DateTimeExperimentalMode,
        PhoneNumber,
        IpAddress,
        Mention,
        Hashtag,
        Email,
        URL,
        Boolean,
    }

    public enum DateTimeExtractors
    {
        Date,
        Time,
        DatePeriod,
        TimePeriod,
        DateTime,
        DateTimePeriod,
        Duration,
        Holiday,
        TimeZone,
        Set,
        Merged,
        MergedSkipFromTo
    }

    public enum DateTimeParsers
    {
        Date,
        Time,
        DatePeriod,
        TimePeriod,
        DateTime,
        DateTimePeriod,
        Duration,
        Holiday,
        TimeZone,
        Set,
        Merged
    }

    public static class TestContextExtensions
    {
        private static IDictionary<Models, Func<TestModel, string, IList<ModelResult>>> modelFunctions = new Dictionary<Models, Func<TestModel, string, IList<ModelResult>>>() {
            { Models.Number, (test, culture) => NumberRecognizer.RecognizeNumber(test.Input, culture, fallbackToDefaultCulture: false) },
            { Models.NumberPercentMode, (test, culture) => NumberRecognizer.RecognizeNumber(test.Input, culture, NumberOptions.PercentageMode, fallbackToDefaultCulture: false) },
            { Models.Ordinal, (test, culture) => NumberRecognizer.RecognizeOrdinal(test.Input, culture, fallbackToDefaultCulture: false) },
            { Models.Percent, (test, culture) => NumberRecognizer.RecognizePercentage(test.Input, culture, fallbackToDefaultCulture: false)},
            { Models.PercentPercentMode, (test, culture) => NumberRecognizer.RecognizePercentage(test.Input, culture, NumberOptions.PercentageMode, fallbackToDefaultCulture: false)},
            { Models.NumberRange, (test, culture) => NumberRecognizer.RecognizeNumberRange(test.Input, culture, fallbackToDefaultCulture: false) },
            { Models.Age, (test, culture) => NumberWithUnitRecognizer.RecognizeAge(test.Input, culture, fallbackToDefaultCulture: false) },
            { Models.Currency, (test, culture) => NumberWithUnitRecognizer.RecognizeCurrency(test.Input, culture, fallbackToDefaultCulture: false) },
            { Models.Dimension, (test, culture) => NumberWithUnitRecognizer.RecognizeDimension(test.Input, culture, fallbackToDefaultCulture: false) },
            { Models.Temperature, (test, culture) => NumberWithUnitRecognizer.RecognizeTemperature(test.Input, culture, fallbackToDefaultCulture: false) },
            { Models.DateTime, (test, culture) => DateTimeRecognizer.RecognizeDateTime(test.Input, culture, refTime: test.GetReferenceDateTime(), fallbackToDefaultCulture: false) },
            { Models.DateTimeSplitDateAndTime, (test, culture) => DateTimeRecognizer.RecognizeDateTime(test.Input, culture, DateTimeOptions.SplitDateAndTime, refTime: test.GetReferenceDateTime(), fallbackToDefaultCulture: false) },
            { Models.DateTimeCalendarMode, (test, culture) => DateTimeRecognizer.RecognizeDateTime(test.Input, culture, DateTimeOptions.CalendarMode, refTime: test.GetReferenceDateTime(), fallbackToDefaultCulture: false) },
            { Models.DateTimeExtendedTypes, (test, culture) => DateTimeRecognizer.RecognizeDateTime(test.Input, culture, DateTimeOptions.ExtendedTypes, refTime: test.GetReferenceDateTime(), fallbackToDefaultCulture: false) },
            { Models.DateTimeComplexCalendar, (test, culture) => DateTimeRecognizer.RecognizeDateTime(test.Input, culture, DateTimeOptions.ExtendedTypes | DateTimeOptions.CalendarMode | DateTimeOptions.EnablePreview, refTime: test.GetReferenceDateTime(), fallbackToDefaultCulture: false) },
            { Models.DateTimeExperimentalMode, (test, culture) => DateTimeRecognizer.RecognizeDateTime(test.Input, culture, DateTimeOptions.ExperimentalMode, refTime: test.GetReferenceDateTime(), fallbackToDefaultCulture: false) },
            { Models.PhoneNumber, (test, culture) => SequenceRecognizer.RecognizePhoneNumber(test.Input, culture, fallbackToDefaultCulture: false) },
            { Models.IpAddress, (test, culture) => SequenceRecognizer.RecognizeIpAddress(test.Input, culture, fallbackToDefaultCulture: false) },
            { Models.Mention, (test, culture) => SequenceRecognizer.RecognizeMention(test.Input, culture, fallbackToDefaultCulture: false) },
            { Models.Hashtag, (test, culture) => SequenceRecognizer.RecognizeHashtag(test.Input, culture, fallbackToDefaultCulture: false) },
            { Models.Email, (test, culture) => SequenceRecognizer.RecognizeEmail(test.Input, culture, fallbackToDefaultCulture: false) },
            { Models.URL, (test, culture) => SequenceRecognizer.RecognizeURL(test.Input, culture, fallbackToDefaultCulture: false) },
            { Models.Boolean, (test, culture) => ChoiceRecognizer.RecognizeBoolean(test.Input, culture, fallbackToDefaultCulture: false) }
        };

        public static IList<ModelResult> GetModelParseResults(this TestContext context, TestModel test)
        {
            var culture = TestUtils.GetCulture(context.FullyQualifiedTestClassName);
            var modelName = TestUtils.GetModel(context.TestName);

            var modelFunction = modelFunctions[modelName];

            return modelFunction(test, culture);
        }
        
        public static IDateTimeExtractor GetExtractor(this TestContext context)
        {
            var culture = TestUtils.GetCulture(context.FullyQualifiedTestClassName);
            var extractorName = TestUtils.GetExtractor(context.TestName);
            switch (culture)
            {
                case Culture.English:
                    return GetEnglishExtractor(extractorName);
                case Culture.Spanish:
                    return GetSpanishExtractor(extractorName);
                case Culture.Portuguese:
                    return GetPortugueseExtractor(extractorName);
                case Culture.Chinese:
                    return GetChineseExtractor(extractorName);
                case Culture.French:
                    return GetFrenchExtractor(extractorName);
                case Culture.German:
                    return GetGermanExtractor(extractorName);
            }

            throw new Exception($"Extractor '{extractorName}' for '{culture}' not supported");
        }

        public static IDateTimeParser GetDateTimeParser(this TestContext context)
        {
            var culture = TestUtils.GetCulture(context.FullyQualifiedTestClassName);
            var parserName = TestUtils.GetParser(context.TestName);
            switch (culture)
            {
                case Culture.English:
                    return GetEnglishParser(parserName);
                case Culture.Spanish:
                    return GetSpanishParser(parserName);
                case Culture.Portuguese:
                    return GetPortugueseParser(parserName);
                case Culture.Chinese:
                    return GetChineseParser(parserName);
                case Culture.French:
                    return GetFrenchParser(parserName);
                case Culture.German:
                    return GetGermanParser(parserName);
            }

            throw new Exception($"Parser '{parserName}' for '{culture}' not supported");
        }

        public static IDateTimeExtractor GetEnglishExtractor(DateTimeExtractors extractorName)
        {
            switch (extractorName)
            {
                case DateTimeExtractors.Date:
                    return new BaseDateExtractor(new EnglishDateExtractorConfiguration());
                case DateTimeExtractors.Time:
                    return new BaseTimeExtractor(new EnglishTimeExtractorConfiguration());
                case DateTimeExtractors.DatePeriod:
                    return new BaseDatePeriodExtractor(new EnglishDatePeriodExtractorConfiguration());
                case DateTimeExtractors.TimePeriod:
                    return new BaseTimePeriodExtractor(new EnglishTimePeriodExtractorConfiguration());
                case DateTimeExtractors.DateTime:
                    return new BaseDateTimeExtractor(new EnglishDateTimeExtractorConfiguration());
                case DateTimeExtractors.DateTimePeriod:
                    return new BaseDateTimePeriodExtractor(new EnglishDateTimePeriodExtractorConfiguration());
                case DateTimeExtractors.Duration:
                    return new BaseDurationExtractor(new EnglishDurationExtractorConfiguration());
                case DateTimeExtractors.Holiday:
                    return new BaseHolidayExtractor(new EnglishHolidayExtractorConfiguration());
                case DateTimeExtractors.TimeZone:
                    return new BaseTimeZoneExtractor(new EnglishTimeZoneExtractorConfiguration(DateTimeOptions.EnablePreview));
                case DateTimeExtractors.Set:
                    return new BaseSetExtractor(new EnglishSetExtractorConfiguration());
                case DateTimeExtractors.Merged:
                    return new BaseMergedDateTimeExtractor(new EnglishMergedExtractorConfiguration(DateTimeOptions.None));
                case DateTimeExtractors.MergedSkipFromTo:
                    return new BaseMergedDateTimeExtractor(new EnglishMergedExtractorConfiguration(DateTimeOptions.SkipFromToMerge));
            }

            throw new Exception($"Extractor '{extractorName}' for English not supported");
        }

        public static IDateTimeParser GetEnglishParser(DateTimeParsers parserName)
        {
            var commonConfiguration = new EnglishCommonDateTimeParserConfiguration(DateTimeOptions.None);
            switch (parserName)
            {
                case DateTimeParsers.Date:
                    return new BaseDateParser(new EnglishDateParserConfiguration(commonConfiguration));
                case DateTimeParsers.Time:
                    return new DateTime.English.TimeParser(new EnglishTimeParserConfiguration(commonConfiguration));
                case DateTimeParsers.DatePeriod:
                    return new BaseDatePeriodParser(new EnglishDatePeriodParserConfiguration(commonConfiguration));
                case DateTimeParsers.TimePeriod:
                    return new BaseTimePeriodParser(new EnglishTimePeriodParserConfiguration(commonConfiguration));
                case DateTimeParsers.DateTime:
                    return new BaseDateTimeParser(new EnglishDateTimeParserConfiguration(commonConfiguration));
                case DateTimeParsers.DateTimePeriod:
                    return new BaseDateTimePeriodParser(new EnglishDateTimePeriodParserConfiguration(commonConfiguration));
                case DateTimeParsers.Duration:
                    return new BaseDurationParser(new EnglishDurationParserConfiguration(commonConfiguration));
                case DateTimeParsers.Holiday:
                    return new BaseHolidayParser(new EnglishHolidayParserConfiguration());
                case DateTimeParsers.TimeZone:
                    return new BaseTimeZoneParser();
                case DateTimeParsers.Set:
                    return new BaseSetParser(new EnglishSetParserConfiguration(commonConfiguration));
                case DateTimeParsers.Merged:
                    return new BaseMergedDateTimeParser(new EnglishMergedParserConfiguration(DateTimeOptions.None));
            }

            throw new Exception($"Parser '{parserName}' for English not supported");
        }

        public static IDateTimeExtractor GetChineseExtractor(DateTimeExtractors extractorName)
        {
            switch (extractorName)
            {
                case DateTimeExtractors.Date:
                    return new DateTime.Chinese.DateExtractorChs();
                case DateTimeExtractors.Time:
                    return new DateTime.Chinese.TimeExtractorChs();
                case DateTimeExtractors.DatePeriod:
                    return new DateTime.Chinese.DatePeriodExtractorChs();
                case DateTimeExtractors.TimePeriod:
                    return new DateTime.Chinese.TimePeriodExtractorChs();
                case DateTimeExtractors.DateTime:
                    return new DateTime.Chinese.DateTimeExtractorChs();
                case DateTimeExtractors.DateTimePeriod:
                    return new DateTime.Chinese.DateTimePeriodExtractorChs();
                case DateTimeExtractors.Duration:
                    return new DateTime.Chinese.DurationExtractorChs();
                case DateTimeExtractors.Holiday:
                    return new BaseHolidayExtractor(new DateTime.Chinese.ChineseHolidayExtractorConfiguration());
                case DateTimeExtractors.Set:
                    return new DateTime.Chinese.SetExtractorChs();
                case DateTimeExtractors.Merged:
                    return new DateTime.Chinese.MergedExtractorChs(DateTimeOptions.None);
                case DateTimeExtractors.MergedSkipFromTo:
                    return new DateTime.Chinese.MergedExtractorChs(DateTimeOptions.SkipFromToMerge);
            }

            throw new Exception($"Extractor '{extractorName}' for English not supported");
        }

        public static IDateTimeParser GetChineseParser(DateTimeParsers parserName)
        {
            //var commonConfiguration = new EnglishCommonDateTimeParserConfiguration();
            switch (parserName)
            {
                case DateTimeParsers.Date:
                    return new DateTime.Chinese.DateParser(new DateTime.Chinese.ChineseDateTimeParserConfiguration());
                case DateTimeParsers.Time:
                    return new DateTime.Chinese.TimeParserChs(new DateTime.Chinese.ChineseDateTimeParserConfiguration());
                case DateTimeParsers.DatePeriod:
                    return new DateTime.Chinese.DatePeriodParserChs(new DateTime.Chinese.ChineseDateTimeParserConfiguration());
                case DateTimeParsers.TimePeriod:
                    return new DateTime.Chinese.TimePeriodParserChs(new DateTime.Chinese.ChineseDateTimeParserConfiguration());
                case DateTimeParsers.DateTime:
                    return new DateTime.Chinese.DateTimeParserChs(new DateTime.Chinese.ChineseDateTimeParserConfiguration());
                case DateTimeParsers.DateTimePeriod:
                    return new DateTime.Chinese.DateTimePeriodParserChs(new DateTime.Chinese.ChineseDateTimeParserConfiguration());
                case DateTimeParsers.Duration:
                    return new DateTime.Chinese.DurationParserChs(new DateTime.Chinese.ChineseDateTimeParserConfiguration());
                case DateTimeParsers.Holiday:
                    return new DateTime.Chinese.HolidayParserChs(new DateTime.Chinese.ChineseDateTimeParserConfiguration());
                case DateTimeParsers.Set:
                    return new DateTime.Chinese.SetParserChs(new DateTime.Chinese.ChineseDateTimeParserConfiguration());
                case DateTimeParsers.Merged:
                    return new FullDateTimeParser(new DateTime.Chinese.ChineseDateTimeParserConfiguration() );
            }

            throw new Exception($"Parser '{parserName}' for English not supported");
        }

        public static IDateTimeExtractor GetSpanishExtractor(DateTimeExtractors extractorName)
        {
            switch (extractorName)
            {
                case DateTimeExtractors.Date:
                    return new BaseDateExtractor(new SpanishDateExtractorConfiguration());
                case DateTimeExtractors.Time:
                    return new BaseTimeExtractor(new SpanishTimeExtractorConfiguration());
                case DateTimeExtractors.DatePeriod:
                    return new BaseDatePeriodExtractor(new SpanishDatePeriodExtractorConfiguration());
                case DateTimeExtractors.TimePeriod:
                    return new BaseTimePeriodExtractor(new SpanishTimePeriodExtractorConfiguration());
                case DateTimeExtractors.DateTime:
                    return new BaseDateTimeExtractor(new SpanishDateTimeExtractorConfiguration());
                case DateTimeExtractors.DateTimePeriod:
                    return new BaseDateTimePeriodExtractor(new SpanishDateTimePeriodExtractorConfiguration());
                case DateTimeExtractors.Duration:
                    return new BaseDurationExtractor(new SpanishDurationExtractorConfiguration());
                case DateTimeExtractors.Holiday:
                    return new BaseHolidayExtractor(new SpanishHolidayExtractorConfiguration());
                case DateTimeExtractors.Set:
                    return new BaseSetExtractor(new SpanishSetExtractorConfiguration());
                case DateTimeExtractors.Merged:
                    return new BaseMergedDateTimeExtractor(new SpanishMergedExtractorConfiguration(DateTimeOptions.None));
            }

            throw new Exception($"Extractor '{extractorName}' for Spanish not supported");
        }

        public static IDateTimeParser GetSpanishParser(DateTimeParsers parserName)
        {
            var commonConfiguration = new SpanishCommonDateTimeParserConfiguration(DateTimeOptions.None);

            switch (parserName)
            {
                case DateTimeParsers.Date:
                    return new BaseDateParser(new SpanishDateParserConfiguration(commonConfiguration));
                case DateTimeParsers.Time:
                    return new BaseTimeParser(new SpanishTimeParserConfiguration(commonConfiguration));
                case DateTimeParsers.DatePeriod:
                    return new BaseDatePeriodParser(new SpanishDatePeriodParserConfiguration(commonConfiguration));
                case DateTimeParsers.TimePeriod:
                    return new BaseTimePeriodParser(new SpanishTimePeriodParserConfiguration(commonConfiguration));
                case DateTimeParsers.DateTime:
                    return new BaseDateTimeParser(new SpanishDateTimeParserConfiguration(commonConfiguration));
                case DateTimeParsers.DateTimePeriod:
                    return new DateTime.Spanish.DateTimePeriodParser(new SpanishDateTimePeriodParserConfiguration(commonConfiguration));
                case DateTimeParsers.Duration:
                    return new BaseDurationParser(new SpanishDurationParserConfiguration(commonConfiguration));
                case DateTimeParsers.Holiday:
                    return new BaseHolidayParser(new SpanishHolidayParserConfiguration());
                case DateTimeParsers.Set:
                    return new BaseSetParser(new SpanishSetParserConfiguration(commonConfiguration));
                case DateTimeParsers.Merged:
                    return new BaseMergedDateTimeParser(new SpanishMergedParserConfiguration(DateTimeOptions.None));
            }

            throw new Exception($"Parser '{parserName}' for Spanish not supported");
        }

        public static IDateTimeExtractor GetPortugueseExtractor(DateTimeExtractors extractorName)
        {
            switch (extractorName)
            {
                case DateTimeExtractors.Date:
                    return new BaseDateExtractor(new PortugueseDateExtractorConfiguration());
                case DateTimeExtractors.Time:
                    return new BaseTimeExtractor(new PortugueseTimeExtractorConfiguration());
                case DateTimeExtractors.DatePeriod:
                    return new BaseDatePeriodExtractor(new PortugueseDatePeriodExtractorConfiguration());
                case DateTimeExtractors.TimePeriod:
                    return new BaseTimePeriodExtractor(new PortugueseTimePeriodExtractorConfiguration());
                case DateTimeExtractors.DateTime:
                    return new BaseDateTimeExtractor(new PortugueseDateTimeExtractorConfiguration());
                case DateTimeExtractors.DateTimePeriod:
                    return new BaseDateTimePeriodExtractor(new PortugueseDateTimePeriodExtractorConfiguration());
                case DateTimeExtractors.Duration:
                    return new BaseDurationExtractor(new PortugueseDurationExtractorConfiguration());
                case DateTimeExtractors.Holiday:
                    return new BaseHolidayExtractor(new PortugueseHolidayExtractorConfiguration());
                case DateTimeExtractors.Set:
                    return new BaseSetExtractor(new PortugueseSetExtractorConfiguration());
                case DateTimeExtractors.Merged:
                    return new BaseMergedDateTimeExtractor(new PortugueseMergedExtractorConfiguration(DateTimeOptions.None));
            }

            throw new Exception($"Extractor '{extractorName}' for Portuguese not supported");
        }

        public static IDateTimeParser GetPortugueseParser(DateTimeParsers parserName)
        {
            var commonConfiguration = new PortugueseCommonDateTimeParserConfiguration(DateTimeOptions.None);

            switch (parserName)
            {
                case DateTimeParsers.Date:
                    return new BaseDateParser(new PortugueseDateParserConfiguration(commonConfiguration));
                case DateTimeParsers.Time:
                    return new BaseTimeParser(new PortugueseTimeParserConfiguration(commonConfiguration));
                case DateTimeParsers.DatePeriod:
                    return new BaseDatePeriodParser(new PortugueseDatePeriodParserConfiguration(commonConfiguration));
                case DateTimeParsers.TimePeriod:
                    return new BaseTimePeriodParser(new PortugueseTimePeriodParserConfiguration(commonConfiguration));
                case DateTimeParsers.DateTime:
                    return new BaseDateTimeParser(new PortugueseDateTimeParserConfiguration(commonConfiguration));
                case DateTimeParsers.DateTimePeriod:
                    return new DateTime.Portuguese.DateTimePeriodParser(new PortugueseDateTimePeriodParserConfiguration(commonConfiguration));
                case DateTimeParsers.Duration:
                    return new BaseDurationParser(new PortugueseDurationParserConfiguration(commonConfiguration));
                case DateTimeParsers.Holiday:
                    return new BaseHolidayParser(new PortugueseHolidayParserConfiguration());
                case DateTimeParsers.Set:
                    return new BaseSetParser(new PortugueseSetParserConfiguration(commonConfiguration));
                case DateTimeParsers.Merged:
                    return new BaseMergedDateTimeParser(new PortugueseMergedParserConfiguration(DateTimeOptions.None));
            }

            throw new Exception($"Parser '{parserName}' for Portuguese not supported");
        }

        public static IDateTimeExtractor GetFrenchExtractor(DateTimeExtractors extractorName)
        {
            switch (extractorName)
            {
                case DateTimeExtractors.Date:
                    return new BaseDateExtractor(new FrenchDateExtractorConfiguration());
                case DateTimeExtractors.Time:
                    return new BaseTimeExtractor(new FrenchTimeExtractorConfiguration());
                case DateTimeExtractors.DatePeriod:
                    return new BaseDatePeriodExtractor(new FrenchDatePeriodExtractorConfiguration());
                case DateTimeExtractors.TimePeriod:
                    return new BaseTimePeriodExtractor(new FrenchTimePeriodExtractorConfiguration());
                case DateTimeExtractors.DateTime:
                    return new BaseDateTimeExtractor(new FrenchDateTimeExtractorConfiguration());
                case DateTimeExtractors.DateTimePeriod:
                    return new BaseDateTimePeriodExtractor(new FrenchDateTimePeriodExtractorConfiguration());
                case DateTimeExtractors.Duration:
                    return new BaseDurationExtractor(new FrenchDurationExtractorConfiguration());
                case DateTimeExtractors.Holiday:
                    return new BaseHolidayExtractor(new FrenchHolidayExtractorConfiguration());
                case DateTimeExtractors.Set:
                    return new BaseSetExtractor(new FrenchSetExtractorConfiguration());
                case DateTimeExtractors.Merged:
                    return new BaseMergedDateTimeExtractor(new FrenchMergedExtractorConfiguration(DateTimeOptions.None));
                case DateTimeExtractors.MergedSkipFromTo:
                    return new BaseMergedDateTimeExtractor(new FrenchMergedExtractorConfiguration(DateTimeOptions.SkipFromToMerge));
            }

            throw new Exception($"Extractor '{extractorName}' for French not supported");
        }

        public static IDateTimeParser GetFrenchParser(DateTimeParsers parserName)
        {
            var commonConfiguration = new FrenchCommonDateTimeParserConfiguration(DateTimeOptions.None);
            switch (parserName)
            {
                case DateTimeParsers.Date:
                    return new BaseDateParser(new FrenchDateParserConfiguration(commonConfiguration));
                case DateTimeParsers.Time:
                    return new DateTime.French.TimeParser(new FrenchTimeParserConfiguration(commonConfiguration));
                case DateTimeParsers.DatePeriod:
                    return new BaseDatePeriodParser(new FrenchDatePeriodParserConfiguration(commonConfiguration));
                case DateTimeParsers.TimePeriod:
                    return new BaseTimePeriodParser(new FrenchTimePeriodParserConfiguration(commonConfiguration));
                case DateTimeParsers.DateTime:
                    return new BaseDateTimeParser(new FrenchDateTimeParserConfiguration(commonConfiguration));
                case DateTimeParsers.DateTimePeriod:
                    return new BaseDateTimePeriodParser(new FrenchDateTimePeriodParserConfiguration(commonConfiguration));
                case DateTimeParsers.Duration:
                    return new BaseDurationParser(new FrenchDurationParserConfiguration(commonConfiguration));
                case DateTimeParsers.Holiday:
                    return new BaseHolidayParser(new FrenchHolidayParserConfiguration());
                case DateTimeParsers.Set:
                    return new BaseSetParser(new FrenchSetParserConfiguration(commonConfiguration));
                case DateTimeParsers.Merged:
                    return new BaseMergedDateTimeParser(new FrenchMergedParserConfiguration(DateTimeOptions.None));
            }

            throw new Exception($"Parser '{parserName}' for French not supported");
        }

        public static IDateTimeExtractor GetGermanExtractor(DateTimeExtractors extractorName)
        {
            switch (extractorName)
            {
                case DateTimeExtractors.Date:
                    return new BaseDateExtractor(new GermanDateExtractorConfiguration());
                case DateTimeExtractors.Time:
                    return new BaseTimeExtractor(new GermanTimeExtractorConfiguration());
                case DateTimeExtractors.DatePeriod:
                    return new BaseDatePeriodExtractor(new GermanDatePeriodExtractorConfiguration());
                case DateTimeExtractors.TimePeriod:
                    return new BaseTimePeriodExtractor(new GermanTimePeriodExtractorConfiguration());
                case DateTimeExtractors.DateTime:
                    return new BaseDateTimeExtractor(new GermanDateTimeExtractorConfiguration());
                case DateTimeExtractors.DateTimePeriod:
                    return new BaseDateTimePeriodExtractor(new GermanDateTimePeriodExtractorConfiguration());
                case DateTimeExtractors.Duration:
                    return new BaseDurationExtractor(new GermanDurationExtractorConfiguration());
                case DateTimeExtractors.Holiday:
                    return new BaseHolidayExtractor(new GermanHolidayExtractorConfiguration());
                case DateTimeExtractors.Set:
                    return new BaseSetExtractor(new GermanSetExtractorConfiguration());
                case DateTimeExtractors.Merged:
                    return new BaseMergedDateTimeExtractor(new GermanMergedExtractorConfiguration(DateTimeOptions.None));
                case DateTimeExtractors.MergedSkipFromTo:
                    return new BaseMergedDateTimeExtractor(new GermanMergedExtractorConfiguration(DateTimeOptions.SkipFromToMerge));
            }

            throw new Exception($"Extractor '{extractorName}' for German not supported");
        }

        public static IDateTimeParser GetGermanParser(DateTimeParsers parserName)
        {
            var commonConfiguration = new GermanCommonDateTimeParserConfiguration(DateTimeOptions.None);
            switch (parserName)
            {
                case DateTimeParsers.Date:
                    return new BaseDateParser(new GermanDateParserConfiguration(commonConfiguration));
                case DateTimeParsers.Time:
                    return new DateTime.French.TimeParser(new GermanTimeParserConfiguration(commonConfiguration));
                case DateTimeParsers.DatePeriod:
                    return new BaseDatePeriodParser(new GermanDatePeriodParserConfiguration(commonConfiguration));
                case DateTimeParsers.TimePeriod:
                    return new BaseTimePeriodParser(new GermanTimePeriodParserConfiguration(commonConfiguration));
                case DateTimeParsers.DateTime:
                    return new BaseDateTimeParser(new GermanDateTimeParserConfiguration(commonConfiguration));
                case DateTimeParsers.DateTimePeriod:
                    return new BaseDateTimePeriodParser(new GermanDateTimePeriodParserConfiguration(commonConfiguration));
                case DateTimeParsers.Duration:
                    return new BaseDurationParser(new GermanDurationParserConfiguration(commonConfiguration));
                case DateTimeParsers.Holiday:
                    return new BaseHolidayParser(new GermanHolidayParserConfiguration());
                case DateTimeParsers.Set:
                    return new BaseSetParser(new GermanSetParserConfiguration(commonConfiguration));
                case DateTimeParsers.Merged:
                    return new BaseMergedDateTimeParser(new GermanMergedParserConfiguration(DateTimeOptions.None));
            }

            throw new Exception($"Parser '{parserName}' for German not supported");
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

            if (testSpec.Context.TryGetValue("ReferenceDateTime", out object dateTimeObject))
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
            return Culture.SupportedCultures.First(c => c.CultureName == langStr).CultureCode;
        }

        public static bool EvaluateSpec(TestModel spec, out string message)
        {
            if (string.IsNullOrEmpty(spec.Input))
            {
                message = $"spec not found";
                return true;
            }

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

        public static string SanitizeSourceName(string source)
        {
            return source.Replace("Model", "").Replace("Extractor", "").Replace("Parser", "");
        }

        public static Models GetModel(string source)
        {
            var model = SanitizeSourceName(source);
            Models modelEnum = Models.Number;
            if (Enum.TryParse(model, out modelEnum))
            {
                return modelEnum;
            }

            throw new Exception($"Model '{model}' not supported");
        }

        public static DateTimeParsers GetParser(string source)
        {
            var parser = SanitizeSourceName(source);
            DateTimeParsers parserEnum = DateTimeParsers.Date;
            if (Enum.TryParse(parser, out parserEnum))
            {
                return parserEnum;
            }

            throw new Exception($"Parser '{parser}' not supported");
        }

        public static DateTimeExtractors GetExtractor(string source)
        {
            var extractor = SanitizeSourceName(source);
            DateTimeExtractors extractorEnum = DateTimeExtractors.Date;
            if (Enum.TryParse(extractor, out extractorEnum))
            {
                return extractorEnum;
            }

            throw new Exception($"Extractor '{extractor}' not supported");
        }
    }

    public static class RecognizerExtensions
    {
        public static ConcurrentDictionary<(string culture, Type modelType, string modelOptions), IModel> GetInternalCache<TRecognizerOptions>(this Recognizer<TRecognizerOptions> source) where TRecognizerOptions : struct
        {
            var modelFactoryProp = typeof(Recognizer<TRecognizerOptions>).GetField("factory", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var modelFactory = modelFactoryProp.GetValue(source);
            var cacheProp = modelFactory.GetType().GetField("cache", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            return cacheProp.GetValue(modelFactory) as ConcurrentDictionary<(string culture, Type modelType, string modelOptions), IModel>;
        }
    }
}
