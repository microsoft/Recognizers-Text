package com.microsoft.recognizers.text.tests.choice;

import com.microsoft.recognizers.text.ModelResult;
import com.microsoft.recognizers.text.ResolutionKey;
import com.microsoft.recognizers.text.choice.ChoiceOptions;
import com.microsoft.recognizers.text.choice.ChoiceRecognizer;
import com.microsoft.recognizers.text.tests.AbstractTest;
import com.microsoft.recognizers.text.tests.DependencyConstants;
import com.microsoft.recognizers.text.tests.NotSupportedException;
import com.microsoft.recognizers.text.tests.TestCase;

import java.util.Arrays;
import java.util.Collection;
import java.util.List;

import org.junit.AssumptionViolatedException;
import org.junit.runners.Parameterized;

public class BooleanModelTest extends AbstractTest {

    private static final String recognizerType = "Choice";

    @Parameterized.Parameters(name = "{0}")
    public static Collection<TestCase> testCases() {
        return AbstractTest.enumerateTestCases(recognizerType, "Model");
    }

    public BooleanModelTest(TestCase currentCase) {
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
            case "BooleanModel":
                return Arrays.asList(ResolutionKey.Value, ResolutionKey.Score);
            default:
                return Arrays.asList(ResolutionKey.Value, ResolutionKey.Score);
        }
    }

    @Override
    protected List<ModelResult> recognize(TestCase currentCase) {
        try {

            String culture = getCultureCode(currentCase.language);
            switch (currentCase.modelName) {

                case "BooleanModel": {
                    return ChoiceRecognizer.recognizeBoolean(currentCase.input, culture, ChoiceOptions.None, false);
                }

                default: {
                    throw new NotSupportedException("Model Type/Name not supported: " + currentCase.modelName + " in " + culture);
                }
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