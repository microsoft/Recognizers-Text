package com.microsoft.recognizers.text.tests.number;

import com.microsoft.recognizers.text.ModelResult;
import com.microsoft.recognizers.text.ResolutionKey;
import com.microsoft.recognizers.text.number.NumberOptions;
import com.microsoft.recognizers.text.number.NumberRecognizer;
import com.microsoft.recognizers.text.tests.AbstractTest;
import com.microsoft.recognizers.text.tests.DependencyConstants;
import com.microsoft.recognizers.text.tests.NotSupportedException;
import com.microsoft.recognizers.text.tests.TestCase;
import org.junit.Assert;
import org.junit.AssumptionViolatedException;
import org.junit.runners.Parameterized;

import java.util.Arrays;
import java.util.Collection;
import java.util.List;

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
        assertResults(currentCase, results, getKeysToTest(currentCase));
    }

    private List<String> getKeysToTest(TestCase currentCase) {
        switch (currentCase.modelName) {
            case "OrdinalModel":
                return Arrays.asList(ResolutionKey.Value, ResolutionKey.Offset, ResolutionKey.RelativeTo);
            default:
                return Arrays.asList(ResolutionKey.Value);
        }
    }

    @Override
    public List<ModelResult> recognize(TestCase currentCase) {

        try {
            String culture = getCultureCode(currentCase.language);
            switch (currentCase.modelName) {
                case "NumberModel":
                    return NumberRecognizer.recognizeNumber(currentCase.input, culture, NumberOptions.None, false);
                case "NumberModelExperimentalMode":
                    return NumberRecognizer.recognizeNumber(currentCase.input, culture, NumberOptions.ExperimentalMode, false);
                case "NumberModelPercentMode":
                    return NumberRecognizer.recognizeNumber(currentCase.input, culture, NumberOptions.PercentageMode, false);
                case "NumberRangeModel":
                    return NumberRecognizer.recognizeNumberRange(currentCase.input, culture, NumberOptions.None, false);
                case "NumberRangeModelExperimentalMode":
                    return NumberRecognizer.recognizeNumberRange(currentCase.input, culture, NumberOptions.ExperimentalMode, false);
                case "OrdinalModel":
                case "OrdinalModelSuppressExtendedTypes":
                    return NumberRecognizer.recognizeOrdinal(currentCase.input, culture, NumberOptions.None, false);
                case "PercentModel":
                    return NumberRecognizer.recognizePercentage(currentCase.input, culture, NumberOptions.None, false);
                case "PercentModelPercentMode":
                    return NumberRecognizer.recognizePercentage(currentCase.input, culture, NumberOptions.PercentageMode, false);
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
