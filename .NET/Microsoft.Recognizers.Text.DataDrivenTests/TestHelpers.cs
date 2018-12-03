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

            var specsFiles = Directory.GetFiles(directorySpecs, "*.json");
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
        NumberExperimentalMode,
        Ordinal,
        Percent,
        PercentPercentMode,
        NumberRange,
        NumberRangeExperimentalMode,
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
        GUID,
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
            { Models.NumberExperimentalMode, (test, culture) => NumberRecognizer.RecognizeNumber(test.Input, culture, NumberOptions.ExperimentalMode, fallbackToDefaultCulture: false) },
            { Models.Ordinal, (test, culture) => NumberRecognizer.RecognizeOrdinal(test.Input, culture, fallbackToDefaultCulture: false) },
            { Models.Percent, (test, culture) => NumberRecognizer.RecognizePercentage(test.Input, culture, fallbackToDefaultCulture: false)},
            { Models.PercentPercentMode, (test, culture) => NumberRecognizer.RecognizePercentage(test.Input, culture, NumberOptions.PercentageMode, fallbackToDefaultCulture: false)},
            { Models.NumberRange, (test, culture) => NumberRecognizer.RecognizeNumberRange(test.Input, culture, fallbackToDefaultCulture: false) },
            { Models.NumberRangeExperimentalMode, (test, culture) => NumberRecognizer.RecognizeNumberRange(test.Input, culture, NumberOptions.ExperimentalMode, fallbackToDefaultCulture: false) },
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
            { Models.GUID, (test, culture) => SequenceRecognizer.RecognizeGUID(test.Input, culture, fallbackToDefaultCulture: false) },
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
                case Culture.EnglishOthers:
                    return GetEnglishOthersExtractor(extractorName);
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
                case Culture.Italian:
                    return GetItalianExtractor(extractorName);
                case Culture.Japanese:
                    return GetJapaneseExtractor(extractorName);
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
                case Culture.EnglishOthers:
                    return GetEnglishOthersParser(parserName);
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
                case Culture.Italian:
                    return GetItalianParser(parserName);
                case Culture.Japanese:
                    return GetJapaneseParser(parserName);
      }

            throw new Exception($"Parser '{parserName}' for '{culture}' not supported");
        }

        public static IDateTimeExtractor GetEnglishExtractor(DateTimeExtractors extractorName)
        {
            var config = new BaseOptionsConfiguration();
            var previewConfig = new BaseOptionsConfiguration(DateTimeOptions.EnablePreview);
            switch (extractorName)
            {
                case DateTimeExtractors.Date:
                    return new BaseDateExtractor(new EnglishDateExtractorConfiguration(config));
                case DateTimeExtractors.Time:
                    return new BaseTimeExtractor(new EnglishTimeExtractorConfiguration(config));
                case DateTimeExtractors.DatePeriod:
                    return new BaseDatePeriodExtractor(new EnglishDatePeriodExtractorConfiguration(config));
                case DateTimeExtractors.TimePeriod:
                    return new BaseTimePeriodExtractor(new EnglishTimePeriodExtractorConfiguration(config));
                case DateTimeExtractors.DateTime:
                    return new BaseDateTimeExtractor(new EnglishDateTimeExtractorConfiguration(config));
                case DateTimeExtractors.DateTimePeriod:
                    return new BaseDateTimePeriodExtractor(new EnglishDateTimePeriodExtractorConfiguration(config));
                case DateTimeExtractors.Duration:
                    return new BaseDurationExtractor(new EnglishDurationExtractorConfiguration(config));
                case DateTimeExtractors.Holiday:
                    return new BaseHolidayExtractor(new EnglishHolidayExtractorConfiguration(config));
                case DateTimeExtractors.TimeZone:
                    return new BaseTimeZoneExtractor(new EnglishTimeZoneExtractorConfiguration(previewConfig));
                case DateTimeExtractors.Set:
                    return new BaseSetExtractor(new EnglishSetExtractorConfiguration(config));
                case DateTimeExtractors.Merged:
                    return new BaseMergedDateTimeExtractor(new EnglishMergedExtractorConfiguration(config));
                case DateTimeExtractors.MergedSkipFromTo:
                    return new BaseMergedDateTimeExtractor(new EnglishMergedExtractorConfiguration(new BaseOptionsConfiguration(DateTimeOptions.SkipFromToMerge)));
            }

            throw new Exception($"Extractor '{extractorName}' for English not supported");
        }

        public static IDateTimeParser GetEnglishParser(DateTimeParsers parserName)
        {
            var commonConfiguration = new EnglishCommonDateTimeParserConfiguration(new BaseOptionsConfiguration());
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
                    return new BaseHolidayParser(new EnglishHolidayParserConfiguration(commonConfiguration));
                case DateTimeParsers.TimeZone:
                    return new BaseTimeZoneParser();
                case DateTimeParsers.Set:
                    return new BaseSetParser(new EnglishSetParserConfiguration(commonConfiguration));
                case DateTimeParsers.Merged:
                    return new BaseMergedDateTimeParser(new EnglishMergedParserConfiguration(new BaseOptionsConfiguration()));
            }

            throw new Exception($"Parser '{parserName}' for English not supported");
        }

        public static IDateTimeExtractor GetEnglishOthersExtractor(DateTimeExtractors extractorName)
        {
            var enableDmyConfig = new BaseOptionsConfiguration(DateTimeOptions.None, true);
            var enableDmyPreviewConfig = new BaseOptionsConfiguration(DateTimeOptions.EnablePreview, true);
            switch (extractorName)
            {
                case DateTimeExtractors.Date:
                    return new BaseDateExtractor(new EnglishDateExtractorConfiguration(enableDmyConfig));
                case DateTimeExtractors.Time:
                    return new BaseTimeExtractor(new EnglishTimeExtractorConfiguration(enableDmyConfig));
                case DateTimeExtractors.DatePeriod:
                    return new BaseDatePeriodExtractor(new EnglishDatePeriodExtractorConfiguration(enableDmyConfig));
                case DateTimeExtractors.TimePeriod:
                    return new BaseTimePeriodExtractor(new EnglishTimePeriodExtractorConfiguration(enableDmyConfig));
                case DateTimeExtractors.DateTime:
                    return new BaseDateTimeExtractor(new EnglishDateTimeExtractorConfiguration(enableDmyConfig));
                case DateTimeExtractors.DateTimePeriod:
                    return new BaseDateTimePeriodExtractor(new EnglishDateTimePeriodExtractorConfiguration(enableDmyConfig));
                case DateTimeExtractors.Duration:
                    return new BaseDurationExtractor(new EnglishDurationExtractorConfiguration(enableDmyConfig));
                case DateTimeExtractors.Holiday:
                    return new BaseHolidayExtractor(new EnglishHolidayExtractorConfiguration(enableDmyConfig));
                case DateTimeExtractors.TimeZone:
                    return new BaseTimeZoneExtractor(new EnglishTimeZoneExtractorConfiguration(enableDmyPreviewConfig));
                case DateTimeExtractors.Set:
                    return new BaseSetExtractor(new EnglishSetExtractorConfiguration(enableDmyConfig));
                case DateTimeExtractors.Merged:
                    return new BaseMergedDateTimeExtractor(new EnglishMergedExtractorConfiguration(enableDmyConfig));
                case DateTimeExtractors.MergedSkipFromTo:
                    return new BaseMergedDateTimeExtractor(new EnglishMergedExtractorConfiguration(new BaseOptionsConfiguration(DateTimeOptions.SkipFromToMerge, true)));
            }

            throw new Exception($"Extractor '{extractorName}' for English-Others not supported");
        }

        public static IDateTimeParser GetEnglishOthersParser(DateTimeParsers parserName)
        {
            var commonConfiguration = new EnglishCommonDateTimeParserConfiguration(new BaseOptionsConfiguration(DateTimeOptions.None, true));
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
                    return new BaseHolidayParser(new EnglishHolidayParserConfiguration(commonConfiguration));
                case DateTimeParsers.TimeZone:
                    return new BaseTimeZoneParser();
                case DateTimeParsers.Set:
                    return new BaseSetParser(new EnglishSetParserConfiguration(commonConfiguration));
                case DateTimeParsers.Merged:
                    return new BaseMergedDateTimeParser(new EnglishMergedParserConfiguration(new BaseOptionsConfiguration()));
            }

            throw new Exception($"Parser '{parserName}' for English-Others not supported");
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
                    return new FullDateTimeParser(new DateTime.Chinese.ChineseDateTimeParserConfiguration());
            }

            throw new Exception($"Parser '{parserName}' for English not supported");
        }

        public static IDateTimeExtractor GetJapaneseExtractor(DateTimeExtractors extractorName)
        {
          switch (extractorName)
          {
            case DateTimeExtractors.Date:
              return new DateTime.Japanese.DateExtractor();
            case DateTimeExtractors.Time:
              return new DateTime.Japanese.TimeExtractor();
            case DateTimeExtractors.DatePeriod:
              return new DateTime.Japanese.DatePeriodExtractor();
            case DateTimeExtractors.TimePeriod:
              return new DateTime.Japanese.TimePeriodExtractor();
            case DateTimeExtractors.DateTime:
              return new DateTime.Japanese.DateTimeExtractor();
            case DateTimeExtractors.DateTimePeriod:
              return new DateTime.Japanese.DateTimePeriodExtractor();
            case DateTimeExtractors.Duration:
              return new DateTime.Japanese.DurationExtractor();
            case DateTimeExtractors.Holiday:
              return new BaseHolidayExtractor(new DateTime.Japanese.JapaneseHolidayExtractorConfiguration());
            case DateTimeExtractors.Set:
              return new DateTime.Japanese.SetExtractor();
            case DateTimeExtractors.Merged:
              return new DateTime.Japanese.JapaneseMergedExtractor(DateTimeOptions.None);
            case DateTimeExtractors.MergedSkipFromTo:
              return new DateTime.Japanese.JapaneseMergedExtractor(DateTimeOptions.SkipFromToMerge);
          }

          throw new Exception($"Extractor '{extractorName}' for Japanese not supported");
        }

        public static IDateTimeParser GetJapaneseParser(DateTimeParsers parserName)
        {
          switch (parserName)
          {
            case DateTimeParsers.Date:
              return new DateTime.Japanese.DateParser(new DateTime.Japanese.JapaneseDateTimeParserConfiguration());
            case DateTimeParsers.Time:
              return new DateTime.Japanese.TimeParser(new DateTime.Japanese.JapaneseDateTimeParserConfiguration());
            case DateTimeParsers.DatePeriod:
              return new DateTime.Japanese.DatePeriodParser(new DateTime.Japanese.JapaneseDateTimeParserConfiguration());
            case DateTimeParsers.TimePeriod:
              return new DateTime.Japanese.TimePeriodParser(new DateTime.Japanese.JapaneseDateTimeParserConfiguration());
            case DateTimeParsers.DateTime:
              return new DateTime.Japanese.DateTimeParser(new DateTime.Japanese.JapaneseDateTimeParserConfiguration());
            case DateTimeParsers.DateTimePeriod:
              return new DateTime.Japanese.DateTimePeriodParser(new DateTime.Japanese.JapaneseDateTimeParserConfiguration());
            case DateTimeParsers.Duration:
              return new DateTime.Japanese.DurationParser(new DateTime.Japanese.JapaneseDateTimeParserConfiguration());
            case DateTimeParsers.Holiday:
              return new DateTime.Japanese.HolidayParser(new DateTime.Japanese.JapaneseDateTimeParserConfiguration());
            case DateTimeParsers.Set:
              return new DateTime.Japanese.SetParser(new DateTime.Japanese.JapaneseDateTimeParserConfiguration());
            case DateTimeParsers.Merged:
              return new FullDateTimeParser(new DateTime.Japanese.JapaneseDateTimeParserConfiguration());
          }

          throw new Exception($"Parser '{parserName}' for Japanese not supported");
        }

        public static IDateTimeExtractor GetSpanishExtractor(DateTimeExtractors extractorName)
        {
            var config = new BaseOptionsConfiguration(DateTimeOptions.None);
            switch (extractorName)
            {
                case DateTimeExtractors.Date:
                    return new BaseDateExtractor(new SpanishDateExtractorConfiguration(config));
                case DateTimeExtractors.Time:
                    return new BaseTimeExtractor(new SpanishTimeExtractorConfiguration(config));
                case DateTimeExtractors.DatePeriod:
                    return new BaseDatePeriodExtractor(new SpanishDatePeriodExtractorConfiguration(config));
                case DateTimeExtractors.TimePeriod:
                    return new BaseTimePeriodExtractor(new SpanishTimePeriodExtractorConfiguration(config));
                case DateTimeExtractors.DateTime:
                    return new BaseDateTimeExtractor(new SpanishDateTimeExtractorConfiguration(config));
                case DateTimeExtractors.DateTimePeriod:
                    return new BaseDateTimePeriodExtractor(new SpanishDateTimePeriodExtractorConfiguration(config));
                case DateTimeExtractors.Duration:
                    return new BaseDurationExtractor(new SpanishDurationExtractorConfiguration(config));
                case DateTimeExtractors.Holiday:
                    return new BaseHolidayExtractor(new SpanishHolidayExtractorConfiguration(config));
                case DateTimeExtractors.Set:
                    return new BaseSetExtractor(new SpanishSetExtractorConfiguration(config));
                case DateTimeExtractors.Merged:
                    return new BaseMergedDateTimeExtractor(new SpanishMergedExtractorConfiguration(DateTimeOptions.None));
            }

            throw new Exception($"Extractor '{extractorName}' for Spanish not supported");
        }

        public static IDateTimeParser GetSpanishParser(DateTimeParsers parserName)
        {
            var commonConfiguration = new SpanishCommonDateTimeParserConfiguration(new BaseOptionsConfiguration());

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
                    return new BaseHolidayParser(new SpanishHolidayParserConfiguration(commonConfiguration));
                case DateTimeParsers.Set:
                    return new BaseSetParser(new SpanishSetParserConfiguration(commonConfiguration));
                case DateTimeParsers.Merged:
                    return new BaseMergedDateTimeParser(new SpanishMergedParserConfiguration(commonConfiguration));
            }

            throw new Exception($"Parser '{parserName}' for Spanish not supported");
        }

        public static IDateTimeExtractor GetPortugueseExtractor(DateTimeExtractors extractorName)
        {
            var config = new BaseOptionsConfiguration();
            switch (extractorName)
            {
                case DateTimeExtractors.Date:
                    return new BaseDateExtractor(new PortugueseDateExtractorConfiguration(config));
                case DateTimeExtractors.Time:
                    return new BaseTimeExtractor(new PortugueseTimeExtractorConfiguration(config));
                case DateTimeExtractors.DatePeriod:
                    return new BaseDatePeriodExtractor(new PortugueseDatePeriodExtractorConfiguration(config));
                case DateTimeExtractors.TimePeriod:
                    return new BaseTimePeriodExtractor(new PortugueseTimePeriodExtractorConfiguration(config));
                case DateTimeExtractors.DateTime:
                    return new BaseDateTimeExtractor(new PortugueseDateTimeExtractorConfiguration(config));
                case DateTimeExtractors.DateTimePeriod:
                    return new BaseDateTimePeriodExtractor(new PortugueseDateTimePeriodExtractorConfiguration(config));
                case DateTimeExtractors.Duration:
                    return new BaseDurationExtractor(new PortugueseDurationExtractorConfiguration(config));
                case DateTimeExtractors.Holiday:
                    return new BaseHolidayExtractor(new PortugueseHolidayExtractorConfiguration(config));
                case DateTimeExtractors.Set:
                    return new BaseSetExtractor(new PortugueseSetExtractorConfiguration(config));
                case DateTimeExtractors.Merged:
                    return new BaseMergedDateTimeExtractor(new PortugueseMergedExtractorConfiguration(DateTimeOptions.None));
            }

            throw new Exception($"Extractor '{extractorName}' for Portuguese not supported");
        }

        public static IDateTimeParser GetPortugueseParser(DateTimeParsers parserName)
        {
            var commonConfiguration = new PortugueseCommonDateTimeParserConfiguration(new BaseOptionsConfiguration());

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
                    return new BaseHolidayParser(new PortugueseHolidayParserConfiguration(commonConfiguration));
                case DateTimeParsers.Set:
                    return new BaseSetParser(new PortugueseSetParserConfiguration(commonConfiguration));
                case DateTimeParsers.Merged:
                    return new BaseMergedDateTimeParser(new PortugueseMergedParserConfiguration(commonConfiguration));
            }

            throw new Exception($"Parser '{parserName}' for Portuguese not supported");
        }

        public static IDateTimeExtractor GetFrenchExtractor(DateTimeExtractors extractorName)
        {
            var config = new BaseOptionsConfiguration();
            switch (extractorName)
            {
                case DateTimeExtractors.Date:
                    return new BaseDateExtractor(new FrenchDateExtractorConfiguration(config));
                case DateTimeExtractors.Time:
                    return new BaseTimeExtractor(new FrenchTimeExtractorConfiguration(config));
                case DateTimeExtractors.DatePeriod:
                    return new BaseDatePeriodExtractor(new FrenchDatePeriodExtractorConfiguration(config));
                case DateTimeExtractors.TimePeriod:
                    return new BaseTimePeriodExtractor(new FrenchTimePeriodExtractorConfiguration(config));
                case DateTimeExtractors.DateTime:
                    return new BaseDateTimeExtractor(new FrenchDateTimeExtractorConfiguration(config));
                case DateTimeExtractors.DateTimePeriod:
                    return new BaseDateTimePeriodExtractor(new FrenchDateTimePeriodExtractorConfiguration(config));
                case DateTimeExtractors.Duration:
                    return new BaseDurationExtractor(new FrenchDurationExtractorConfiguration(config));
                case DateTimeExtractors.Holiday:
                    return new BaseHolidayExtractor(new FrenchHolidayExtractorConfiguration(config));
                case DateTimeExtractors.Set:
                    return new BaseSetExtractor(new FrenchSetExtractorConfiguration(config));
                case DateTimeExtractors.Merged:
                    return new BaseMergedDateTimeExtractor(new FrenchMergedExtractorConfiguration(DateTimeOptions.None));
                case DateTimeExtractors.MergedSkipFromTo:
                    return new BaseMergedDateTimeExtractor(new FrenchMergedExtractorConfiguration(DateTimeOptions.SkipFromToMerge));
            }

            throw new Exception($"Extractor '{extractorName}' for French not supported");
        }

        public static IDateTimeParser GetFrenchParser(DateTimeParsers parserName)
        {
            var commonConfiguration = new FrenchCommonDateTimeParserConfiguration(new BaseOptionsConfiguration());
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
                    return new BaseHolidayParser(new FrenchHolidayParserConfiguration(commonConfiguration));
                case DateTimeParsers.Set:
                    return new BaseSetParser(new FrenchSetParserConfiguration(commonConfiguration));
                case DateTimeParsers.Merged:
                    return new BaseMergedDateTimeParser(new FrenchMergedParserConfiguration(commonConfiguration));
            }

            throw new Exception($"Parser '{parserName}' for French not supported");
        }

        public static IDateTimeExtractor GetGermanExtractor(DateTimeExtractors extractorName)
        {
            var config = new BaseOptionsConfiguration();
            switch (extractorName)
            {
                case DateTimeExtractors.Date:
                    return new BaseDateExtractor(new GermanDateExtractorConfiguration(config));
                case DateTimeExtractors.Time:
                    return new BaseTimeExtractor(new GermanTimeExtractorConfiguration(config));
                case DateTimeExtractors.DatePeriod:
                    return new BaseDatePeriodExtractor(new GermanDatePeriodExtractorConfiguration(config));
                case DateTimeExtractors.TimePeriod:
                    return new BaseTimePeriodExtractor(new GermanTimePeriodExtractorConfiguration(config));
                case DateTimeExtractors.DateTime:
                    return new BaseDateTimeExtractor(new GermanDateTimeExtractorConfiguration(config));
                case DateTimeExtractors.DateTimePeriod:
                    return new BaseDateTimePeriodExtractor(new GermanDateTimePeriodExtractorConfiguration(config));
                case DateTimeExtractors.Duration:
                    return new BaseDurationExtractor(new GermanDurationExtractorConfiguration(config));
                case DateTimeExtractors.Holiday:
                    return new BaseHolidayExtractor(new GermanHolidayExtractorConfiguration(config));
                case DateTimeExtractors.Set:
                    return new BaseSetExtractor(new GermanSetExtractorConfiguration(config));
                case DateTimeExtractors.Merged:
                    return new BaseMergedDateTimeExtractor(new GermanMergedExtractorConfiguration(DateTimeOptions.None));
                case DateTimeExtractors.MergedSkipFromTo:
                    return new BaseMergedDateTimeExtractor(new GermanMergedExtractorConfiguration(DateTimeOptions.SkipFromToMerge));
            }

            throw new Exception($"Extractor '{extractorName}' for German not supported");
        }

        public static IDateTimeParser GetGermanParser(DateTimeParsers parserName)
        {
            var commonConfiguration = new GermanCommonDateTimeParserConfiguration(new BaseOptionsConfiguration());
            switch (parserName)
            {
                case DateTimeParsers.Date:
                    return new BaseDateParser(new GermanDateParserConfiguration(commonConfiguration));
                case DateTimeParsers.Time:
                    return new DateTime.German.TimeParser(new GermanTimeParserConfiguration(commonConfiguration));
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
                    return new HolidayParserGer(new GermanHolidayParserConfiguration(commonConfiguration));
                case DateTimeParsers.Set:
                    return new BaseSetParser(new GermanSetParserConfiguration(commonConfiguration));
                case DateTimeParsers.Merged:
                    return new BaseMergedDateTimeParser(new GermanMergedParserConfiguration(commonConfiguration));
            }

            throw new Exception($"Parser '{parserName}' for German not supported");
        }

        public static IDateTimeExtractor GetItalianExtractor(DateTimeExtractors extractorName)
        {
            var config = new BaseOptionsConfiguration();
            switch (extractorName)
            {
                case DateTimeExtractors.Date:
                    return new BaseDateExtractor(new ItalianDateExtractorConfiguration(config));
                case DateTimeExtractors.Time:
                    return new BaseTimeExtractor(new ItalianTimeExtractorConfiguration(config));
                case DateTimeExtractors.DatePeriod:
                    return new BaseDatePeriodExtractor(new ItalianDatePeriodExtractorConfiguration(config));
                case DateTimeExtractors.TimePeriod:
                    return new BaseTimePeriodExtractor(new ItalianTimePeriodExtractorConfiguration(config));
                case DateTimeExtractors.DateTime:
                    return new BaseDateTimeExtractor(new ItalianDateTimeExtractorConfiguration(config));
                case DateTimeExtractors.DateTimePeriod:
                    return new BaseDateTimePeriodExtractor(new ItalianDateTimePeriodExtractorConfiguration(config));
                case DateTimeExtractors.Duration:
                    return new BaseDurationExtractor(new ItalianDurationExtractorConfiguration(config));
                case DateTimeExtractors.Holiday:
                    return new BaseHolidayExtractor(new ItalianHolidayExtractorConfiguration(config));
                case DateTimeExtractors.Set:
                    return new BaseSetExtractor(new ItalianSetExtractorConfiguration(config));
                case DateTimeExtractors.Merged:
                    return new BaseMergedDateTimeExtractor(new ItalianMergedExtractorConfiguration(config));
                case DateTimeExtractors.MergedSkipFromTo:
                    return new BaseMergedDateTimeExtractor(new ItalianMergedExtractorConfiguration(config));
                case DateTimeExtractors.TimeZone:
                    return new BaseTimeZoneExtractor(new ItalianTimeZoneExtractorConfiguration(config));
            }

            throw new Exception($"Extractor '{extractorName}' for Italian not supported");
        }

        public static IDateTimeParser GetItalianParser(DateTimeParsers parserName)
        {
            var commonConfiguration = new ItalianCommonDateTimeParserConfiguration(new BaseOptionsConfiguration());
            switch (parserName)
            {
                case DateTimeParsers.Date:
                    return new BaseDateParser(new ItalianDateParserConfiguration(commonConfiguration));
                case DateTimeParsers.Time:
                    return new DateTime.Italian.TimeParser(new ItalianTimeParserConfiguration(commonConfiguration));
                case DateTimeParsers.DatePeriod:
                    return new BaseDatePeriodParser(new ItalianDatePeriodParserConfiguration(commonConfiguration));
                case DateTimeParsers.TimePeriod:
                    return new BaseTimePeriodParser(new ItalianTimePeriodParserConfiguration(commonConfiguration));
                case DateTimeParsers.DateTime:
                    return new BaseDateTimeParser(new ItalianDateTimeParserConfiguration(commonConfiguration));
                case DateTimeParsers.DateTimePeriod:
                    return new BaseDateTimePeriodParser(new ItalianDateTimePeriodParserConfiguration(commonConfiguration));
                case DateTimeParsers.Duration:
                    return new BaseDurationParser(new ItalianDurationParserConfiguration(commonConfiguration));
                case DateTimeParsers.Holiday:
                    return new BaseHolidayParser(new ItalianHolidayParserConfiguration(commonConfiguration));
                case DateTimeParsers.Set:
                    return new BaseSetParser(new ItalianSetParserConfiguration(commonConfiguration));
                case DateTimeParsers.Merged:
                    return new BaseMergedDateTimeParser(new ItalianMergedParserConfiguration(commonConfiguration));
            }

            throw new Exception($"Parser '{parserName}' for Italian not supported");
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