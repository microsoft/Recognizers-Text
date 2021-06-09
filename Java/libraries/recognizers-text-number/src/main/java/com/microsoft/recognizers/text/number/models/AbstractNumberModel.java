package com.microsoft.recognizers.text.number.models;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IModel;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.ModelResult;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.ResolutionKey;
import com.microsoft.recognizers.text.utilities.QueryProcessor;

import java.util.ArrayList;
import java.util.List;
import java.util.SortedMap;
import java.util.TreeMap;
import java.util.stream.Collectors;

public abstract class AbstractNumberModel implements IModel {

    protected final IParser parser;
    protected final IExtractor extractor;

    protected AbstractNumberModel(IParser parser, IExtractor extractor) {
        this.parser = parser;
        this.extractor = extractor;
    }

    @Override
    public List<ModelResult> parse(String query) {

        // Pre-process the query
        query = QueryProcessor.preprocess(query, true);

        List<ParseResult> parsedNumbers = new ArrayList<ParseResult>();

        try {
            List<ExtractResult> extractResults = extractor.extract(query);
            for (ExtractResult result : extractResults) {
                ParseResult parsedResult = parser.parse(result);
                if (parsedResult != null) {
                    parsedNumbers.add(parsedResult);
                }
            }
        } catch (Exception ex) {
            // Nothing to do. Exceptions in parse should not break users of recognizers.
            // No result.
            ex.printStackTrace();
        }

        return parsedNumbers.stream().map(o -> {
            SortedMap<String, Object> sortedMap = new TreeMap<String, Object>();
            sortedMap.put(ResolutionKey.Value, o.getResolutionStr());

            // We decreased the end property by 1 in order to keep parity with other platforms (C#/JS).
            return new ModelResult(
                    o.getText(),
                    o.getStart(),
                o.getStart() + o.getLength() - 1,
                getModelTypeName(),
                sortedMap
            );                
        }).collect(Collectors.toCollection(ArrayList::new));
    }
}
