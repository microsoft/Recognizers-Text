package com.microsoft.recognizers.text.tests.datetime;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.ModelResult;
import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.config.IOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.english.extractors.EnglishDateExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.english.extractors.EnglishDatePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.english.extractors.EnglishDateTimeExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.english.extractors.EnglishDateTimePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.english.extractors.EnglishDurationExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.english.extractors.EnglishHolidayExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.english.extractors.EnglishMergedExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.english.extractors.EnglishSetExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.english.extractors.EnglishTimeExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.english.extractors.EnglishTimePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.english.extractors.EnglishTimeZoneExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.BaseDateExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseDatePeriodExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseDateTimePeriodExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseDurationExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseHolidayExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseMergedDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseSetExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseTimePeriodExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseTimeZoneExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.french.extractors.FrenchDateExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.french.extractors.FrenchDatePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.french.extractors.FrenchDateTimeExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.french.extractors.FrenchDateTimePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.french.extractors.FrenchDurationExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.french.extractors.FrenchHolidayExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.french.extractors.FrenchMergedExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.french.extractors.FrenchSetExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.french.extractors.FrenchTimeExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.french.extractors.FrenchTimePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.french.extractors.FrenchTimeZoneExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.spanish.extractors.SpanishDateExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.spanish.extractors.SpanishDatePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.spanish.extractors.SpanishDateTimeExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.spanish.extractors.SpanishDateTimePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.spanish.extractors.SpanishDurationExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.spanish.extractors.SpanishHolidayExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.spanish.extractors.SpanishMergedExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.spanish.extractors.SpanishSetExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.spanish.extractors.SpanishTimeExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.spanish.extractors.SpanishTimePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.tests.AbstractTest;
import com.microsoft.recognizers.text.tests.NotSupportedException;
import com.microsoft.recognizers.text.tests.TestCase;

import java.util.Collection;
import java.util.List;
import java.util.Locale;
import java.util.stream.IntStream;

import org.javatuples.Pair;
import org.junit.Assert;
import org.junit.AssumptionViolatedException;
import org.junit.runners.Parameterized;

public class DateTimeExtractorTest extends AbstractTest {

    private static final String recognizerType = "DateTime";

    @Parameterized.Parameters(name = "{0}")
    public static Collection<TestCase> testCases() {
        return AbstractTest.enumerateTestCases(recognizerType, "Extractor");
    }

    public DateTimeExtractorTest(TestCase currentCase) {
        super(currentCase);
    }

    @Override
    protected List<ModelResult> recognize(TestCase currentCase) {
        return null;
    }

    protected List<ExtractResult> extract(TestCase currentCase) {
        IDateTimeExtractor extractor = getExtractor(currentCase);
        return extractor.extract(currentCase.input.toLowerCase(Locale.ROOT), currentCase.getReferenceDateTime());
    }

    @Override
    protected void recognizeAndAssert(TestCase currentCase) {
        List<ExtractResult> results = extract(currentCase);
        assertExtractResults(currentCase, results);
    }

    public static void assertExtractResults(TestCase currentCase, List<ExtractResult> results) {

        List<ExtractResult> expectedResults = readExpectedExtractResults(ExtractResult.class, currentCase.results);
        Assert.assertEquals(getMessage(currentCase, "\"Result Count\""), expectedResults.size(), results.size());

        IntStream.range(0, expectedResults.size())
                .mapToObj(i -> Pair.with(expectedResults.get(i), results.get(i)))
                .forEach(t -> {
                    ExtractResult expected = t.getValue0();
                    ExtractResult actual = t.getValue1();

                    Assert.assertEquals(getMessage(currentCase, "type"), expected.getType(), actual.getType());
                    Assert.assertTrue(getMessage(currentCase, "text"), expected.getText().equalsIgnoreCase(actual.getText()));
                    Assert.assertEquals(getMessage(currentCase, "start"), expected.getStart(), actual.getStart());
                    Assert.assertEquals(getMessage(currentCase, "length"), expected.getLength(), actual.getLength());
                });
    }

    public static IDateTimeExtractor getExtractor(TestCase currentCase) {
        return getExtractor(currentCase.language, currentCase.modelName);
    }

    public static IDateTimeExtractor getExtractor(String language, String modelName) {

        try {
            String culture = getCultureCode(language);
            switch (culture) {
                case Culture.English:
                    return getEnglishExtractor(modelName);
                case Culture.Spanish:
                    return getSpanishExtractor(modelName);
                case Culture.French:
                    return getFrenchExtractor(modelName);
                default:
                    throw new NotSupportedException("Extractor Type/Name not supported in: " + culture);
            }
        } catch (NotSupportedException ex) {
            throw new AssumptionViolatedException(ex.getMessage(), ex);
        }
    }

    private static IDateTimeExtractor getEnglishExtractor(String name) throws NotSupportedException {

        IOptionsConfiguration config = new BaseOptionsConfiguration();
        switch (name) {
            case "DateExtractor":
                return new BaseDateExtractor(new EnglishDateExtractorConfiguration(config));
            case "DatePeriodExtractor":
                return new BaseDatePeriodExtractor(new EnglishDatePeriodExtractorConfiguration(config));
            //case "DateTimeAltExtractor":
            //    return new BaseDateTimeAltExtractor(new EnglishDateTimeAltExtractorConfiguration());
            case "DateTimeExtractor":
                return new BaseDateTimeExtractor(new EnglishDateTimeExtractorConfiguration());
            case "DateTimePeriodExtractor":
                return new BaseDateTimePeriodExtractor(new EnglishDateTimePeriodExtractorConfiguration());
            case "DurationExtractor":
                return new BaseDurationExtractor(new EnglishDurationExtractorConfiguration());
            case "HolidayExtractor":
                return new BaseHolidayExtractor(new EnglishHolidayExtractorConfiguration());
            case "MergedExtractor":
                return new BaseMergedDateTimeExtractor(new EnglishMergedExtractorConfiguration(DateTimeOptions.None));
            case "MergedExtractorSkipFromTo":
                return new BaseMergedDateTimeExtractor(new EnglishMergedExtractorConfiguration(DateTimeOptions.SkipFromToMerge));
            case "SetExtractor":
                return new BaseSetExtractor(new EnglishSetExtractorConfiguration());
            case "TimeExtractor":
                return new BaseTimeExtractor(new EnglishTimeExtractorConfiguration());
            case "TimePeriodExtractor":
                return new BaseTimePeriodExtractor(new EnglishTimePeriodExtractorConfiguration());
            case "TimeZoneExtractor":
                return new BaseTimeZoneExtractor(new EnglishTimeZoneExtractorConfiguration(DateTimeOptions.EnablePreview));

            default:
                throw new NotSupportedException("English extractor Type/Name not supported for type: " + name);
        }
    }

    private static IDateTimeExtractor getSpanishExtractor(String name) throws NotSupportedException {

        IOptionsConfiguration config = new BaseOptionsConfiguration();
        switch (name) {
            case "DateExtractor":
                return new BaseDateExtractor(new SpanishDateExtractorConfiguration(config));
            case "DatePeriodExtractor":
                return new BaseDatePeriodExtractor(new SpanishDatePeriodExtractorConfiguration(config));
            //case "DateTimeAltExtractor":
            //    return new BaseDateTimeAltExtractor(new SpanishDateTimeAltExtractorConfiguration());
            case "DateTimeExtractor":
                return new BaseDateTimeExtractor(new SpanishDateTimeExtractorConfiguration());
            case "DateTimePeriodExtractor":
                return new BaseDateTimePeriodExtractor(new SpanishDateTimePeriodExtractorConfiguration());
            case "DurationExtractor":
                return new BaseDurationExtractor(new SpanishDurationExtractorConfiguration());
            case "HolidayExtractor":
                return new BaseHolidayExtractor(new SpanishHolidayExtractorConfiguration());
            case "MergedExtractor":
               return new BaseMergedDateTimeExtractor(new SpanishMergedExtractorConfiguration(DateTimeOptions.None));
            //case "MergedExtractorSkipFromTo":
            //    return new BaseMergedDateTimeExtractor(new SpanishMergedExtractorConfiguration(DateTimeOptions.SkipFromToMerge));
            case "SetExtractor":
                return new BaseSetExtractor(new SpanishSetExtractorConfiguration());
            case "TimeExtractor":
                return new BaseTimeExtractor(new SpanishTimeExtractorConfiguration());
            case "TimePeriodExtractor":
                return new BaseTimePeriodExtractor(new SpanishTimePeriodExtractorConfiguration());
            //case "TimeZoneExtractor":
            //    return new BaseTimeZoneExtractor(new SpanishTimeZoneExtractorConfiguration(DateTimeOptions.EnablePreview));

            default:
                throw new NotSupportedException("Spanish extractor Type/Name not supported for type: " + name);
        }
    }

    private static IDateTimeExtractor getFrenchExtractor(String name) throws NotSupportedException {

        IOptionsConfiguration config = new BaseOptionsConfiguration();
        switch (name) {
            case "DateExtractor":
                return new BaseDateExtractor(new FrenchDateExtractorConfiguration(config));
            case "DatePeriodExtractor":
                return new BaseDatePeriodExtractor(new FrenchDatePeriodExtractorConfiguration(config));
//            case "DateTimeAltExtractor":
//                return new BaseDateTimeAltExtractor(new FrenchDateTimeAltExtractorConfiguration(config));
            case "DateTimeExtractor":
                return new BaseDateTimeExtractor(new FrenchDateTimeExtractorConfiguration());
            case "DateTimePeriodExtractor":
                return new BaseDateTimePeriodExtractor(new FrenchDateTimePeriodExtractorConfiguration());
            case "DurationExtractor":
                return new BaseDurationExtractor(new FrenchDurationExtractorConfiguration());
            case "HolidayExtractor":
                return new BaseHolidayExtractor(new FrenchHolidayExtractorConfiguration());
            case "MergedExtractor":
                return new BaseMergedDateTimeExtractor(new FrenchMergedExtractorConfiguration(DateTimeOptions.None));
            case "MergedExtractorSkipFromTo":
                return new BaseMergedDateTimeExtractor(new FrenchMergedExtractorConfiguration(DateTimeOptions.SkipFromToMerge));
            case "SetExtractor":
                return new BaseSetExtractor(new FrenchSetExtractorConfiguration());
            case "TimeExtractor":
                return new BaseTimeExtractor(new FrenchTimeExtractorConfiguration());
            case "TimePeriodExtractor":
                return new BaseTimePeriodExtractor(new FrenchTimePeriodExtractorConfiguration());
            case "TimeZoneExtractor":
                return new BaseTimeZoneExtractor(new FrenchTimeZoneExtractorConfiguration(DateTimeOptions.EnablePreview));

            default:
                throw new NotSupportedException("French extractor Type/Name not supported for type: " + name);
        }
    }
}
