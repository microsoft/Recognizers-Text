package com.microsoft.recognizers.text.number;

import com.microsoft.recognizers.text.BaseRecognizer;

public class NumberRecognizer extends BaseRecognizer<NumberOptions> {

    public NumberRecognizer() {
        this(null, NumberOptions.None, true);
    }

    public NumberRecognizer(String culture) {
        this(culture, NumberOptions.None, false);
    }

    public NumberRecognizer(NumberOptions numberOptions, boolean lazyInitialization) {
        this(null, numberOptions, lazyInitialization);
    }

    protected NumberRecognizer(String culture, NumberOptions numberOptions, boolean lazyInitialization) {
        super(culture, numberOptions, lazyInitialization);
    }

    /*
    public NumberModel GetNumberModel(string culture = null, bool fallbackToDefaultCulture = true)
    {
        return GetModel<NumberModel>(culture, fallbackToDefaultCulture);
    }

    public OrdinalModel GetOrdinalModel(string culture = null, bool fallbackToDefaultCulture = true)
    {
        return GetModel<OrdinalModel>(culture, fallbackToDefaultCulture);
    }

    public PercentModel GetPercentageModel(string culture = null, bool fallbackToDefaultCulture = true)
    {
        return GetModel<PercentModel>(culture, fallbackToDefaultCulture);
    }

    public NumberRangeModel GetNumberRangeModel(string culture = null, bool fallbackToDefaultCulture = true)
    {
        return GetModel<NumberRangeModel>(culture, fallbackToDefaultCulture);
    }
    */

    /*
    public static List<ModelResult> RecognizeNumber(string query, string culture, NumberOptions options = NumberOptions.None, bool fallbackToDefaultCulture = true)
    {
        return RecognizeByModel(recognizer => recognizer.GetNumberModel(culture, fallbackToDefaultCulture), query, options);
    }

    public static List<ModelResult> RecognizeOrdinal(string query, string culture, NumberOptions options = NumberOptions.None, bool fallbackToDefaultCulture = true)
    {
        return RecognizeByModel(recognizer => recognizer.GetOrdinalModel(culture, fallbackToDefaultCulture), query, options);
    }

    public static List<ModelResult> RecognizePercentage(string query, string culture, NumberOptions options = NumberOptions.None, bool fallbackToDefaultCulture = true)
    {
        return RecognizeByModel(recognizer => recognizer.GetPercentageModel(culture, fallbackToDefaultCulture), query, options);
    }

    public static List<ModelResult> RecognizeNumberRange(string query, string culture, NumberOptions options = NumberOptions.None, bool fallbackToDefaultCulture = true)
    {
        return RecognizeByModel(recognizer => recognizer.GetNumberRangeModel(culture, fallbackToDefaultCulture), query, options);
    }

    private static List<ModelResult> RecognizeByModel(Func<NumberRecognizer, IModel> getModelFunc, string query, NumberOptions options)
    {
        var recognizer = new NumberRecognizer(options);
        var model = getModelFunc(recognizer);
        return model.Parse(query);
    }
    */

    @Override
    protected void initializeConfiguration() {
        //region English
        // TODO
        //endregion
    }
}
