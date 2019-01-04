package com.microsoft.recognizers.text.datetime.models;

import com.microsoft.recognizers.text.ExtendedModelResult;
import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.IModel;
import com.microsoft.recognizers.text.ModelResult;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.parsers.DateTimeParseResult;
import com.microsoft.recognizers.text.datetime.parsers.IDateTimeParser;
import com.microsoft.recognizers.text.utilities.FormatUtility;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;
import java.util.SortedMap;
import java.util.stream.Collectors;

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
        query = FormatUtility.preprocess(query);

        List<DateTimeParseResult> parsedDateTimes = new ArrayList<>();

        try {
            List<ExtractResult> extractResults = extractor.extract(query, reference);

            for (ExtractResult result : extractResults) {
                DateTimeParseResult parseResult = parser.parse(result, reference);

                if (parseResult.getValue() instanceof List) {
                    parsedDateTimes.addAll((List<DateTimeParseResult>)parseResult.getValue());
                } else {
                    parsedDateTimes.add(parseResult);
                }
            }

            // Filter out ambiguous cases. Naïve approach.
            parsedDateTimes = parser.filterResults(query, parsedDateTimes);

        } catch (Exception e) {
            // Nothing to do. Exceptions in parse should not break users of recognizers.
            // No result.
            e.getMessage();
        }

        return parsedDateTimes.stream().map(this::getModelResult).collect(Collectors.toList());
    }

    private ModelResult getModelResult(DateTimeParseResult parsedDateTime) {

        int start = parsedDateTime.getStart();
        int end = parsedDateTime.getStart() + parsedDateTime.getLength() - 1;
        String typeName = parsedDateTime.getType();
        SortedMap<String, Object> resolution = (SortedMap<String, Object>)parsedDateTime.getValue();
        String text = parsedDateTime.getText();

        ModelResult result = new ModelResult(text, start, end, typeName, resolution);

        String[] types = parsedDateTime.getType().split("\\.");
        String type = types[types.length - 1];
        if (type.equals(Constants.SYS_DATETIME_DATETIMEALT)) {
            result = new ExtendedModelResult(result, getParentText(parsedDateTime));
        }
        
        return result;
    }

    private String getParentText(DateTimeParseResult parsedDateTime) {
        Map<String, Object> map = (Map<String, Object>)parsedDateTime.getData();
        Object result = map.get(ExtendedModelResult.ParentTextKey);
        return String.valueOf(result);
    }
}
