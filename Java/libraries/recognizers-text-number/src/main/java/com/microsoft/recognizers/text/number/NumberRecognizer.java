// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.number;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.IModel;
import com.microsoft.recognizers.text.ModelResult;
import com.microsoft.recognizers.text.Recognizer;
import com.microsoft.recognizers.text.number.chinese.parsers.ChineseNumberParserConfiguration;
import com.microsoft.recognizers.text.number.chinese.parsers.ChineseNumberRangeParserConfiguration;
import com.microsoft.recognizers.text.number.english.parsers.EnglishNumberParserConfiguration;
import com.microsoft.recognizers.text.number.english.parsers.EnglishNumberRangeParserConfiguration;
import com.microsoft.recognizers.text.number.french.parsers.FrenchNumberParserConfiguration;
import com.microsoft.recognizers.text.number.german.parsers.GermanNumberParserConfiguration;
import com.microsoft.recognizers.text.number.models.NumberModel;
import com.microsoft.recognizers.text.number.models.NumberRangeModel;
import com.microsoft.recognizers.text.number.models.OrdinalModel;
import com.microsoft.recognizers.text.number.models.PercentModel;
import com.microsoft.recognizers.text.number.parsers.AgnosticNumberParserFactory;
import com.microsoft.recognizers.text.number.parsers.AgnosticNumberParserType;
import com.microsoft.recognizers.text.number.parsers.BaseNumberRangeParser;
import com.microsoft.recognizers.text.number.portuguese.parsers.PortugueseNumberParserConfiguration;
import com.microsoft.recognizers.text.number.spanish.parsers.SpanishNumberParserConfiguration;

import java.util.List;
import java.util.function.Function;

public class NumberRecognizer extends Recognizer<NumberOptions> {

    public NumberRecognizer() {
        this(null, NumberOptions.None, true);
    }

    public NumberRecognizer(String culture) {
        this(culture, NumberOptions.None, false);
    }

    public NumberRecognizer(NumberOptions numberOptions) {
        this(null, numberOptions, true);
    }

    public NumberRecognizer(NumberOptions numberOptions, boolean lazyInitialization) {
        this(null, numberOptions, lazyInitialization);
    }

    public NumberRecognizer(String culture, NumberOptions numberOptions, boolean lazyInitialization) {
        super(culture, numberOptions, lazyInitialization);
    }

    //region Helper methods for less verbosity
    public NumberModel getNumberModel() {
        return getNumberModel(null, true);
    }

    public NumberModel getNumberModel(String culture, boolean fallbackToDefaultCulture) {
        return getModel(NumberModel.class, culture, fallbackToDefaultCulture);
    }

    public OrdinalModel getOrdinalModel() {
        return getOrdinalModel(null, true);
    }

    public OrdinalModel getOrdinalModel(String culture, boolean fallbackToDefaultCulture) {
        return getModel(OrdinalModel.class, culture, fallbackToDefaultCulture);
    }

    public PercentModel getPercentageModel() {
        return getPercentageModel(null, true);
    }

    public PercentModel getPercentageModel(String culture, boolean fallbackToDefaultCulture) {
        return getModel(PercentModel.class, culture, fallbackToDefaultCulture);
    }

    public NumberRangeModel getNumberRangeModel() {
        return getNumberRangeModel(null, true);
    }

    public NumberRangeModel getNumberRangeModel(String culture, boolean fallbackToDefaultCulture) {
        return getModel(NumberRangeModel.class, culture, fallbackToDefaultCulture);
    }

    public static List<ModelResult> recognizeNumber(String query, String culture) {
        return recognizeNumber(query, culture, NumberOptions.None, true);
    }

    public static List<ModelResult> recognizeNumber(String query, String culture, NumberOptions options) {
        return recognizeNumber(query, culture, options, true);
    }

    public static List<ModelResult> recognizeNumber(String query, String culture, NumberOptions options, boolean fallbackToDefaultCulture) {
        return recognizeByModel((NumberRecognizer recognizer) -> recognizer.getNumberModel(culture, fallbackToDefaultCulture), query, options);
    }

    public static List<ModelResult> recognizeOrdinal(String query, String culture) {
        return recognizeOrdinal(query, culture, NumberOptions.None, true);
    }

    public static List<ModelResult> recognizeOrdinal(String query, String culture, NumberOptions options) {
        return recognizeOrdinal(query, culture, options, true);
    }

    public static List<ModelResult> recognizeOrdinal(String query, String culture, NumberOptions options, boolean fallbackToDefaultCulture) {
        return recognizeByModel((NumberRecognizer recognizer) -> recognizer.getOrdinalModel(culture, fallbackToDefaultCulture), query, options);
    }

    public static List<ModelResult> recognizePercentage(String query, String culture) {
        return recognizePercentage(query, culture, NumberOptions.None, true);
    }

    public static List<ModelResult> recognizePercentage(String query, String culture, NumberOptions options) {
        return recognizePercentage(query, culture, options, true);
    }

    public static List<ModelResult> recognizePercentage(String query, String culture, NumberOptions options, boolean fallbackToDefaultCulture) {
        return recognizeByModel((NumberRecognizer recognizer) -> recognizer.getPercentageModel(culture, fallbackToDefaultCulture), query, options);
    }

    public static List<ModelResult> recognizeNumberRange(String query, String culture) {
        return recognizeNumberRange(query, culture, NumberOptions.None, true);
    }

    public static List<ModelResult> recognizeNumberRange(String query, String culture, NumberOptions options) {
        return recognizeNumberRange(query, culture, options, true);
    }

    public static List<ModelResult> recognizeNumberRange(String query, String culture, NumberOptions options, boolean fallbackToDefaultCulture) {
        return recognizeByModel((NumberRecognizer recognizer) -> recognizer.getNumberRangeModel(culture, fallbackToDefaultCulture), query, options);
    }
    //endregion

    private static List<ModelResult> recognizeByModel(Function<NumberRecognizer, IModel> getModelFun, String query, NumberOptions options) {
        NumberRecognizer recognizer = new NumberRecognizer(options);
        IModel model = getModelFun.apply(recognizer);
        return model.parse(query);
    }

    @Override
    protected void initializeConfiguration() {
        //region English
        registerModel(NumberModel.class, Culture.English, (options) -> new NumberModel(
                AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Number, new EnglishNumberParserConfiguration(options)),
                com.microsoft.recognizers.text.number.english.extractors.NumberExtractor.getInstance(NumberMode.PureNumber, options)));
        registerModel(OrdinalModel.class, Culture.English, (options) -> new OrdinalModel(
                AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Ordinal, new EnglishNumberParserConfiguration(options)),
                com.microsoft.recognizers.text.number.english.extractors.OrdinalExtractor.getInstance()));
        registerModel(PercentModel.class, Culture.English, (options) -> new PercentModel(
                AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Percentage, new EnglishNumberParserConfiguration(options)),
                new com.microsoft.recognizers.text.number.english.extractors.PercentageExtractor(options)));
        registerModel(NumberRangeModel.class, Culture.English, (options) -> new NumberRangeModel(
                new BaseNumberRangeParser(new EnglishNumberRangeParserConfiguration()),
                new com.microsoft.recognizers.text.number.english.extractors.NumberRangeExtractor()));

        //endregion

        //region Spanish
        registerModel(NumberModel.class, Culture.Spanish, (options) -> new NumberModel(
                AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Number, new SpanishNumberParserConfiguration()),
                com.microsoft.recognizers.text.number.spanish.extractors.NumberExtractor.getInstance(NumberMode.PureNumber)));
        registerModel(OrdinalModel.class, Culture.Spanish, (options) -> new OrdinalModel(
                AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Ordinal, new SpanishNumberParserConfiguration()),
                new com.microsoft.recognizers.text.number.spanish.extractors.OrdinalExtractor()));
        registerModel(PercentModel.class, Culture.Spanish, (options) -> new PercentModel(
                AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Percentage, new SpanishNumberParserConfiguration()),
                new com.microsoft.recognizers.text.number.spanish.extractors.PercentageExtractor()));
        //endregion

        //region Portuguese
        registerModel(NumberModel.class, Culture.Portuguese, (options) -> new NumberModel(
                AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Number, new PortugueseNumberParserConfiguration()),
                com.microsoft.recognizers.text.number.portuguese.extractors.NumberExtractor.getInstance(NumberMode.PureNumber)));
        registerModel(OrdinalModel.class, Culture.Portuguese, (options) -> new OrdinalModel(
                AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Ordinal, new PortugueseNumberParserConfiguration()),
                new com.microsoft.recognizers.text.number.portuguese.extractors.OrdinalExtractor()));
        registerModel(PercentModel.class, Culture.Portuguese, (options) -> new PercentModel(
                AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Percentage, new PortugueseNumberParserConfiguration()),
                new com.microsoft.recognizers.text.number.portuguese.extractors.PercentageExtractor()));
        //endregion

        //region French
        registerModel(NumberModel.class, Culture.French, (options) -> new NumberModel(
                AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Number, new FrenchNumberParserConfiguration()),
                com.microsoft.recognizers.text.number.french.extractors.NumberExtractor.getInstance(NumberMode.PureNumber)));
        registerModel(OrdinalModel.class, Culture.French, (options) -> new OrdinalModel(
                AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Ordinal, new FrenchNumberParserConfiguration()),
                new com.microsoft.recognizers.text.number.french.extractors.OrdinalExtractor()));
        registerModel(PercentModel.class, Culture.French, (options) -> new PercentModel(
                AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Percentage, new FrenchNumberParserConfiguration()),
                new com.microsoft.recognizers.text.number.french.extractors.PercentageExtractor()));
        //endregion

        //region German
        registerModel(NumberModel.class, Culture.German, (options) -> new NumberModel(
                AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Number, new GermanNumberParserConfiguration()),
                com.microsoft.recognizers.text.number.german.extractors.NumberExtractor.getInstance(NumberMode.PureNumber)));
        registerModel(OrdinalModel.class, Culture.German, (options) -> new OrdinalModel(
                AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Ordinal, new GermanNumberParserConfiguration()),
                new com.microsoft.recognizers.text.number.german.extractors.OrdinalExtractor()));
        registerModel(PercentModel.class, Culture.German, (options) -> new PercentModel(
                AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Percentage, new GermanNumberParserConfiguration()),
                new com.microsoft.recognizers.text.number.german.extractors.PercentageExtractor()));
        //endregion
        
        //region Chinese
        registerModel(NumberModel.class, Culture.Chinese, (options) -> new NumberModel(
                AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Number, new ChineseNumberParserConfiguration()),
                new com.microsoft.recognizers.text.number.chinese.extractors.NumberExtractor()));
        registerModel(OrdinalModel.class, Culture.Chinese, (options) -> new OrdinalModel(
                AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Ordinal, new ChineseNumberParserConfiguration()),
                new com.microsoft.recognizers.text.number.chinese.extractors.OrdinalExtractor()));
        registerModel(PercentModel.class, Culture.Chinese, (options) -> new PercentModel(
                AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Percentage, new ChineseNumberParserConfiguration()),
                new com.microsoft.recognizers.text.number.chinese.extractors.PercentageExtractor()));
        registerModel(NumberRangeModel.class, Culture.Chinese, (options) -> new NumberRangeModel(
                new BaseNumberRangeParser(new ChineseNumberRangeParserConfiguration()),
                new com.microsoft.recognizers.text.number.chinese.extractors.NumberRangeExtractor()));
        //endregion
    }
}
