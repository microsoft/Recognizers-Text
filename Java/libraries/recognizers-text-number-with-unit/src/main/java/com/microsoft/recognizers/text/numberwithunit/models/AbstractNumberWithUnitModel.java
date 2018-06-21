package com.microsoft.recognizers.text.numberwithunit.models;

import com.google.common.collect.ImmutableSortedMap;
import com.microsoft.recognizers.text.*;
import com.microsoft.recognizers.text.utilities.FormatUtility;

import java.util.*;
import java.util.stream.Collectors;

public abstract class AbstractNumberWithUnitModel implements IModel {

    private final Map<IExtractor, IParser> extractorParserMap;

    public abstract String getModelTypeName();

    protected Map<IExtractor, IParser> getExtractorParserMap() {
        return this.extractorParserMap;
    }

    protected AbstractNumberWithUnitModel(Map<IExtractor, IParser> extractorParserMap) {
        this.extractorParserMap = extractorParserMap;
    }

    public List<ModelResult> parse(String query) {

        // Preprocess the query
        query = FormatUtility.preprocess(query, false);

        List<ModelResult> extractionResults = new ArrayList<ModelResult>();

        try {
            for (Map.Entry<IExtractor, IParser> kv : extractorParserMap.entrySet()) {
                IExtractor extractor = kv.getKey();
                IParser parser = kv.getValue();

                List<ExtractResult> extractedResults = extractor.extract(query);

                List<ParseResult> parsedResults = new ArrayList<ParseResult>();

                for (ExtractResult result : extractedResults) {
                    ParseResult parseResult = parser.parse(result);
                    if (parseResult.value instanceof List) {
                        parsedResults.addAll((List<ParseResult>) parseResult.value);
                    } else {
                        parsedResults.add(parseResult);
                    }
                }

                List<ModelResult> modelResults = parsedResults.stream().map(o -> {

                    SortedMap<String, Object> resolutionValues =
                            (o.value instanceof UnitValue) ?
                                    new TreeMap<String, Object>() {
                                        {
                                            put(ResolutionKey.Value, ((UnitValue) o.value).number);
                                            put(ResolutionKey.Unit, ((UnitValue) o.value).unit);
                                        }
                                    } :
                                    (o.value instanceof CurrencyUnitValue) ?
                                            new TreeMap<String, Object>() {
                                                {
                                                    put(ResolutionKey.Value, ((CurrencyUnitValue) o.value).number);
                                                    put(ResolutionKey.Unit, ((CurrencyUnitValue) o.value).unit);
                                                    put(ResolutionKey.IsoCurrency, ((CurrencyUnitValue) o.value).isoCurrency);
                                                }
                                            } :
                                            new TreeMap<String, Object>() {
                                                {
                                                    put(ResolutionKey.Value, (String) o.value);
                                                }
                                            };

                    return new ModelResult(
                            o.text,
                            o.start,
                            o.start + o.length - 1,
                            getModelTypeName(),
                            resolutionValues);
                }).collect(Collectors.toList());


                for (ModelResult result : modelResults) {
                    boolean bAdd = true;

                    for (ModelResult extractionResult : extractionResults) {
                        if (extractionResult.start == result.start && extractionResult.end == result.end) {
                            bAdd = false;
                        }
                    }

                    if (bAdd) {
                        extractionResults.add(result);
                    }
                }
            }
        } catch (Exception ex) {
            // Nothing to do. Exceptions in parse should not break users of recognizers.
            // No result.
            ex.printStackTrace();
        }

        return extractionResults;
    }
}
