// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text;

import java.util.function.Function;
import org.javatuples.Pair;

public abstract class Recognizer<TRecognizerOptions extends Enum<TRecognizerOptions>> {

    public final String targetCulture;
    public final TRecognizerOptions options;

    private final ModelFactory<TRecognizerOptions> factory;

    protected Recognizer(String targetCulture, TRecognizerOptions options, boolean lazyInitialization) {
        this.targetCulture = targetCulture;
        this.options = options;

        this.factory = new ModelFactory<>();
        this.initializeConfiguration();

        if (!lazyInitialization) {
            this.initializeModels(targetCulture, options);
        }
    }

    public <T extends IModel> T getModel(Class<T> modelType, String culture, boolean fallbackToDefaultCulture) {
        return this.factory.getModel(
                modelType,
                culture != null ? culture : targetCulture,
                fallbackToDefaultCulture,
                options);
    }

    public <T extends IModel> void registerModel(Class<T> modelType, String culture, Function<TRecognizerOptions, IModel> modelCreator) {
        this.factory.put(new Pair<>(culture, modelType), modelCreator);
    }

    private void initializeModels(String targetCulture, TRecognizerOptions options) {
        this.factory.initializeModels(targetCulture, options);
    }

    protected abstract void initializeConfiguration();
}
