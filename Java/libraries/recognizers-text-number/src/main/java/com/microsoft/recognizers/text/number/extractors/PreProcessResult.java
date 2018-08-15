package com.microsoft.recognizers.text.number.extractors;

import com.microsoft.recognizers.text.ExtractResult;

import java.util.List;
import java.util.Map;

public class PreProcessResult {
    public final String string;
    public final Map<Integer, Integer> positionMap;
    public final List<ExtractResult> numExtractResults;

    public PreProcessResult(String string, Map<Integer, Integer> positionMap, List<ExtractResult> numExtractResults) {
        this.string = string;
        this.positionMap = positionMap;
        this.numExtractResults = numExtractResults;
    }
}
