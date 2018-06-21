package com.microsoft.recognizers.text.numberwithunit.models;

import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;

import java.util.Map;

public class DimensionModel extends AbstractNumberWithUnitModel {

    public DimensionModel(Map<IExtractor, IParser> extractorParserMap) {
        super(extractorParserMap);
    }

    @Override
    public String getModelTypeName() {
        return "dimension";
    }
}
