package com.microsoft.recognizers.text.datetime.extractors;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.extractors.config.ITimePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.config.ResultIndex;
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

public class BaseTimePeriodExtractor implements IDateTimeExtractor {

    private final ITimePeriodExtractorConfiguration config;

    @Override
    public String getExtractorName() {
        return Constants.SYS_DATETIME_TIMEPERIOD;
    }

    public BaseTimePeriodExtractor(ITimePeriodExtractorConfiguration config) {
        this.config = config;
    }

    @Override
    public List<ExtractResult> extract(String input, LocalDateTime reference) {

        List<Token> tokens = new ArrayList<>();
        tokens.addAll(matchSimpleCases(input));
        tokens.addAll(mergeTwoTimePoints(input, reference));
        tokens.addAll(matchTimeOfDay(input));

        return Token.mergeAllTokens(tokens, input, getExtractorName());
    }

    @Override
    public List<ExtractResult> extract(String input) {
        return this.extract(input, LocalDateTime.now());
    }

    // Cases like "from 3 to 5am" or "between 3:30 and 5" are extracted here
    // Note that cases like "from 3 to 5" will not be extracted here because no "am/pm" or "hh:mm" to infer it's a time period
    // Also cases like "from 3:30 to 4 people" should not be extracted as a time period
    private List<Token> matchSimpleCases(String input) {

        List<Token> ret = new ArrayList<>();

        for (Pattern regex : this.config.getSimpleCasesRegex()) {
            Match[] matches = RegExpUtility.getMatches(regex, input);

            for (Match match : matches) {
                // Cases like "from 10:30 to 11", don't necessarily need "am/pm"
                if (!match.getGroup(Constants.MinuteGroupName).value.equals("") || !match.getGroup(Constants.SecondGroupName).value.equals("")) {
                    // Cases like "from 3:30 to 4" should be supported
                    // Cases like "from 3:30 to 4 on 1/1/2015" should be supported
                    // Cases like "from 3:30 to 4 people" is considered not valid
                    Boolean endWithValidToken = false;

                    // "No extra tokens after the time period"
                    if (match.index + match.length == input.length()) {
                        endWithValidToken = true;
                    } else {
                        String afterStr = input.substring(match.index + match.length);

                        // "End with general ending tokens or "TokenBeforeDate" (like "on")
                        Pattern generalEndingRegex = this.config.getGeneralEndingRegex();
                        Optional<Match> endingMatch = Arrays.stream(RegExpUtility.getMatches(generalEndingRegex, afterStr)).findFirst();
                        if (endingMatch.isPresent() || afterStr.trim().startsWith(this.config.getTokenBeforeDate())) {
                            endWithValidToken = true;
                        }
                    }

                    if (endWithValidToken) {
                        ret.add(new Token(match.index, match.index + match.length));
                    }
                } else {
                    // Is there Constants.PmGroupName or Constants.AmGroupName ?
                    String pmStr = match.getGroup(Constants.PmGroupName).value;
                    String amStr = match.getGroup(Constants.AmGroupName).value;
                    String descStr = match.getGroup(Constants.DescGroupName).value;

                    // Check Constants.PmGroupName, Constants.AmGroupName
                    if (!StringUtility.isNullOrEmpty(pmStr) || !StringUtility.isNullOrEmpty(amStr) || !StringUtility.isNullOrEmpty(descStr)) {
                        ret.add(new Token(match.index, match.index + match.length));
                    }
                }
            }
        }

        return ret;
    }

    private List<Token> mergeTwoTimePoints(String input, LocalDateTime reference) {

        List<Token> ret = new ArrayList<>();
        List<ExtractResult> ers = this.config.getSingleTimeExtractor().extract(input, reference);

        // Handling ending number as a time point.
        List<ExtractResult> numErs = this.config.getIntegerExtractor().extract(input);

        // Check if it is an ending number
        if (numErs.size() > 0) {
            List<ExtractResult> timeNumbers = new ArrayList<>();

            // check if it is a ending number
            boolean endingNumber = false;
            ExtractResult num = numErs.get(numErs.size() - 1);
            if (num.start + num.length == input.length()) {
                endingNumber = true;
            } else {
                String afterStr = input.substring(num.start + num.length);
                Pattern generalEndingRegex = this.config.getGeneralEndingRegex();
                Optional<Match> endingMatch = Arrays.stream(RegExpUtility.getMatches(generalEndingRegex, input)).findFirst();
                if (endingMatch.isPresent()) {
                    endingNumber = true;
                }
            }
            if (endingNumber) {
                timeNumbers.add(num);
            }

            int i = 0;
            int j = 0;

            while (i < numErs.size()) {
                // find subsequent time point
                int numEndPoint = numErs.get(i).start + numErs.get(i).length;
                while (j < ers.size() && ers.get(j).start <= numEndPoint) {
                    j++;
                }

                if (j >= ers.size()) {
                    break;
                }

                // check connector string
                String midStr = input.substring(numEndPoint, ers.get(j).start);
                Pattern tillRegex = this.config.getTillRegex();
                Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(tillRegex, midStr)).findFirst();
                if (match.isPresent() && match.get().length == midStr.trim().length()) {
                    timeNumbers.add(numErs.get(i));
                }

                i++;
            }

            // check overlap
            for (ExtractResult timeNum : timeNumbers) {
                boolean overlap = false;
                for (ExtractResult er : ers) {
                    if (er.start <= timeNum.start && er.start + er.length >= timeNum.start) {
                        overlap = true;
                    }
                }

                if (!overlap) {
                    ers.add(timeNum);
                }
            }

            ers.sort((x, y) -> x.start - y.start);
        }

        int idx = 0;
        while (idx < ers.size() - 1) {
            int middleBegin = ers.get(idx).start + ers.get(idx).length;
            int middleEnd = ers.get(idx + 1).start;

            if (middleEnd - middleBegin <= 0) {
                idx++;
                continue;
            }

            String middleStr = input.substring(middleBegin, middleEnd).trim().toLowerCase(java.util.Locale.ROOT);
            Pattern tillRegex = this.config.getTillRegex();
            Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(tillRegex, middleStr)).findFirst();

            // Handle "{TimePoint} to {TimePoint}"
            if (match.isPresent() && match.get().index == 0 && match.get().length == middleStr.length()) {
                int periodBegin = ers.get(idx).start;
                int periodEnd = ers.get(idx + 1).start + ers.get(idx + 1).length;

                // Handle "from"
                String beforeStr = input.substring(0, periodBegin).trim().toLowerCase(java.util.Locale.ROOT);
                ResultIndex fromIndex = this.config.getFromTokenIndex(beforeStr);
                ResultIndex betweenIndex = this.config.getBetweenTokenIndex(beforeStr);
                if (fromIndex.result) {
                    // Handle "from"
                    periodBegin = fromIndex.index;
                } else if (betweenIndex.result) {
                    // Handle "between"
                    periodBegin = betweenIndex.index;
                }

                ret.add(new Token(periodBegin, periodEnd));
                idx += 2;
                continue;
            }

            // Handle "between {TimePoint} and {TimePoint}"
            if (this.config.hasConnectorToken(middleStr)) {
                int periodBegin = ers.get(idx).start;
                int periodEnd = ers.get(idx + 1).start + ers.get(idx + 1).length;

                // Handle "between"
                String beforeStr = input.substring(0, periodBegin).trim().toLowerCase(java.util.Locale.ROOT);
                ResultIndex betweenIndex = this.config.getBetweenTokenIndex(beforeStr);
                if (betweenIndex.result) {
                    periodBegin = betweenIndex.index;
                    ret.add(new Token(periodBegin, periodEnd));
                    idx += 2;
                    continue;
                }
            }

            idx++;
        }

        return ret;
    }

    private List<Token> matchTimeOfDay(String input) {

        List<Token> ret = new ArrayList<>();
        Pattern timeOfDayRegex = this.config.getTimeOfDayRegex();
        Match[] matches = RegExpUtility.getMatches(timeOfDayRegex, input);
        for (Match match : matches) {
            ret.add(new Token(match.index, match.index + match.length));
        }

        return ret;
    }
}
