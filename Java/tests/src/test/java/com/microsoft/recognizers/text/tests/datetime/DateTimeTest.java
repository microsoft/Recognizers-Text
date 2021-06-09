package com.microsoft.recognizers.text.tests.datetime;

import com.microsoft.recognizers.text.ExtendedModelResult;
import com.microsoft.recognizers.text.ModelResult;
import com.microsoft.recognizers.text.ResolutionKey;
import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.DateTimeRecognizer;
import com.microsoft.recognizers.text.tests.AbstractTest;
import com.microsoft.recognizers.text.tests.DependencyConstants;
import com.microsoft.recognizers.text.tests.NotSupportedException;
import com.microsoft.recognizers.text.tests.TestCase;

import java.time.LocalDateTime;
import java.util.Collection;
import java.util.List;
import java.util.Map;
import java.util.stream.IntStream;

import org.javatuples.Pair;
import org.junit.Assert;
import org.junit.AssumptionViolatedException;
import org.junit.runners.Parameterized;

public class DateTimeTest extends AbstractTest {

    private static final String recognizerType = "DateTime";

    @Parameterized.Parameters(name = "{0}")
    public static Collection<TestCase> testCases() {
        return AbstractTest.enumerateTestCases(recognizerType, "Model");
    }

    public DateTimeTest(TestCase currentCase) {
        super(currentCase);
    }

    @Override
    protected void recognizeAndAssert(TestCase currentCase) {

        // parse
        List<ModelResult> results = recognize(currentCase);

        // assert
        assertResultsDateTime(currentCase, results);
    }

    public static <T extends ModelResult> void assertResultsDateTime(TestCase currentCase, List<T> results) {

        List<ExtendedModelResult> expectedResults = readExpectedResults(ExtendedModelResult.class, currentCase.results);
        Assert.assertEquals(getMessage(currentCase, "\"Result Count\""), expectedResults.size(), results.size());

        IntStream.range(0, expectedResults.size())
                .mapToObj(i -> Pair.with(expectedResults.get(i), results.get(i)))
                .forEach(t -> {
                    ExtendedModelResult expected = t.getValue0();
                    T actual = t.getValue1();

                    Assert.assertEquals(getMessage(currentCase, "typeName"), expected.typeName, actual.typeName);
                    Assert.assertEquals(getMessage(currentCase, "text"), expected.text, actual.text);
                    if (actual instanceof ExtendedModelResult) {
                        Assert.assertEquals(getMessage(currentCase, "parentText"),
                                            expected.parentText, ((ExtendedModelResult)actual).parentText);
                    }

                    if (expected.resolution.containsKey(ResolutionKey.ValueSet)) {

                        Assert.assertNotNull(getMessage(currentCase, "resolution"), actual.resolution);

                        Assert.assertNotNull(getMessage(currentCase,
                                             ResolutionKey.ValueSet), actual.resolution.get(ResolutionKey.ValueSet));

                        assertValueSet(currentCase,
                            (List<Map<String, Object>>)expected.resolution.get(ResolutionKey.ValueSet),
                            (List<Map<String, Object>>)actual.resolution.get(ResolutionKey.ValueSet));
                    }
                });
    }

    private static void assertValueSet(TestCase currentCase, List<Map<String, Object>> expected, List<Map<String, Object>> actual) {

        Assert.assertEquals(getMessage(currentCase, "\"Result Count\""), expected.size(), actual.size());

        expected.sort((a, b) -> {
            String timexA = (String)a.getOrDefault("timex", "");
            String timexB = (String)b.getOrDefault("timex", "");
            return timexA.compareTo(timexB);
        });

        actual.sort((a, b) -> {
            String timexA = (String)a.getOrDefault("timex", "");
            String timexB = (String)b.getOrDefault("timex", "");
            return timexA.compareTo(timexB);
        });

        IntStream.range(0, expected.size())
                .mapToObj(i -> Pair.with(expected.get(i), actual.get(i)))
                .forEach(t -> {
                    Map<String, Object> expectedMap = t.getValue0();
                    Map<String, Object> actualMap = t.getValue1();

                    expectedMap.keySet().forEach(key -> {
                        Assert.assertTrue(getMessage(currentCase, key), actualMap.containsKey(key));
                        Assert.assertEquals(getMessage(currentCase, key), expectedMap.get(key), actualMap.get(key));
                    });
                });
    }

    @Override
    protected List<ModelResult> recognize(TestCase currentCase) {

        try {
            String culture = getCultureCode(currentCase.language);
            LocalDateTime reference = currentCase.getReferenceDateTime();
            switch (currentCase.modelName) {
                case "DateTimeModel":
                    return DateTimeRecognizer.recognizeDateTime(currentCase.input, culture, DateTimeOptions.None, false, reference);
                case "DateTimeModelCalendarMode":
                    return DateTimeRecognizer.recognizeDateTime(currentCase.input, culture, DateTimeOptions.CalendarMode, false, reference);
                case "DateTimeModelExperimentalMode":
                    return DateTimeRecognizer.recognizeDateTime(currentCase.input, culture, DateTimeOptions.ExperimentalMode, false, reference);
                case "DateTimeModelExtendedTypes":
                    return DateTimeRecognizer.recognizeDateTime(currentCase.input, culture, DateTimeOptions.ExtendedTypes, false, reference);
                case "DateTimeModelSplitDateAndTime":
                    return DateTimeRecognizer.recognizeDateTime(currentCase.input, culture, DateTimeOptions.SplitDateAndTime, false, reference);
                case "DateTimeModelComplexCalendar":
                    return DateTimeRecognizer.recognizeDateTime(currentCase.input, culture, DateTimeOptions.ComplexCalendar, false, reference);
                default:
                    throw new NotSupportedException("Model Type/Name not supported: " + currentCase.modelName + " in " + culture);
            }
        } catch (IllegalArgumentException ex) {

            // Model not existing in a given culture can be considered a skip. Other illegal argument exceptions should fail tests.
            if (ex.getMessage().toLowerCase().contains(DependencyConstants.BASE_RECOGNIZERS_MODEL_UNAVAILABLE)) {
                throw new AssumptionViolatedException(ex.getMessage(), ex);
            } else throw new IllegalArgumentException(ex.getMessage(), ex);
        } catch (NotSupportedException nex) {
            throw new AssumptionViolatedException(nex.getMessage(), nex);
        }
    }
}
