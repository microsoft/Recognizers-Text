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
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
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
                int begin = er.getStart() + er.getLength();
                int end = timeZoneEr.getStart();

                if (begin < end) {
                    String gapText = text.substring(begin, end);

                    if (StringUtility.isNullOrWhiteSpace(gapText)) {
                        int newLenght = timeZoneEr.getStart() + timeZoneEr.getLength();
                        String newText = text.substring(er.getStart(), newLenght);
                        Map<String, Object> newData = new HashMap<>();
                        newData.put(Constants.SYS_DATETIME_TIMEZONE, timeZoneEr);

                        er.setData(newData);
                        er.setText(newText);
                        er.setLength(newLenght - er.getStart());
                        timeErs.set(erIndex, er);
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
                
                // @TODO Remove when lookbehinds are handled correctly
                if (isDecimal(match, text)) {
                    continue;
                }

                // @TODO Workaround to avoid incorrect partial-only matches. Remove after time regex reviews across languages.
                String lth = match.getGroup("lth").value;

                if ((lth == null || lth.length() == 0) ||
                        (lth.length() != match.length && !(match.length == lth.length() + 1 && match.value.endsWith(" ")))) {

                    ret.add(new Token(match.index, match.index + match.length));
                }
            }
        }

        return ret;
    }
    
    // Check if the match is part of a decimal number (e.g. 123.24)
    private boolean isDecimal(Match match, String text) {
        boolean isDecimal = false;
        if (match.index > 1 && (text.charAt(match.index - 1) == ',' ||
                text.charAt(match.index - 1) == '.') && Character.isDigit(text.charAt(match.index - 2)) && Character.isDigit(match.value.charAt(0))) {
            isDecimal = true;
        }
        
        return isDecimal;
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
