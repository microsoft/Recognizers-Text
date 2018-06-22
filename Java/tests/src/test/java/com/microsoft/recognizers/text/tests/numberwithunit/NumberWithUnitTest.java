package com.microsoft.recognizers.text.tests.numberwithunit;

import com.microsoft.recognizers.text.ModelResult;
import com.microsoft.recognizers.text.ResolutionKey;
import com.microsoft.recognizers.text.numberwithunit.NumberWithUnitOptions;
import com.microsoft.recognizers.text.numberwithunit.NumberWithUnitRecognizer;
import com.microsoft.recognizers.text.tests.AbstractTest;
import com.microsoft.recognizers.text.tests.TestCase;
import org.junit.AssumptionViolatedException;
import org.junit.runners.Parameterized;

import java.util.Arrays;
import java.util.Collection;
import java.util.List;

public class NumberWithUnitTest extends AbstractTest {

    private static final String recognizerType = "NumberWithUnit";

    @Parameterized.Parameters(name = "{0}")
    public static Collection<TestCase> testCases() {
        return AbstractTest.enumerateTestCases(recognizerType);
    }

    public NumberWithUnitTest(TestCase currentCase) {
        super(currentCase);
    }

    @Override
    protected void recognizeAndAssert(TestCase currentCase) {
        // parse
        List<ModelResult> results = recognize(currentCase);

        // assert
        assertResultsWithKeys(currentCase, results, getKeysToTest(currentCase));
    }

    private List<String> getKeysToTest(TestCase currentCase) {
        switch (currentCase.modelName) {
            case "CurrencyModel":
                return Arrays.asList(ResolutionKey.Unit, ResolutionKey.Unit, ResolutionKey.IsoCurrency);
            default:
                return Arrays.asList(ResolutionKey.Unit);
        }
    }

    @Override
    protected List<ModelResult> recognize(TestCase currentCase) {
        try {
            String culture = getCultureCode(currentCase.language);
            switch (currentCase.modelName) {
                case "AgeModel":
                    return NumberWithUnitRecognizer.recognizeAge(currentCase.input, culture, NumberWithUnitOptions.None, false);
                case "CurrencyModel":
                    return NumberWithUnitRecognizer.recognizeCurrency(currentCase.input, culture, NumberWithUnitOptions.None, false);
                case "DimensionModel":
                    return NumberWithUnitRecognizer.recognizeDimension(currentCase.input, culture, NumberWithUnitOptions.None, false);
                case "TemperatureModel":
                    return NumberWithUnitRecognizer.recognizeTemperature(currentCase.input, culture, NumberWithUnitOptions.None, false);
                default:
                    throw new AssumptionViolatedException("Model Type/Name not supported.");
            }
        } catch (IllegalArgumentException ex) {
            throw new AssumptionViolatedException(ex.getMessage(), ex);
        }
    }
}

