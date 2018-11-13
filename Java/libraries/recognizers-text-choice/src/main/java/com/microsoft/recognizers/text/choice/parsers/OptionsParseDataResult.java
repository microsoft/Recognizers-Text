package com.microsoft.recognizers.text.choice.parsers;

import java.util.ArrayList;
import java.util.List;

public class OptionsParseDataResult {

    public final double score;
    public final List<OptionsOtherMatchParseResult> otherMatches;

    public OptionsParseDataResult(double optionScore, List<OptionsOtherMatchParseResult> optionOtherMatches) {
        score = optionScore;
        otherMatches = optionOtherMatches;
    }
}