package com.microsoft.recognizers.text.datetime.parsers;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.TimeTypeConstants;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.parsers.config.ISetParserConfiguration;
import com.microsoft.recognizers.text.datetime.utilities.ConditionalMatch;
import com.microsoft.recognizers.text.datetime.utilities.DateTimeResolutionResult;
import com.microsoft.recognizers.text.datetime.utilities.MatchedTimexResult;
import com.microsoft.recognizers.text.datetime.utilities.RegexExtension;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import com.microsoft.recognizers.text.utilities.StringUtility;

import java.time.LocalDateTime;
import java.util.Arrays;
import java.util.HashMap;
import java.util.List;
import java.util.Optional;
import java.util.regex.Matcher;

public class BaseSetParser implements IDateTimeParser {
    @Override
    public String getParserName() {
        return Constants.SYS_DATETIME_SET;
    }

    private ISetParserConfiguration config;

    public BaseSetParser(ISetParserConfiguration configuration) {
        this.config = configuration;
    }

    @Override
    public DateTimeParseResult parse(ExtractResult er, LocalDateTime reference) {
        DateTimeResolutionResult value = null;

        if (er.getType().equals(getParserName())) {

            DateTimeResolutionResult innerResult = parseEachUnit(er.getText());
            if (!innerResult.getSuccess()) {
                innerResult = parseEachDuration(er.getText(), reference);
            }

            if (!innerResult.getSuccess()) {
                innerResult = parserTimeEveryday(er.getText(), reference);
            }

            // NOTE: Please do not change the order of following function
            // datetimeperiod>dateperiod>timeperiod>datetime>date>time
            if (!innerResult.getSuccess()) {
                innerResult = parseEach(config.getDateTimePeriodExtractor(), config.getDateTimePeriodParser(), er.getText(), reference);
            }

            if (!innerResult.getSuccess()) {
                innerResult = parseEach(config.getDatePeriodExtractor(), config.getDatePeriodParser(), er.getText(), reference);
            }

            if (!innerResult.getSuccess()) {
                innerResult = parseEach(config.getTimePeriodExtractor(), config.getTimePeriodParser(), er.getText(), reference);
            }

            if (!innerResult.getSuccess()) {
                innerResult = parseEach(config.getDateTimeExtractor(), config.getDateTimeParser(), er.getText(), reference);
            }

            if (!innerResult.getSuccess()) {
                innerResult = parseEach(config.getDateExtractor(), config.getDateParser(), er.getText(), reference);
            }

            if (!innerResult.getSuccess()) {
                innerResult = parseEach(config.getTimeExtractor(), config.getTimeParser(), er.getText(), reference);
            }

            if (innerResult.getSuccess()) {
                HashMap<String, String> futureMap = new HashMap<>();
                futureMap.put(TimeTypeConstants.SET, innerResult.getFutureValue().toString());
                innerResult.setFutureResolution(futureMap);

                HashMap<String, String> pastMap = new HashMap<>();
                pastMap.put(TimeTypeConstants.SET, innerResult.getPastValue().toString());
                innerResult.setPastResolution(pastMap);

                value = innerResult;
            }
        }

        DateTimeParseResult ret = new DateTimeParseResult(
                er.getStart(),
                er.getLength(),
                er.getText(),
                er.getType(),
                er.getData(),
                value,
                "",
                value == null ? "" : value.getTimex()
        );

        return ret;
    }

    @Override
    public ParseResult parse(ExtractResult extractResult) {
        return this.parse(extractResult, LocalDateTime.now());
    }

    @Override
    public List<DateTimeParseResult> filterResults(String query, List<DateTimeParseResult> candidateResults) {
        throw new UnsupportedOperationException();
    }

    private DateTimeResolutionResult parseEachDuration(String text, LocalDateTime refDate) {
        DateTimeResolutionResult ret = new DateTimeResolutionResult();
        List<ExtractResult> ers = this.config.getDurationExtractor().extract(text, refDate);
        if (ers.size() != 1 || !StringUtility.isNullOrWhiteSpace(text.substring(ers.get(0).getStart() + ers.get(0).getLength()))) {
            return ret;
        }

        String beforeStr = text.substring(0, ers.get(0).getStart());
        Matcher regexMatch = this.config.getEachPrefixRegex().matcher(beforeStr);
        if (regexMatch.find()) {
            DateTimeParseResult pr = this.config.getDurationParser().parse(ers.get(0), LocalDateTime.now());
            ret.setTimex(pr.getTimexStr());
            ret.setFutureValue("Set: " + pr.getTimexStr());
            ret.setPastValue("Set: " + pr.getTimexStr());
            ret.setSuccess(true);
            return ret;
        }

        return ret;
    }

    private DateTimeResolutionResult parseEachUnit(String text) {
        DateTimeResolutionResult ret = new DateTimeResolutionResult();
        // handle "daily", "weekly"
        Optional<Match> matched = Arrays.stream(RegExpUtility.getMatches(this.config.getPeriodicRegex(), text)).findFirst();
        if (matched.isPresent()) {

            MatchedTimexResult result = this.config.getMatchedDailyTimex(text);
            if (!result.getResult()) {
                return ret;
            }

            ret.setTimex(result.getTimex());
            ret.setFutureValue("Set: " + ret.getTimex());
            ret.setPastValue("Set: " + ret.getTimex());
            ret.setSuccess(true);

            return ret;
        }

        // Handle "each month"
        ConditionalMatch exactMatch = RegexExtension.matchExact(this.config.getEachUnitRegex(), text, true);
        if (exactMatch.getSuccess()) {

            String sourceUnit = exactMatch.getMatch().get().getGroup("unit").value;
            if (!StringUtility.isNullOrEmpty(sourceUnit) && this.config.getUnitMap().containsKey(sourceUnit)) {

                MatchedTimexResult result = this.config.getMatchedUnitTimex(sourceUnit);
                if (!result.getResult()) {
                    return ret;
                }

                // Handle "every other month"
                Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(this.config.getEachUnitRegex(), text)).findFirst();

                if (exactMatch.getMatch().get().getGroup("other").value != "") {
                    result.setTimex(result.getTimex().replace("1", "2"));
                }

                ret.setTimex(result.getTimex());
                ret.setFutureValue("Set: " + ret.getTimex());
                ret.setPastValue("Set: " + ret.getTimex());
                ret.setSuccess(true);

                return ret;
            }
        }

        return ret;
    }

    private DateTimeResolutionResult parserTimeEveryday(String text, LocalDateTime refDate) {
        DateTimeResolutionResult ret = new DateTimeResolutionResult();
        List<ExtractResult> ers = this.config.getTimeExtractor().extract(text, refDate);
        if (ers.size() != 1) {
            return ret;
        }

        String afterStr = text.replace(ers.get(0).getText(), "");
        Matcher match = this.config.getEachDayRegex().matcher(afterStr);
        if (match.find()) {
            DateTimeParseResult pr = this.config.getTimeParser().parse(ers.get(0), LocalDateTime.now());
            ret.setTimex(pr.getTimexStr());
            ret.setFutureValue("Set: " + ret.getTimex());
            ret.setPastValue("Set: " + ret.getTimex());
            ret.setSuccess(true);

            return ret;
        }

        return ret;
    }

    private DateTimeResolutionResult parseEach(IDateTimeExtractor extractor, IDateTimeParser parser, String text, LocalDateTime refDate) {
        DateTimeResolutionResult ret = new DateTimeResolutionResult();
        List<ExtractResult> ers = null;

        // remove key words of set type from text
        boolean success = false;
        Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(this.config.getSetEachRegex(), text)).findFirst();
        if (match.isPresent()) {

            StringBuilder sb = new StringBuilder(text);
            String trimmedText = sb.delete(match.get().index, match.get().index + match.get().length).toString();
            ers = extractor.extract(trimmedText, refDate);
            if (ers.size() == 1 && ers.get(0).getLength() == trimmedText.length()) {

                success = true;
            }
        }

        // remove suffix 's' and "on" if existed and re-try
        match = Arrays.stream(RegExpUtility.getMatches(this.config.getSetWeekDayRegex(), text)).findFirst();
        if (match.isPresent()) {

            StringBuilder sb = new StringBuilder(text);
            String trimmedText = sb.delete(match.get().index, match.get().index + match.get().length).toString();
            trimmedText = new StringBuilder(trimmedText).insert(match.get().index, match.get().getGroup("weekday").value).toString();
            ers = extractor.extract(trimmedText, refDate);
            if (ers.size() == 1 && ers.get(0).getLength() == trimmedText.length()) {

                success = true;
            }
        }

        if (success) {
            DateTimeParseResult pr = parser.parse(ers.get(0), refDate);
            ret.setTimex(pr.getTimexStr());
            ret.setFutureValue("Set: " + ret.getTimex());
            ret.setPastValue("Set: " + ret.getTimex());
            ret.setSuccess(true);

            return ret;
        }

        return ret;
    }
}
