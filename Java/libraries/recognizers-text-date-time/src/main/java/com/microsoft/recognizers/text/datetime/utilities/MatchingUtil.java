package com.microsoft.recognizers.text.datetime.utilities;

import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.Arrays;
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
}

