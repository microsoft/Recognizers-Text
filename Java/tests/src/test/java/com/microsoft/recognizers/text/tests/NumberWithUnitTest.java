package com.microsoft.recognizers.text.tests;

import org.junit.runners.Parameterized;

import java.util.Collection;

public class NumberWithUnitTest extends AbstractTest {

    public NumberWithUnitTest(TestCase currentCase) {
        super(currentCase);
    }

    private static final String recognizerType = "NumberWithUnit";

    @Parameterized.Parameters(name = "{0}")
    public static Collection<TestCase> testCases() {
        return AbstractTest.testCases(recognizerType);
    }
}
