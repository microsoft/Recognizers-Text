package com.microsoft.recognizers.text.numberwithunit.models;

import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;

import java.util.Map;

public class AgeModel extends AbstractNumberWithUnitModel {

    protected AgeModel(Map<IExtractor, IParser> extractorParserMap) {
        super(extractorParserMap);
    }

    @Override
    public String getModelTypeName() {
        return "age";
    }
}
