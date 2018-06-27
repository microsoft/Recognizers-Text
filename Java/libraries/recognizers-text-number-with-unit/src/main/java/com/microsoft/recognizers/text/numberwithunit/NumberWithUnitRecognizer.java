package com.microsoft.recognizers.text.numberwithunit;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.*;
import com.microsoft.recognizers.text.numberwithunit.extractors.BaseMergedUnitExtractor;
import com.microsoft.recognizers.text.numberwithunit.extractors.NumberWithUnitExtractor;
import com.microsoft.recognizers.text.numberwithunit.models.AgeModel;
import com.microsoft.recognizers.text.numberwithunit.models.CurrencyModel;
import com.microsoft.recognizers.text.numberwithunit.models.DimensionModel;
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

    public NumberWithUnitRecognizer(String culture, NumberWithUnitOptions options, boolean lazyInitialization) {
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

    public DimensionModel getDimensionModel() {
        return getDimensionModel(null, true);
    }

    public DimensionModel getDimensionModel(String culture, boolean fallbackToDefaultCulture) {
        return getModel(DimensionModel.class, culture, fallbackToDefaultCulture);
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

    public static List<ModelResult> recognizeDimension(String query, String culture) {
        return recognizeByModel(recognizer -> recognizer.getDimensionModel(culture, true), query, NumberWithUnitOptions.None);
    }

    public static List<ModelResult> recognizeDimension(String query, String culture, NumberWithUnitOptions options) {
        return recognizeByModel(recognizer -> recognizer.getDimensionModel(culture, true), query, options);
    }

    public static List<ModelResult> recognizeDimension(String query, String culture, NumberWithUnitOptions options, boolean fallbackToDefaultCulture) {
        return recognizeByModel(recognizer -> recognizer.getDimensionModel(culture, fallbackToDefaultCulture), query, options);
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
                new CurrencyModel(ImmutableMap.of(
                        new BaseMergedUnitExtractor(new com.microsoft.recognizers.text.numberwithunit.english.extractors.CurrencyExtractorConfiguration()),
                        new BaseMergedUnitParser(new com.microsoft.recognizers.text.numberwithunit.english.parsers.CurrencyParserConfiguration()))));
        registerModel(TemperatureModel.class, Culture.English, (options) ->
                new TemperatureModel(ImmutableMap.of(
                        new NumberWithUnitExtractor(new com.microsoft.recognizers.text.numberwithunit.english.extractors.TemperatureExtractorConfiguration()),
                        new NumberWithUnitParser(new com.microsoft.recognizers.text.numberwithunit.english.parsers.TemperatureParserConfiguration()))));
        registerModel(DimensionModel.class, Culture.English, (options) ->
                new DimensionModel(ImmutableMap.of(
                        new NumberWithUnitExtractor(new com.microsoft.recognizers.text.numberwithunit.english.extractors.DimensionExtractorConfiguration()),
                        new NumberWithUnitParser(new com.microsoft.recognizers.text.numberwithunit.english.parsers.DimensionParserConfiguration()))));
        registerModel(AgeModel.class, Culture.English, (options) ->
                new AgeModel(ImmutableMap.of(
                        new NumberWithUnitExtractor(new com.microsoft.recognizers.text.numberwithunit.english.extractors.AgeExtractorConfiguration()),
                        new NumberWithUnitParser(new com.microsoft.recognizers.text.numberwithunit.english.parsers.AgeParserConfiguration()))));
        //endregion

        //region Spanish
        registerModel(CurrencyModel.class, Culture.Spanish, (options) ->
                new CurrencyModel(ImmutableMap.of(
                    new NumberWithUnitExtractor(new com.microsoft.recognizers.text.numberwithunit.spanish.extractors.CurrencyExtractorConfiguration()),
                    new NumberWithUnitParser(new com.microsoft.recognizers.text.numberwithunit.spanish.parsers.CurrencyParserConfiguration()))));
        registerModel(TemperatureModel.class, Culture.Spanish, (options) ->
                new TemperatureModel(ImmutableMap.of(
                    new NumberWithUnitExtractor(new com.microsoft.recognizers.text.numberwithunit.spanish.extractors.TemperatureExtractorConfiguration()),
                    new NumberWithUnitParser(new com.microsoft.recognizers.text.numberwithunit.spanish.parsers.TemperatureParserConfiguration()))));
        registerModel(DimensionModel.class, Culture.Spanish, (options) ->
                new DimensionModel(ImmutableMap.of(
                        new NumberWithUnitExtractor(new com.microsoft.recognizers.text.numberwithunit.spanish.extractors.DimensionExtractorConfiguration()),
                        new NumberWithUnitParser(new com.microsoft.recognizers.text.numberwithunit.spanish.parsers.DimensionParserConfiguration()))));
        registerModel(AgeModel.class, Culture.Spanish, (options) ->
                new AgeModel(ImmutableMap.of(
                    new NumberWithUnitExtractor(new com.microsoft.recognizers.text.numberwithunit.spanish.extractors.AgeExtractorConfiguration()),
                        new NumberWithUnitParser(new com.microsoft.recognizers.text.numberwithunit.spanish.parsers.AgeParserConfiguration()))));
        //endregion

        //region Portuguese
        registerModel(CurrencyModel.class, Culture.Portuguese, (options) ->
                new CurrencyModel(ImmutableMap.of(
                        new NumberWithUnitExtractor(new com.microsoft.recognizers.text.numberwithunit.portuguese.extractors.CurrencyExtractorConfiguration()),
                        new NumberWithUnitParser(new com.microsoft.recognizers.text.numberwithunit.portuguese.parsers.CurrencyParserConfiguration()))));
        registerModel(TemperatureModel.class, Culture.Portuguese, (options) ->
                new TemperatureModel(ImmutableMap.of(
                        new NumberWithUnitExtractor(new com.microsoft.recognizers.text.numberwithunit.portuguese.extractors.TemperatureExtractorConfiguration()),
                        new NumberWithUnitParser(new com.microsoft.recognizers.text.numberwithunit.portuguese.parsers.TemperatureParserConfiguration()))));
        registerModel(DimensionModel.class, Culture.Portuguese, (options) ->
                new DimensionModel(ImmutableMap.of(
                        new NumberWithUnitExtractor(new com.microsoft.recognizers.text.numberwithunit.portuguese.extractors.DimensionExtractorConfiguration()),
                        new NumberWithUnitParser(new com.microsoft.recognizers.text.numberwithunit.portuguese.parsers.DimensionParserConfiguration()))));
        registerModel(AgeModel.class, Culture.Portuguese, (options) ->
                new AgeModel(ImmutableMap.of(
                        new NumberWithUnitExtractor(new com.microsoft.recognizers.text.numberwithunit.portuguese.extractors.AgeExtractorConfiguration()),
                        new NumberWithUnitParser(new com.microsoft.recognizers.text.numberwithunit.portuguese.parsers.AgeParserConfiguration()))));
        //endregion

        //region French
        registerModel(CurrencyModel.class, Culture.French, (options) ->
                new CurrencyModel(ImmutableMap.of(
                        new NumberWithUnitExtractor(new com.microsoft.recognizers.text.numberwithunit.french.extractors.CurrencyExtractorConfiguration()),
                        new NumberWithUnitParser(new com.microsoft.recognizers.text.numberwithunit.french.parsers.CurrencyParserConfiguration()))));
        registerModel(TemperatureModel.class, Culture.French, (options) ->
                new TemperatureModel(ImmutableMap.of(
                        new NumberWithUnitExtractor(new com.microsoft.recognizers.text.numberwithunit.french.extractors.TemperatureExtractorConfiguration()),
                        new NumberWithUnitParser(new com.microsoft.recognizers.text.numberwithunit.french.parsers.TemperatureParserConfiguration()))));
        registerModel(DimensionModel.class, Culture.French, (options) ->
                new DimensionModel(ImmutableMap.of(
                        new NumberWithUnitExtractor(new com.microsoft.recognizers.text.numberwithunit.french.extractors.DimensionExtractorConfiguration()),
                        new NumberWithUnitParser(new com.microsoft.recognizers.text.numberwithunit.french.parsers.DimensionParserConfiguration()))));
        registerModel(AgeModel.class, Culture.French, (options) ->
                new AgeModel(ImmutableMap.of(
                        new NumberWithUnitExtractor(new com.microsoft.recognizers.text.numberwithunit.french.extractors.AgeExtractorConfiguration()),
                        new NumberWithUnitParser(new com.microsoft.recognizers.text.numberwithunit.french.parsers.AgeParserConfiguration()))));
        //endregion

        //region German
        registerModel(CurrencyModel.class, Culture.German, (options) ->
                new CurrencyModel(ImmutableMap.of(
                        new NumberWithUnitExtractor(new com.microsoft.recognizers.text.numberwithunit.german.extractors.CurrencyExtractorConfiguration()),
                        new NumberWithUnitParser(new com.microsoft.recognizers.text.numberwithunit.german.parsers.CurrencyParserConfiguration()))));
        registerModel(TemperatureModel.class, Culture.German, (options) ->
                new TemperatureModel(ImmutableMap.of(
                        new NumberWithUnitExtractor(new com.microsoft.recognizers.text.numberwithunit.german.extractors.TemperatureExtractorConfiguration()),
                        new NumberWithUnitParser(new com.microsoft.recognizers.text.numberwithunit.german.parsers.TemperatureParserConfiguration()))));
        registerModel(DimensionModel.class, Culture.German, (options) ->
                new DimensionModel(ImmutableMap.of(
                        new NumberWithUnitExtractor(new com.microsoft.recognizers.text.numberwithunit.german.extractors.DimensionExtractorConfiguration()),
                        new NumberWithUnitParser(new com.microsoft.recognizers.text.numberwithunit.german.parsers.DimensionParserConfiguration()))));
        registerModel(AgeModel.class, Culture.German, (options) ->
                new AgeModel(ImmutableMap.of(
                        new NumberWithUnitExtractor(new com.microsoft.recognizers.text.numberwithunit.german.extractors.AgeExtractorConfiguration()),
                        new NumberWithUnitParser(new com.microsoft.recognizers.text.numberwithunit.german.parsers.AgeParserConfiguration()))));
        //endregion
    }
}
