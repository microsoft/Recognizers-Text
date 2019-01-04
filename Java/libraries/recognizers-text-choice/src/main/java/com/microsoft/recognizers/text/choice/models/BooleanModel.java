package com.microsoft.recognizers.text.choice.models;

import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.choice.Constants;
import com.microsoft.recognizers.text.choice.parsers.OptionsOtherMatchParseResult;
import com.microsoft.recognizers.text.choice.parsers.OptionsParseDataResult;

import java.util.SortedMap;
import java.util.TreeMap;

public class BooleanModel extends ChoiceModel {

    public BooleanModel(IParser parser, IExtractor extractor) {
        super(parser, extractor);
    }

    public String getModelTypeName() {
        return Constants.MODEL_BOOLEAN;
    }

    @Override
    protected SortedMap<String, Object> getResolution(ParseResult parseResult) {

        OptionsParseDataResult parseResultData = (OptionsParseDataResult)parseResult.getData();
        SortedMap<String, Object> results = new TreeMap<String, Object>();
        SortedMap<String, Object> otherMatchesMap = new TreeMap<String, Object>();

        results.put("value", parseResult.getValue());
        results.put("score", parseResultData.score);

        for (OptionsOtherMatchParseResult otherMatchParseRes : parseResultData.otherMatches) {
            otherMatchesMap.put("text", otherMatchParseRes.text);
            otherMatchesMap.put("value", otherMatchParseRes.value);
            otherMatchesMap.put("score", otherMatchParseRes.score);
        }

        results.put("otherResults", otherMatchesMap);

        return results;
    }
}