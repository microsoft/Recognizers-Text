package com.microsoft.recognizers.text.tests.numberwithunit;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.*;
import com.microsoft.recognizers.text.numberwithunit.NumberWithUnitRecognizer;
import com.microsoft.recognizers.text.numberwithunit.english.extractors.CurrencyExtractorConfiguration;
import com.microsoft.recognizers.text.numberwithunit.english.parsers.CurrencyParserConfiguration;
import com.microsoft.recognizers.text.numberwithunit.extractors.NumberWithUnitExtractor;
import com.microsoft.recognizers.text.numberwithunit.models.CurrencyModel;
import com.microsoft.recognizers.text.numberwithunit.parsers.NumberWithUnitParser;
import org.javatuples.Pair;
import org.junit.Assert;
import org.junit.Test;

import java.util.List;
import java.util.stream.IntStream;

public class NumberWithUnitRecognizerInitializationTest {

    private final String TestInput = "two dollars";

    private final String EnglishCulture = Culture.English;
    private final String SpanishCulture = Culture.Spanish;
    private final String InvalidCulture = "vo-id";

    private final IModel controlModel;

    public NumberWithUnitRecognizerInitializationTest() {
        controlModel = new CurrencyModel(ImmutableMap.of(
                new NumberWithUnitExtractor(new CurrencyExtractorConfiguration()),
                new NumberWithUnitParser(new CurrencyParserConfiguration())));
    }

    @Test
    public void WithoutCulture_UseTargetCulture() {
        NumberWithUnitRecognizer recognizer = new NumberWithUnitRecognizer(EnglishCulture);
        IModel testedModel = recognizer.getCurrencyModel();

        TestNumberWithUnit(testedModel, controlModel, TestInput);
    }

    @Test
    public void WithOtherCulture_NotUseTargetCulture() {
        NumberWithUnitRecognizer recognizer = new NumberWithUnitRecognizer(SpanishCulture);
        IModel testedModel = recognizer.getCurrencyModel(EnglishCulture, false);

        TestNumberWithUnit(testedModel, controlModel, TestInput);
    }

    @Test
    public void WithInvalidCulture_UseTargetCulture() {
        NumberWithUnitRecognizer recognizer = new NumberWithUnitRecognizer(EnglishCulture);
        IModel testedModel = recognizer.getCurrencyModel(InvalidCulture, true);

        TestNumberWithUnit(testedModel, controlModel, TestInput);
    }

    @Test
    public void WithInvalidCulture_AlwaysUseEnglish() {
        NumberWithUnitRecognizer recognizer = new NumberWithUnitRecognizer();
        IModel testedModel = recognizer.getCurrencyModel(InvalidCulture, true);

        TestNumberWithUnit(testedModel, controlModel, TestInput);
    }

    @Test
    public void WithoutTargetCultureAndWithoutCulture_FallbackToEnglishCulture() {
        NumberWithUnitRecognizer recognizer = new NumberWithUnitRecognizer();
        IModel testedModel = recognizer.getCurrencyModel();

        TestNumberWithUnit(testedModel, controlModel, TestInput);
    }

    @Test
    public void WithInvalidCultureAndWithoutFallback_ThrowError() {
        NumberWithUnitRecognizer recognizer = new NumberWithUnitRecognizer();
        try {
            recognizer.getCurrencyModel(InvalidCulture, false);
            Assert.fail("should have thrown IllegalArgumentException");
        } catch (IllegalArgumentException ex) {
        }
    }

    private void TestNumberWithUnit(IModel testedModel, IModel controlModel, String source) {
        List<ModelResult> expectedResults = controlModel.parse(source);
        List<ModelResult> actualResults = testedModel.parse(source);

        Assert.assertEquals(source, expectedResults.size(), actualResults.size());
        Assert.assertTrue(source, expectedResults.size() > 0);

        IntStream.range(0, expectedResults.size())
                .mapToObj(i -> Pair.with(expectedResults.get(i), actualResults.get(i)))
                .forEach(t -> {
                    ModelResult expected = t.getValue0();
                    ModelResult actual = t.getValue1();

                    Assert.assertEquals("typeName", expected.typeName, actual.typeName);
                    Assert.assertEquals("text", expected.text, actual.text);

                    Assert.assertEquals(ResolutionKey.Value, expected.resolution.get(ResolutionKey.Value), actual.resolution.get(ResolutionKey.Value));
                    Assert.assertEquals(ResolutionKey.Unit, expected.resolution.get(ResolutionKey.Unit), actual.resolution.get(ResolutionKey.Unit));
                });
    }
}
