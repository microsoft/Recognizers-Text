package com.microsoft.recognizers.text.datetime.extractors;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.extractors.config.ISetExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.utilities.Token;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import com.microsoft.recognizers.text.utilities.StringUtility;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.Optional;
import java.util.regex.Pattern;

public class BaseSetExtractor implements IDateTimeExtractor {

    private final ISetExtractorConfiguration config;

    @Override
    public String getExtractorName() {
        return Constants.SYS_DATETIME_SET;
    }

    public BaseSetExtractor(ISetExtractorConfiguration config) {
        this.config = config;
    }

    @Override
    public List<ExtractResult> extract(String input) {
        return this.extract(input, LocalDateTime.now());
    }

    @Override
    public List<ExtractResult> extract(String input, LocalDateTime reference) {
        List<Token> tokens = new ArrayList<>();
        tokens.addAll(matchEachUnit(input));
        tokens.addAll(timeEveryday(input, reference));
        tokens.addAll(matchEachDuration(input, reference));
        tokens.addAll(matchEach(this.config.getDateExtractor(), input, reference));
        tokens.addAll(matchEach(this.config.getTimeExtractor(), input, reference));
        tokens.addAll(matchEach(this.config.getDateTimeExtractor(), input, reference));
        tokens.addAll(matchEach(this.config.getDatePeriodExtractor(), input, reference));
        tokens.addAll(matchEach(this.config.getTimePeriodExtractor(), input, reference));
        tokens.addAll(matchEach(this.config.getDateTimePeriodExtractor(), input, reference));

        return Token.mergeAllTokens(tokens, input, getExtractorName());
    }

    public final List<Token> matchEachUnit(String text) {
        List<Token> ret = new ArrayList<>();

        // handle "daily", "monthly"
        Pattern pattern = this.config.getPeriodicRegex();
        Match[] matches = RegExpUtility.getMatches(pattern, text);

        for (Match match : matches) {
            ret.add(new Token(match.index, match.index + match.length));
        }

        // handle "each month"
        pattern = this.config.getEachUnitRegex();
        matches = RegExpUtility.getMatches(pattern, text);

        for (Match match : matches) {
            ret.add(new Token(match.index, match.index + match.length));
        }

        return ret;
    }

    public final List<Token> matchEachDuration(String text, LocalDateTime reference) {
        List<Token> ret = new ArrayList<>();
        List<ExtractResult> ers = this.config.getDurationExtractor().extract(text, reference);

        for (ExtractResult er : ers) {
            // "each last summer" doesn't make sense
            Pattern lastRegex = this.config.getLastRegex();
            if (RegExpUtility.getMatches(lastRegex, er.getText()).length > 0) {
                continue;
            }

            String beforeStr = text.substring(0, (er.getStart() != null) ? er.getStart() : 0);
            Pattern eachPrefixRegex = this.config.getEachPrefixRegex();
            Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(eachPrefixRegex, beforeStr)).findFirst();
            if (match.isPresent()) {
                ret.add(new Token(match.get().index, er.getStart() + er.getLength()));
            }
        }
        return ret;
    }

    public final List<Token> timeEveryday(String text, LocalDateTime reference) {
        List<Token> ret = new ArrayList<>();
        List<ExtractResult> ers = this.config.getTimeExtractor().extract(text, reference);
        for (ExtractResult er : ers) {
            String afterStr = text.substring(er.getStart() + er.getLength());
            if (StringUtility.isNullOrEmpty(afterStr) && this.config.getBeforeEachDayRegex() != null) {
                String beforeStr = text.substring(0, er.getStart());
                Pattern beforeEachDayRegex = this.config.getBeforeEachDayRegex();
                Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(beforeEachDayRegex, beforeStr)).findFirst();
                if (match.isPresent()) {
                    ret.add(new Token(match.get().index, er.getStart() + er.getLength()));
                }
            } else {
                Pattern eachDayRegex = this.config.getEachDayRegex();
                Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(eachDayRegex, afterStr)).findFirst();
                if (match.isPresent()) {
                    ret.add(new Token(er.getStart(), er.getStart() + er.getLength() + match.get().length));
                }
            }
        }

        return ret;
    }

    public final List<Token> matchEach(IDateTimeExtractor extractor, String input, LocalDateTime reference) {
        StringBuilder sb = new StringBuilder(input);
        List<Token> ret = new ArrayList<>();
        Pattern setEachRegex = this.config.getSetEachRegex();
        Match[] matches = RegExpUtility.getMatches(setEachRegex, input);
        for (Match match : matches) {
            if (match != null) {
                String trimedText = sb.delete(match.index, match.index + match.length).toString();
                List<ExtractResult> ers = extractor.extract(trimedText, reference);
                for (ExtractResult er : ers) {
                    if (er.getStart() <= match.index && (er.getStart() + er.getLength()) > match.index) {
                        ret.add(new Token(er.getStart(), er.getStart() + er.getLength() + match.length));
                    }
                }
            }
        }

        // handle "Mondays"
        Pattern setWeekDayRegex = this.config.getSetWeekDayRegex();
        matches = RegExpUtility.getMatches(setWeekDayRegex, input);
        for (Match match : matches) {
            if (match != null) {
                sb = new StringBuilder(input);
                sb.delete(match.index, match.index + match.length);
                String trimedText = sb.insert(match.index, match.getGroup("weekday").value).toString();

                List<ExtractResult> ers = extractor.extract(trimedText, reference);
                for (ExtractResult er : ers) {
                    if (er.getStart() <= match.index && er.getText().contains(match.getGroup("weekday").value)) {
                        int len = er.getLength() + 1;
                        if (match.getGroup(Constants.PrefixGroupName).value != "") {
                            len += match.getGroup(Constants.PrefixGroupName).value.length();
                        }
                        ret.add(new Token(er.getStart(), er.getStart() + len));
                    }
                }
            }
        }

        return ret;
    }
}
