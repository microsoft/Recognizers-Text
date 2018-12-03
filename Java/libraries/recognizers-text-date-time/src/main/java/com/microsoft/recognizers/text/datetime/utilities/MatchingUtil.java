package com.microsoft.recognizers.text.datetime.utilities;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.datetime.extractors.config.ProcessedSuperfluousWords;
import com.microsoft.recognizers.text.matcher.MatchResult;
import com.microsoft.recognizers.text.matcher.StringMatcher;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.Arrays;
import java.util.List;
import java.util.Optional;
import java.util.regex.Pattern;

public class MatchingUtil {

    public static MatchingUtilResult getAgoLaterIndex(String text, Pattern pattern) {
        Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(pattern, text.trim().toLowerCase())).findFirst();

        if (match.isPresent() && match.get().index == 0) {
            int index = text.toLowerCase().lastIndexOf(match.get().value) + match.get().value.length();
            return new MatchingUtilResult(true, index);
        }

        return new MatchingUtilResult();
    }

    public static MatchingUtilResult getTermIndex(String text, Pattern pattern) {
        String[] parts = text.trim().toLowerCase().split(" ");
        String lastPart = parts[parts.length - 1];
        Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(pattern, lastPart)).findFirst();

        if (match.isPresent()) {
            int index = text.length() - text.toLowerCase().lastIndexOf(match.get().value);
            return new MatchingUtilResult(true, index);
        }

        return new MatchingUtilResult();
    }

    public static Boolean containsAgoLaterIndex(String text, Pattern regex) {
        MatchingUtilResult result = getAgoLaterIndex(text, regex);
        return result.result;
    }

    public static Boolean containsTermIndex(String text, Pattern regex) {
        MatchingUtilResult result = getTermIndex(text, regex);
        return result.result;
    }

    // Temporary solution for remove superfluous words only under the Preview mode
    public static ProcessedSuperfluousWords preProcessTextRemoveSuperfluousWords(String text, StringMatcher matcher) {
        Iterable<MatchResult<String>> superfluousWordMatches = matcher.find(text);
        int bias = 0;

        for (MatchResult<String> match : superfluousWordMatches) {
            text = text.substring(0, match.getStart() - bias) + text.substring(match.getEnd() - bias);
            bias += match.getLength();
        }

        return new ProcessedSuperfluousWords(text, superfluousWordMatches);
    }

    // Temporary solution for recover superfluous words only under the Preview mode
    public static List<ExtractResult> posProcessExtractionRecoverSuperfluousWords(List<ExtractResult> extractResults,
                                                                                  Iterable<MatchResult<String>> superfluousWordMatches, String originText) {
        for (MatchResult<String> match : superfluousWordMatches) {
            for (ExtractResult extractResult : extractResults.toArray(new ExtractResult[0])) {
                int index = 0;
                int extractResultEnd = extractResult.start + extractResult.length;
                if (match.getStart() > extractResult.start && extractResultEnd >= match.getStart()) {
                    extractResults.set(index, extractResult.withLength(extractResult.length + match.getLength()));
                }

                if (match.getStart() <= extractResult.start) {
                    extractResults.set(index, extractResult.withStart(extractResult.start + match.getLength()));
                }
                index++;
            }
        }

        int index = 0;
        for (ExtractResult er : extractResults.toArray(new ExtractResult[0])) {
            extractResults.set(index, er.withText(originText.substring(er.start, er.start + er.length)));
            index++;
        }

        return extractResults;
    }
}

