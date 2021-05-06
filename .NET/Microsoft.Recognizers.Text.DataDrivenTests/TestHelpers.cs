using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.Recognizers.Text.Choice;
using Microsoft.Recognizers.Text.DateTime;
using Microsoft.Recognizers.Text.DateTime.Arabic;
using Microsoft.Recognizers.Text.DateTime.Chinese;
using Microsoft.Recognizers.Text.DateTime.Dutch;
using Microsoft.Recognizers.Text.DateTime.English;
using Microsoft.Recognizers.Text.DateTime.French;
using Microsoft.Recognizers.Text.DateTime.German;
using Microsoft.Recognizers.Text.DateTime.Hindi;
using Microsoft.Recognizers.Text.DateTime.Italian;
using Microsoft.Recognizers.Text.DateTime.Japanese;
using Microsoft.Recognizers.Text.DateTime.Korean;
using Microsoft.Recognizers.Text.DateTime.Portuguese;
using Microsoft.Recognizers.Text.DateTime.Spanish;
using Microsoft.Recognizers.Text.DateTime.Turkish;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.NumberWithUnit;
using Microsoft.Recognizers.Text.Sequence;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DataDrivenTests
{
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1602: CSharp.Naming : Enumeration items should be documented", Justification = "TODO")]
    public enum Models
    {
        Number,
        NumberPercentMode,
        NumberExperimentalMode,
        Ordinal,
        OrdinalSuppressExtendedTypes,
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
        QuotedText,
        Email,
        URL,
        GUID,
        Boolean,
    }

    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1602: CSharp.Naming : Enumeration items should be documented", Justification = "TODO")]
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
        MergedSkipFromTo,
    }

    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1602: CSharp.Naming : Enumeration items should be documented", Justification = "TODO")]
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
        Merged,
    }

    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1649: CSharp.Naming", Justification = "TODO")]
    public static class TestContextExtensions
    {
        private static IDictionary<Models, Func<TestModel, string, IList<ModelResult>>> modelFunctions = new Dictionary<Models, Func<TestModel, string, IList<ModelResult>>>()
        {
            { Models.Number, (test, culture) => NumberRecognizer.RecognizeNumber(test.Input, culture, fallbackToDefaultCulture: false) },
            { Models.NumberPercentMode, (test, culture) => NumberRecognizer.RecognizeNumber(test.Input, culture, NumberOptions.PercentageMode, fallbackToDefaultCulture: false) },
            { Models.NumberExperimentalMode, (test, culture) => NumberRecognizer.RecognizeNumber(test.Input, culture, NumberOptions.ExperimentalMode, fallbackToDefaultCulture: false) },
            { Models.Ordinal, (test, culture) => NumberRecognizer.RecognizeOrdinal(test.Input, culture, fallbackToDefaultCulture: false) },
            { Models.OrdinalSuppressExtendedTypes, (test, culture) => NumberRecognizer.RecognizeOrdinal(test.Input, culture, NumberOptions.SuppressExtendedTypes, fallbackToDefaultCulture: false) },
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
            { Models.QuotedText, (test, culture) => SequenceRecognizer.RecognizeQuotedText(test.Input, culture, fallbackToDefaultCulture: false) },
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
                case Culture.Turkish:
                    return GetTurkishExtractor(extractorName);
                case Culture.Hindi:
                    return GetHindiExtractor(extractorName);
                case Culture.Arabic:
                    return GetArabicExtractor(extractorName);
                case Culture.Korean:
                    return GetKoreanExtractor(extractorName);
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
                case Culture.Turkish:
                    return GetTurkishParser(parserName);
                case Culture.Hindi:
                    return GetHindiParser(parserName);
                case Culture.Arabic:
                    return GetArabicParser(parserName);
                case Culture.Korean:
                    return GetKoreanParser(parserName);
            }

            throw new Exception($"Parser '{parserName}' for '{culture}' not supported");
        }

        public static IDateTimeExtractor GetArabicExtractor(DateTimeExtractors extractorName)
        {
            var config = new BaseDateTimeOptionsConfiguration(Culture.Arabic);
            var previewConfig = new BaseDateTimeOptionsConfiguration(Culture.Arabic, DateTimeOptions.EnablePreview);
            var skipConfig = new BaseDateTimeOptionsConfiguration(Culture.Arabic, DateTimeOptions.SkipFromToMerge);

            switch (extractorName)
            {
                case DateTimeExtractors.Date:
                    return new BaseDateExtractor(new ArabicDateExtractorConfiguration(config));
                case DateTimeExtractors.Time:
                    return new BaseTimeExtractor(new ArabicTimeExtractorConfiguration(config));
                case DateTimeExtractors.DatePeriod:
                    return new BaseDatePeriodExtractor(new ArabicDatePeriodExtractorConfiguration(config));
                case DateTimeExtractors.TimePeriod:
                    return new BaseTimePeriodExtractor(new ArabicTimePeriodExtractorConfiguration(config));
                case DateTimeExtractors.DateTime:
                    return new BaseDateTimeExtractor(new ArabicDateTimeExtractorConfiguration(config));
                case DateTimeExtractors.DateTimePeriod:
                    return new BaseDateTimePeriodExtractor(new ArabicDateTimePeriodExtractorConfiguration(config));
                case DateTimeExtractors.Duration:
                    return new BaseDurationExtractor(new ArabicDurationExtractorConfiguration(config));
                case DateTimeExtractors.Holiday:
                    return new BaseHolidayExtractor(new ArabicHolidayExtractorConfiguration(config));
                case DateTimeExtractors.Set:
                    return new BaseSetExtractor(new ArabicSetExtractorConfiguration(config));
                case DateTimeExtractors.Merged:
                    return new BaseMergedDateTimeExtractor(new ArabicMergedExtractorConfiguration(config));
                case DateTimeExtractors.MergedSkipFromTo:
                    return new BaseMergedDateTimeExtractor(new ArabicMergedExtractorConfiguration(skipConfig));
            }

            throw new Exception($"Extractor '{extractorName}' for Arabic not supported");
        }

        public static IDateTimeParser GetArabicParser(DateTimeParsers parserName)
        {
            var commonConfiguration = new ArabicCommonDateTimeParserConfiguration(new BaseDateTimeOptionsConfiguration(Culture.Arabic));

            switch (parserName)
            {
                case DateTimeParsers.Date:
                    return new BaseDateParser(new ArabicDateParserConfiguration(commonConfiguration));
                case DateTimeParsers.Time:
                    return new DateTime.Arabic.TimeParser(new ArabicTimeParserConfiguration(commonConfiguration));
                case DateTimeParsers.DatePeriod:
                    return new BaseDatePeriodParser(new ArabicDatePeriodParserConfiguration(commonConfiguration));
                case DateTimeParsers.TimePeriod:
                    return new BaseTimePeriodParser(new ArabicTimePeriodParserConfiguration(commonConfiguration));
                case DateTimeParsers.DateTime:
                    return new BaseDateTimeParser(new ArabicDateTimeParserConfiguration(commonConfiguration));
                case DateTimeParsers.DateTimePeriod:
                    return new BaseDateTimePeriodParser(new ArabicDateTimePeriodParserConfiguration(commonConfiguration));
                case DateTimeParsers.Duration:
                    return new BaseDurationParser(new ArabicDurationParserConfiguration(commonConfiguration));
                case DateTimeParsers.Holiday:
                    return new BaseHolidayParser(new ArabicHolidayParserConfiguration(commonConfiguration));
                case DateTimeParsers.TimeZone:
                    return new BaseTimeZoneParser();
                case DateTimeParsers.Set:
                    return new BaseSetParser(new ArabicSetParserConfiguration(commonConfiguration));
                case DateTimeParsers.Merged:
                    return new BaseMergedDateTimeParser(new ArabicMergedParserConfiguration(commonConfiguration));
            }

            throw new Exception($"Parser '{parserName}' for Arabic not supported");
        }

        public static IDateTimeExtractor GetDutchExtractor(DateTimeExtractors extractorName)
        {
            var enableDmyConfig = new BaseDateTimeOptionsConfiguration(Culture.Dutch, DateTimeOptions.None, dmyDateFormat: true);
            var dmySkipConfig = new BaseDateTimeOptionsConfiguration(Culture.Dutch, DateTimeOptions.SkipFromToMerge, true);

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
                    return new BaseMergedDateTimeExtractor(new DutchMergedExtractorConfiguration(dmySkipConfig));
            }

            throw new Exception($"Extractor '{extractorName}' for Dutch not supported");
        }

        public static IDateTimeParser GetDutchParser(DateTimeParsers parserName)
        {
            var commonConfiguration = new DutchCommonDateTimeParserConfiguration(
                new BaseDateTimeOptionsConfiguration(Culture.Dutch, DateTimeOptions.None, dmyDateFormat: true));

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
                    return new BaseMergedDateTimeParser(new DutchMergedParserConfiguration(commonConfiguration));
            }

            throw new Exception($"Parser '{parserName}' for Dutch not supported");
        }

        public static IDateTimeExtractor GetEnglishExtractor(DateTimeExtractors extractorName)
        {
            var config = new BaseDateTimeOptionsConfiguration(Culture.English);
            var previewConfig = new BaseDateTimeOptionsConfiguration(Culture.English, DateTimeOptions.EnablePreview);
            var skipConfig = new BaseDateTimeOptionsConfiguration(Culture.English, DateTimeOptions.SkipFromToMerge);

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
                    return new BaseMergedDateTimeExtractor(new EnglishMergedExtractorConfiguration(skipConfig));
            }

            throw new Exception($"Extractor '{extractorName}' for English not supported");
        }

        public static IDateTimeParser GetEnglishParser(DateTimeParsers parserName)
        {
            var commonConfiguration = new EnglishCommonDateTimeParserConfiguration(new BaseDateTimeOptionsConfiguration(Culture.English));

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
                    return new BaseMergedDateTimeParser(new EnglishMergedParserConfiguration(commonConfiguration));
            }

            throw new Exception($"Parser '{parserName}' for English not supported");
        }

        public static IDateTimeExtractor GetEnglishOthersExtractor(DateTimeExtractors extractorName)
        {
            var enableDmyConfig = new BaseDateTimeOptionsConfiguration(Culture.EnglishOthers, DateTimeOptions.None, true);
            var enableDmyPreviewConfig = new BaseDateTimeOptionsConfiguration(Culture.EnglishOthers, DateTimeOptions.EnablePreview, true);
            var enableDmySkipConfig = new BaseDateTimeOptionsConfiguration(Culture.EnglishOthers, DateTimeOptions.SkipFromToMerge, true);

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
                    return new BaseMergedDateTimeExtractor(new EnglishMergedExtractorConfiguration(enableDmySkipConfig));
            }

            throw new Exception($"Extractor '{extractorName}' for English-Others not supported");
        }

        public static IDateTimeParser GetEnglishOthersParser(DateTimeParsers parserName)
        {
            var commonConfiguration = new EnglishCommonDateTimeParserConfiguration(
                new BaseDateTimeOptionsConfiguration(Culture.EnglishOthers, DateTimeOptions.None, true));

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
                    return new BaseMergedDateTimeParser(new EnglishMergedParserConfiguration(commonConfiguration));
            }

            throw new Exception($"Parser '{parserName}' for English-Others not supported");
        }

        public static IDateTimeExtractor GetChineseExtractor(DateTimeExtractors extractorName)
        {

            var defaultConfig = new BaseDateTimeOptionsConfiguration(Culture.Chinese, DateTimeOptions.None);
            var skipConfig = new BaseDateTimeOptionsConfiguration(Culture.Chinese, DateTimeOptions.SkipFromToMerge);

            switch (extractorName)
            {
                case DateTimeExtractors.Date:
                    return new BaseCJKDateExtractor(new ChineseDateExtractorConfiguration(defaultConfig));
                case DateTimeExtractors.Time:
                    return new BaseCJKTimeExtractor(new ChineseTimeExtractorConfiguration(defaultConfig));
                case DateTimeExtractors.DatePeriod:
                    return new BaseCJKDatePeriodExtractor(new ChineseDatePeriodExtractorConfiguration(defaultConfig));
                case DateTimeExtractors.TimePeriod:
                    return new BaseCJKTimePeriodExtractor(new ChineseTimePeriodExtractorConfiguration(defaultConfig));
                case DateTimeExtractors.DateTime:
                    return new BaseCJKDateTimeExtractor(new ChineseDateTimeExtractorConfiguration(defaultConfig));
                case DateTimeExtractors.DateTimePeriod:
                    return new BaseCJKDateTimePeriodExtractor(new ChineseDateTimePeriodExtractorConfiguration(defaultConfig));
                case DateTimeExtractors.Duration:
                    return new BaseCJKDurationExtractor(new ChineseDurationExtractorConfiguration(defaultConfig));
                case DateTimeExtractors.Holiday:
                    return new BaseCJKHolidayExtractor(new ChineseHolidayExtractorConfiguration(defaultConfig));
                case DateTimeExtractors.Set:
                    return new BaseCJKSetExtractor(new ChineseSetExtractorConfiguration(defaultConfig));
                case DateTimeExtractors.Merged:
                    return new BaseCJKMergedDateTimeExtractor(new ChineseMergedExtractorConfiguration(defaultConfig));
                case DateTimeExtractors.MergedSkipFromTo:
                    return new BaseCJKMergedDateTimeExtractor(new ChineseMergedExtractorConfiguration(skipConfig));
            }

            throw new Exception($"Extractor '{extractorName}' for Chinese not supported");
        }

        public static IDateTimeParser GetChineseParser(DateTimeParsers parserName)
        {
            var config = new ChineseCommonDateTimeParserConfiguration(new BaseDateTimeOptionsConfiguration(Culture.Chinese, DateTimeOptions.None));

            switch (parserName)
            {
                case DateTimeParsers.Date:
                    return new BaseCJKDateParser(new ChineseDateParserConfiguration(config));
                case DateTimeParsers.Time:
                    return new BaseCJKTimeParser(new ChineseTimeParserConfiguration(config));
                case DateTimeParsers.DatePeriod:
                    return new BaseCJKDatePeriodParser(new ChineseDatePeriodParserConfiguration(config));
                case DateTimeParsers.TimePeriod:
                    return new BaseCJKTimePeriodParser(new ChineseTimePeriodParserConfiguration(config));
                case DateTimeParsers.DateTime:
                    return new BaseCJKDateTimeParser(new ChineseDateTimeParserConfiguration(config));
                case DateTimeParsers.DateTimePeriod:
                    return new BaseCJKDateTimePeriodParser(new ChineseDateTimePeriodParserConfiguration(config));
                case DateTimeParsers.Duration:
                    return new BaseCJKDurationParser(new ChineseDurationParserConfiguration(config));
                case DateTimeParsers.Holiday:
                    return new BaseCJKHolidayParser(new ChineseHolidayParserConfiguration(config));
                case DateTimeParsers.Set:
                    return new BaseCJKSetParser(new ChineseSetParserConfiguration(config));
                case DateTimeParsers.Merged:
                    return new BaseCJKMergedDateTimeParser(new ChineseMergedParserConfiguration(config));
            }

            throw new Exception($"Parser '{parserName}' for Chinese not supported");
        }

        public static IDateTimeExtractor GetJapaneseExtractor(DateTimeExtractors extractorName)
        {

            var defaultConfig = new BaseDateTimeOptionsConfiguration(Culture.Japanese, DateTimeOptions.None);
            var skipConfig = new BaseDateTimeOptionsConfiguration(Culture.Japanese, DateTimeOptions.SkipFromToMerge);

            switch (extractorName)
            {
                case DateTimeExtractors.Date:
                    return new BaseCJKDateExtractor(new JapaneseDateExtractorConfiguration(defaultConfig));
                case DateTimeExtractors.Time:
                    return new BaseCJKTimeExtractor(new JapaneseTimeExtractorConfiguration(defaultConfig));
                case DateTimeExtractors.DatePeriod:
                    return new BaseCJKDatePeriodExtractor(new JapaneseDatePeriodExtractorConfiguration(defaultConfig));
                case DateTimeExtractors.TimePeriod:
                    return new BaseCJKTimePeriodExtractor(new JapaneseTimePeriodExtractorConfiguration(defaultConfig));
                case DateTimeExtractors.DateTime:
                    return new BaseCJKDateTimeExtractor(new JapaneseDateTimeExtractorConfiguration(defaultConfig));
                case DateTimeExtractors.DateTimePeriod:
                    return new BaseCJKDateTimePeriodExtractor(new JapaneseDateTimePeriodExtractorConfiguration(defaultConfig));
                case DateTimeExtractors.Duration:
                    return new BaseCJKDurationExtractor(new JapaneseDurationExtractorConfiguration(defaultConfig));
                case DateTimeExtractors.Holiday:
                    return new BaseCJKHolidayExtractor(new JapaneseHolidayExtractorConfiguration(defaultConfig));
                case DateTimeExtractors.Set:
                    return new BaseCJKSetExtractor(new JapaneseSetExtractorConfiguration(defaultConfig));
                case DateTimeExtractors.Merged:
                    return new BaseCJKMergedDateTimeExtractor(new JapaneseMergedExtractorConfiguration(defaultConfig));
                case DateTimeExtractors.MergedSkipFromTo:
                    return new BaseCJKMergedDateTimeExtractor(new JapaneseMergedExtractorConfiguration(skipConfig));
            }

            throw new Exception($"Extractor '{extractorName}' for Japanese not supported");
        }

        public static IDateTimeParser GetJapaneseParser(DateTimeParsers parserName)
        {
            var config = new JapaneseCommonDateTimeParserConfiguration(new BaseDateTimeOptionsConfiguration(Culture.Japanese, DateTimeOptions.None));

            switch (parserName)
            {
                case DateTimeParsers.Date:
                    return new BaseCJKDateParser(new JapaneseDateParserConfiguration(config));
                case DateTimeParsers.Time:
                    return new BaseCJKTimeParser(new JapaneseTimeParserConfiguration(config));
                case DateTimeParsers.DatePeriod:
                    return new BaseCJKDatePeriodParser(new JapaneseDatePeriodParserConfiguration(config));
                case DateTimeParsers.TimePeriod:
                    return new BaseCJKTimePeriodParser(new JapaneseTimePeriodParserConfiguration(config));
                case DateTimeParsers.DateTime:
                    return new BaseCJKDateTimeParser(new JapaneseDateTimeParserConfiguration(config));
                case DateTimeParsers.DateTimePeriod:
                    return new BaseCJKDateTimePeriodParser(new JapaneseDateTimePeriodParserConfiguration(config));
                case DateTimeParsers.Duration:
                    return new BaseCJKDurationParser(new JapaneseDurationParserConfiguration(config));
                case DateTimeParsers.Holiday:
                    return new BaseCJKHolidayParser(new JapaneseHolidayParserConfiguration(config));
                case DateTimeParsers.Set:
                    return new BaseCJKSetParser(new JapaneseSetParserConfiguration(config));
                case DateTimeParsers.Merged:
                    return new BaseCJKMergedDateTimeParser(new JapaneseMergedParserConfiguration(config));
            }

            throw new Exception($"Parser '{parserName}' for Japanese not supported");
        }

        public static IDateTimeExtractor GetSpanishExtractor(DateTimeExtractors extractorName)
        {
            var config = new BaseDateTimeOptionsConfiguration(Culture.Spanish, DateTimeOptions.None);

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
                    return new BaseMergedDateTimeExtractor(new SpanishMergedExtractorConfiguration(config));
            }

            throw new Exception($"Extractor '{extractorName}' for Spanish not supported");
        }

        public static IDateTimeParser GetSpanishParser(DateTimeParsers parserName)
        {
            var commonConfiguration = new SpanishCommonDateTimeParserConfiguration(new BaseDateTimeOptionsConfiguration(Culture.Spanish));

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
            var config = new BaseDateTimeOptionsConfiguration(Culture.Portuguese);

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
                    return new BaseMergedDateTimeExtractor(new PortugueseMergedExtractorConfiguration(config));
            }

            throw new Exception($"Extractor '{extractorName}' for Portuguese not supported");
        }

        public static IDateTimeParser GetPortugueseParser(DateTimeParsers parserName)
        {
            var commonConfiguration = new PortugueseCommonDateTimeParserConfiguration(new BaseDateTimeOptionsConfiguration(Culture.Portuguese));

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
            var config = new BaseDateTimeOptionsConfiguration(Culture.French);
            var skipConfig = new BaseDateTimeOptionsConfiguration(Culture.French, DateTimeOptions.SkipFromToMerge);

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
                    return new BaseMergedDateTimeExtractor(new FrenchMergedExtractorConfiguration(config));
                case DateTimeExtractors.MergedSkipFromTo:
                    return new BaseMergedDateTimeExtractor(new FrenchMergedExtractorConfiguration(skipConfig));
            }

            throw new Exception($"Extractor '{extractorName}' for French not supported");
        }

        public static IDateTimeParser GetFrenchParser(DateTimeParsers parserName)
        {
            var commonConfiguration = new FrenchCommonDateTimeParserConfiguration(new BaseDateTimeOptionsConfiguration(Culture.French));

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
            var config = new BaseDateTimeOptionsConfiguration(Culture.German);
            var skipConfig = new BaseDateTimeOptionsConfiguration(Culture.German, DateTimeOptions.SkipFromToMerge);

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
                    return new BaseMergedDateTimeExtractor(new GermanMergedExtractorConfiguration(config));
                case DateTimeExtractors.MergedSkipFromTo:
                    return new BaseMergedDateTimeExtractor(new GermanMergedExtractorConfiguration(skipConfig));
            }

            throw new Exception($"Extractor '{extractorName}' for German not supported");
        }

        public static IDateTimeParser GetGermanParser(DateTimeParsers parserName)
        {
            var commonConfiguration = new GermanCommonDateTimeParserConfiguration(new BaseDateTimeOptionsConfiguration(Culture.German));

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
            var config = new BaseDateTimeOptionsConfiguration(Culture.Italian);

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
            var commonConfiguration = new ItalianCommonDateTimeParserConfiguration(new BaseDateTimeOptionsConfiguration(Culture.Italian));

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

        public static IDateTimeExtractor GetTurkishExtractor(DateTimeExtractors extractorName)
        {
            var config = new BaseDateTimeOptionsConfiguration(Culture.Turkish);
            var skipConfig = new BaseDateTimeOptionsConfiguration(Culture.Turkish, DateTimeOptions.SkipFromToMerge);

            switch (extractorName)
            {
                case DateTimeExtractors.Date:
                    return new BaseDateExtractor(new TurkishDateExtractorConfiguration(config));
                case DateTimeExtractors.Time:
                    return new BaseTimeExtractor(new TurkishTimeExtractorConfiguration(config));
                case DateTimeExtractors.DatePeriod:
                    return new BaseDatePeriodExtractor(new TurkishDatePeriodExtractorConfiguration(config));
                case DateTimeExtractors.TimePeriod:
                    return new BaseTimePeriodExtractor(new TurkishTimePeriodExtractorConfiguration(config));
                case DateTimeExtractors.DateTime:
                    return new BaseDateTimeExtractor(new TurkishDateTimeExtractorConfiguration(config));
                case DateTimeExtractors.DateTimePeriod:
                    return new BaseDateTimePeriodExtractor(new TurkishDateTimePeriodExtractorConfiguration(config));
                case DateTimeExtractors.Duration:
                    return new BaseDurationExtractor(new TurkishDurationExtractorConfiguration(config));
                case DateTimeExtractors.Holiday:
                    return new BaseHolidayExtractor(new TurkishHolidayExtractorConfiguration(config));
                case DateTimeExtractors.Set:
                    return new BaseSetExtractor(new TurkishSetExtractorConfiguration(config));
                case DateTimeExtractors.Merged:
                    return new BaseMergedDateTimeExtractor(new TurkishMergedExtractorConfiguration(config));
                case DateTimeExtractors.MergedSkipFromTo:
                    return new BaseMergedDateTimeExtractor(new TurkishMergedExtractorConfiguration(skipConfig));
            }

            throw new Exception($"Extractor '{extractorName}' for Turkish not supported");
        }

        public static IDateTimeParser GetTurkishParser(DateTimeParsers parserName)
        {
            var commonConfiguration = new TurkishCommonDateTimeParserConfiguration(new BaseDateTimeOptionsConfiguration(Culture.Turkish));

            switch (parserName)
            {
                case DateTimeParsers.Date:
                    return new BaseDateParser(new TurkishDateParserConfiguration(commonConfiguration));
                case DateTimeParsers.Time:
                    return new DateTime.Turkish.TimeParser(new TurkishTimeParserConfiguration(commonConfiguration));
                case DateTimeParsers.DatePeriod:
                    return new BaseDatePeriodParser(new TurkishDatePeriodParserConfiguration(commonConfiguration));
                case DateTimeParsers.TimePeriod:
                    return new BaseTimePeriodParser(new TurkishTimePeriodParserConfiguration(commonConfiguration));
                case DateTimeParsers.DateTime:
                    return new BaseDateTimeParser(new TurkishDateTimeParserConfiguration(commonConfiguration));
                case DateTimeParsers.DateTimePeriod:
                    return new BaseDateTimePeriodParser(new TurkishDateTimePeriodParserConfiguration(commonConfiguration));
                case DateTimeParsers.Duration:
                    return new BaseDurationParser(new TurkishDurationParserConfiguration(commonConfiguration));
                case DateTimeParsers.Holiday:
                    return new BaseHolidayParser(new TurkishHolidayParserConfiguration(commonConfiguration));
                case DateTimeParsers.Set:
                    return new BaseSetParser(new TurkishSetParserConfiguration(commonConfiguration));
                case DateTimeParsers.Merged:
                    return new BaseMergedDateTimeParser(new TurkishMergedParserConfiguration(commonConfiguration));
            }

            throw new Exception($"Parser '{parserName}' for Turkish not supported");
        }

        public static IDateTimeExtractor GetHindiExtractor(DateTimeExtractors extractorName)
        {
            var config = new BaseDateTimeOptionsConfiguration(Culture.Hindi);
            var skipConfig = new BaseDateTimeOptionsConfiguration(Culture.Hindi, DateTimeOptions.SkipFromToMerge);

            switch (extractorName)
            {
                case DateTimeExtractors.Date:
                    return new BaseDateExtractor(new HindiDateExtractorConfiguration(config));
                case DateTimeExtractors.Time:
                    return new BaseTimeExtractor(new HindiTimeExtractorConfiguration(config));
                case DateTimeExtractors.DatePeriod:
                    return new BaseDatePeriodExtractor(new HindiDatePeriodExtractorConfiguration(config));
                case DateTimeExtractors.TimePeriod:
                    return new BaseTimePeriodExtractor(new HindiTimePeriodExtractorConfiguration(config));
                case DateTimeExtractors.DateTime:
                    return new BaseDateTimeExtractor(new HindiDateTimeExtractorConfiguration(config));
                case DateTimeExtractors.DateTimePeriod:
                    return new BaseDateTimePeriodExtractor(new HindiDateTimePeriodExtractorConfiguration(config));
                case DateTimeExtractors.Duration:
                    return new BaseDurationExtractor(new HindiDurationExtractorConfiguration(config));
                case DateTimeExtractors.Holiday:
                    return new BaseHolidayExtractor(new HindiHolidayExtractorConfiguration(config));
                case DateTimeExtractors.Set:
                    return new BaseSetExtractor(new HindiSetExtractorConfiguration(config));
                case DateTimeExtractors.Merged:
                    return new BaseMergedDateTimeExtractor(new HindiMergedExtractorConfiguration(config));
            }

            throw new Exception($"Extractor '{extractorName}' for Hindi not supported");
        }

        public static IDateTimeParser GetHindiParser(DateTimeParsers parserName)
        {
            var commonConfiguration = new HindiCommonDateTimeParserConfiguration(new BaseDateTimeOptionsConfiguration(Culture.Hindi));

            switch (parserName)
            {
                case DateTimeParsers.Date:
                    return new BaseDateParser(new HindiDateParserConfiguration(commonConfiguration));
                case DateTimeParsers.Time:
                    return new DateTime.Hindi.TimeParser(new HindiTimeParserConfiguration(commonConfiguration));
                case DateTimeParsers.DatePeriod:
                    return new BaseDatePeriodParser(new HindiDatePeriodParserConfiguration(commonConfiguration));
                case DateTimeParsers.TimePeriod:
                    return new BaseTimePeriodParser(new HindiTimePeriodParserConfiguration(commonConfiguration));
                case DateTimeParsers.DateTime:
                    return new BaseDateTimeParser(new HindiDateTimeParserConfiguration(commonConfiguration));
                case DateTimeParsers.DateTimePeriod:
                    return new BaseDateTimePeriodParser(new HindiDateTimePeriodParserConfiguration(commonConfiguration));
                case DateTimeParsers.Duration:
                    return new BaseDurationParser(new HindiDurationParserConfiguration(commonConfiguration));
                case DateTimeParsers.Holiday:
                    return new BaseHolidayParser(new HindiHolidayParserConfiguration(commonConfiguration));
                case DateTimeParsers.Set:
                    return new BaseSetParser(new HindiSetParserConfiguration(commonConfiguration));
                case DateTimeParsers.Merged:
                    return new BaseMergedDateTimeParser(new HindiMergedParserConfiguration(commonConfiguration));
            }

            throw new Exception($"Parser '{parserName}' for Hindi not supported");
        }

        public static IDateTimeExtractor GetKoreanExtractor(DateTimeExtractors extractorName)
        {

            var defaultConfig = new BaseDateTimeOptionsConfiguration(Culture.Korean, DateTimeOptions.None);
            var skipConfig = new BaseDateTimeOptionsConfiguration(Culture.Korean, DateTimeOptions.SkipFromToMerge);

            switch (extractorName)
            {
                case DateTimeExtractors.Date:
                    return new BaseCJKDateExtractor(new KoreanDateExtractorConfiguration(defaultConfig));
                case DateTimeExtractors.Time:
                    return new BaseCJKTimeExtractor(new KoreanTimeExtractorConfiguration(defaultConfig));
                case DateTimeExtractors.DatePeriod:
                    return new BaseCJKDatePeriodExtractor(new KoreanDatePeriodExtractorConfiguration(defaultConfig));
                case DateTimeExtractors.TimePeriod:
                    return new BaseCJKTimePeriodExtractor(new KoreanTimePeriodExtractorConfiguration(defaultConfig));
                case DateTimeExtractors.DateTime:
                    return new BaseCJKDateTimeExtractor(new KoreanDateTimeExtractorConfiguration(defaultConfig));
                case DateTimeExtractors.DateTimePeriod:
                    return new BaseCJKDateTimePeriodExtractor(new KoreanDateTimePeriodExtractorConfiguration(defaultConfig));
                case DateTimeExtractors.Duration:
                    return new BaseCJKDurationExtractor(new KoreanDurationExtractorConfiguration(defaultConfig));
                case DateTimeExtractors.Holiday:
                    return new BaseCJKHolidayExtractor(new KoreanHolidayExtractorConfiguration(defaultConfig));
                case DateTimeExtractors.Set:
                    return new BaseCJKSetExtractor(new KoreanSetExtractorConfiguration(defaultConfig));
                case DateTimeExtractors.Merged:
                    return new BaseCJKMergedDateTimeExtractor(new KoreanMergedExtractorConfiguration(defaultConfig));
                case DateTimeExtractors.MergedSkipFromTo:
                    return new BaseCJKMergedDateTimeExtractor(new KoreanMergedExtractorConfiguration(skipConfig));
            }

            throw new Exception($"Extractor '{extractorName}' for Korean not supported");
        }

        public static IDateTimeParser GetKoreanParser(DateTimeParsers parserName)
        {
            var config = new KoreanCommonDateTimeParserConfiguration(new BaseDateTimeOptionsConfiguration(Culture.Korean, DateTimeOptions.None));

            switch (parserName)
            {
                case DateTimeParsers.Date:
                    return new BaseCJKDateParser(new KoreanDateParserConfiguration(config));
                case DateTimeParsers.Time:
                    return new BaseCJKTimeParser(new KoreanTimeParserConfiguration(config));
                case DateTimeParsers.DatePeriod:
                    return new BaseCJKDatePeriodParser(new KoreanDatePeriodParserConfiguration(config));
                case DateTimeParsers.TimePeriod:
                    return new BaseCJKTimePeriodParser(new KoreanTimePeriodParserConfiguration(config));
                case DateTimeParsers.DateTime:
                    return new BaseCJKDateTimeParser(new KoreanDateTimeParserConfiguration(config));
                case DateTimeParsers.DateTimePeriod:
                    return new BaseCJKDateTimePeriodParser(new KoreanDateTimePeriodParserConfiguration(config));
                case DateTimeParsers.Duration:
                    return new BaseCJKDurationParser(new KoreanDurationParserConfiguration(config));
                case DateTimeParsers.Holiday:
                    return new BaseCJKHolidayParser(new KoreanHolidayParserConfiguration(config));
                case DateTimeParsers.Set:
                    return new BaseCJKSetParser(new KoreanSetParserConfiguration(config));
                case DateTimeParsers.Merged:
                    return new BaseCJKMergedDateTimeParser(new KoreanMergedParserConfiguration(config));
            }

            throw new Exception($"Parser '{parserName}' for Korean not supported");
        }
    }

    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1402: CSharp.Naming : File may only contain a single type", Justification = "TODO")]
    public static class TestModelExtensions
    {
        public static bool IsNotSupported(this TestModel testSpec)
        {
            return testSpec.NotSupported.HasFlag(Platform.DotNet);
        }

        public static bool IsNotSupportedByDesign(this TestModel testSpec)
        {
            return testSpec.NotSupportedByDesign.HasFlag(Platform.DotNet);
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

    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1402: CSharp.Naming : File may only contain a single type", Justification = "TODO")]
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
            return source.Replace("Model", string.Empty).Replace("Extractor", string.Empty).Replace("Parser", string.Empty);
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

    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1402: CSharp.Naming : File may only contain a single type", Justification = "TODO")]
    public static class RecognizerExtensions
    {
        public static ConcurrentDictionary<(string culture, Type modelType, string modelOptions), IModel> GetInternalCache<TRecognizerOptions>(this Recognizer<TRecognizerOptions> source)
            where TRecognizerOptions : struct
        {
            var modelFactoryProp = typeof(Recognizer<TRecognizerOptions>).GetField("factory", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var modelFactory = modelFactoryProp.GetValue(source);
            var cacheProp = modelFactory.GetType().GetField("cache", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            return cacheProp.GetValue(modelFactory) as ConcurrentDictionary<(string culture, Type modelType, string modelOptions), IModel>;
        }
    }

}