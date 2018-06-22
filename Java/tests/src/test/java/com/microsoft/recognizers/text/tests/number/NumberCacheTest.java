package com.microsoft.recognizers.text.tests.number;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.IModel;
import com.microsoft.recognizers.text.ModelFactory;
import com.microsoft.recognizers.text.Recognizer;
import com.microsoft.recognizers.text.number.NumberOptions;
import com.microsoft.recognizers.text.number.NumberRecognizer;
import org.javatuples.Triplet;
import org.junit.Assert;
import org.junit.Before;
import org.junit.Test;

import java.lang.reflect.Field;
import java.lang.reflect.Type;
import java.util.Collections;
import java.util.Map;

public class NumberCacheTest {

    @Before
    public void initialization() {
        NumberRecognizer recognizer = new NumberRecognizer();
        getInternalModelCache(recognizer).clear();
    }

    @Test
    public void withLazyInitializationCacheShouldBeEmpty() {
        NumberRecognizer recognizer = new NumberRecognizer(NumberOptions.None, true);
        Map<Triplet<String, Type, String>, IModel> internalCache = getInternalModelCache(recognizer);
        Assert.assertEquals(0, internalCache.size());
    }

    @Test
    public void withoutLazyInitializationCacheShouldBeFull() {
        NumberRecognizer recognizer = new NumberRecognizer(NumberOptions.None, false);
        Map<Triplet<String, Type, String>, IModel> internalCache = getInternalModelCache(recognizer);
        Assert.assertNotEquals(0, internalCache.size());
    }

    @Test
    public void withoutLazyInitializationAndCultureCacheForSpecificCultureShouldBeSet() {
        NumberRecognizer recognizer = new NumberRecognizer(Culture.English, NumberOptions.None, false);
        Map<Triplet<String, Type, String>, IModel> internalCache = getInternalModelCache(recognizer);

        Assert.assertTrue(internalCache.entrySet().stream().allMatch(kv -> kv.getKey().getValue0() == Culture.English));
    }

    private static Map<Triplet<String, Type, String>, IModel> getInternalModelCache(NumberRecognizer recognizer) {
        try {
            Field field = Recognizer.class.getDeclaredField("factory");
            field.setAccessible(true);
            ModelFactory<NumberOptions> factory = (ModelFactory<NumberOptions>)field.get(recognizer);
            Field cacheField = factory.getClass().getDeclaredField("cache");
            cacheField.setAccessible(true);
            Map<Triplet<String, Type, String>, IModel> cache = (Map<Triplet<String, Type, String>, IModel>) cacheField.get(null);

            return cache;
        } catch (Exception ex) {
            ex.printStackTrace();
            return Collections.emptyMap();
        }

    }
}
