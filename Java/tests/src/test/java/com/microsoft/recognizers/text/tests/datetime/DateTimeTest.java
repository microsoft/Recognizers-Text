package com.microsoft.recognizers.text.tests.datetime;

import com.microsoft.recognizers.text.ModelResult;
import com.microsoft.recognizers.text.ResolutionKey;
import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.DateTimeRecognizer;
import com.microsoft.recognizers.text.datetime.DateTimeResolutionKey;
import com.microsoft.recognizers.text.tests.AbstractTest;
import com.microsoft.recognizers.text.tests.DependencyConstants;
import com.microsoft.recognizers.text.tests.NotSupportedException;
import com.microsoft.recognizers.text.tests.TestCase;

import java.time.LocalDateTime;
import java.util.Arrays;
import java.util.Collection;
import java.util.HashMap;
import java.util.List;
import java.util.stream.IntStream;

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
        assertResults(currentCase, results, getKeysToTest(currentCase));
    }

    private List<String> getKeysToTest(TestCase currentCase) {
        switch (currentCase.modelName) {
            case "DateTimeModelExtendedTypes":
            case "DateTimeModelSplitDateAndTime":
                return Arrays.asList(DateTimeResolutionKey.Timex, ResolutionKey.Type, ResolutionKey.Value, DateTimeResolutionKey.START, DateTimeResolutionKey.END, DateTimeResolutionKey.Mod);
            default:
                return Arrays.asList(DateTimeResolutionKey.Timex, ResolutionKey.Type, ResolutionKey.Value, DateTimeResolutionKey.START, DateTimeResolutionKey.END, DateTimeResolutionKey.Mod, DateTimeResolutionKey.SourceEntity);
        }
    }

    @Override
    protected void assertResolutionKeys(ModelResult expected, ModelResult actual, TestCase currentCase, List<String> testResolutionKeys) {
        if (expected.resolution.get(ResolutionKey.ValueSet) instanceof List) {
            List<HashMap<String, String>> expectedValueSet = (List<HashMap<String, String>>) expected.resolution.get(ResolutionKey.ValueSet);
            List<HashMap<String, String>> actualValueSet = (List<HashMap<String, String>>) actual.resolution.get(ResolutionKey.ValueSet);

            IntStream.range(0, expectedValueSet.size())
                .forEach(idx -> {
                    // Here we assign the index of the expected and actual lists of values
                    // Inside the 2 new variables
                    HashMap<String, String> expectedValues = expectedValueSet.get(idx);
                    HashMap<String, String> actualValues = actualValueSet.get(idx);
                    for (String key: testResolutionKeys) {
                        Assert.assertEquals(
                            getMessage(currentCase, key),
                            expectedValues.get(key),
                            actualValues.get(key));
                    }
                }
            );
        }
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
