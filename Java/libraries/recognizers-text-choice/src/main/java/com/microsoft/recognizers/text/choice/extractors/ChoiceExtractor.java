package com.microsoft.recognizers.text.choice.extractors;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.choice.utilities.UnicodeUtils;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import com.microsoft.recognizers.text.utilities.StringUtility;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Comparator;
import java.util.List;
import java.util.Map;
import java.util.Optional;
import java.util.regex.Pattern;

public class ChoiceExtractor implements IExtractor {

    private IChoiceExtractorConfiguration config;

    public ChoiceExtractor(IChoiceExtractorConfiguration config) {
        this.config = config;
    }

    @Override
    public List<ExtractResult> extract(String text) {

        List<ExtractResult> results = new ArrayList<>();
        String trimmedText = text.toLowerCase();
        List<ExtractResult> partialResults = new ArrayList<>();
        List<String> sourceTokens = tokenize(trimmedText);

        if (text.isEmpty()) {
            return results;
        }

        for (Map.Entry<Pattern, String> entry : this.config.getMapRegexes().entrySet()) {

            Pattern regexKey = entry.getKey();
            String constantValue = entry.getValue();
            Match[] matches = RegExpUtility.getMatches(regexKey, trimmedText);
            double topScore = 0;

            for (Match match : matches) {

                List<String> matchToken = tokenize(match.value);
                for (int i = 0; i < sourceTokens.size(); i++) {
                    double score = matchValue(sourceTokens, matchToken, i);
                    topScore = Math.max(topScore, score);
                }

                if (topScore > 0.0) {
                    int start = match.index;
                    int length = match.length;
                    partialResults.add(
                        new ExtractResult(
                            start,
                            length,
                            text.substring(start, length + start),
                            constantValue,
                            new ChoiceExtractDataResult(text, topScore, new ArrayList<>())
                        )
                    );
                }

            }
        }

        if (partialResults.size() == 0) {
            return results;
        }

        partialResults.sort(Comparator.comparingInt(er -> er.getStart()));

        if (this.config.getOnlyTopMatch()) {

            double topScore = 0;
            int topResultIndex = 0;

            for (int i = 0; i < partialResults.size(); i++) {

                ChoiceExtractDataResult data = (ChoiceExtractDataResult)partialResults.get(i).getData();
                if (data.score > topScore) {
                    topScore = data.score;
                    topResultIndex = i;
                }

            }
            results.add(partialResults.get(topResultIndex));
            partialResults.remove(topResultIndex);
        } else {
            results = partialResults;
        }

        return results;
    }

    private final double matchValue(List<String> source, List<String> match, int startPosition) {

        double matched = 0;
        double totalDeviation = 0;
        double score = 0;

        for (String token : match) {
            int pos = indexOfToken(source, token, startPosition);
            if (pos >= 0) {
                int distance = matched > 0 ? pos - startPosition : 0;
                if (distance <= config.getMaxDistance()) {
                    matched++;
                    totalDeviation += distance;
                    startPosition = pos + 1;
                }
            }
        }

        if (matched > 0 && (matched == match.size() || config.getAllowPartialMatch())) {
            double completeness = matched / match.size();
            double accuracy = completeness * (matched / (matched + totalDeviation));
            double initialScore = accuracy * (matched / source.size());
            score = 0.4 + (0.6 * initialScore);
        }

        return score;
    }

    private static int indexOfToken(List<String> tokens, String token, int startPos) {

        if (tokens.size() <= startPos) {
            return -1;
        }

        return tokens.indexOf(token);
    }

    private final List<String> tokenize(String text) {

        List<String> tokens = new ArrayList<>();
        List<String> letters = UnicodeUtils.letters(text);
        String token = "";

        for (String letter : letters) {

            Optional<Match> isMatch = Arrays.stream(RegExpUtility.getMatches(this.config.getTokenRegex(), letter)).findFirst();
            if (UnicodeUtils.isEmoji(letter)) {

                // Character is in a Supplementary Unicode Plane. This is where emoji live so
                // we're going to just break each character in this range out as its own token.
                tokens.add(letter);
                if (!StringUtility.isNullOrWhiteSpace(token)) {
                    tokens.add(token);
                    token = "";
                }

            } else if (!(isMatch.isPresent() || StringUtility.isNullOrWhiteSpace(letter))) {
                token = token + letter;
            } else if (!StringUtility.isNullOrWhiteSpace(token)) {
                tokens.add(token);
                token = "";
            }
        }

        if (!StringUtility.isNullOrWhiteSpace(token)) {
            tokens.add(token);
            token = "";
        }

        return tokens;
    }
}
