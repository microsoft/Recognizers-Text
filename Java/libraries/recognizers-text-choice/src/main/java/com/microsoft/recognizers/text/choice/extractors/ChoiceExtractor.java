package com.microsoft.recognizers.text.choice.extractors;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.List;
import java.util.Map;
import java.util.Optional;
import java.util.regex.Pattern;
import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.choice.utilities.UnicodeUtils;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

public class ChoiceExtractor implements IExtractor {
	private IChoiceExtractorConfiguration config;

	public ChoiceExtractor(IChoiceExtractorConfiguration config) {
		this.config = config;
	}

	@Override
	public List<ExtractResult> extract(String text) {
		List<ExtractResult> results = new ArrayList<>();
		String trimmedText = text.toLowerCase();
		if (text.isEmpty()) {
			return results;
		}
		List<ExtractResult> partialResults = new ArrayList<>();
		List<String> sourceTokens = Tokenize(trimmedText);
		for (Map.Entry<Pattern, String> entry : this.config.getMapRegexes().entrySet()) {
			Pattern RegexKey = entry.getKey();
			String constantValue = entry.getValue();
			Match[] matches = RegExpUtility.getMatches(RegexKey, trimmedText);
			double topScore = 0;
			for (Match match : matches) {
				List<String> matchToken = Tokenize(match.value);
				for (int i = 0; i < sourceTokens.size(); i++) {
					double score = MatchValue(sourceTokens, matchToken, i);
					topScore = Math.max(topScore, score);
				}
				if (topScore > 0.0) {
					int start = match.index;
					int length = match.length;
					partialResults.add(new ExtractResult(start, length, text.substring(start, length).trim(), constantValue, new ChoiceExtractDataResult(text, topScore, new ArrayList<>())));
				}
			}
		}
		if (partialResults.size() == 0) {
			return results;
		}
		Collections.sort(partialResults, (ExtractResult ExtractResult1, ExtractResult ExtractResult2) -> (ExtractResult1.start < ExtractResult2.start) ? 1 : -1);
		if (this.config.getOnlyTopMatch()) {
			double topScore = 0;
			int topResultIndex = 0;
			for (int i = 0; i < partialResults.size(); i++) {
				ChoiceExtractDataResult data = (ChoiceExtractDataResult) partialResults.get(i).data;
				if (data.score > topScore) {
					topScore = data.score;
					topResultIndex = i;
				}
			}
			ChoiceExtractDataResult topResultData = (ChoiceExtractDataResult) partialResults.get(topResultIndex).data;
			topResultData.otherMatches = partialResults;
			results.add(partialResults.get(topResultIndex));
			partialResults.remove(topResultIndex);
		} else {
			results = partialResults;
		}
		return results;
	}

	private final double MatchValue(List<String> source, List<String> match, int startPosition) {
		double matched = 0;
		double totalDeviation = 0;
		for (String token : match) {
			int pos = IndexOfToken(source, token, startPosition);
			if (pos >= 0) {
				int distance = matched > 0 ? pos - startPosition : 0;
				if (distance <= config.getMaxDistance()) {
					matched++;
					totalDeviation += distance;
					startPosition = pos + 1;
				}
			}
		}
		double score = 0;
		if (matched > 0 && (matched == match.size() || config.getAllowPartialMatch())) {
			double completeness = matched / match.size();
			double accuracy = completeness * (matched / (matched + totalDeviation));
			double initialScore = accuracy * (matched / source.size());

			score = 0.4 + (0.6 * initialScore);
		}
		return score;
	}

	private static int IndexOfToken(List<String> tokens, String token, int startPos) {
		if (tokens.size() <= startPos) {
			return -1;
		}
		return tokens.indexOf(token);
	}

	private final List<String> Tokenize(String text) {
		List<String> tokens = new ArrayList<>();
		List<String> letters = UnicodeUtils.Letters(text);
		String token = "";
		for (String letter : letters) {
			Optional<Match> isMatch = Arrays.stream(RegExpUtility.getMatches(this.config.getTokenRegex(), letter)).findFirst();
			if (UnicodeUtils.IsEmoji(letter)) {
				// Character is in a Supplementary Unicode Plane. This is where emoji live so
				// we're going to just break each character in this range out as its own token.
				tokens.add(letter);
				if (!token.isBlank() && !token.isEmpty()) {
					tokens.add(token);
					token = "";
				}
			} else if (!(isMatch.isPresent() || letter.isBlank())) {
				token = token + letter;
			} else if (!token.isBlank() && !token.isEmpty()) {
				tokens.add(token);
				token = "";
			}
		}
		if (!token.isBlank() && !token.isEmpty()) {
			tokens.add(token);
			token = "";
		}
		return tokens;
	}
}
