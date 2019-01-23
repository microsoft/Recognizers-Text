package com.microsoft.recognizers.text.datetime.extractors;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.extractors.config.ITimePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.config.ResultIndex;
import com.microsoft.recognizers.text.datetime.utilities.ConditionalMatch;
import com.microsoft.recognizers.text.datetime.utilities.RegexExtension;
import com.microsoft.recognizers.text.datetime.utilities.TimeZoneUtility;
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

        List<ExtractResult> timePeriodErs = Token.mergeAllTokens(tokens, input, getExtractorName());

        if (config.getOptions().match(DateTimeOptions.EnablePreview)) {
            timePeriodErs = TimeZoneUtility.mergeTimeZones(timePeriodErs, config.getTimeZoneExtractor().extract(input, reference), input);
        }

        return timePeriodErs;
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
                        boolean endWithGeneralEndings = Arrays.stream(RegExpUtility.getMatches(this.config.getGeneralEndingRegex(), afterStr))
                                .findFirst().isPresent();
                        boolean endWithAmPm = !match.getGroup(Constants.RightAmPmGroupName).value.equals("");
                        if (endWithGeneralEndings || endWithAmPm || afterStr.trim().startsWith(this.config.getTokenBeforeDate())) {
                            endWithValidToken = true;
                        } else if (this.config.getOptions().match(DateTimeOptions.EnablePreview)) {
                            endWithValidToken = startsWithTimeZone(afterStr);
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

                    // Check "pm", "am"
                    if (!StringUtility.isNullOrEmpty(pmStr) || !StringUtility.isNullOrEmpty(amStr) || !StringUtility.isNullOrEmpty(descStr)) {
                        ret.add(new Token(match.index, match.index + match.length));
                    } else {
                        String afterStr = input.substring(match.index + match.length);

                        if ((this.config.getOptions().match(DateTimeOptions.EnablePreview)) && startsWithTimeZone(afterStr)) {
                            ret.add(new Token(match.index, match.index + match.length));
                        }
                    }
                }
            }
        }

        return ret;
    }

    private boolean startsWithTimeZone(String afterText) {
        boolean startsWithTimeZone = false;

        List<ExtractResult> timeZoneErs = config.getTimeZoneExtractor().extract(afterText);
        Optional<ExtractResult> firstTimeZone = timeZoneErs.stream().sorted(Comparator.comparingInt(t -> t.getStart())).findFirst();

        if (firstTimeZone.isPresent()) {
            String beforeText = afterText.substring(0, firstTimeZone.get().getStart());

            if (StringUtility.isNullOrWhiteSpace(beforeText)) {
                startsWithTimeZone = true;
            }
        }

        return startsWithTimeZone;
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
            if (num.getStart() + num.getLength() == input.length()) {
                endingNumber = true;
            } else {
                String afterStr = input.substring(num.getStart() + num.getLength());
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
                int numEndPoint = numErs.get(i).getStart() + numErs.get(i).getLength();
                while (j < ers.size() && ers.get(j).getStart() <= numEndPoint) {
                    j++;
                }

                if (j >= ers.size()) {
                    break;
                }

                // check connector string
                String midStr = input.substring(numEndPoint, ers.get(j).getStart());
                Pattern tillRegex = this.config.getTillRegex();
                if (RegexExtension.isExactMatch(tillRegex, midStr, true) || config.hasConnectorToken(midStr.trim())) {
                    timeNumbers.add(numErs.get(i));
                }

                i++;
            }

            // check overlap
            for (ExtractResult timeNum : timeNumbers) {
                boolean overlap = false;
                for (ExtractResult er : ers) {
                    if (er.getStart() <= timeNum.getStart() && er.getStart() + er.getLength() >= timeNum.getStart()) {
                        overlap = true;
                    }
                }

                if (!overlap) {
                    ers.add(timeNum);
                }
            }

            ers.sort((x, y) -> x.getStart() - y.getStart());
        }

        int idx = 0;
        while (idx < ers.size() - 1) {
            int middleBegin = ers.get(idx).getStart() + ers.get(idx).getLength();
            int middleEnd = ers.get(idx + 1).getStart();

            if (middleEnd - middleBegin <= 0) {
                idx++;
                continue;
            }

            String middleStr = input.substring(middleBegin, middleEnd).trim().toLowerCase(java.util.Locale.ROOT);
            Pattern tillRegex = this.config.getTillRegex();

            // Handle "{TimePoint} to {TimePoint}"
            if (RegexExtension.isExactMatch(tillRegex, middleStr, true)) {
                int periodBegin = ers.get(idx).getStart();
                int periodEnd = ers.get(idx + 1).getStart() + ers.get(idx + 1).getLength();

                // Handle "from"
                String beforeStr = StringUtility.trimEnd(input.substring(0, periodBegin)).toLowerCase();
                ResultIndex fromIndex = this.config.getFromTokenIndex(beforeStr);
                ResultIndex betweenIndex = this.config.getBetweenTokenIndex(beforeStr);
                if (fromIndex.getResult()) {
                    // Handle "from"
                    periodBegin = fromIndex.getIndex();
                } else if (betweenIndex.getResult()) {
                    // Handle "between"
                    periodBegin = betweenIndex.getIndex();
                }

                ret.add(new Token(periodBegin, periodEnd));
                idx += 2;
                continue;
            }

            // Handle "between {TimePoint} and {TimePoint}"
            if (this.config.hasConnectorToken(middleStr)) {
                int periodBegin = ers.get(idx).getStart();
                int periodEnd = ers.get(idx + 1).getStart() + ers.get(idx + 1).getLength();

                // Handle "between"
                String beforeStr = input.substring(0, periodBegin).trim().toLowerCase(java.util.Locale.ROOT);
                ResultIndex betweenIndex = this.config.getBetweenTokenIndex(beforeStr);
                if (betweenIndex.getResult()) {
                    periodBegin = betweenIndex.getIndex();
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
