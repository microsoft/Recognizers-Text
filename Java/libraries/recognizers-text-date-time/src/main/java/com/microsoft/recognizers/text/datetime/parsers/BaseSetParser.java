package com.microsoft.recognizers.text.datetime.parsers;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.TimeTypeConstants;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.parsers.config.ISetParserConfiguration;
import com.microsoft.recognizers.text.datetime.utilities.DateTimeResolutionResult;
import com.microsoft.recognizers.text.datetime.utilities.MatchedTimexResult;
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

        if (er.type.equals(getParserName())) {

            DateTimeResolutionResult innerResult = parseEachUnit(er.text);
            if (!innerResult.getSuccess()) {
                innerResult = parseEachDuration(er.text, reference);
            }

            if (!innerResult.getSuccess()) {
                innerResult = parserTimeEveryday(er.text, reference);
            }

            // NOTE: Please do not change the order of following function
            // datetimeperiod>dateperiod>timeperiod>datetime>date>time
            if (!innerResult.getSuccess()) {
                innerResult = parseEach(config.getDateTimePeriodExtractor(), config.getDateTimePeriodParser(), er.text, reference);
            }

            if (!innerResult.getSuccess()) {
                innerResult = parseEach(config.getDatePeriodExtractor(), config.getDatePeriodParser(), er.text, reference);
            }

            if (!innerResult.getSuccess()) {
                innerResult = parseEach(config.getTimePeriodExtractor(), config.getTimePeriodParser(), er.text, reference);
            }

            if (!innerResult.getSuccess()) {
                innerResult = parseEach(config.getDateTimeExtractor(), config.getDateTimeParser(), er.text, reference);
            }

            if (!innerResult.getSuccess()) {
                innerResult = parseEach(config.getDateExtractor(), config.getDateParser(), er.text, reference);
            }

            if (!innerResult.getSuccess()) {
                innerResult = parseEach(config.getTimeExtractor(), config.getTimeParser(), er.text, reference);
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
                er.start,
                er.length,
                er.text,
                er.type,
                er.data,
                value,
                "",
                value.getTimex()
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
        if (ers.size() != 1 || !StringUtility.isNullOrWhiteSpace(text.substring(ers.get(0).start + ers.get(0).length))) {
            return ret;
        }

        String beforeStr = text.substring(0, ers.get(0).start);
        Matcher regexMatch = this.config.getEachPrefixRegex().matcher(beforeStr);
        if (regexMatch.find()) {
            DateTimeParseResult pr = this.config.getDurationParser().parse(ers.get(0), LocalDateTime.now());
            ret.setTimex(pr.timexStr);
            ret.setFutureValue("Set: " + pr.timexStr);
            ret.setPastValue("Set: " + pr.timexStr);
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
            if (!result.result) {
                return ret;
            }

            ret.setTimex(result.timex);
            ret.setFutureValue("Set: " + ret.getTimex());
            ret.setPastValue("Set: " + ret.getTimex());
            ret.setSuccess(true);

            return ret;
        }

        // Handle "each month"
        matched = Arrays.stream(RegExpUtility.getMatches(this.config.getEachUnitRegex(), text)).findFirst();
        if (matched.isPresent() && matched.get().length == text.length()) {

            String sourceUnit = matched.get().getGroup("unit").value;
            if (!StringUtility.isNullOrEmpty(sourceUnit) && this.config.getUnitMap().containsKey(sourceUnit)) {

                MatchedTimexResult result = this.config.getMatchedUnitTimex(sourceUnit);
                if (!result.result) {
                    return ret;
                }

                // Handle "every other month"
                Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(this.config.getEachUnitRegex(), text)).findFirst();

                if (match.get().getGroup("other").value != "") {
                    result = result.withTimex(result.timex.replace("1", "2"));
                }

                ret.setTimex(result.timex);
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

        String afterStr = text.replace(ers.get(0).text, "");
        Matcher match = this.config.getEachDayRegex().matcher(afterStr);
        if (match.find()) {
            DateTimeParseResult pr = this.config.getTimeParser().parse(ers.get(0), LocalDateTime.now());
            ret.setTimex(pr.timexStr);
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
            if (ers.size() == 1 && ers.get(0).length == trimmedText.length()) {

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
            if (ers.size() == 1 && ers.get(0).length == trimmedText.length()) {

                success = true;
            }
        }

        if (success) {
            DateTimeParseResult pr = parser.parse(ers.get(0), refDate);
            ret.setTimex(pr.timexStr);
            ret.setFutureValue("Set: " + ret.getTimex());
            ret.setPastValue("Set: " + ret.getTimex());
            ret.setSuccess(true);

            return ret;
        }

        return ret;
    }
}
