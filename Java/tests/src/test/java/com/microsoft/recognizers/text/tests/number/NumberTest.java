package com.microsoft.recognizers.text.tests.number;

import com.microsoft.recognizers.text.ExtendedModelResult;
import com.microsoft.recognizers.text.ModelResult;
import com.microsoft.recognizers.text.number.NumberOptions;
import com.microsoft.recognizers.text.number.NumberRecognizer;
import com.microsoft.recognizers.text.tests.AbstractTest;
import com.microsoft.recognizers.text.tests.DependencyConstants;
import com.microsoft.recognizers.text.tests.NotSupportedException;
import com.microsoft.recognizers.text.tests.TestCase;
import org.javatuples.Pair;
import org.junit.Assert;
import org.junit.AssumptionViolatedException;
import org.junit.runners.Parameterized;

import java.util.ArrayList;
import java.util.Collection;
import java.util.Collections;
import java.util.List;
import java.util.stream.IntStream;

public class NumberTest extends AbstractTest {

    private static final String recognizerType = "Number";
    @Parameterized.Parameters(name = "{0}")
    public static Collection<TestCase> testCases() {
        return AbstractTest.enumerateTestCases(recognizerType, "Model");
    }

    public NumberTest(TestCase currentCase) {
        super(currentCase);
    }

    @Override
    protected void recognizeAndAssert(TestCase currentCase) {

        // parse
        List<ModelResult> results = recognize(currentCase);

        // assert
        assertResultsNumber(currentCase, results, new ArrayList() {{ add("value");}});
    }

    public static <T extends ModelResult> void assertResultsNumber(TestCase currentCase, List<T> results, List<String> testResolutionKeys) {

        List<ExtendedModelResult> expectedResults = readExpectedResults(ExtendedModelResult.class, currentCase.results);
        Assert.assertEquals(getMessage(currentCase, "\"Result Count\""), expectedResults.size(), results.size());

        IntStream.range(0, expectedResults.size())
                .mapToObj(i -> Pair.with(expectedResults.get(i), results.get(i)))
                .forEach(t -> {
                    ExtendedModelResult expected = t.getValue0();
                    T actual = t.getValue1();

                    Assert.assertEquals(getMessage(currentCase, "typeName"), expected.typeName, actual.typeName);
                    Assert.assertEquals(getMessage(currentCase, "text"), expected.text, actual.text);

                    // Number and NumberWithUnit are supported currently.
                    Assert.assertEquals(getMessage(currentCase, "start"), expected.start, actual.start);
                    Assert.assertEquals(getMessage(currentCase, "end"), expected.end, actual.end);

                    for (String key : testResolutionKeys) {
                        Assert.assertEquals(getMessage(currentCase, key), expected.resolution.get(key), actual.resolution.get(key));
                    }
                });
    }

    @Override
    public List<ModelResult> recognize(TestCase currentCase) {

        try {
            String culture = getCultureCode(currentCase.language);
            switch (currentCase.modelName) {
                case "NumberModel":
                    return NumberRecognizer.recognizeNumber(currentCase.input, culture, NumberOptions.None, false);
                case "NumberModelPercentMode":
                    return NumberRecognizer.recognizeNumber(currentCase.input, culture, NumberOptions.PercentageMode, false);
                case "OrdinalModel":
                    return NumberRecognizer.recognizeOrdinal(currentCase.input, culture, NumberOptions.None, false);
                case "PercentModel":
                    return NumberRecognizer.recognizePercentage(currentCase.input, culture, NumberOptions.None, false);
                case "PercentModelPercentMode":
                    return NumberRecognizer.recognizePercentage(currentCase.input, culture, NumberOptions.PercentageMode, false);
                case "NumberRangeModel":
                    return NumberRecognizer.recognizeNumberRange(currentCase.input, culture, NumberOptions.None, false);
                default:
                    throw new NotSupportedException("Model Type/Name not supported: " + currentCase.modelName + " in " + culture);
            }
        } catch (IllegalArgumentException ex) {

            // Model not existing can be considered a skip. Other exceptions should fail tests.
            if (ex.getMessage().toLowerCase().contains(DependencyConstants.BASE_RECOGNIZERS_MODEL_UNAVAILABLE)) {
                throw new AssumptionViolatedException(ex.getMessage(), ex);
            } else throw new IllegalArgumentException(ex.getMessage(), ex);
        } catch (NotSupportedException nex) {
            throw new AssumptionViolatedException(nex.getMessage(), nex);
        }
    }
}
