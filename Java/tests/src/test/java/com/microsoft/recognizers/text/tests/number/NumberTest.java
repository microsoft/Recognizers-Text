package com.microsoft.recognizers.text.tests.number;

import com.microsoft.recognizers.text.ModelResult;
import com.microsoft.recognizers.text.number.NumberOptions;
import com.microsoft.recognizers.text.number.NumberRecognizer;
import com.microsoft.recognizers.text.tests.AbstractTest;
import com.microsoft.recognizers.text.tests.TestCase;
import org.junit.AssumptionViolatedException;
import org.junit.runners.Parameterized;

import java.util.Collection;
import java.util.List;

public class NumberTest extends AbstractTest {

    private static final String recognizerType = "Number";
    @Parameterized.Parameters(name = "{0}")
    public static Collection<TestCase> testCases() {
        return AbstractTest.enumerateTestCases(recognizerType);
    }

    public NumberTest(TestCase currentCase) {
        super(currentCase);
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
                    throw new AssumptionViolatedException("Model Type/Name not supported.");
            }
        } catch (IllegalArgumentException ex) {
            throw new AssumptionViolatedException(ex.getMessage(), ex);
        }
    }
}
