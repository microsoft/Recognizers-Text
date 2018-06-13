package com.microsoft.recognizers.text.number.models;

import com.microsoft.recognizers.text.*;

import java.util.*;
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

        return parsedNumbers.stream().map(o -> new ModelResult(
                o.text,
                o.start,
                o.start + o.length,
                getModelTypeName(),
                new TreeMap<String, Object>() {
                    {
                        put(ResolutionKey.Value, o.resolutionStr);
                    }
                })
        ).collect(Collectors.toCollection(ArrayList::new));
    }
}
