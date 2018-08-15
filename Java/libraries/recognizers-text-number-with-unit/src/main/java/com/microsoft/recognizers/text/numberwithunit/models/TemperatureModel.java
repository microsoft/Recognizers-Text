package com.microsoft.recognizers.text.numberwithunit.models;

import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;

import java.util.Map;

public class TemperatureModel extends AbstractNumberWithUnitModel {

    public TemperatureModel(Map<IExtractor, IParser> extractorParserMap) {
        super(extractorParserMap);
    }

    @Override
    public String getModelTypeName() {
        return "temperature";
    }
}
