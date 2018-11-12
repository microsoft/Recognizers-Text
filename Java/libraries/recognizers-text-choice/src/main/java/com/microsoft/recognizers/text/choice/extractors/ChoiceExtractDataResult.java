package com.microsoft.recognizers.text.choice.extractors;

import java.util.ArrayList;
import java.util.List;

import com.microsoft.recognizers.text.ExtractResult;

public class ChoiceExtractDataResult {
	public List<ExtractResult> otherMatches = new ArrayList<>();
	public String source = "";
	public double score = 0;
	public ChoiceExtractDataResult(String Source, double Score, List<ExtractResult> OtherMatches) {
		otherMatches = OtherMatches;
		source = Source;
		score = Score;
	}
}