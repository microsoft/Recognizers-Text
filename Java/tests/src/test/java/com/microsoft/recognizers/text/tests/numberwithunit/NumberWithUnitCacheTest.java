package com.microsoft.recognizers.text.tests.numberwithunit;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.IModel;
import com.microsoft.recognizers.text.ModelFactory;
import com.microsoft.recognizers.text.Recognizer;
import com.microsoft.recognizers.text.numberwithunit.NumberWithUnitOptions;
import com.microsoft.recognizers.text.numberwithunit.NumberWithUnitRecognizer;
import org.javatuples.Triplet;
import org.junit.Assert;
import org.junit.Before;
import org.junit.Test;

import java.lang.reflect.Field;
import java.lang.reflect.Type;
import java.util.Collections;
import java.util.Map;

public class NumberWithUnitCacheTest {

    @Before
    public void initialization() {
        NumberWithUnitRecognizer recognizer = new NumberWithUnitRecognizer();
        getInternalModelCache(recognizer).clear();
    }

    @Test
    public void withLazyInitializationCacheShouldBeEmpty() {
        NumberWithUnitRecognizer recognizer = new NumberWithUnitRecognizer(NumberWithUnitOptions.None, true);
        Map<Triplet<String, Type, String>, IModel> internalCache = getInternalModelCache(recognizer);
        Assert.assertEquals(0, internalCache.size());
    }

    @Test
    public void withoutLazyInitializationCacheShouldBeFull() {
        NumberWithUnitRecognizer recognizer = new NumberWithUnitRecognizer(NumberWithUnitOptions.None, false);
        Map<Triplet<String, Type, String>, IModel> internalCache = getInternalModelCache(recognizer);
        Assert.assertNotEquals(0, internalCache.size());
    }

    @Test
    public void withoutLazyInitializationAndCultureCacheForSpecificCultureShouldBeSet() {
        NumberWithUnitRecognizer recognizer = new NumberWithUnitRecognizer(Culture.English, NumberWithUnitOptions.None, false);
        Map<Triplet<String, Type, String>, IModel> internalCache = getInternalModelCache(recognizer);

        Assert.assertTrue(internalCache.entrySet().stream().allMatch(kv -> kv.getKey().getValue0() == Culture.English));
    }

    private static Map<Triplet<String, Type, String>, IModel> getInternalModelCache(NumberWithUnitRecognizer recognizer) {
        try {
            Field field = Recognizer.class.getDeclaredField("factory");
            field.setAccessible(true);
            ModelFactory<NumberWithUnitOptions> factory = (ModelFactory<NumberWithUnitOptions>) field.get(recognizer);
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
