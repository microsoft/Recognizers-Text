package com.microsoft.recognizers.text.datetime.extractors;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.extractors.config.ITimeExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.utilities.Token;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import com.microsoft.recognizers.text.utilities.StringUtility;

import java.time.LocalDateTime;
import java.util.AbstractMap;
import java.util.ArrayList;
import java.util.List;
import java.util.regex.Pattern;

public class BaseTimeExtractor implements IDateTimeExtractor {

    private final ITimeExtractorConfiguration config;

    @Override
    public String getExtractorName() {
        return Constants.SYS_DATETIME_TIME;
    }

    public BaseTimeExtractor(ITimeExtractorConfiguration config) {
        this.config = config;
    }

    @Override
    public List<ExtractResult> extract(String input, LocalDateTime reference) {
        List<Token> tokens = new ArrayList<>();
        tokens.addAll(basicRegexMatch(input));
        tokens.addAll(atRegexMatch(input));
        tokens.addAll(beforeAfterRegexMatch(input));
        tokens.addAll(specialCasesRegexMatch(input));

        List<ExtractResult> timeErs = Token.mergeAllTokens(tokens, input, getExtractorName());
        if (this.config.getOptions().match(DateTimeOptions.EnablePreview)) {
            timeErs = mergeTimeZones(timeErs, config.getTimeZoneExtractor().extract(input, reference), input);
        }
        return timeErs;
    }

    @Override
    public List<ExtractResult> extract(String input) {
        return this.extract(input, LocalDateTime.now());
    }

    private List<ExtractResult> mergeTimeZones(List<ExtractResult> timeErs, List<ExtractResult> timeZoneErs, String text) {
        int erIndex = 0;
        for (ExtractResult er : timeErs.toArray(new ExtractResult[0])) {
            for (ExtractResult timeZoneEr : timeZoneErs) {
                int begin = er.start + er.length;
                int end = timeZoneEr.start;

                if (begin < end) {
                    String gapText = text.substring(begin, end);

                    if (StringUtility.isNullOrWhiteSpace(gapText)) {
                        int newLenght = timeZoneEr.start + timeZoneEr.length;

                        timeErs.set(erIndex, er
                                .withText(text.substring(er.start, newLenght))
                                .withLength(newLenght - er.start)
                                .withData(new AbstractMap.SimpleEntry<>(Constants.SYS_DATETIME_TIMEZONE, timeZoneEr)));
                    }
                }
            }
            erIndex++;
        }
        return timeErs;
    }

    public final List<Token> basicRegexMatch(String text) {
        List<Token> ret = new ArrayList<>();
        for (Pattern regex : this.config.getTimeRegexList()) {
            Match[] matches = RegExpUtility.getMatches(regex, text);
            for (Match match : matches) {
                ret.add(new Token(match.index, match.index + match.length));
            }
        }
        return ret;
    }

    private List<Token> atRegexMatch(String text) {
        List<Token> ret = new ArrayList<>();
        // handle "at 5", "at seven"
        Pattern pattern = this.config.getAtRegex();
        Match[] matches = RegExpUtility.getMatches(pattern, text);
        if (matches.length > 0) {
            for (Match match : matches) {
                if (match.index + match.length < text.length() && text.charAt(match.index + match.length) == '%') {
                    continue;
                }
                ret.add(new Token(match.index, match.index + match.length));
            }
        }
        return ret;
    }

    private List<Token> beforeAfterRegexMatch(String text) {
        List<Token> ret = new ArrayList<>();
        // only enabled in CalendarMode
        if (this.config.getOptions().match(DateTimeOptions.CalendarMode)) {
            // handle "before 3", "after three"
            Pattern beforeAfterRegex = this.config.getTimeBeforeAfterRegex();
            Match[] matches = RegExpUtility.getMatches(beforeAfterRegex, text);
            if (matches.length > 0) {
                for (Match match : matches) {
                    ret.add(new Token(match.index, match.index + match.length));
                }
            }
        }
        return ret;
    }

    private List<Token> specialCasesRegexMatch(String text) {
        List<Token> ret = new ArrayList<>();
        // handle "ish"
        if (this.config.getIshRegex() != null && RegExpUtility.getMatches(this.config.getIshRegex(), text).length > 0) {
            Match[] matches = RegExpUtility.getMatches(this.config.getIshRegex(), text);
            for (Match match : matches) {
                ret.add(new Token(match.index, match.index + match.length));
            }
        }
        return ret;
    }
}
