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
                    if (parseResult.getValue() instanceof List) {
                        parsedResults.addAll((List<ParseResult>)parseResult.getValue());
                    } else {
                        parsedResults.add(parseResult);
                    }
                }

                List<ModelResult> modelResults = parsedResults.stream().map(o -> {
                    
                    SortedMap<String, Object> resolutionValues = new TreeMap<String, Object>();
                    if (o.getValue() instanceof UnitValue) {
                        resolutionValues.put(ResolutionKey.Value, ((UnitValue)o.getValue()).number);
                        resolutionValues.put(ResolutionKey.Unit, ((UnitValue)o.getValue()).unit);
                    } else if (o.getValue() instanceof CurrencyUnitValue) {
                        resolutionValues.put(ResolutionKey.Value, ((CurrencyUnitValue)o.getValue()).number);
                        resolutionValues.put(ResolutionKey.Unit, ((CurrencyUnitValue)o.getValue()).unit);
                        resolutionValues.put(ResolutionKey.IsoCurrency, ((CurrencyUnitValue)o.getValue()).isoCurrency);
                    } else {
                        resolutionValues.put(ResolutionKey.Value, (String)o.getValue());
                    }

                    return new ModelResult(
                            o.getText(),
                            o.getStart(),
                            o.getStart() + o.getLength() - 1,
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
