package com.microsoft.recognizers.text.datetime.utilities;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.datetime.extractors.config.ProcessedSuperfluousWords;
import com.microsoft.recognizers.text.matcher.MatchResult;
import com.microsoft.recognizers.text.matcher.StringMatcher;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.Optional;
import java.util.regex.Pattern;
import java.util.stream.Collectors;
import java.util.stream.StreamSupport;

public class MatchingUtil {

    public static MatchingUtilResult getAgoLaterIndex(String text, Pattern pattern) {
        int index = -1;
        ConditionalMatch match = RegexExtension.matchBegin(pattern, text, true);

        if (match.getSuccess()) {
            index = match.getMatch().get().index + match.getMatch().get().length;
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
        List<MatchResult<String>> superfluousWordMatches = removeSubMatches(matcher.find(text));
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
            int index = 0;
            for (ExtractResult extractResult : extractResults.toArray(new ExtractResult[0])) {
                int extractResultEnd = extractResult.getStart() + extractResult.getLength();
                if (match.getStart() > extractResult.getStart() && extractResultEnd >= match.getStart()) {
                    extractResult.setLength(extractResult.getLength() + match.getLength());
                    extractResults.set(index, extractResult);
                }

                if (match.getStart() <= extractResult.getStart()) {
                    extractResult.setStart(extractResult.getStart() + match.getLength());
                    extractResults.set(index, extractResult);
                }
                index++;
            }
        }

        int index = 0;
        for (ExtractResult er : extractResults.toArray(new ExtractResult[0])) {
            er.setText(originText.substring(er.getStart(), er.getStart() + er.getLength()));
            extractResults.set(index, er);
            index++;
        }

        return extractResults;
    }

    public static List<MatchResult<String>> removeSubMatches(Iterable<MatchResult<String>> matchResults) {

        return StreamSupport.stream(matchResults.spliterator(), false)
                .filter(item -> !StreamSupport.stream(matchResults.spliterator(), false)
                        .anyMatch(ritem -> (ritem.getStart() < item.getStart() && ritem.getEnd() >= item.getEnd()) ||
                                (ritem.getStart() <= item.getStart() && ritem.getEnd() > item.getEnd())))
                .collect(Collectors.toCollection(ArrayList::new));
    }
}