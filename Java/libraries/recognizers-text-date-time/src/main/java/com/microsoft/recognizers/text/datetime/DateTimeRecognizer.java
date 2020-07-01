package com.microsoft.recognizers.text.datetime;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.ModelResult;
import com.microsoft.recognizers.text.Recognizer;
import com.microsoft.recognizers.text.datetime.english.extractors.EnglishMergedExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.english.parsers.EnglishMergedParserConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.BaseMergedDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.french.extractors.FrenchMergedExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.french.parsers.FrenchMergedParserConfiguration;
import com.microsoft.recognizers.text.datetime.models.DateTimeModel;
import com.microsoft.recognizers.text.datetime.parsers.BaseMergedDateTimeParser;
import com.microsoft.recognizers.text.datetime.spanish.extractors.SpanishMergedExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.spanish.parsers.SpanishMergedParserConfiguration;

import java.time.LocalDateTime;
import java.util.List;
import java.util.function.Function;

public class DateTimeRecognizer extends Recognizer<DateTimeOptions> {

    public DateTimeRecognizer() {
        this(null, DateTimeOptions.None, true);
    }

    public DateTimeRecognizer(String culture) {
        this(culture, DateTimeOptions.None, false);
    }

    public DateTimeRecognizer(DateTimeOptions options) {
        this(null, options, true);
    }

    public DateTimeRecognizer(DateTimeOptions options, boolean lazyInitialization) {
        this(null, options, lazyInitialization);
    }

    public DateTimeRecognizer(String culture, DateTimeOptions options, boolean lazyInitialization) {
        super(culture, options, lazyInitialization);
    }

    public DateTimeModel getDateTimeModel() {
        return getDateTimeModel(null, true);
    }

    public DateTimeModel getDateTimeModel(String culture, boolean fallbackToDefaultCulture) {
        return getModel(DateTimeModel.class, culture, fallbackToDefaultCulture);
    }

    //region Helper methods for less verbosity
    public static List<ModelResult> recognizeDateTime(String query, String culture) {
        return recognizeByModel(recognizer -> recognizer.getDateTimeModel(culture, true), query, DateTimeOptions.None, LocalDateTime.now());
    }

    public static List<ModelResult> recognizeDateTime(String query, String culture, DateTimeOptions options) {
        return recognizeByModel(recognizer -> recognizer.getDateTimeModel(culture, true), query, options, LocalDateTime.now());
    }

    public static List<ModelResult> recognizeDateTime(String query, String culture, DateTimeOptions options, boolean fallbackToDefaultCulture) {
        return recognizeByModel(recognizer -> recognizer.getDateTimeModel(culture, fallbackToDefaultCulture), query, options, LocalDateTime.now());
    }

    public static List<ModelResult> recognizeDateTime(String query, String culture, DateTimeOptions options, boolean fallbackToDefaultCulture, LocalDateTime reference) {
        return recognizeByModel(recognizer -> recognizer.getDateTimeModel(culture, fallbackToDefaultCulture), query, options, reference);
    }
    //endregion

    private static List<ModelResult> recognizeByModel(Function<DateTimeRecognizer, DateTimeModel> getModelFun, String query, DateTimeOptions options, LocalDateTime reference) {
        DateTimeRecognizer recognizer = new DateTimeRecognizer(options);
        DateTimeModel model = getModelFun.apply(recognizer);
        return model.parse(query, reference);
    }

    @Override
    protected void initializeConfiguration() {
        // English
        registerModel(DateTimeModel.class, Culture.English,
            (options) -> new DateTimeModel(
                new BaseMergedDateTimeParser(new EnglishMergedParserConfiguration(options)),
                new BaseMergedDateTimeExtractor(new EnglishMergedExtractorConfiguration(options))));

        // Spanish
        registerModel(DateTimeModel.class, Culture.Spanish,
            (options) -> new DateTimeModel(
                new BaseMergedDateTimeParser(new SpanishMergedParserConfiguration(options)),
                new BaseMergedDateTimeExtractor(new SpanishMergedExtractorConfiguration(options))));

        registerModel(DateTimeModel.class, Culture.French, dateTimeOptions -> new DateTimeModel(
                new BaseMergedDateTimeParser(new FrenchMergedParserConfiguration(options)),
                new BaseMergedDateTimeExtractor(new FrenchMergedExtractorConfiguration(options))
        ));
    }
}
