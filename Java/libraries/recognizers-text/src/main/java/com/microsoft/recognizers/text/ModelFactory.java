// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text;

import com.microsoft.recognizers.text.utilities.StringUtility;

import java.lang.reflect.Type;
import java.util.HashMap;
import java.util.concurrent.ConcurrentHashMap;
import java.util.function.Function;
import org.javatuples.Pair;
import org.javatuples.Triplet;

public class ModelFactory<TModelOptions> extends HashMap<Pair<String, Type>, Function<TModelOptions, IModel>> {

    // cacheKey: (string culture, Type modelType, string modelOptions)
    private static ConcurrentHashMap<Triplet<String, Type, String>, IModel> cache = new ConcurrentHashMap<Triplet<String, Type, String>, IModel>();

    private static final String fallbackCulture = Culture.English;

    public <T extends IModel> T getModel(Class<T> modelType, String culture, boolean fallbackToDefaultCulture, TModelOptions options) throws IllegalArgumentException {
        IModel model = this.getModel(modelType, culture, options);
        if (model != null) {
            return (T)model;
        }

        if (fallbackToDefaultCulture) {
            model = this.getModel(modelType, fallbackCulture, options);
            if (model != null) {
                return (T)model;
            }
        }

        throw new IllegalArgumentException(
                String.format("Could not find Model with the specified configuration: %s, %s", culture, modelType.getTypeName()));
    }

    private IModel getModel(Type modelType, String culture, TModelOptions options) {
        if (StringUtility.isNullOrEmpty(culture)) {
            return null;
        }

        // Look in cache
        Triplet<String, Type, String> cacheKey = new Triplet<>(culture.toLowerCase(), modelType, options.toString());
        if (cache.containsKey(cacheKey)) {
            return cache.get(cacheKey);
        }

        // Use Factory to create instance
        Pair<String, Type> key = generateKey(culture, modelType);
        if (this.containsKey(key)) {
            Function<TModelOptions, IModel> factoryMethod = this.get(key);
            IModel model = factoryMethod.apply(options);

            // Store in cache
            cache.put(cacheKey, model);
            return model;
        }

        return null;
    }

    public void initializeModels(String targetCulture, TModelOptions options) {
        this.keySet().stream()
                .filter(key -> StringUtility.isNullOrEmpty(targetCulture) || key.getValue0().equalsIgnoreCase(targetCulture))
                .forEach(key -> this.initializeModel(key.getValue1(), key.getValue0(), options));
    }

    private void initializeModel(Type modelType, String culture, TModelOptions options) {
        this.getModel(modelType, culture, options);
    }

    @Override
    public Function<TModelOptions, IModel> put(Pair<String, Type> config, Function<TModelOptions, IModel> modelCreator) {
        return super.put(
                generateKey(config.getValue0(), config.getValue1()),
                modelCreator);
    }

    private static Pair<String, Type> generateKey(String culture, Type modelType) {
        return new Pair<>(culture.toLowerCase(), modelType);
    }
}
