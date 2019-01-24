package com.microsoft.recognizers.text.datetime.extractors;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.extractors.config.ITimeZoneExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.utilities.MatchingUtil;
import com.microsoft.recognizers.text.datetime.utilities.Token;
import com.microsoft.recognizers.text.matcher.MatchResult;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.QueryProcessor;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;
import java.util.regex.Pattern;
import java.util.stream.Collectors;

public class BaseTimeZoneExtractor implements IDateTimeZoneExtractor {

    private final ITimeZoneExtractorConfiguration config;

    @Override
    public String getExtractorName() {
        return Constants.SYS_DATETIME_TIMEZONE;
    }

    public BaseTimeZoneExtractor(ITimeZoneExtractorConfiguration config) {
        this.config = config;
    }

    @Override
    public List<ExtractResult> extract(String input) {
        return this.extract(input, LocalDateTime.now());
    }

    @Override
    public List<ExtractResult> extract(String input, LocalDateTime reference) {
        String normalizedText = QueryProcessor.removeDiacritics(input);
        List<Token> tokens = new ArrayList<>();
        tokens.addAll(matchTimeZones(normalizedText));
        tokens.addAll(matchLocationTimes(normalizedText, tokens));
        return Token.mergeAllTokens(tokens, input, getExtractorName());
    }

    @Override
    public List<ExtractResult> removeAmbiguousTimezone(List<ExtractResult> extractResults) {
        return extractResults.stream().filter(o -> !config.getAmbiguousTimezoneList().contains(o.getText().toLowerCase())).collect(Collectors.toList());
    }

    private List<Token> matchLocationTimes(String text, List<Token> tokens) {
        List<Token> ret = new ArrayList<>();

        if (config.getLocationTimeSuffixRegex() == null) {
            return ret;
        }

        Match[] timeMatch = RegExpUtility.getMatches(config.getLocationTimeSuffixRegex(), text);

        // Before calling a Find() in location matcher, check if all the matched suffixes by
        // LocationTimeSuffixRegex are already inside tokens extracted by TimeZone matcher.
        // If so, don't call the Find() as they have been extracted by TimeZone matcher, otherwise, call it.

        boolean isAllSuffixInsideTokens = true;

        for (Match match : timeMatch) {
            boolean isInside = false;
            for (Token token : tokens) {
                if (token.getStart() <= match.index && token.getEnd() >= match.index + match.length) {
                    isInside = true;
                    break;
                }
            }

            if (!isInside) {
                isAllSuffixInsideTokens = false;
            }

            if (!isAllSuffixInsideTokens) {
                break;
            }
        }

        if (timeMatch.length != 0 && !isAllSuffixInsideTokens) {
            int lastMatchIndex = timeMatch[timeMatch.length - 1].index;
            Iterable<MatchResult<String>> matches = config.getLocationMatcher().find(text.substring(0, lastMatchIndex).toLowerCase());
            List<MatchResult<String>> locationMatches = MatchingUtil.removeSubMatches(matches);

            int i = 0;
            for (Match match : timeMatch) {
                boolean hasCityBefore = false;

                while (i < locationMatches.size() && locationMatches.get(i).getEnd() <= match.index) {
                    hasCityBefore = true;
                    i++;

                    if (i == locationMatches.size()) {
                        break;
                    }
                }

                if (hasCityBefore && locationMatches.get(i - 1).getEnd() == match.index) {
                    ret.add(new Token(locationMatches.get(i - 1).getStart(), match.index + match.length));
                }

                if (i == locationMatches.size()) {
                    break;
                }
            }
        }

        return ret;
    }

    private List<Token> matchTimeZones(String text) {
        List<Token> ret = new ArrayList<>();

        // Direct UTC matches
        Match[] directUtcMatches = RegExpUtility.getMatches(config.getDirectUtcRegex(), text.toLowerCase());
        if (directUtcMatches.length > 0) {
            for (Match match : directUtcMatches) {
                ret.add(new Token(match.index, match.index + match.length));
            }
        }

        Iterable<MatchResult<String>> matches = config.getTimeZoneMatcher().find(text.toLowerCase());
        for (MatchResult<String> match : matches) {
            ret.add(new Token(match.getStart(), match.getStart() + match.getLength()));
        }

        return ret;
    }
}