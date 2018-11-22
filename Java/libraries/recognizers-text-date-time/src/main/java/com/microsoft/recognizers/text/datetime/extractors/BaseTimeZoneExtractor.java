package com.microsoft.recognizers.text.datetime.extractors;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.extractors.config.ITimeZoneExtractorConfiguration;
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
import java.util.stream.StreamSupport;

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
        tokens.addAll(timeZoneMatch(normalizedText));
        tokens.addAll(cityTimeMatch(normalizedText));
        return Token.mergeAllTokens(tokens, input, getExtractorName());
    }

    @Override
    public List<ExtractResult> removeAmbiguousTimezone(List<ExtractResult> extractResults) {
        return extractResults.stream().filter(o -> !config.getAmbiguousTimezoneList().contains(o.text.toLowerCase())).collect(Collectors.toList());
    }

    private List<Token> timeZoneMatch(String text) {
        List<Token> ret = new ArrayList<>();
        for (Pattern regex : config.getTimeZoneRegexes()) {
            Match[] matches = RegExpUtility.getMatches(regex, text);
            for (Match match : matches) {
                ret.add(new Token(match.index, match.index + match.length));
            }
        }
        return ret;
    }

    private List<Token> cityTimeMatch(String text) {
        List<Token> ret = new ArrayList<>();

        if (config.getLocationTimeSuffixRegex() == null) {
            return ret;
        }

        Match[] timeMatch = RegExpUtility.getMatches(config.getLocationTimeSuffixRegex(), text);

        if (timeMatch.length != 0) {
            int lastMatchIndex = timeMatch[timeMatch.length - 1].index;
            Iterable<MatchResult<String>> findResult = config.getCityMatcher().find(text.substring(0, lastMatchIndex).toLowerCase());
            List<MatchResult<String>> cityMatchResult = StreamSupport.stream(findResult.spliterator(), false).collect(Collectors.toList());

            int i = 0;
            for (Match match : timeMatch) {
                boolean hasCityBefore = false;

                while (i < cityMatchResult.size() && cityMatchResult.get(i).getEnd() <= match.index) {
                    hasCityBefore = true;
                    i++;

                    if (i == cityMatchResult.size()) {
                        break;
                    }
                }

                if (hasCityBefore && cityMatchResult.get(i - 1).getEnd() == match.index) {
                    ret.add(new Token(cityMatchResult.get(i - 1).getStart(), match.index + match.length));
                }

                if (i == cityMatchResult.size()) {
                    break;
                }
            }
        }

        return ret;
    }
}