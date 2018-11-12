package com.microsoft.recognizers.text.datetime.models;

import com.microsoft.recognizers.text.IModel;
import com.microsoft.recognizers.text.ModelResult;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.parsers.IDateTimeParser;

import java.time.LocalDateTime;
import java.util.List;

public class DateTimeModel implements IModel {

    protected final IDateTimeExtractor extractor;
    protected final IDateTimeParser parser;

    @Override
    public String getModelTypeName() {
        return Constants.MODEL_DATETIME;
    }

    public DateTimeModel(IDateTimeParser parser, IDateTimeExtractor extractor) {
        this.extractor = extractor;
        this.parser = parser;
    }

    @Override
    public List<ModelResult> parse(String query) {
        return this.parse(query, LocalDateTime.now());
    }

    public List<ModelResult> parse(String query, LocalDateTime reference) {
        throw new UnsupportedOperationException();
    }
}
