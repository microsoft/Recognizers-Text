package com.microsoft.recognizers.text.choice.parsers;

import java.util.ArrayList;
import java.util.List;

public class OptionsParseDataResult {
	public double score = 0;
	public List<OptionsOtherMatchParseResult> otherMatches = new ArrayList<>();
	public OptionsParseDataResult(double Score, List<OptionsOtherMatchParseResult> OtherMatches) {
		score = Score;
		otherMatches = OtherMatches;
	}
}