package com.microsoft.recognizers.text.datetime.extractors;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.extractors.config.IDateTimeExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.utilities.AgoLaterUtil;
import com.microsoft.recognizers.text.datetime.utilities.ConditionalMatch;
import com.microsoft.recognizers.text.datetime.utilities.RegexExtension;
import com.microsoft.recognizers.text.datetime.utilities.Token;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import com.microsoft.recognizers.text.utilities.StringUtility;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Comparator;
import java.util.List;
import java.util.Optional;

public class BaseDateTimeExtractor implements IDateTimeExtractor {
    private static final String SYS_NUM_INTEGER = com.microsoft.recognizers.text.number.Constants.SYS_NUM;

    private final IDateTimeExtractorConfiguration config;

    @Override
    public String getExtractorName() {
        return Constants.SYS_DATETIME_DATETIME;
    }

    public BaseDateTimeExtractor(IDateTimeExtractorConfiguration config) {
        this.config = config;
    }

    @Override
    public List<ExtractResult> extract(String input, LocalDateTime reference) {
        List<Token> tokens = new ArrayList<>();

        tokens.addAll(mergeDateAndTime(input, reference));
        tokens.addAll(basicRegexMatch(input));
        tokens.addAll(timeOfTodayBefore(input, reference));
        tokens.addAll(timeOfTodayAfter(input, reference));
        tokens.addAll(specialTimeOfDate(input, reference));
        tokens.addAll(durationWithBeforeAndAfter(input, reference));

        return Token.mergeAllTokens(tokens, input, getExtractorName());
    }

    @Override
    public List<ExtractResult> extract(String input) {
        return this.extract(input, LocalDateTime.now());
    }

    private List<Token> durationWithBeforeAndAfter(String input, LocalDateTime reference) {
        List<Token> ret = new ArrayList<>();

        List<ExtractResult> ers = this.config.getDurationExtractor().extract(input, reference);
        for (ExtractResult er : ers) {
            // if it is a multiple duration and its type is equal to Date then skip it.
            if (er.getData() != null && er.getData().toString() == Constants.MultipleDuration_Date) {
                continue;
            }

            Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(this.config.getUnitRegex(), er.getText())).findFirst();
            if (!match.isPresent()) {
                continue;
            }

            ret = AgoLaterUtil.extractorDurationWithBeforeAndAfter(input, er, ret, this.config.getUtilityConfiguration());
        }

        return ret;
    }

    public List<Token> specialTimeOfDate(String input, LocalDateTime reference) {
        List<Token> ret = new ArrayList<>();
        List<ExtractResult> ers = this.config.getDatePointExtractor().extract(input, reference);

        // handle "the end of the day"
        for (ExtractResult er : ers) {
            String beforeStr = input.substring(0, (er != null) ? er.getStart() : 0);

            Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(this.config.getSpecificEndOfRegex(), beforeStr)).findFirst();
            if (match.isPresent()) {
                ret.add(new Token(match.get().index, (er != null) ? er.getStart() + er.getLength() : 0));
            } else {
                String afterStr = input.substring((er != null) ? er.getStart() + er.getLength() : 0);

                match = Arrays.stream(RegExpUtility.getMatches(this.config.getSpecificEndOfRegex(), afterStr)).findFirst();
                if (match.isPresent()) {
                    ret.add(new Token(
                            (er != null) ? er.getStart() : 0,
                            ((er != null) ? er.getStart() + er.getLength() : 0) + ((match != null) ? match.get().index + match.get().length : 0)));
                }
            }
        }

        // Handle "eod, end of day"
        Match[] matches = RegExpUtility.getMatches(config.getUnspecificEndOfRegex(), input);
        for (Match match : matches) {
            ret.add(new Token(match.index, match.index + match.length));
        }

        return ret;
    }

    // Parses a specific time of today, tonight, this afternoon, like "seven this afternoon"
    public List<Token> timeOfTodayAfter(String input, LocalDateTime reference) {
        List<Token> ret = new ArrayList<>();

        List<ExtractResult> ers = this.config.getTimePointExtractor().extract(input, reference);

        for (ExtractResult er : ers) {
            String afterStr = input.substring(er.getStart() + er.getLength());

            if (StringUtility.isNullOrEmpty(afterStr)) {
                continue; //@here
            }

            Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(this.config.getTimeOfTodayAfterRegex(), afterStr)).findFirst();
            if (match.isPresent()) {
                int begin = er.getStart();
                int end = er.getStart() + er.getLength() + match.get().length;
                ret.add(new Token(begin, end));
            }
        }

        Match[] matches = RegExpUtility.getMatches(this.config.getSimpleTimeOfTodayAfterRegex(), input);
        for (Match match : matches) {
            // @TODO Remove when lookbehinds are handled correctly
            if (isDecimal(match, input)) {
                continue;
            }
            
            ret.add(new Token(match.index, match.index + match.length));
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

    public List<Token> timeOfTodayBefore(String input, LocalDateTime reference) {
        List<Token> ret = new ArrayList<>();
        List<ExtractResult> ers = this.config.getTimePointExtractor().extract(input, reference);
        for (ExtractResult er : ers) {
            String beforeStr = input.substring(0, (er != null) ? er.getStart() : 0);

            // handle "this morningh at 7am"
            ConditionalMatch innerMatch = RegexExtension.matchBegin(this.config.getTimeOfDayRegex(), er.getText(), true);
            if (innerMatch.getSuccess()) {
                beforeStr = input.substring(0, ((er != null) ? er.getStart() : 0) + innerMatch.getMatch().get().length);
            }

            if (StringUtility.isNullOrEmpty(beforeStr)) {
                continue;
            }

            Match match = Arrays.stream(RegExpUtility.getMatches(this.config.getTimeOfTodayBeforeRegex(), beforeStr)).findFirst().orElse(null);
            if (match != null) {
                int begin = match.index;
                int end = er.getStart() + er.getLength();
                ret.add(new Token(begin, end));
            }
        }

        Match[] matches = RegExpUtility.getMatches(this.config.getSimpleTimeOfTodayBeforeRegex(), input);
        for (Match match : matches) {
            ret.add(new Token(match.index, match.index + match.length));
        }

        return ret;
    }

    // Match "now"
    public List<Token> basicRegexMatch(String input) {
        List<Token> ret = new ArrayList<>();
        input = input.trim().toLowerCase();

        // Handle "now"
        Match[] matches = RegExpUtility.getMatches(this.config.getNowRegex(), input);

        for (Match match : matches) {
            ret.add(new Token(match.index, match.index + match.length));
        }

        return ret;
    }

    // Merge a Date entity and a Time entity, like "at 7 tomorrow"
    public List<Token> mergeDateAndTime(String input, LocalDateTime reference) {
        List<Token> ret = new ArrayList<>();
        List<ExtractResult> dateErs = this.config.getDatePointExtractor().extract(input, reference);
        if (dateErs.size() == 0) {
            return ret;
        }

        List<ExtractResult> timeErs = this.config.getTimePointExtractor().extract(input, reference);
        Match[] timeNumMatches = RegExpUtility.getMatches(config.getNumberAsTimeRegex(), input);

        if (timeErs.size() == 0 && timeNumMatches.length == 0) {
            return ret;
        }

        List<ExtractResult> ers = dateErs;
        ers.addAll(timeErs);

        // handle cases which use numbers as time points
        // only enabled in CalendarMode
        if (this.config.getOptions().match(DateTimeOptions.CalendarMode)) {
            List<ExtractResult> numErs = new ArrayList<>();
            for (Match timeNumMatch : timeNumMatches) {
                ExtractResult node = new ExtractResult(timeNumMatch.index, timeNumMatch.length, timeNumMatch.value, SYS_NUM_INTEGER);
                numErs.add(node);
            }
            ers.addAll(numErs);
        }

        ers.sort(Comparator.comparingInt(erA -> erA.getStart()));

        int i = 0;
        while (i < ers.size() - 1) {
            int j = i + 1;
            while (j < ers.size() && ers.get(i).isOverlap(ers.get(j))) {
                j++;
            }
            if (j >= ers.size()) {
                break;
            }

            ExtractResult ersI = ers.get(i);
            ExtractResult ersJ = ers.get(j);
            if (ersI.getType() == Constants.SYS_DATETIME_DATE && ersJ.getType() == Constants.SYS_DATETIME_TIME ||
                    ersI.getType() == Constants.SYS_DATETIME_TIME && ersJ.getType() == Constants.SYS_DATETIME_DATE ||
                    ersI.getType() == Constants.SYS_DATETIME_DATE && ersJ.getType() == SYS_NUM_INTEGER) {
                int middleBegin = ersI != null ? ersI.getStart() + ersI.getLength() : 0;
                int middleEnd = ersJ != null ? ersJ.getStart() : 0;
                if (middleBegin > middleEnd) {
                    i = j + 1;
                    continue;
                }

                String middleStr = input.substring(middleBegin, middleEnd).trim().toLowerCase();
                boolean valid = false;
                // for cases like "tomorrow 3",  "tomorrow at 3"
                if (ersJ.getType() == SYS_NUM_INTEGER) {
                    Optional<Match> matches = Arrays.stream(RegExpUtility.getMatches(this.config.getDateNumberConnectorRegex(), input)).findFirst();
                    if (StringUtility.isNullOrEmpty(middleStr) || matches.isPresent()) {
                        valid = true;
                    }
                } else {
                    // For case like "3pm or later on monday"
                    Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(this.config.getSuffixAfterRegex(), middleStr)).findFirst();
                    if (match.isPresent()) {
                        middleStr = middleStr.substring(match.get().index + match.get().length).trim();
                    }

                    if (!(match.isPresent() && middleStr.isEmpty())) {
                        if (this.config.isConnector(middleStr)) {
                            valid = true;
                        }
                    }
                }

                if (valid) {
                    int begin = ersI.getStart();
                    int end = ersJ.getStart() + ersJ.getLength();
                    ret.add(new Token(begin, end));
                    i = j + 1;
                    continue;
                }
            }
            i = j;
        }

        // Handle "in the afternoon" at the end of entity
        for (int idx = 0; idx < ret.size(); idx++) {
            Token idxToken = ret.get(idx);
            String afterStr = input.substring(idxToken.getEnd());
            Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(this.config.getSuffixRegex(), afterStr)).findFirst();
            if (match.isPresent()) {
                ret.set(idx, new Token(idxToken.getStart(), idxToken.getEnd() + match.get().length));
            }
        }

        // Handle "day" prefixes
        for (int idx = 0; idx < ret.size(); idx++) {
            Token idxToken = ret.get(idx);
            String beforeStr = input.substring(0, idxToken.getStart());
            Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(this.config.getUtilityConfiguration().getCommonDatePrefixRegex(), beforeStr)).findFirst();
            if (match.isPresent()) {
                ret.set(idx, new Token(idxToken.getStart() - match.get().length, idxToken.getEnd()));
            }
        }

        return ret;
    }
}
