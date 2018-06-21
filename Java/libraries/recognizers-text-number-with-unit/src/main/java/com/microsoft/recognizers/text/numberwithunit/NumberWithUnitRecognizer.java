package com.microsoft.recognizers.text.numberwithunit;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.*;
import com.microsoft.recognizers.text.numberwithunit.extractors.BaseMergedUnitExtractor;
import com.microsoft.recognizers.text.numberwithunit.extractors.NumberWithUnitExtractor;
import com.microsoft.recognizers.text.numberwithunit.models.AgeModel;
import com.microsoft.recognizers.text.numberwithunit.models.CurrencyModel;
import com.microsoft.recognizers.text.numberwithunit.models.TemperatureModel;
import com.microsoft.recognizers.text.numberwithunit.parsers.BaseMergedUnitParser;
import com.microsoft.recognizers.text.numberwithunit.parsers.NumberWithUnitParser;

import java.util.List;
import java.util.function.Function;

public class NumberWithUnitRecognizer extends Recognizer<NumberWithUnitOptions> {

    public NumberWithUnitRecognizer() {
        this(null, NumberWithUnitOptions.None, true);
    }

    public NumberWithUnitRecognizer(String culture) {
        this(culture, NumberWithUnitOptions.None, false);
    }

    public NumberWithUnitRecognizer(NumberWithUnitOptions options) {
        this(null, options, true);
    }

    public NumberWithUnitRecognizer(NumberWithUnitOptions options, boolean lazyInitialization) {
        this(null, options, lazyInitialization);
    }

    protected NumberWithUnitRecognizer(String culture, NumberWithUnitOptions options, boolean lazyInitialization) {
        super(culture, options, lazyInitialization);
    }

    public CurrencyModel getCurrencyModel() {
        return getCurrencyModel(null, true);
    }

    public CurrencyModel getCurrencyModel(String culture, boolean fallbackToDefaultCulture) {
        return getModel(CurrencyModel.class, culture, fallbackToDefaultCulture);
    }

    public TemperatureModel getTemperatureModel() {
        return getTemperatureModel(null, true);
    }

    public TemperatureModel getTemperatureModel(String culture, boolean fallbackToDefaultCulture) {
        return getModel(TemperatureModel.class, culture, fallbackToDefaultCulture);
    }

    public AgeModel getAgeModel() {
        return getAgeModel(null, true);
    }

    public AgeModel getAgeModel(String culture, boolean fallbackToDefaultCulture) {
        return getModel(AgeModel.class, culture, fallbackToDefaultCulture);
    }

    //region Helper methods for less verbosity
    public static List<ModelResult> recognizeCurrency(String query, String culture) {
        return recognizeByModel(recognizer -> recognizer.getCurrencyModel(culture, true), query, NumberWithUnitOptions.None);
    }

    public static List<ModelResult> recognizeCurrency(String query, String culture, NumberWithUnitOptions options) {
        return recognizeByModel(recognizer -> recognizer.getCurrencyModel(culture, true), query, options);
    }

    public static List<ModelResult> recognizeCurrency(String query, String culture, NumberWithUnitOptions options, boolean fallbackToDefaultCulture) {
        return recognizeByModel(recognizer -> recognizer.getCurrencyModel(culture, fallbackToDefaultCulture), query, options);
    }

    public static List<ModelResult> recognizeTemperature(String query, String culture) {
        return recognizeByModel(recognizer -> recognizer.getTemperatureModel(culture, true), query, NumberWithUnitOptions.None);
    }

    public static List<ModelResult> recognizeTemperature(String query, String culture, NumberWithUnitOptions options) {
        return recognizeByModel(recognizer -> recognizer.getTemperatureModel(culture, true), query, options);
    }

    public static List<ModelResult> recognizeTemperature(String query, String culture, NumberWithUnitOptions options, boolean fallbackToDefaultCulture) {
        return recognizeByModel(recognizer -> recognizer.getTemperatureModel(culture, fallbackToDefaultCulture), query, options);
    }

    public static List<ModelResult> recognizeAge(String query, String culture) {
        return recognizeByModel(recognizer -> recognizer.getAgeModel(culture, true), query, NumberWithUnitOptions.None);
    }

    public static List<ModelResult> recognizeAge(String query, String culture, NumberWithUnitOptions options) {
        return recognizeByModel(recognizer -> recognizer.getAgeModel(culture, true), query, options);
    }

    public static List<ModelResult> recognizeAge(String query, String culture, NumberWithUnitOptions options, boolean fallbackToDefaultCulture) {
        return recognizeByModel(recognizer -> recognizer.getAgeModel(culture, fallbackToDefaultCulture), query, options);
    }
    //endregion

    private static List<ModelResult> recognizeByModel(Function<NumberWithUnitRecognizer, IModel> getModelFun, String query, NumberWithUnitOptions options) {
        NumberWithUnitRecognizer recognizer = new NumberWithUnitRecognizer(options);
        IModel model = getModelFun.apply(recognizer);
        return model.parse(query);
    }

    @Override
    protected void initializeConfiguration() {

        //region English
        registerModel(CurrencyModel.class, Culture.English, (options) ->
                new CurrencyModel(
                        ImmutableMap.of(
                                new BaseMergedUnitExtractor(new com.microsoft.recognizers.text.numberwithunit.english.extractos.CurrencyExtractorConfiguration()),
                                new BaseMergedUnitParser(new com.microsoft.recognizers.text.numberwithunit.english.parsers.CurrencyParserConfiguration()))));
        registerModel(TemperatureModel.class, Culture.English, (options) ->
                new TemperatureModel(
                        ImmutableMap.of(
                                new NumberWithUnitExtractor(new com.microsoft.recognizers.text.numberwithunit.english.extractos.TemperatureExtractorConfiguration()),
                                new NumberWithUnitParser(new com.microsoft.recognizers.text.numberwithunit.english.parsers.TemperatureParserConfiguration()))));

//        registerModel(DimensionModel.class, Culture.English, (options) -> new DimensionModel(new Dictionary<IExtractor, IParser>
//        {
//            {
//                new NumberWithUnitExtractor(new English.DimensionExtractorConfiguration()),
//                        new NumberWithUnitParser(new English.DimensionParserConfiguration())
//            }
//        }));
//        registerModel(AgeModel.class, Culture.English, (options) -> new AgeModel(new Dictionary<IExtractor, IParser>
//        {
//            {
//                new NumberWithUnitExtractor(new English.AgeExtractorConfiguration()),
//                        new NumberWithUnitParser(new English.AgeParserConfiguration())
//            }
//        }));
        //endregion
    }
}
