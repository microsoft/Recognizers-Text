package com.microsoft.recognizers.text.choice.extractors;

import java.util.ArrayList;
import java.util.List;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.IExtractor;

public class ChoiceExtractor implements IExtractor {
	
	private IChoiceExtractorConfiguration config;

	public ChoiceExtractor(IChoiceExtractorConfiguration config) {
		this.config = config;
	}
	
	@Override
	public List<ExtractResult> extract(String text) {
		throw new UnsupportedOperationException();
	}
	
	private final double MatchValue(List<String> source, List<String> match, int startPosition) {
		double matched = 0;
		return matched;
	}
	
	private static int IndexOfToken(List<String> tokens, String token, int startPos) {
		return 0;
	}
	
	private final List<String> Tokenize(String text) {
		return new ArrayList<>();
	}
} 
class ChoiceExtractDataResult {
	
	public final List<ExtractResult> OtherMatches = new ArrayList<>();
	
	public final String Source = "";
	
	public final double Score = 0;
}