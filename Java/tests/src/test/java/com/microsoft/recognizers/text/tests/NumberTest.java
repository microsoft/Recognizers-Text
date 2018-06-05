package com.microsoft.recognizers.text.tests;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.ModelResult;
import com.microsoft.recognizers.text.number.NumberOptions;
import com.microsoft.recognizers.text.number.NumberRecognizer;
import org.junit.Assert;
import org.junit.AssumptionViolatedException;
import org.junit.runners.Parameterized;

import java.util.Arrays;
import java.util.Collection;
import java.util.List;

public class NumberTest extends AbstractTest {

    public NumberTest(TestCase currentCase) {
        super(currentCase);
    }

    @Override
    void test() {
        // parse
        List<ModelResult> results = recognize(this.currentCase);

        // assert
        List<ModelResult> expected = readExpectedResults(ModelResult.class, this.currentCase.results);

        Assert.assertEquals("Result count does not match.", expected.size(), results.size());

//        foreach (var tuple in Enumerable.Zip(expectedResults, actualResults, Tuple.Create))
//        {
//            var expected = tuple.Item1;
//            var actual = tuple.Item2;
//
//            Assert.AreEqual(expected.TypeName, actual.TypeName, GetMessage(TestSpec));
//            Assert.AreEqual(expected.Text, actual.Text, GetMessage(TestSpec));
//
//            Assert.AreEqual(expected.Resolution[ResolutionKey.Value], actual.Resolution[ResolutionKey.Value], GetMessage(TestSpec));
//
//            foreach (var key in testResolutionKeys ?? Enumerable.Empty<string>())
//            {
//                if (!actual.Resolution.ContainsKey(key) && !expected.Resolution.ContainsKey(key))
//                {
//                    continue;
//                }
//
//                Assert.AreEqual(expected.Resolution[key], actual.Resolution[key], GetMessage(TestSpec));
//            }
//        }
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
            throw new AssumptionViolatedException(ex.getMessage());
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
