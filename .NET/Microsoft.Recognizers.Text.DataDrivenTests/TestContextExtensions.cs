using System;
using System.Collections.Generic;
using Microsoft.Recognizers.Text.Choice;
using Microsoft.Recognizers.Text.DateTime;
using Microsoft.Recognizers.Text.DateTime.Dutch;
using Microsoft.Recognizers.Text.DateTime.English;
using Microsoft.Recognizers.Text.DateTime.French;
using Microsoft.Recognizers.Text.DateTime.German;
using Microsoft.Recognizers.Text.DateTime.Italian;
using Microsoft.Recognizers.Text.DateTime.Portuguese;
using Microsoft.Recognizers.Text.DateTime.Spanish;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.NumberWithUnit;
using Microsoft.Recognizers.Text.Sequence;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DataDrivenTests
{
    public static class TestContextExtensions
    {
        private static IDictionary<Models, Func<TestModel, string, IList<ModelResult>>> modelFunctions = new Dictionary<Models, Func<TestModel, string, IList<ModelResult>>>()
        {
            { Models.Number, (test, culture) => NumberRecognizer.RecognizeNumber(test.Input, culture, fallbackToDefaultCulture: false) },
            { Models.NumberPercentMode, (test, culture) => NumberRecognizer.RecognizeNumber(test.Input, culture, NumberOptions.PercentageMode, fallbackToDefaultCulture: false) },
            { Models.NumberExperimentalMode, (test, culture) => NumberRecognizer.RecognizeNumber(test.Input, culture, NumberOptions.ExperimentalMode, fallbackToDefaultCulture: false) },
            { Models.Ordinal, (test, culture) => NumberRecognizer.RecognizeOrdinal(test.Input, culture, fallbackToDefaultCulture: false) },
            { Models.OrdinalEnablePreview, (test, culture) => NumberRecognizer.RecognizeOrdinal(test.Input, culture, NumberOptions.EnablePreview, fallbackToDefaultCulture: false) },
            { Models.Percent, (test, culture) => NumberRecognizer.RecognizePercentage(test.Input, culture, fallbackToDefaultCulture: false) },
            { Models.PercentPercentMode, (test, culture) => NumberRecognizer.RecognizePercentage(test.Input, culture, NumberOptions.PercentageMode, fallbackToDefaultCulture: false) },
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
            { Models.Boolean, (test, culture) => ChoiceRecognizer.RecognizeBoolean(test.Input, culture, fallbackToDefaultCulture: false) },
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
                case Culture.Dutch:
                    return GetDutchExtractor(extractorName);
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
                case Culture.Dutch:
                    return GetDutchParser(parserName);
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
                    return new DateTime.Chinese.ChineseDateExtractorConfiguration();
                case DateTimeExtractors.Time:
                    return new DateTime.Chinese.ChineseTimeExtractorConfiguration();
                case DateTimeExtractors.DatePeriod:
                    return new DateTime.Chinese.ChineseDatePeriodExtractorConfiguration();
                case DateTimeExtractors.TimePeriod:
                    return new DateTime.Chinese.ChineseTimePeriodExtractorChsConfiguration();
                case DateTimeExtractors.DateTime:
                    return new DateTime.Chinese.ChineseDateTimeExtractorConfiguration();
                case DateTimeExtractors.DateTimePeriod:
                    return new DateTime.Chinese.ChineseDateTimePeriodExtractorConfiguration();
                case DateTimeExtractors.Duration:
                    return new DateTime.Chinese.ChineseDurationExtractorConfiguration();
                case DateTimeExtractors.Holiday:
                    return new BaseHolidayExtractor(new DateTime.Chinese.ChineseHolidayExtractorConfiguration());
                case DateTimeExtractors.Set:
                    return new DateTime.Chinese.ChineseSetExtractorConfiguration();
                case DateTimeExtractors.Merged:
                    return new DateTime.Chinese.ChineseMergedExtractorConfiguration(DateTimeOptions.None);
                case DateTimeExtractors.MergedSkipFromTo:
                    return new DateTime.Chinese.ChineseMergedExtractorConfiguration(DateTimeOptions.SkipFromToMerge);
            }

            throw new Exception($"Extractor '{extractorName}' for English not supported");
        }

        public static IDateTimeParser GetChineseParser(DateTimeParsers parserName)
        {
            // var commonConfiguration = new EnglishCommonDateTimeParserConfiguration();
            switch (parserName)
            {
                case DateTimeParsers.Date:
                    return new DateTime.Chinese.ChineseDateParserConfiguration(new DateTime.Chinese.ChineseDateTimeParserConfiguration());
                case DateTimeParsers.Time:
                    return new DateTime.Chinese.ChineseTimeParserConfiguration(new DateTime.Chinese.ChineseDateTimeParserConfiguration());
                case DateTimeParsers.DatePeriod:
                    return new DateTime.Chinese.ChineseDatePeriodParserConfiguration(new DateTime.Chinese.ChineseDateTimeParserConfiguration());
                case DateTimeParsers.TimePeriod:
                    return new DateTime.Chinese.ChineseTimePeriodParserConfiguration(new DateTime.Chinese.ChineseDateTimeParserConfiguration());
                case DateTimeParsers.DateTime:
                    return new DateTime.Chinese.ChineseDateTimeParser(new DateTime.Chinese.ChineseDateTimeParserConfiguration());
                case DateTimeParsers.DateTimePeriod:
                    return new DateTime.Chinese.ChineseDateTimePeriodParserConfiguration(new DateTime.Chinese.ChineseDateTimeParserConfiguration());
                case DateTimeParsers.Duration:
                    return new DateTime.Chinese.ChineseDurationParserConfiguration(new DateTime.Chinese.ChineseDateTimeParserConfiguration());
                case DateTimeParsers.Holiday:
                    return new DateTime.Chinese.ChineseHolidayParserConfiguration(new DateTime.Chinese.ChineseDateTimeParserConfiguration());
                case DateTimeParsers.Set:
                    return new DateTime.Chinese.ChineseSetParserConfiguration(new DateTime.Chinese.ChineseDateTimeParserConfiguration());
                case DateTimeParsers.Merged:
                    return new FullDateTimeParser(new DateTime.Chinese.ChineseDateTimeParserConfiguration());
            }

            throw new Exception($"Parser '{parserName}' for English not supported");
        }

        public static IDateTimeExtractor GetDutchExtractor(DateTimeExtractors extractorName)
        {
            var enableDmyConfig = new BaseOptionsConfiguration(DateTimeOptions.None, dmyDateFormat: true);

            switch (extractorName)
            {
                case DateTimeExtractors.Date:
                    return new BaseDateExtractor(new DutchDateExtractorConfiguration(enableDmyConfig));
                case DateTimeExtractors.Time:
                    return new BaseTimeExtractor(new DutchTimeExtractorConfiguration(enableDmyConfig));
                case DateTimeExtractors.DatePeriod:
                    return new BaseDatePeriodExtractor(new DutchDatePeriodExtractorConfiguration(enableDmyConfig));
                case DateTimeExtractors.TimePeriod:
                    return new BaseTimePeriodExtractor(new DutchTimePeriodExtractorConfiguration(enableDmyConfig));
                case DateTimeExtractors.DateTime:
                    return new BaseDateTimeExtractor(new DutchDateTimeExtractorConfiguration(enableDmyConfig));
                case DateTimeExtractors.DateTimePeriod:
                    return new BaseDateTimePeriodExtractor(new DutchDateTimePeriodExtractorConfiguration(enableDmyConfig));
                case DateTimeExtractors.Duration:
                    return new BaseDurationExtractor(new DutchDurationExtractorConfiguration(enableDmyConfig));
                case DateTimeExtractors.Holiday:
                    return new BaseHolidayExtractor(new DutchHolidayExtractorConfiguration(enableDmyConfig));
                case DateTimeExtractors.TimeZone:
                    return new BaseTimeZoneExtractor(new DutchTimeZoneExtractorConfiguration(enableDmyConfig));
                case DateTimeExtractors.Set:
                    return new BaseSetExtractor(new DutchSetExtractorConfiguration(enableDmyConfig));
                case DateTimeExtractors.Merged:
                    return new BaseMergedDateTimeExtractor(new DutchMergedExtractorConfiguration(enableDmyConfig));
                case DateTimeExtractors.MergedSkipFromTo:
                    return new BaseMergedDateTimeExtractor(new DutchMergedExtractorConfiguration(new BaseOptionsConfiguration(DateTimeOptions.SkipFromToMerge)));
            }

            throw new Exception($"Extractor '{extractorName}' for Dutch not supported");
        }

        public static IDateTimeParser GetDutchParser(DateTimeParsers parserName)
        {
            var commonConfiguration = new DutchCommonDateTimeParserConfiguration(new BaseOptionsConfiguration(DateTimeOptions.None, dmyDateFormat: true));

            switch (parserName)
            {
                case DateTimeParsers.Date:
                    return new BaseDateParser(new DutchDateParserConfiguration(commonConfiguration));
                case DateTimeParsers.Time:
                    return new DateTime.Dutch.TimeParser(new DutchTimeParserConfiguration(commonConfiguration));
                case DateTimeParsers.DatePeriod:
                    return new BaseDatePeriodParser(new DutchDatePeriodParserConfiguration(commonConfiguration));
                case DateTimeParsers.TimePeriod:
                    return new BaseTimePeriodParser(new DutchTimePeriodParserConfiguration(commonConfiguration));
                case DateTimeParsers.DateTime:
                    return new BaseDateTimeParser(new DutchDateTimeParserConfiguration(commonConfiguration));
                case DateTimeParsers.DateTimePeriod:
                    return new BaseDateTimePeriodParser(new DutchDateTimePeriodParserConfiguration(commonConfiguration));
                case DateTimeParsers.Duration:
                    return new BaseDurationParser(new DutchDurationParserConfiguration(commonConfiguration));
                case DateTimeParsers.Holiday:
                    return new BaseHolidayParser(new DutchHolidayParserConfiguration(commonConfiguration));
                case DateTimeParsers.TimeZone:
                    return new BaseTimeZoneParser();
                case DateTimeParsers.Set:
                    return new BaseSetParser(new DutchSetParserConfiguration(commonConfiguration));
                case DateTimeParsers.Merged:
                    return new BaseMergedDateTimeParser(new DutchMergedParserConfiguration(new BaseOptionsConfiguration(DateTimeOptions.None, dmyDateFormat: true)));
            }

            throw new Exception($"Parser '{parserName}' for Dutch not supported");
        }

        public static IDateTimeExtractor GetJapaneseExtractor(DateTimeExtractors extractorName)
        {
          switch (extractorName)
          {
            case DateTimeExtractors.Date:
              return new DateTime.Japanese.JapaneseDateExtractorConfiguration();
            case DateTimeExtractors.Time:
              return new DateTime.Japanese.JapaneseTimeExtractorConfiguration();
            case DateTimeExtractors.DatePeriod:
              return new DateTime.Japanese.JapaneseDatePeriodExtractorConfiguration();
            case DateTimeExtractors.TimePeriod:
              return new DateTime.Japanese.JapaneseTimePeriodExtractorConfiguration();
            case DateTimeExtractors.DateTime:
              return new DateTime.Japanese.JapaneseDateTimeExtractorConfiguration();
            case DateTimeExtractors.DateTimePeriod:
              return new DateTime.Japanese.JapaneseDateTimePeriodExtractorConfiguration();
            case DateTimeExtractors.Duration:
              return new DateTime.Japanese.JapaneseDurationExtractorConfiguration();
            case DateTimeExtractors.Holiday:
              return new BaseHolidayExtractor(new DateTime.Japanese.JapaneseHolidayExtractorConfiguration());
            case DateTimeExtractors.Set:
              return new DateTime.Japanese.JapaneseSetExtractorConfiguration();
            case DateTimeExtractors.Merged:
              return new DateTime.Japanese.JapaneseMergedExtractorConfiguration(DateTimeOptions.None);
            case DateTimeExtractors.MergedSkipFromTo:
              return new DateTime.Japanese.JapaneseMergedExtractorConfiguration(DateTimeOptions.SkipFromToMerge);
          }

          throw new Exception($"Extractor '{extractorName}' for Japanese not supported");
        }

        public static IDateTimeParser GetJapaneseParser(DateTimeParsers parserName)
        {
          switch (parserName)
          {
            case DateTimeParsers.Date:
              return new DateTime.Japanese.JapaneseDateParserConfiguration(new DateTime.Japanese.JapaneseDateTimeParserConfiguration());
            case DateTimeParsers.Time:
              return new DateTime.Japanese.JapaneseTimeParserConfiguration(new DateTime.Japanese.JapaneseDateTimeParserConfiguration());
            case DateTimeParsers.DatePeriod:
              return new DateTime.Japanese.JapaneseDatePeriodParserConfiguration(new DateTime.Japanese.JapaneseDateTimeParserConfiguration());
            case DateTimeParsers.TimePeriod:
              return new DateTime.Japanese.JapaneseTimePeriodParserConfiguration(new DateTime.Japanese.JapaneseDateTimeParserConfiguration());
            case DateTimeParsers.DateTime:
              return new DateTime.Japanese.JapaneseDateTimeParser(new DateTime.Japanese.JapaneseDateTimeParserConfiguration());
            case DateTimeParsers.DateTimePeriod:
              return new DateTime.Japanese.JapaneseDateTimePeriodParserConfiguration(new DateTime.Japanese.JapaneseDateTimeParserConfiguration());
            case DateTimeParsers.Duration:
              return new DateTime.Japanese.JapaneseDurationParserConfiguration(new DateTime.Japanese.JapaneseDateTimeParserConfiguration());
            case DateTimeParsers.Holiday:
              return new DateTime.Japanese.JapaneseHolidayParserConfiguration(new DateTime.Japanese.JapaneseDateTimeParserConfiguration());
            case DateTimeParsers.Set:
              return new DateTime.Japanese.JapaneseSetParserConfiguration(new DateTime.Japanese.JapaneseDateTimeParserConfiguration());
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
}