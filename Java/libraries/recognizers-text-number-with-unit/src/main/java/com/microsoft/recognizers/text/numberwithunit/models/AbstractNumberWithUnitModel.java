package com.microsoft.recognizers.text.numberwithunit.models;

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
import java.util.Map;
import java.util.SortedMap;
import java.util.TreeMap;
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

    @SuppressWarnings("unchecked")
    public List<ModelResult> parse(String query) {

        // Pre-process the query
        query = QueryProcessor.preprocess(query, true);

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
                        parsedResults.addAll((List<ParseResult>)parseResult.value);
                    } else {
                        parsedResults.add(parseResult);
                    }
                }

                List<ModelResult> modelResults = parsedResults.stream().map(o -> {
                    
                    SortedMap<String, Object> resolutionValues = new TreeMap<String, Object>();
                    if (o.value instanceof UnitValue) {
                        resolutionValues.put(ResolutionKey.Value, ((UnitValue)o.value).number);
                        resolutionValues.put(ResolutionKey.Unit, ((UnitValue)o.value).unit);
                    } else if (o.value instanceof CurrencyUnitValue) {
                        resolutionValues.put(ResolutionKey.Value, ((CurrencyUnitValue)o.value).number);
                        resolutionValues.put(ResolutionKey.Unit, ((CurrencyUnitValue)o.value).unit);
                        resolutionValues.put(ResolutionKey.IsoCurrency, ((CurrencyUnitValue)o.value).isoCurrency);
                    } else {
                        resolutionValues.put(ResolutionKey.Value, (String)o.value);
                    }

                    return new ModelResult(
                            o.text,
                            o.start,
                            o.start + o.length - 1,
                            getModelTypeName(),
                            resolutionValues);
                }).collect(Collectors.toList());


                for (ModelResult result : modelResults) {
                    boolean badd = true;

                    for (ModelResult extractionResult : extractionResults) {
                        if (extractionResult.start == result.start && extractionResult.end == result.end) {
                            badd = false;
                        }
                    }

                    if (badd) {
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
