// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.tests.sequence;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.IModel;
import com.microsoft.recognizers.text.ModelFactory;
import com.microsoft.recognizers.text.Recognizer;
import com.microsoft.recognizers.text.sequence.SequenceOptions;
import com.microsoft.recognizers.text.sequence.SequenceRecognizer;
import org.javatuples.Triplet;
import org.junit.Assert;
import org.junit.Before;
import org.junit.Test;

import java.lang.reflect.Field;
import java.lang.reflect.Type;
import java.util.Collections;
import java.util.Map;

public class SequenceRecognizerCacheTest {

    @Before
    public void initialization() {
        SequenceRecognizer recognizer = new SequenceRecognizer();
        getInternalModelCache(recognizer).clear();
    }

    @Test
    public void withLazyInitializationCacheShouldBeEmpty() {
        SequenceRecognizer recognizer = new SequenceRecognizer(SequenceOptions.None, true);
        Map<Triplet<String, Type, String>, IModel> internalCache = getInternalModelCache(recognizer);
        Assert.assertEquals(0, internalCache.size());
    }

    @Test
    public void withoutLazyInitializationCacheShouldBeFull() {
        SequenceRecognizer recognizer = new SequenceRecognizer(SequenceOptions.None, false);
        Map<Triplet<String, Type, String>, IModel> internalCache = getInternalModelCache(recognizer);
        Assert.assertNotEquals(0, internalCache.size());
    }

    @Test
    public void withoutLazyInitializationAndCultureCacheForSpecificCultureShouldBeSet() {
        SequenceRecognizer recognizer = new SequenceRecognizer(Culture.English, SequenceOptions.None, false);
        Map<Triplet<String, Type, String>, IModel> internalCache = getInternalModelCache(recognizer);

        Assert.assertTrue(internalCache.entrySet().stream().allMatch(kv -> kv.getKey().getValue0() == Culture.English));
    }

    private static Map<Triplet<String, Type, String>, IModel> getInternalModelCache(SequenceRecognizer recognizer) {
        try {
            Field field = Recognizer.class.getDeclaredField("factory");
            field.setAccessible(true);
            ModelFactory<SequenceOptions> factory = (ModelFactory<SequenceOptions>) field.get(recognizer);
            Field cacheField = factory.getClass().getDeclaredField("cache");
            cacheField.setAccessible(true);
            Map<Triplet<String, Type, String>, IModel> cache = (Map<Triplet<String, Type, String>, IModel>) cacheField
                    .get(null);

            return cache;
        } catch (Exception ex) {
            ex.printStackTrace();
            return Collections.emptyMap();
        }

    }
}
