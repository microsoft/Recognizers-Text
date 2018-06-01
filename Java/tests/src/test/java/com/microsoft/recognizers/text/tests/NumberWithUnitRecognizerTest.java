package com.microsoft.recognizers.text.tests;

import org.junit.runners.Parameterized;

import java.util.Collection;

public class NumberWithUnitRecognizerTest extends RecognizerTest {

    public NumberWithUnitRecognizerTest(TestCase currentCase) {
        super(currentCase);
    }

    private static final String recognizerType = "NumberWithUnit";

    @Parameterized.Parameters(name = "{0}")
    public static Collection<TestCase> testCases() {
        return RecognizerTest.testCases(recognizerType);
    }
}
