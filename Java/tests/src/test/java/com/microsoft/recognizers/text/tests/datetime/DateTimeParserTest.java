package com.microsoft.recognizers.text.tests.datetime;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.core.type.TypeReference;
import com.fasterxml.jackson.databind.MapperFeature;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.ModelResult;
import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.english.parsers.EnglishCommonDateTimeParserConfiguration;
import com.microsoft.recognizers.text.datetime.english.parsers.EnglishDateParserConfiguration;
import com.microsoft.recognizers.text.datetime.english.parsers.EnglishDatePeriodParserConfiguration;
import com.microsoft.recognizers.text.datetime.english.parsers.EnglishDateTimeAltParserConfiguration;
import com.microsoft.recognizers.text.datetime.english.parsers.EnglishDateTimeParserConfiguration;
import com.microsoft.recognizers.text.datetime.english.parsers.EnglishDateTimePeriodParserConfiguration;
import com.microsoft.recognizers.text.datetime.english.parsers.EnglishDurationParserConfiguration;
import com.microsoft.recognizers.text.datetime.english.parsers.EnglishHolidayParserConfiguration;
import com.microsoft.recognizers.text.datetime.english.parsers.EnglishMergedParserConfiguration;
import com.microsoft.recognizers.text.datetime.english.parsers.EnglishSetParserConfiguration;
import com.microsoft.recognizers.text.datetime.english.parsers.EnglishTimeParserConfiguration;
import com.microsoft.recognizers.text.datetime.english.parsers.EnglishTimePeriodParserConfiguration;
import com.microsoft.recognizers.text.datetime.english.parsers.TimeParser;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.french.parsers.FrenchCommonDateTimeParserConfiguration;
import com.microsoft.recognizers.text.datetime.french.parsers.FrenchDateParserConfiguration;
import com.microsoft.recognizers.text.datetime.french.parsers.FrenchDatePeriodParserConfiguration;
import com.microsoft.recognizers.text.datetime.french.parsers.FrenchDateTimeParserConfiguration;
import com.microsoft.recognizers.text.datetime.french.parsers.FrenchDateTimePeriodParserConfiguration;
import com.microsoft.recognizers.text.datetime.french.parsers.FrenchDurationParserConfiguration;
import com.microsoft.recognizers.text.datetime.french.parsers.FrenchHolidayParserConfiguration;
import com.microsoft.recognizers.text.datetime.french.parsers.FrenchMergedParserConfiguration;
import com.microsoft.recognizers.text.datetime.french.parsers.FrenchSetParserConfiguration;
import com.microsoft.recognizers.text.datetime.french.parsers.FrenchTimeParser;
import com.microsoft.recognizers.text.datetime.french.parsers.FrenchTimeParserConfiguration;
import com.microsoft.recognizers.text.datetime.french.parsers.FrenchTimePeriodParserConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.BaseDateParser;
import com.microsoft.recognizers.text.datetime.parsers.BaseDatePeriodParser;
import com.microsoft.recognizers.text.datetime.parsers.BaseDateTimeAltParser;
import com.microsoft.recognizers.text.datetime.parsers.BaseDateTimeParser;
import com.microsoft.recognizers.text.datetime.parsers.BaseDateTimePeriodParser;
import com.microsoft.recognizers.text.datetime.parsers.BaseDurationParser;
import com.microsoft.recognizers.text.datetime.parsers.BaseHolidayParser;
import com.microsoft.recognizers.text.datetime.parsers.BaseMergedDateTimeParser;
import com.microsoft.recognizers.text.datetime.parsers.BaseSetParser;
import com.microsoft.recognizers.text.datetime.parsers.BaseTimePeriodParser;
import com.microsoft.recognizers.text.datetime.parsers.BaseTimeZoneParser;
import com.microsoft.recognizers.text.datetime.parsers.DateTimeParseResult;
import com.microsoft.recognizers.text.datetime.parsers.IDateTimeParser;
import com.microsoft.recognizers.text.datetime.spanish.parsers.DateTimePeriodParser;
import com.microsoft.recognizers.text.datetime.spanish.parsers.SpanishCommonDateTimeParserConfiguration;
import com.microsoft.recognizers.text.datetime.spanish.parsers.SpanishDateTimePeriodParserConfiguration;
import com.microsoft.recognizers.text.datetime.spanish.parsers.SpanishDateParserConfiguration;
import com.microsoft.recognizers.text.datetime.spanish.parsers.SpanishDatePeriodParserConfiguration;
import com.microsoft.recognizers.text.datetime.spanish.parsers.SpanishDateTimeParserConfiguration;
import com.microsoft.recognizers.text.datetime.spanish.parsers.SpanishDurationParserConfiguration;
import com.microsoft.recognizers.text.datetime.spanish.parsers.SpanishHolidayParserConfiguration;
import com.microsoft.recognizers.text.datetime.spanish.parsers.SpanishSetParserConfiguration;
import com.microsoft.recognizers.text.datetime.spanish.parsers.SpanishTimeParserConfiguration;
import com.microsoft.recognizers.text.datetime.spanish.parsers.SpanishTimePeriodParserConfiguration;
import com.microsoft.recognizers.text.datetime.utilities.DateTimeResolutionResult;
import com.microsoft.recognizers.text.datetime.utilities.TimeZoneResolutionResult;
import com.microsoft.recognizers.text.tests.AbstractTest;
import com.microsoft.recognizers.text.tests.NotSupportedException;
import com.microsoft.recognizers.text.tests.TestCase;
import com.microsoft.recognizers.text.tests.helpers.DateTimeResolutionResultMixIn;
import com.microsoft.recognizers.text.tests.helpers.TimeZoneResolutionResultMixIn;

import java.io.IOException;
import java.time.LocalDateTime;
import java.util.Collection;
import java.util.Comparator;
import java.util.List;
import java.util.Locale;
import java.util.Map;
import java.util.stream.Collectors;
import java.util.stream.IntStream;

import org.javatuples.Pair;
import org.junit.Assert;
import org.junit.AssumptionViolatedException;
import org.junit.runners.Parameterized;

public class DateTimeParserTest extends AbstractTest {

    private static final String recognizerType = "DateTime";

    @Parameterized.Parameters(name = "{0}")
    public static Collection<TestCase> testCases() {
        return AbstractTest.enumerateTestCases(recognizerType, "Parser");
    }

    public DateTimeParserTest(TestCase currentCase) {
        super(currentCase);
    }

    @Override
    protected List<ModelResult> recognize(TestCase currentCase) {
        return null;
    }

    protected List<DateTimeParseResult> parse(TestCase currentCase) {

        IDateTimeExtractor extractor = getExtractor(currentCase);
        IDateTimeParser parser = getParser(currentCase);
        LocalDateTime referenceDateTime = currentCase.getReferenceDateTime();
        List<ExtractResult> extractResult = extractor.extract(currentCase.input.toLowerCase(Locale.ROOT), referenceDateTime);
        return extractResult.stream().map(er -> parser.parse(er, referenceDateTime)).collect(Collectors.toList());
    }

    @Override
    protected void recognizeAndAssert(TestCase currentCase) {

        List<DateTimeParseResult> results = parse(currentCase);
        assertParseResults(currentCase, results);
    }

    public static void assertParseResults(TestCase currentCase, List<DateTimeParseResult> results) {

        List<DateTimeParseResult> expectedResults = readExpectedDateTimeParseResults(DateTimeParseResult.class, currentCase.results);
        Assert.assertEquals(getMessage(currentCase, "\"Result Count\""), expectedResults.size(), results.size());

        IntStream.range(0, expectedResults.size())
                .mapToObj(i -> Pair.with(expectedResults.get(i), results.get(i)))
                .forEach(t -> {
                    DateTimeParseResult expected = t.getValue0();
                    DateTimeParseResult actual = t.getValue1();

                    Assert.assertEquals(getMessage(currentCase, "type"), expected.getType(), actual.getType());
                    Assert.assertTrue(getMessage(currentCase, "text") + String.format(" expected: \"%s\" actual: \"%s\"", expected.getText(), actual.getText()), expected.getText().equalsIgnoreCase(actual.getText()));

                    Assert.assertEquals(getMessage(currentCase, "start"), expected.getStart(), actual.getStart());
                    Assert.assertEquals(getMessage(currentCase, "length"), expected.getLength(), actual.getLength());

                    if (currentCase.modelName.equals("MergedParser")) {
                        assertMergedParserResults(currentCase, expected, actual);
                    } else {
                        assertParserResults(currentCase, expected, actual);
                    }
                });
    }

    private static void assertParserResults(TestCase currentCase, DateTimeParseResult expected, DateTimeParseResult actual) {

        if (expected.getValue() != null) {
            DateTimeResolutionResult expectedValue = parseDateTimeResolutionResult(DateTimeResolutionResult.class, expected.getValue());
            DateTimeResolutionResult actualValue = (DateTimeResolutionResult)actual.getValue();

            Assert.assertEquals(getMessage(currentCase, "timex"), expectedValue.getTimex(), actualValue.getTimex());
            Assert.assertEquals(getMessage(currentCase, "futureResolution"), expectedValue.getFutureResolution(), actualValue.getFutureResolution());
            Assert.assertEquals(getMessage(currentCase, "pastResolution"), expectedValue.getPastResolution(), actualValue.getPastResolution());
        }
    }

    private static void assertMergedParserResults(TestCase currentCase, DateTimeParseResult expected, DateTimeParseResult actual) {

        if (expected.getValue() != null) {

            Map<String, List<Map<String, Object>>> expectedValue = parseDateTimeResolutionResult(expected.getValue());
            Map<String, List<Map<String, Object>>> actualValue = (Map<String, List<Map<String, Object>>>)actual.getValue();

            List<Map<String, Object>> expectedResults = expectedValue.get("values");
            List<Map<String, Object>> actualResults = actualValue.get("values");

            expectedResults.sort(Comparator.comparingInt(Map::hashCode));
            actualResults.sort(Comparator.comparingInt(Map::hashCode));

            Assert.assertEquals("Actual results size differs", expectedResults.size(), actualResults.size());

            IntStream.range(0, expectedResults.size()).mapToObj(i -> new Pair<>(expectedResults.get(i), actualResults.get(i))).forEach(o -> {

                Map<String, Object> expectedItem = o.getValue0();
                Map<String, Object> actualItem = o.getValue1();
                Assert.assertTrue(String.format("Keys error \n\tExpected:\t%s\n\tActual:\t%s",
                    String.join(",", expectedItem.keySet()), String.join(",", actualItem.keySet())), actualItem.keySet().containsAll(expectedItem.keySet()));
                for (String key : expectedItem.keySet()) {
                    if (actualItem.containsKey(key)) {
                        Assert.assertEquals(getMessage(currentCase, "values." + key), expectedItem.get(key), actualItem.get(key));
                    }
                }
            });
        }
    }

    private static IDateTimeParser getParser(TestCase currentCase) {

        try {
            String culture = getCultureCode(currentCase.language);
            String name = currentCase.modelName;
            switch (culture) {
                case Culture.English:
                    return getEnglishParser(name);
                case Culture.Spanish:
                    return getSpanishParser(name);
                case Culture.French:
                    return getFrenchParser(name);
                default:
                    throw new NotSupportedException("Parser Type/Name not supported for culture: " + culture);
            }
        } catch (NotSupportedException ex) {
            throw new AssumptionViolatedException(ex.getMessage(), ex);
        }
    }

    private static IDateTimeParser getEnglishParser(String name) throws NotSupportedException {

        switch (name) {
            case "DateParser":
                return new BaseDateParser(new EnglishDateParserConfiguration(new EnglishCommonDateTimeParserConfiguration(DateTimeOptions.None)));
            case "DatePeriodParser":
                return new BaseDatePeriodParser(new EnglishDatePeriodParserConfiguration(new EnglishCommonDateTimeParserConfiguration(DateTimeOptions.None)));
            case "DateTimeParser":
                return new BaseDateTimeParser(new EnglishDateTimeParserConfiguration(new EnglishCommonDateTimeParserConfiguration(DateTimeOptions.None)));
            case "DateTimePeriodParser":
                return new BaseDateTimePeriodParser(new EnglishDateTimePeriodParserConfiguration(new EnglishCommonDateTimeParserConfiguration(DateTimeOptions.None)));
            case "DurationParser":
                return new BaseDurationParser(new EnglishDurationParserConfiguration(new EnglishCommonDateTimeParserConfiguration(DateTimeOptions.None)));
            case "HolidayParser":
                return new BaseHolidayParser(new EnglishHolidayParserConfiguration());
            case "SetParser":
                return new BaseSetParser(new EnglishSetParserConfiguration(new EnglishCommonDateTimeParserConfiguration(DateTimeOptions.None)));
            case "TimeParser":
                return new TimeParser(new EnglishTimeParserConfiguration(new EnglishCommonDateTimeParserConfiguration(DateTimeOptions.None)));
            case "TimePeriodParser":
                return new BaseTimePeriodParser(new EnglishTimePeriodParserConfiguration(new EnglishCommonDateTimeParserConfiguration(DateTimeOptions.None)));
            case "TimeZoneParser":
                return new BaseTimeZoneParser();
            case "DateTimeAltParser":
                return new BaseDateTimeAltParser(new EnglishDateTimeAltParserConfiguration(new EnglishCommonDateTimeParserConfiguration(DateTimeOptions.None)));
            case "MergedParser":
                return new BaseMergedDateTimeParser(new EnglishMergedParserConfiguration(DateTimeOptions.None));
            default:
                throw new NotSupportedException("English parser Type/Name not supported for type: " + name);
        }
    }

    private static IDateTimeParser getSpanishParser(String name) throws NotSupportedException {

        switch (name) {
            case "DateParser":
                return new BaseDateParser(new SpanishDateParserConfiguration(new SpanishCommonDateTimeParserConfiguration(DateTimeOptions.None)));
            case "DatePeriodParser":
                return new BaseDatePeriodParser(new SpanishDatePeriodParserConfiguration(new SpanishCommonDateTimeParserConfiguration(DateTimeOptions.None)));
            //case "DateTimeAltParser":
            //    return new BaseDateTimeAltParser(new SpanishDateTimeAltParserConfiguration(new EnglishCommonDateTimeParserConfiguration(DateTimeOptions.None)));
            case "DateTimeParser":
                return new BaseDateTimeParser(new SpanishDateTimeParserConfiguration(new SpanishCommonDateTimeParserConfiguration(DateTimeOptions.None)));
            case "DateTimePeriodParser":
                return new DateTimePeriodParser(new SpanishDateTimePeriodParserConfiguration(new SpanishCommonDateTimeParserConfiguration(DateTimeOptions.None)));
            case "DurationParser":
                return new BaseDurationParser(new SpanishDurationParserConfiguration(new SpanishCommonDateTimeParserConfiguration(DateTimeOptions.None)));
            case "HolidayParser":
                return new BaseHolidayParser(new SpanishHolidayParserConfiguration());
            case "SetParser":
                return new BaseSetParser(new SpanishSetParserConfiguration(new SpanishCommonDateTimeParserConfiguration(DateTimeOptions.None)));
            //case "MergedParser":
            //    return new BaseMergedDateTimeParser(new SpanishMergedParserConfiguration(DateTimeOptions.None));
            case "TimeParser":
                return new TimeParser(new SpanishTimeParserConfiguration(new SpanishCommonDateTimeParserConfiguration(DateTimeOptions.None)));
            case "TimePeriodParser":
                return new BaseTimePeriodParser(new SpanishTimePeriodParserConfiguration(new SpanishCommonDateTimeParserConfiguration(DateTimeOptions.None)));
            default:
                throw new NotSupportedException("Spanish parser Type/Name not supported for type: " + name);
        }
    }
    private static IDateTimeParser getFrenchParser(String name) throws NotSupportedException {

        switch (name) {
            case "DateParser":
                return new BaseDateParser(new FrenchDateParserConfiguration(new FrenchCommonDateTimeParserConfiguration(DateTimeOptions.None)));
            case "DatePeriodParser":
                return new BaseDatePeriodParser(new FrenchDatePeriodParserConfiguration(new FrenchCommonDateTimeParserConfiguration(DateTimeOptions.None)));
            //case "DateTimeAltParser":
            //    return new BaseDateTimeAltParser(new FrenchDateTimeAltParserConfiguration(new EnglishCommonDateTimeParserConfiguration(DateTimeOptions.None)));
            case "DateTimeParser":
                return new BaseDateTimeParser(new FrenchDateTimeParserConfiguration(new FrenchCommonDateTimeParserConfiguration(DateTimeOptions.None)));
            case "DateTimePeriodParser":
                return new BaseDateTimePeriodParser(new FrenchDateTimePeriodParserConfiguration(new FrenchCommonDateTimeParserConfiguration(DateTimeOptions.None)));
            case "DurationParser":
                return new BaseDurationParser(new FrenchDurationParserConfiguration(new FrenchCommonDateTimeParserConfiguration(DateTimeOptions.None)));
            case "HolidayParser":
                return new BaseHolidayParser(new FrenchHolidayParserConfiguration());
            case "SetParser":
                return new BaseSetParser(new FrenchSetParserConfiguration(new FrenchCommonDateTimeParserConfiguration(DateTimeOptions.None)));
            case "MergedParser":
                return new BaseMergedDateTimeParser(new FrenchMergedParserConfiguration(DateTimeOptions.None));
            case "TimeParser":
                return new FrenchTimeParser(new FrenchTimeParserConfiguration(new FrenchCommonDateTimeParserConfiguration(DateTimeOptions.None)));
            case "TimePeriodParser":
                return new BaseTimePeriodParser(new FrenchTimePeriodParserConfiguration(new FrenchCommonDateTimeParserConfiguration(DateTimeOptions.None)));
            default:
                throw new NotSupportedException("French parser Type/Name not supported for type: " + name);
        }
    }

    private IDateTimeExtractor getExtractor(TestCase currentCase) {

        String extractorName = currentCase.modelName.replace("Parser", "Extractor");
        return DateTimeExtractorTest.getExtractor(currentCase.language, extractorName);
    }

    public static <T extends DateTimeResolutionResult> T parseDateTimeResolutionResult(Class<T> dateTimeResolutionResultClass, Object result) {

        // Deserializer
        ObjectMapper mapper = new ObjectMapper();
        mapper.configure(MapperFeature.ACCEPT_CASE_INSENSITIVE_PROPERTIES, true);
        mapper.addMixIn(DateTimeResolutionResult.class, DateTimeResolutionResultMixIn.class);
        mapper.addMixIn(TimeZoneResolutionResult.class, TimeZoneResolutionResultMixIn.class);

        try {
            String json = mapper.writeValueAsString(result);
            return mapper.readValue(json, dateTimeResolutionResultClass);

        } catch (JsonProcessingException e) {
            e.printStackTrace();
            return null;

        } catch (IOException e) {
            e.printStackTrace();
            return null;
        }
    }

    public static <T extends Map> T parseDateTimeResolutionResult(Object result) {

        // Deserializer
        ObjectMapper mapper = new ObjectMapper();

        try {
            String json = mapper.writeValueAsString(result);
            return mapper.readValue(json, new TypeReference<Map<String, Object>>() {
            });

        } catch (JsonProcessingException e) {
            e.printStackTrace();
            return null;

        } catch (IOException e) {
            e.printStackTrace();
            return null;
        }
    }
}
