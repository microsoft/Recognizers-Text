package com.microsoft.recognizers.text.choice.models;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IModel;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.ModelResult;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.choice.Constants;

import java.util.List;
import java.util.SortedMap;
import java.util.stream.Collectors;

public abstract class ChoiceModel implements IModel {
    protected IExtractor extractor;
    protected IParser parser;
        
    public ChoiceModel(IParser choiceParser, IExtractor choiceExtractor) {
        parser = choiceParser;
        extractor = choiceExtractor;
    }

    @Override
    public String getModelTypeName() {
        return Constants.MODEL_BOOLEAN;
    }

    @Override
    public List<ModelResult> parse(String query) {

        List<ExtractResult> extractResults = extractor.extract(query);
        List<ParseResult> parseResults = extractResults.stream().map(exRes -> parser.parse(exRes)).collect(Collectors.toList());
        
        List<ModelResult> modelResults = parseResults.stream().map(
            parseRes -> new ModelResult(parseRes.getText(), parseRes.getStart(), parseRes.getStart() + parseRes.getLength() - 1, getModelTypeName(), getResolution(parseRes))
        ).collect(Collectors.toList());
         
        return modelResults;
    }

    protected abstract SortedMap<String, Object> getResolution(ParseResult parseResult);
}