// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.tests.sequence;

import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Map.Entry;
import java.util.stream.Collectors;
import java.util.stream.IntStream;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.IModel;
import com.microsoft.recognizers.text.ModelResult;
import com.microsoft.recognizers.text.ResolutionKey;
import com.microsoft.recognizers.text.sequence.SequenceOptions;
import com.microsoft.recognizers.text.sequence.SequenceRecognizer;
import com.microsoft.recognizers.text.sequence.english.extractors.EnglishPhoneNumberExtractorConfiguration;
import com.microsoft.recognizers.text.sequence.english.parsers.PhoneNumberParser;
import com.microsoft.recognizers.text.sequence.extractors.BasePhoneNumberExtractor;
import com.microsoft.recognizers.text.sequence.models.PhoneNumberModel;

import org.javatuples.Pair;
import org.junit.Assert;
import org.junit.Test;

public class SequenceRecognizerInitializationTest {
    private final String testInput = "1 (877) 609-2233";

    private final String englishCulture = Culture.English;
    private final String spanishCulture = Culture.Spanish;
    private final String invalidCulture = "vo-id";

    private final IModel controlModel;

    public SequenceRecognizerInitializationTest() {
        SequenceOptions config = SequenceOptions.None;

        controlModel = new PhoneNumberModel(new PhoneNumberParser(),
                new BasePhoneNumberExtractor(new EnglishPhoneNumberExtractorConfiguration(config)));
    }

    @Test
    public void withoutCultureUseTargetCulture() {
        SequenceRecognizer recognizer = new SequenceRecognizer(englishCulture);
        IModel testedModel = recognizer.getPhoneNumberModel();

        TestSequence(testedModel, controlModel, testInput);
    }

    @Test
    public void withOtherCultureNotUseTargetCulture() {
        SequenceRecognizer recognizer = new SequenceRecognizer(spanishCulture);
        IModel testedModel = recognizer.getPhoneNumberModel(englishCulture, false);

        TestSequence(testedModel, controlModel, testInput);
    }

    @Test
    public void withinvalidCultureUseTargetCulture() {
        SequenceRecognizer recognizer = new SequenceRecognizer(englishCulture);
        IModel testedModel = recognizer.getPhoneNumberModel(invalidCulture, true);

        TestSequence(testedModel, controlModel, testInput);
    }

    @Test
    public void withinvalidCultureAlwaysUseEnglish() {
        SequenceRecognizer recognizer = new SequenceRecognizer();
        IModel testedModel = recognizer.getPhoneNumberModel(invalidCulture, true);

        TestSequence(testedModel, controlModel, testInput);
    }

    @Test
    public void withoutTargetCultureAndWithoutCultureFallbackToEnglishCulture() {
        SequenceRecognizer recognizer = new SequenceRecognizer();
        IModel testedModel = recognizer.getPhoneNumberModel();

        TestSequence(testedModel, controlModel, testInput);
    }

    @Test
    public void withInvalidCultureAndWithoutFallbackThrowError() {
        SequenceRecognizer recognizer = new SequenceRecognizer();
        try {
            recognizer.getPhoneNumberModel(invalidCulture, false);
            Assert.fail("should have thrown IllegalArgumentException");
        } catch (IllegalArgumentException ex) {
        }
    }

    private void TestSequence(IModel testedModel, IModel controlModel, String source) {
        List<ModelResult> expectedResults = controlModel.parse(source);
        List<ModelResult> actualResults = testedModel.parse(source);

        Assert.assertEquals(source, expectedResults.size(), actualResults.size());
        Assert.assertTrue(source, expectedResults.size() > 0);

        IntStream.range(0, expectedResults.size())
                .mapToObj(i -> Pair.with(expectedResults.get(i), actualResults.get(i))).forEach(t -> {
                    ModelResult expected = t.getValue0();
                    ModelResult actual = t.getValue1();

                    Assert.assertEquals("typeName", expected.typeName, actual.typeName);
                    Assert.assertEquals("text", expected.text, actual.text);
                    Assert.assertEquals("start", expected.start, actual.start);
                    Assert.assertEquals("end", expected.end, actual.end);

                    Assert.assertEquals("resolution.value", expected.resolution.get(ResolutionKey.Value),
                            actual.resolution.get(ResolutionKey.Value));
                });
    }
}
