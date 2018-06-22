package com.microsoft.recognizers.text.tests.number;

import com.microsoft.recognizers.text.*;
import com.microsoft.recognizers.text.number.NumberMode;
import com.microsoft.recognizers.text.number.NumberOptions;
import com.microsoft.recognizers.text.number.NumberRecognizer;
import com.microsoft.recognizers.text.number.english.extractors.NumberExtractor;
import com.microsoft.recognizers.text.number.english.parsers.EnglishNumberParserConfiguration;
import com.microsoft.recognizers.text.number.models.NumberModel;
import com.microsoft.recognizers.text.number.parsers.AgnosticNumberParserFactory;
import com.microsoft.recognizers.text.number.parsers.AgnosticNumberParserType;
import org.javatuples.Pair;
import org.junit.Assert;
import org.junit.Test;

import java.util.List;
import java.util.stream.IntStream;

public class NumberRecognizerInitializationTest {

    private static final String TestInput = "one";

    private static final String EnglishCulture = Culture.English;
    private static final String SpanishCulture = Culture.Spanish;
    private static final String InvalidCulture = "vo-id";

    private final IModel controlModel;

    public NumberRecognizerInitializationTest() {
        controlModel = new NumberModel(
                AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Number, new EnglishNumberParserConfiguration()),
                NumberExtractor.getInstance(NumberMode.PureNumber, NumberOptions.None));
    }

    @Test
    public void WithoutCulture_UseTargetCulture() {
        NumberRecognizer recognizer = new NumberRecognizer(EnglishCulture);
        IModel testedModel = recognizer.getNumberModel();

        TestNumber(testedModel, controlModel, TestInput);
    }

    @Test
    public void WithOtherCulture_NotUseTargetCulture() {
        NumberRecognizer recognizer = new NumberRecognizer(SpanishCulture);
        IModel testedModel = recognizer.getNumberModel(EnglishCulture, false);

        TestNumber(testedModel, controlModel, TestInput);
    }

    @Test
    public void WithInvalidCulture_UseTargetCulture() {
        NumberRecognizer recognizer = new NumberRecognizer(EnglishCulture);
        IModel testedModel = recognizer.getNumberModel(InvalidCulture, true);

        TestNumber(testedModel, controlModel, TestInput);
    }

    @Test
    public void WithInvalidCulture_AlwaysUseEnglish() {
        NumberRecognizer recognizer = new NumberRecognizer();
        IModel testedModel = recognizer.getNumberModel(InvalidCulture, true);

        TestNumber(testedModel, controlModel, TestInput);
    }

    @Test
    public void WithoutTargetCultureAndWithoutCulture_FallbackToEnglishCulture() {
        NumberRecognizer recognizer = new NumberRecognizer();
        IModel testedModel = recognizer.getNumberModel();

        TestNumber(testedModel, controlModel, TestInput);
    }

    @Test
    public void WithInvalidCultureAndWithoutFallback_ThrowError() {
        NumberRecognizer recognizer = new NumberRecognizer();
        try {
            recognizer.getNumberModel(InvalidCulture, false);
            Assert.fail("should have thrown IllegalArgumentException");
        } catch (IllegalArgumentException ex) {
        }
    }

    private void TestNumber(IModel testedModel, IModel controlModel, String source) {
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

                    Assert.assertEquals("resolution.value", expected.resolution.get(ResolutionKey.Value), actual.resolution.get(ResolutionKey.Value));
                });
    }
}
