package com.microsoft.recognizers.text.choice.extractors;

import com.microsoft.recognizers.text.ExtractResult;

import java.util.ArrayList;
import java.util.List;

public class ChoiceExtractDataResult {

    public final List<ExtractResult> otherMatches;
    public final String source;
    public final double score;

    public ChoiceExtractDataResult(String extractDataSource, double extractDataScore, List<ExtractResult> extractDataOtherMatches) {
        otherMatches = extractDataOtherMatches;
        source = extractDataSource;
        score = extractDataScore;
    }
}