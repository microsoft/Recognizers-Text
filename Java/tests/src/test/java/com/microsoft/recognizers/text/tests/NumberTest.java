package com.microsoft.recognizers.text.tests;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.ModelResult;
import com.microsoft.recognizers.text.ResolutionKey;
import com.microsoft.recognizers.text.number.NumberOptions;
import com.microsoft.recognizers.text.number.NumberRecognizer;
import org.junit.Assert;
import org.junit.AssumptionViolatedException;
import org.junit.runners.Parameterized;
import org.javatuples.Pair;

import java.util.Arrays;
import java.util.Collection;
import java.util.List;
import java.util.stream.IntStream;

public class NumberTest extends AbstractTest {

    public NumberTest(TestCase currentCase) {
        super(currentCase);
    }

    @Override
    void test() {
        // parse
        List<ModelResult> results = recognize(currentCase);

        // assert
        List<ModelResult> expectedResults = readExpectedResults(ModelResult.class, currentCase.results);
        Assert.assertEquals(getMessage(currentCase, "\"Result Count\""), expectedResults.size(), results.size());

        IntStream.range(0, Math.min(results.size(), expectedResults.size()))
            .mapToObj(i -> Pair.with(expectedResults.get(i), results.get(i)))
            .forEach(t -> {
                ModelResult expected = t.getValue0();
                ModelResult actual = t.getValue1();

                Assert.assertEquals(getMessage(currentCase, "typeName"), expected.typeName, actual.typeName);
                Assert.assertEquals(getMessage(currentCase, "text"), expected.text, actual.text);

                Assert.assertEquals(getMessage(currentCase, "resolution.value"), expected.resolution.get(ResolutionKey.Value), actual.resolution.get(ResolutionKey.Value));
            });
    }

    private String getMessage(TestCase testCase, String propName) {
        return "Does not match " + propName + " on Input: \"" + testCase.input + "\"";
    }

    private static List<ModelResult> recognize(TestCase currentCase) {
        try {
            switch (currentCase.modelName) {
                case "NumberModel":
                    return NumberRecognizer.recognizeNumber(currentCase.input, getCultureCode(currentCase.language), NumberOptions.None, false);
                case "NumberModelPercentMode":
                    return NumberRecognizer.recognizeNumber(currentCase.input, getCultureCode(currentCase.language), NumberOptions.PercentageMode, false);
                case "OrdinalModel":
                    return NumberRecognizer.recognizeOrdinal(currentCase.input, getCultureCode(currentCase.language), NumberOptions.None, false);
                case "PercentModel":
                    return NumberRecognizer.recognizePercentage(currentCase.input, getCultureCode(currentCase.language), NumberOptions.None, false);
                case "PercentModelPercentMode":
                    return NumberRecognizer.recognizePercentage(currentCase.input, getCultureCode(currentCase.language), NumberOptions.PercentageMode, false);
                case "NumberRangeModel":
                    return NumberRecognizer.recognizeNumberRange(currentCase.input, getCultureCode(currentCase.language), NumberOptions.None, false);
                default:
                    throw new AssumptionViolatedException("Model Type/Name not supported.");
            }
        } catch (IllegalArgumentException ex) {
            throw new AssumptionViolatedException(ex.getMessage(), ex);
        }
    }

    private static String getCultureCode(String language) {
        return Arrays.stream(Culture.SupportedCultures)
                .filter(c -> c.cultureName.equalsIgnoreCase(language))
                .findFirst().get().cultureCode;
    }

    private static final String recognizerType = "Number";

    @Parameterized.Parameters(name = "{0}")
    public static Collection<TestCase> testCases() {
        return AbstractTest.enumerateTestCases(recognizerType);
    }
}
