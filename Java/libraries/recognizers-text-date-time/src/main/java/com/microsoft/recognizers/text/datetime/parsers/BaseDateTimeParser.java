package com.microsoft.recognizers.text.datetime.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.TimeTypeConstants;
import com.microsoft.recognizers.text.datetime.extractors.config.ResultTimex;
import com.microsoft.recognizers.text.datetime.parsers.config.IDateTimeParserConfiguration;
import com.microsoft.recognizers.text.datetime.utilities.AgoLaterUtil;
import com.microsoft.recognizers.text.datetime.utilities.ConditionalMatch;
import com.microsoft.recognizers.text.datetime.utilities.DateTimeFormatUtil;
import com.microsoft.recognizers.text.datetime.utilities.DateTimeResolutionResult;
import com.microsoft.recognizers.text.datetime.utilities.DateUtil;
import com.microsoft.recognizers.text.datetime.utilities.RegexExtension;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import com.microsoft.recognizers.text.utilities.StringUtility;

import java.time.LocalDateTime;
import java.time.LocalTime;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.Map;
import java.util.Optional;

public class BaseDateTimeParser implements IDateTimeParser {

    private final IDateTimeParserConfiguration config;

    public BaseDateTimeParser(IDateTimeParserConfiguration config) {
        this.config = config;
    }

    @Override
    public String getParserName() {
        return Constants.SYS_DATETIME_DATETIME;
    }

    @Override
    public ParseResult parse(ExtractResult extractResult) {
        return this.parse(extractResult, LocalDateTime.now());
    }

    @Override
    public DateTimeParseResult parse(ExtractResult er, LocalDateTime reference) {

        LocalDateTime referenceDate = reference;

        Object value = null;

        if (er.getType().equals(getParserName())) {
            DateTimeResolutionResult innerResult = this.mergeDateAndTime(er.getText(), referenceDate);

            if (!innerResult.getSuccess()) {
                innerResult = this.parseBasicRegex(er.getText(), referenceDate);
            }

            if (!innerResult.getSuccess()) {
                innerResult = this.parseTimeOfToday(er.getText(), referenceDate);
            }

            if (!innerResult.getSuccess()) {
                innerResult = this.parseSpecialTimeOfDate(er.getText(), referenceDate);
            }

            if (!innerResult.getSuccess()) {
                innerResult = this.parserDurationWithAgoAndLater(er.getText(), referenceDate);
            }

            if (innerResult.getSuccess()) {
                Map<String, String> futureResolution = ImmutableMap.<String, String>builder()
                        .put(TimeTypeConstants.DATETIME,
                                DateTimeFormatUtil.formatDateTime((LocalDateTime)innerResult.getFutureValue()))
                        .build();
                innerResult.setFutureResolution(futureResolution);

                Map<String, String> pastResolution = ImmutableMap.<String, String>builder()
                        .put(TimeTypeConstants.DATETIME,
                                DateTimeFormatUtil.formatDateTime((LocalDateTime)innerResult.getPastValue()))
                        .build();
                innerResult.setPastResolution(pastResolution);

                value = innerResult;
            }
        }

        DateTimeParseResult ret = new DateTimeParseResult(er.getStart(), er.getLength(), er.getText(), er.getType(), er.getData(), value, "",
                value == null ? "" : ((DateTimeResolutionResult)value).getTimex());

        return ret;
    }

    @Override
    public List<DateTimeParseResult> filterResults(String query, List<DateTimeParseResult> candidateResults) {
        throw new UnsupportedOperationException();
    }

    // Merge a Date entity and a Time entity
    private DateTimeResolutionResult mergeDateAndTime(String text, LocalDateTime reference) {
        DateTimeResolutionResult result = new DateTimeResolutionResult();
        List<ExtractResult> ersDate = config.getDateExtractor().extract(text, reference);
        if (ersDate.isEmpty()) {
            ersDate = config.getDateExtractor().extract(config.getTokenBeforeDate() + text, reference);
            if (ersDate.size() == 1) {
                int newStart = ersDate.get(0).getStart() - config.getTokenBeforeDate().length();
                ersDate.get(0).setStart(newStart);
                ersDate.set(0, ersDate.get(0));
            } else {
                return result;
            }
        } else {
            // This is to understand if there is an ambiguous token in the text. For some
            // languages (e.g. spanish),
            // the same word could mean different things (e.g a time in the day or an
            // specific day).
            if (config.containsAmbiguousToken(text, ersDate.get(0).getText())) {
                return result;
            }
        }

        List<ExtractResult> ersTime = config.getTimeExtractor().extract(text, reference);
        if (ersTime.isEmpty()) {
            // Here we filter out "morning, afternoon, night..." time entities
            ersTime = config.getTimeExtractor().extract(config.getTokenBeforeTime() + text, reference);
            if (ersTime.size() == 1) {
                int newStart = ersTime.get(0).getStart() - config.getTokenBeforeTime().length();
                ersTime.get(0).setStart(newStart);
                ersTime.set(0, ersTime.get(0));
            } else if (ersTime.isEmpty()) {
                // check whether there is a number being used as a time point
                boolean hasTimeNumber = false;
                List<ExtractResult> numErs = config.getIntegerExtractor().extract(text);
                if (!numErs.isEmpty() && ersDate.size() == 1) {
                    for (ExtractResult num : numErs) {
                        int middleBegin = ersDate.get(0).getStart() + ersDate.get(0).getLength();
                        int middleEnd = num.getStart();
                        if (middleBegin > middleEnd) {
                            continue;
                        }

                        String middleStr = text.substring(middleBegin, middleEnd).trim().toLowerCase();
                        Optional<Match> match = Arrays
                                .stream(RegExpUtility.getMatches(config.getDateNumberConnectorRegex(), middleStr))
                                .findFirst();
                        if (StringUtility.isNullOrEmpty(middleStr) || match.isPresent()) {
                            num.setType(Constants.SYS_DATETIME_TIME);
                            ersTime.add(num);
                            hasTimeNumber = true;
                        }
                    }
                }

                if (!hasTimeNumber) {
                    return result;
                }
            }
        }

        // Handle cases like "Oct. 5 in the afternoon at 7:00";
        // in this case "5 in the afternoon" will be extracted as a Time entity
        int correctTimeIdx = 0;
        while (correctTimeIdx < ersTime.size() && ersTime.get(correctTimeIdx).isOverlap(ersDate.get(0))) {
            correctTimeIdx++;
        }

        if (correctTimeIdx >= ersTime.size()) {
            return result;
        }

        DateTimeParseResult prDate = config.getDateParser().parse(ersDate.get(0), reference);
        DateTimeParseResult prTime = config.getTimeParser().parse(ersTime.get(correctTimeIdx), reference);

        if (prDate.getValue() == null || prTime.getValue() == null) {
            return result;
        }

        LocalDateTime futureDate = (LocalDateTime)((DateTimeResolutionResult)prDate.getValue()).getFutureValue();
        LocalDateTime pastDate = (LocalDateTime)((DateTimeResolutionResult)prDate.getValue()).getPastValue();
        LocalDateTime time = (LocalDateTime)((DateTimeResolutionResult)prTime.getValue()).getPastValue();

        int hour = time.getHour();
        int min = time.getMinute();
        int sec = time.getSecond();

        // Handle morning, afternoon
        if (RegExpUtility.getMatches(config.getPMTimeRegex(), text).length != 0 && withinAfternoonHours(hour)) {
            hour += Constants.HalfDayHourCount;
        } else if (RegExpUtility.getMatches(config.getAMTimeRegex(), text).length != 0 &&
                withinMorningHoursAndNoon(hour, min, sec)) {
            hour -= Constants.HalfDayHourCount;
        }

        String timeStr = prTime.getTimexStr();
        if (timeStr.endsWith(Constants.Comment_AmPm)) {
            timeStr = timeStr.substring(0, timeStr.length() - 4);
        }

        timeStr = String.format("T%02d%s", hour, timeStr.substring(3));
        result.setTimex(prDate.getTimexStr() + timeStr);
        DateTimeResolutionResult val = (DateTimeResolutionResult)prTime.getValue();
        if (hour <= Constants.HalfDayHourCount && RegExpUtility.getMatches(config.getPMTimeRegex(), text).length == 0 &&
                RegExpUtility.getMatches(config.getAMTimeRegex(), text).length == 0 &&
                !StringUtility.isNullOrEmpty(val.getComment())) {
            result.setComment(Constants.Comment_AmPm);
        }

        result.setFutureValue(DateUtil.safeCreateFromMinValue(futureDate.getYear(), futureDate.getMonthValue(),
                futureDate.getDayOfMonth(), hour, min, sec));
        result.setPastValue(DateUtil.safeCreateFromMinValue(pastDate.getYear(), pastDate.getMonthValue(),
                pastDate.getDayOfMonth(), hour, min, sec));

        result.setSuccess(true);

        // Change the value of time object
        prTime.setTimexStr(timeStr);
        if (!StringUtility.isNullOrEmpty(result.getComment())) {
            DateTimeResolutionResult newValue = (DateTimeResolutionResult)prTime.getValue();
            newValue.setComment(result.getComment().equals(Constants.Comment_AmPm) ? Constants.Comment_AmPm : "");
            prTime.setValue(newValue);
            prTime.setTimexStr(timeStr);
        }

        // Add the date and time object in case we want to split them
        List<Object> entities = new ArrayList<>();
        entities.add(prDate);
        entities.add(prTime);
        result.setSubDateTimeEntities(entities);

        result.setTimeZoneResolution(((DateTimeResolutionResult)prTime.getValue()).getTimeZoneResolution());

        return result;
    }

    private DateTimeResolutionResult parseBasicRegex(String text, LocalDateTime reference) {
        DateTimeResolutionResult result = new DateTimeResolutionResult();
        String trimmedText = text.trim().toLowerCase();

        // Handle "now"
        if (RegexExtension.isExactMatch(config.getNowRegex(), trimmedText, true)) {
            ResultTimex timexResult = config.getMatchedNowTimex(trimmedText);
            result.setTimex(timexResult.getTimex());
            result.setFutureValue(reference);
            result.setPastValue(reference);
            result.setSuccess(true);
        }

        return result;
    }

    private DateTimeResolutionResult parseTimeOfToday(String text, LocalDateTime reference) {
        DateTimeResolutionResult result = new DateTimeResolutionResult();
        String trimmedText = text.trim().toLowerCase();

        int hour = 0;
        int minute = 0;
        int second = 0;
        String timeStr;

        ConditionalMatch wholeMatch = RegexExtension.matchExact(config.getSimpleTimeOfTodayAfterRegex(), trimmedText, true);
        if (!wholeMatch.getSuccess()) {
            wholeMatch = RegexExtension.matchExact(config.getSimpleTimeOfTodayBeforeRegex(), trimmedText, true);
        }

        if (wholeMatch.getSuccess()) {
            String hourStr = wholeMatch.getMatch().get().getGroup(Constants.HourGroupName).value;
            if (StringUtility.isNullOrEmpty(hourStr)) {
                hourStr = wholeMatch.getMatch().get().getGroup("hournum").value.toLowerCase();
                hour = config.getNumbers().get(hourStr);
            } else {
                hour = Integer.parseInt(hourStr);
            }

            timeStr = String.format("T%02d", hour);
        } else {
            List<ExtractResult> ers = config.getTimeExtractor().extract(trimmedText, reference);
            if (ers.size() != 1) {
                ers = config.getTimeExtractor().extract(config.getTokenBeforeTime() + trimmedText, reference);
                if (ers.size() == 1) {
                    int newStart = ers.get(0).getStart() - config.getTokenBeforeTime().length();
                    ers.get(0).setStart(newStart);
                    ers.set(0, ers.get(0));
                } else {
                    return result;
                }
            }

            DateTimeParseResult pr = config.getTimeParser().parse(ers.get(0), reference);
            if (pr.getValue() == null) {
                return result;
            }

            LocalDateTime time = (LocalDateTime)((DateTimeResolutionResult)pr.getValue()).getFutureValue();
            hour = time.getHour();
            minute = time.getMinute();
            second = time.getSecond();
            timeStr = pr.getTimexStr();
        }

        Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(config.getSpecificTimeOfDayRegex(), trimmedText))
                .findFirst();

        if (match.isPresent()) {
            String matchStr = match.get().value.toLowerCase();

            // Handle "last", "next"
            int swift = config.getSwiftDay(matchStr);
            LocalDateTime date = reference.plusDays(swift);

            // Handle "morning", "afternoon"
            hour = config.getHour(matchStr, hour);

            // In this situation, timeStr cannot end up with "ampm", because we always have
            // a "morning" or "night"
            if (timeStr.endsWith(Constants.Comment_AmPm)) {
                timeStr = timeStr.substring(0, timeStr.length() - 4);
            }

            timeStr = String.format("T%02d%s", hour, timeStr.substring(3));

            result.setTimex(DateTimeFormatUtil.formatDate(date) + timeStr);
            LocalDateTime dateResult = DateUtil.safeCreateFromMinValue(date.getYear(), date.getMonthValue(),
                    date.getDayOfMonth(), hour, minute, second);

            result.setFutureValue(dateResult);
            result.setPastValue(dateResult);
            result.setSuccess(true);
        }

        return result;
    }

    private DateTimeResolutionResult parseSpecialTimeOfDate(String text, LocalDateTime reference) {
        DateTimeResolutionResult result = parseUnspecificTimeOfDate(text, reference);

        if (result.getSuccess()) {
            return result;
        }

        List<ExtractResult> ers = config.getDateExtractor().extract(text, reference);
        if (ers.size() != 1) {
            return result;
        }

        String beforeStr = text.substring(0, ers.get(0).getStart());
        if (RegExpUtility.getMatches(config.getSpecificEndOfRegex(), beforeStr).length != 0) {
            DateTimeParseResult pr = config.getDateParser().parse(ers.get(0), reference);
            LocalDateTime futureDate = (LocalDateTime)((DateTimeResolutionResult)pr.getValue()).getFutureValue();
            LocalDateTime pastDate = (LocalDateTime)((DateTimeResolutionResult)pr.getValue()).getPastValue();

            result = resolveEndOfDay(pr.getTimexStr(), futureDate, pastDate);
        }

        return result;
    }

    private DateTimeResolutionResult parseUnspecificTimeOfDate(String text, LocalDateTime reference) {
        // Handle 'eod', 'end of day'
        DateTimeResolutionResult result = new DateTimeResolutionResult();

        Optional<Match> eod = Arrays.stream(RegExpUtility.getMatches(config.getUnspecificEndOfRegex(), text)).findFirst();

        if (eod.isPresent()) {
            result = resolveEndOfDay(DateTimeFormatUtil.formatDate(reference), reference, reference);
        }

        return result;
    }

    private DateTimeResolutionResult resolveEndOfDay(String timexPrefix, LocalDateTime futureDate, LocalDateTime pastDate) {
        String timex = String.format("%sT23:59:59", timexPrefix);
        LocalDateTime futureValue = LocalDateTime.of(futureDate.toLocalDate(), LocalTime.MIDNIGHT).plusDays(1).minusSeconds(1);
        LocalDateTime pastValue = LocalDateTime.of(pastDate.toLocalDate(), LocalTime.MIDNIGHT).plusDays(1).minusSeconds(1);

        DateTimeResolutionResult result = new DateTimeResolutionResult();
        result.setTimex(timex);
        result.setFutureValue(futureValue);
        result.setPastValue(pastValue);
        result.setSuccess(true);

        return result;
    }

    private boolean withinAfternoonHours(int hour) {
        return hour < Constants.HalfDayHourCount;
    }

    private boolean withinMorningHoursAndNoon(int hour, int min, int sec) {
        return (hour > Constants.HalfDayHourCount || (hour == Constants.HalfDayHourCount && (min > 0 || sec > 0)));
    }

    private DateTimeResolutionResult parserDurationWithAgoAndLater(String text, LocalDateTime reference) {
        return AgoLaterUtil.parseDurationWithAgoAndLater(text, reference, config.getDurationExtractor(),
                config.getDurationParser(), config.getUnitMap(), config.getUnitRegex(),
                config.getUtilityConfiguration(), config::getSwiftDay);
    }
}
