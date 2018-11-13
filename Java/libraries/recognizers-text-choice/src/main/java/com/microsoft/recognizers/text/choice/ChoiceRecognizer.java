package com.microsoft.recognizers.text.choice;

import com.microsoft.recognizers.text.IModel;
import com.microsoft.recognizers.text.ModelResult;
import com.microsoft.recognizers.text.Recognizer;

import java.util.List;

public class ChoiceRecognizer extends Recognizer<ChoiceOptions> {

    public ChoiceRecognizer(String targetCulture, ChoiceOptions options,
            boolean lazyInitialization) {
        super(targetCulture, options, lazyInitialization);
    }

    public ChoiceRecognizer(String targetCulture, int options, boolean lazyInitialization) {
        this(targetCulture, ChoiceOptions.values()[options], lazyInitialization);
    }

    public ChoiceRecognizer(int options, boolean lazyInitialization) {
        this(null, ChoiceOptions.values()[options], lazyInitialization);
    }

    public ChoiceRecognizer(ChoiceOptions options, boolean lazyInitialization) {
        this(null, options, lazyInitialization);
    }

    public ChoiceRecognizer(boolean lazyInitialization) {
        this(null, ChoiceOptions.None, lazyInitialization);
    }

    public ChoiceRecognizer(int options) {
        this(null, ChoiceOptions.values()[options], true);
    }

    public ChoiceRecognizer(ChoiceOptions options) {
        this(null, options, true);
    }

    public ChoiceRecognizer() {
        this(null, ChoiceOptions.None, true);
    }

    public IModel getBooleanModel(String culture, boolean fallbackToDefaultCulture) {
        throw new UnsupportedOperationException();
        // return GetModel<BooleanModel>(culture, fallbackToDefaultCulture);
    }

    public static List<ModelResult> recognizeBoolean(String query, String culture, ChoiceOptions options, boolean fallbackToDefaultCulture) {
        throw new UnsupportedOperationException();
    }

    public static List<ModelResult> recognizeBoolean(String query, String culture, ChoiceOptions options) {
        return recognizeBoolean(query, culture, options, true);
    }

    public static List<ModelResult> recognizeBoolean(String query, String culture) {
        return recognizeBoolean(query, culture, ChoiceOptions.None);
    }

    @Override
    protected void initializeConfiguration() {
        throw new UnsupportedOperationException();
    }
}
