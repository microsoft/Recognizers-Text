package com.microsoft.recognizers.text.datetime.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.TimeTypeConstants;
import com.microsoft.recognizers.text.datetime.parsers.config.IDateTimePeriodParserConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.config.MatchedTimeRangeResult;
import com.microsoft.recognizers.text.datetime.utilities.DateTimeResolutionResult;
import com.microsoft.recognizers.text.datetime.utilities.DateUtil;
import com.microsoft.recognizers.text.datetime.utilities.FormatUtil;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.MatchGroup;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import com.microsoft.recognizers.text.utilities.StringUtility;

import java.time.LocalDateTime;
import java.time.temporal.ChronoUnit;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.Map;
import java.util.Optional;
import java.util.regex.Pattern;
import org.javatuples.Pair;

public class BaseDateTimePeriodParser implements IDateTimeParser {

    private final IDateTimePeriodParserConfiguration config;

    public BaseDateTimePeriodParser(IDateTimePeriodParserConfiguration config) {
        this.config = config;
    }

    @Override
    public String getParserName() {
        return Constants.SYS_DATETIME_DATETIMEPERIOD;
    }

    @Override
    public List<DateTimeParseResult> filterResults(String query, List<DateTimeParseResult> candidateResults) {
        throw new UnsupportedOperationException();
    }

    @Override
    public ParseResult parse(ExtractResult extractResult) {
        return this.parse(extractResult, LocalDateTime.now());
    }

    @Override
    public DateTimeParseResult parse(ExtractResult er, LocalDateTime reference) {
        
        LocalDateTime referenceDate = reference;

        Object value = null;

        if (er.type.equals(getParserName())) {
            DateTimeResolutionResult innerResult = this.mergeDateAndTimePeriods(er.text, referenceDate);

            if (!innerResult.getSuccess()) {
                innerResult = this.mergeTwoTimePoints(er.text, referenceDate);
            }

            if (!innerResult.getSuccess()) {
                innerResult = this.parseSpecificTimeOfDay(er.text, referenceDate);
            }

            if (!innerResult.getSuccess()) {
                innerResult = this.parseDuration(er.text, referenceDate);
            }

            if (!innerResult.getSuccess()) {
                innerResult = this.parseRelativeUnit(er.text, referenceDate);
            }

            if (!innerResult.getSuccess()) {
                innerResult = this.parseDateWithPeriodPrefix(er.text, referenceDate);
            }

            if (!innerResult.getSuccess()) {
                // Cases like "today after 2:00pm", "1/1/2015 before 2:00 in the afternoon"
                innerResult = this.parseDateWithTimePeriodSuffix(er.text, referenceDate);
            }

            if (innerResult.getSuccess()) {
                if (!isBeforeOrAfterMod(innerResult.getMod())) {
                    Map<String, String> futureResolution = ImmutableMap.<String, String>builder()
                        .put(TimeTypeConstants.START_DATETIME, FormatUtil.formatDateTime(((Pair<LocalDateTime, LocalDateTime>)innerResult.getFutureValue()).getValue0()))
                        .put(TimeTypeConstants.END_DATETIME, FormatUtil.formatDateTime(((Pair<LocalDateTime, LocalDateTime>)innerResult.getFutureValue()).getValue1()))
                        .build();
                    innerResult.setFutureResolution(futureResolution);

                    Map<String, String> pastResolution = ImmutableMap.<String, String>builder()
                        .put(TimeTypeConstants.START_DATETIME, FormatUtil.formatDateTime(((Pair<LocalDateTime, LocalDateTime>)innerResult.getPastValue()).getValue0()))
                        .put(TimeTypeConstants.END_DATETIME, FormatUtil.formatDateTime(((Pair<LocalDateTime, LocalDateTime>)innerResult.getPastValue()).getValue1()))
                        .build();
                    innerResult.setPastResolution(pastResolution);

                } else {
                    if (innerResult.getMod().equals(Constants.AFTER_MOD)) {
                        // Cases like "1/1/2015 after 2:00" there is no EndTime
                        Map<String, String> futureResolution = ImmutableMap.<String, String>builder()
                            .put(TimeTypeConstants.START_DATETIME, FormatUtil.formatDateTime((LocalDateTime)innerResult.getFutureValue()))
                            .build();
                        innerResult.setFutureResolution(futureResolution);

                        Map<String, String> pastResolution = ImmutableMap.<String, String>builder()
                            .put(TimeTypeConstants.START_DATETIME, FormatUtil.formatDateTime((LocalDateTime)innerResult.getPastValue()))
                            .build();
                        innerResult.setPastResolution(pastResolution);
                    } else {
                        // Cases like "1/1/2015 before 5:00 in the afternoon" there is no StartTime
                        Map<String, String> futureResolution = ImmutableMap.<String, String>builder()
                            .put(TimeTypeConstants.END_DATETIME, FormatUtil.formatDateTime((LocalDateTime)innerResult.getFutureValue()))
                            .build();
                        innerResult.setFutureResolution(futureResolution);

                        Map<String, String> pastResolution = ImmutableMap.<String, String>builder()
                            .put(TimeTypeConstants.END_DATETIME, FormatUtil.formatDateTime((LocalDateTime)innerResult.getPastValue()))
                            .build();
                        innerResult.setPastResolution(pastResolution);
                    }
                }

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
                value == null ? "" : ((DateTimeResolutionResult)value).getTimex());

        return ret;
    }

    private boolean isBeforeOrAfterMod(String mod) {
        return !StringUtility.isNullOrEmpty(mod) &&
            (mod == Constants.BEFORE_MOD || mod == Constants.AFTER_MOD);
    }

    private DateTimeResolutionResult mergeDateAndTimePeriods(String text, LocalDateTime referenceDate) {
        DateTimeResolutionResult result = new DateTimeResolutionResult();

        String trimmedText = text.trim().toLowerCase();

        List<ExtractResult> ers = config.getTimePeriodExtractor().extract(trimmedText, referenceDate);
        if (ers.size() != 1) {
            return parseSimpleCases(text, referenceDate);
        }

        ParseResult timePeriodParseResult = config.getTimePeriodParser().parse(ers.get(0));
        DateTimeResolutionResult timePeriodResolutionResult = (DateTimeResolutionResult)timePeriodParseResult.value;

        if (timePeriodResolutionResult == null) {
            return parseSimpleCases(text, referenceDate);
        }

        if (timePeriodResolutionResult.getTimeZoneResolution() != null) {
            result.setTimeZoneResolution(timePeriodResolutionResult.getTimeZoneResolution());
        }

        String timePeriodTimex = timePeriodResolutionResult.getTimex();

        // If it is a range type timex
        if (!StringUtility.isNullOrEmpty(timePeriodTimex) && timePeriodTimex.startsWith("(")) {
            List<ExtractResult> dateResult = config.getDateExtractor().extract(trimmedText.replace(ers.get(0).text, ""), referenceDate);
            String dateText = trimmedText.replace(ers.get(0).text, "").replace(config.getTokenBeforeDate(), "").trim();

            if (dateResult.size() == 1 && dateText.equals(dateResult.get(0).text)) {
                String dateStr;
                LocalDateTime futureTime;
                LocalDateTime pastTime;

                DateTimeParseResult pr = config.getDateParser().parse(dateResult.get(0), referenceDate);

                if (pr.value != null) {
                    futureTime = (LocalDateTime)((DateTimeResolutionResult)pr.value).getFutureValue();
                    pastTime = (LocalDateTime)((DateTimeResolutionResult)pr.value).getPastValue();

                    dateStr = pr.timexStr;
                } else {
                    return parseSimpleCases(text, referenceDate);
                }

                timePeriodTimex = timePeriodTimex.replace("(", "").replace(")", "");
                String[] timePeriodTimexArray = timePeriodTimex.split(",");
                Pair<LocalDateTime, LocalDateTime> timePeriodFutureValue = (Pair<LocalDateTime, LocalDateTime>)timePeriodResolutionResult.getFutureValue();
                LocalDateTime beginTime = timePeriodFutureValue.getValue0();
                LocalDateTime endTime = timePeriodFutureValue.getValue1();

                if (timePeriodTimexArray.length == 3) {
                    String beginStr = dateStr + timePeriodTimexArray[0];
                    String endStr = dateStr + timePeriodTimexArray[1];

                    result.setTimex(String.format("(%s,%s,%s)", beginStr, endStr, timePeriodTimexArray[2]));

                    result.setFutureValue(new Pair<LocalDateTime, LocalDateTime>(
                        DateUtil.safeCreateFromMinValue(futureTime.toLocalDate(), beginTime.toLocalTime()),
                        DateUtil.safeCreateFromMinValue(futureTime.toLocalDate(), endTime.toLocalTime())
                    ));

                    result.setPastValue(new Pair<LocalDateTime, LocalDateTime>(
                        DateUtil.safeCreateFromMinValue(pastTime.toLocalDate(), beginTime.toLocalTime()),
                        DateUtil.safeCreateFromMinValue(pastTime.toLocalDate(), endTime.toLocalTime())
                    ));

                    if (!StringUtility.isNullOrEmpty(timePeriodResolutionResult.getComment()) && timePeriodResolutionResult.getComment().equals(Constants.Comment_AmPm)) {
                        // AmPm comment is used for later SetParserResult to judge whether this parse result should have two parsing results
                        // Cases like "from 10:30 to 11 on 1/1/2015" should have AmPm comment, as it can be parsed to "10:30am to 11am" and also be parsed to "10:30pm to 11pm"
                        // Cases like "from 10:30 to 3 on 1/1/2015" should not have AmPm comment
                        if (beginTime.getHour() < Constants.HalfDayHourCount && endTime.getHour() < Constants.HalfDayHourCount) {
                            result.setComment(Constants.Comment_AmPm);
                        }
                    }

                    result.setSuccess(true);
                    List<Object> subDateTimeEntities = new ArrayList<>();
                    subDateTimeEntities.add(pr);
                    subDateTimeEntities.add(timePeriodParseResult);
                    result.setSubDateTimeEntities(subDateTimeEntities);

                    return result;
                }

            } else {
                return parseSimpleCases(text, referenceDate);
            }
        }

        return parseSimpleCases(text, referenceDate);
    }

    private DateTimeResolutionResult parseSimpleCases(String text, LocalDateTime referenceDate) {
        DateTimeResolutionResult result = new DateTimeResolutionResult();

        String trimmedText = text.trim().toLowerCase();

        Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(config.getPureNumberFromToRegex(), trimmedText)).findFirst();
        if (!match.isPresent()) {
            match = Arrays.stream(RegExpUtility.getMatches(config.getPureNumberBetweenAndRegex(), trimmedText)).findFirst();
        }

        if (match.isPresent() && (match.get().index == 0 || match.get().index + match.get().length == trimmedText.length())) {
            // This "from .. to .." pattern is valid if followed by a Date OR Constants.PmGroupName
            boolean hasAm = false;
            boolean hasPm = false;
            String dateStr;

            // Get hours
            MatchGroup hourGroup = match.get().getGroup(Constants.HourGroupName);
            String hourStr = hourGroup.captures[0].value;
            int beginHour;

            if (config.getNumbers().containsKey(hourStr)) {
                beginHour = config.getNumbers().get(hourStr);
            } else {
                beginHour = Integer.parseInt(hourStr);
            }

            hourStr = hourGroup.captures[1].value;
            int endHour;
            if (config.getNumbers().containsKey(hourStr)) {
                endHour = config.getNumbers().get(hourStr);
            } else {
                endHour = Integer.parseInt(hourStr);
            }

            // Parse following date
            List<ExtractResult> ers = config.getDateExtractor().extract(trimmedText.replace(match.get().value, ""), referenceDate);
            LocalDateTime futureTime;
            LocalDateTime pastTime;

            if (ers.size() > 0) {
                DateTimeParseResult pr = config.getDateParser().parse(ers.get(0), referenceDate);
                if (pr.value != null) {
                    DateTimeResolutionResult prValue = (DateTimeResolutionResult)pr.value;
                    futureTime = (LocalDateTime)prValue.getFutureValue();
                    pastTime = (LocalDateTime)prValue.getPastValue();

                    dateStr = pr.timexStr;

                    if (prValue.getTimeZoneResolution() != null) {
                        result.setTimeZoneResolution(prValue.getTimeZoneResolution());
                    }
                } else {
                    return result;
                }
            } else {
                return result;
            }

            // Parse Constants.PmGroupName
            String pmStr = match.get().getGroup(Constants.PmGroupName).value;
            String amStr = match.get().getGroup(Constants.AmGroupName).value;
            String descStr = match.get().getGroup(Constants.DescGroupName).value;

            if (!StringUtility.isNullOrEmpty(amStr) || !StringUtility.isNullOrEmpty(descStr) && descStr.startsWith("a")) {
                if (beginHour >= Constants.HalfDayHourCount) {
                    beginHour -= Constants.HalfDayHourCount;
                }

                if (endHour >= Constants.HalfDayHourCount) {
                    endHour -= Constants.HalfDayHourCount;
                }

                hasAm = true;
            } else if (!StringUtility.isNullOrEmpty(pmStr) || !StringUtility.isNullOrEmpty(descStr) && descStr.startsWith("p")) {
                if (beginHour < Constants.HalfDayHourCount) {
                    beginHour += Constants.HalfDayHourCount;
                }

                if (endHour < Constants.HalfDayHourCount) {
                    endHour += Constants.HalfDayHourCount;
                }

                hasAm = true;
            }

            if (!hasAm && !hasPm && beginHour <= Constants.HalfDayHourCount && endHour <= Constants.HalfDayHourCount) {
                if (beginHour > endHour) {
                    if (beginHour == Constants.HalfDayHourCount) {
                        beginHour = 0;
                    } else {
                        endHour += Constants.HalfDayHourCount;
                    }
                }

                result.setComment(Constants.Comment_AmPm);
            }
            
            int pastHours = endHour - beginHour;
            String beginStr = String.format("%sT%02d", dateStr, beginHour);
            String endStr = String.format("%sT%02d", dateStr, endHour);

            result.setTimex(String.format("(%s,%s,PT%dH)", beginStr, endStr, pastHours));

            
            result.setFutureValue(new Pair<LocalDateTime, LocalDateTime>(
                DateUtil.safeCreateFromMinValue(
                    futureTime.getYear(), futureTime.getMonthValue(), futureTime.getDayOfMonth(),
                    beginHour, 0, 0
                ),
                DateUtil.safeCreateFromMinValue(
                    futureTime.getYear(), futureTime.getMonthValue(), futureTime.getDayOfMonth(),
                    endHour, 0, 0
                )
            ));
            result.setPastValue(new Pair<LocalDateTime, LocalDateTime>(
                DateUtil.safeCreateFromMinValue(
                    pastTime.getYear(), pastTime.getMonthValue(), pastTime.getDayOfMonth(),
                    beginHour, 0, 0
                ),
                DateUtil.safeCreateFromMinValue(
                    pastTime.getYear(), pastTime.getMonthValue(), pastTime.getDayOfMonth(),
                    endHour, 0, 0
                )
            ));

            result.setSuccess(true);
        }

        return result;
    }

    private DateTimeResolutionResult mergeTwoTimePoints(String text, LocalDateTime referenceDate) {
        DateTimeResolutionResult result = new DateTimeResolutionResult();
        DateTimeParseResult pr1;
        DateTimeParseResult pr2;
        boolean bothHaveDates = false;
        boolean beginHasDate = false;
        boolean endHasDate = false;
        
        List<ExtractResult> er1 = config.getTimeExtractor().extract(text, referenceDate);
        List<ExtractResult> er2 = config.getDateTimeExtractor().extract(text, referenceDate);

        if (er2.size() == 2) {
            pr1 = config.getDateTimeParser().parse(er2.get(0), referenceDate);
            pr2 = config.getDateTimeParser().parse(er2.get(1), referenceDate);
            bothHaveDates = true;
        } else if (er2.size() == 1 && er1.size() == 2) {
            if (!er2.get(0).isOverlap(er1.get(0))) {
                pr1 = config.getTimeParser().parse(er1.get(0), referenceDate);
                pr2 = config.getDateTimeParser().parse(er2.get(0), referenceDate);
                endHasDate = true;
            } else {
                pr1 = config.getDateTimeParser().parse(er2.get(0), referenceDate);
                pr2 = config.getTimeParser().parse(er1.get(1), referenceDate);
                beginHasDate = true;
            }
        } else if (er2.size() == 1 && er1.size() == 1) {
            if (er1.get(0).start < er2.get(0).start) {
                pr1 = config.getTimeParser().parse(er1.get(0), referenceDate);
                pr2 = config.getDateTimeParser().parse(er2.get(0), referenceDate);
                endHasDate = true;
            } else {
                pr1 = config.getDateTimeParser().parse(er2.get(0), referenceDate);
                pr2 = config.getTimeParser().parse(er1.get(0), referenceDate);
                beginHasDate = true;
            }
        } else if (er1.size() == 2) {
            // If both ends are Time. then this is a TimePeriod, not a DateTimePeriod
            return result;
        } else {
            return result;
        }

        if (pr1.value == null || pr2.value == null) {
            return result;
        }

        LocalDateTime futureBegin = (LocalDateTime)((DateTimeResolutionResult)pr1.value).getFutureValue();
        LocalDateTime futureEnd = (LocalDateTime)((DateTimeResolutionResult)pr2.value).getFutureValue();

        LocalDateTime pastBegin = (LocalDateTime)((DateTimeResolutionResult)pr1.value).getPastValue();
        LocalDateTime pastEnd = (LocalDateTime)((DateTimeResolutionResult)pr2.value).getPastValue();

        if (bothHaveDates) {
            if (futureBegin.isAfter(futureEnd)) {
                futureBegin = pastBegin;
            }

            if (pastEnd.isBefore(pastBegin)) {
                pastEnd = futureEnd;
            }
        }

        if (bothHaveDates) {
            result.setTimex(String.format("(%s,%s,PT%dH)", pr1.timexStr, pr2.timexStr, Math.round(ChronoUnit.SECONDS.between(futureBegin, futureEnd) / 3600f)));
            // Do nothing
        } else if (beginHasDate) {
            futureEnd = DateUtil.safeCreateFromMinValue(futureBegin.toLocalDate(), futureEnd.toLocalTime());
            pastEnd = DateUtil.safeCreateFromMinValue(pastBegin.toLocalDate(), pastEnd.toLocalTime());

            String dateStr = pr1.timexStr.split("T")[0];
            result.setTimex(String.format("(%s,%s,PT%dH)", pr1.timexStr, dateStr + pr2.timexStr, ChronoUnit.HOURS.between(futureBegin, futureEnd)));
        } else if (endHasDate) {
            futureEnd = DateUtil.safeCreateFromMinValue(futureEnd.toLocalDate(), futureBegin.toLocalTime());
            pastEnd = DateUtil.safeCreateFromMinValue(pastEnd.toLocalDate(), pastBegin.toLocalTime());

            String dateStr = pr2.timexStr.split("T")[0];
            result.setTimex(String.format("(%s,%s,PT%dH)", dateStr + pr1.timexStr, pr2.timexStr, ChronoUnit.HOURS.between(futureBegin, futureEnd)));
        }
        
        DateTimeResolutionResult pr1Value = (DateTimeResolutionResult)pr1.value;
        DateTimeResolutionResult pr2Value = (DateTimeResolutionResult)pr2.value;

        String ampmStr1 = pr1Value.getComment();
        String ampmStr2 = pr2Value.getComment();

        if (!StringUtility.isNullOrEmpty(ampmStr1) && ampmStr1.endsWith(Constants.Comment_AmPm) &&
            !StringUtility.isNullOrEmpty(ampmStr2) && ampmStr2.endsWith(Constants.Comment_AmPm)) {
            result.setComment(Constants.Comment_AmPm);
        }

        if (pr1Value.getTimeZoneResolution() != null) {
            result.setTimeZoneResolution(pr1Value.getTimeZoneResolution());
        }

        if (pr2Value.getTimeZoneResolution() != null) {
            result.setTimeZoneResolution(pr2Value.getTimeZoneResolution());
        }

        result.setFutureValue(new Pair<LocalDateTime, LocalDateTime>(futureBegin, futureEnd));
        result.setPastValue(new Pair<LocalDateTime, LocalDateTime>(pastBegin, pastEnd));

        result.setSuccess(true);

        List<Object> subDateTimeEntities = new ArrayList<>();
        subDateTimeEntities.add(pr1);
        subDateTimeEntities.add(pr2);
        result.setSubDateTimeEntities(subDateTimeEntities);

        return result;
    }

    // Parse specific TimeOfDay like "this night", "early morning", "late evening"
    private DateTimeResolutionResult parseSpecificTimeOfDay(String text, LocalDateTime referenceDate) {
        DateTimeResolutionResult result = new DateTimeResolutionResult();
        String trimmedText = text.trim().toLowerCase();
        String timeText = trimmedText;

        Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(config.getPeriodTimeOfDayWithDateRegex(), trimmedText)).findFirst();

        // Extract early/late prefix from text if any
        boolean hasEarly = false;
        boolean hasLate = false;
        if (match.isPresent()) {
            timeText = match.get().getGroup("timeOfDay").value;

            if (!StringUtility.isNullOrEmpty(match.get().getGroup("early").value)) {
                hasEarly = true;
                result.setComment(Constants.Comment_Early);
            }
            
            if (!hasEarly && !StringUtility.isNullOrEmpty(match.get().getGroup("late").value)) {
                hasLate = true;
                result.setComment(Constants.Comment_Late);
            }
        } else {
            match = Arrays.stream(RegExpUtility.getMatches(config.getAmDescRegex(), trimmedText)).findFirst();
            if (!match.isPresent()) {
                match = Arrays.stream(RegExpUtility.getMatches(config.getPmDescRegex(), trimmedText)).findFirst();
            }

            if (match.isPresent()) {
                timeText = match.get().value;
            }
        }

        // Handle time of day

        String timeStr = null;
        int beginHour = -1;
        int endHour = -1;
        int endMin = -1;
        
        // Late/early only works with time of day
        // Only standard time of day (morinng, afternoon, evening and night) will not directly return
        MatchedTimeRangeResult matchedTimeRange = config.getMatchedTimeRange(timeText, timeStr, beginHour, endHour, endMin);
        timeStr = matchedTimeRange.getTimeStr();
        beginHour = matchedTimeRange.getBeginHour();
        endHour = matchedTimeRange.getEndHour();
        endMin = matchedTimeRange.getEndMin();

        if (!matchedTimeRange.getMatched()) {
            return result;
        }

        // Modify time period if "early" or "late" exists
        // Since 'time of day' is defined as four hour periods, 
        // the first 2 hours represent early, the later 2 hours represent late
        if (hasEarly) {
            endHour = beginHour + 2;
            // Handling speical case: night ends with 23:59
            if (endMin == 59) {
                endMin = 0;
            }
        } else if (hasLate) {
            beginHour = beginHour + 2;
        }

        match = Arrays.stream(RegExpUtility.getMatches(config.getSpecificTimeOfDayRegex(), trimmedText)).findFirst();
        if (match.isPresent() && match.get().index == 0 && match.get().length == trimmedText.length()) {
            int swift = config.getSwiftPrefix(trimmedText);

            LocalDateTime date = referenceDate.plusDays(swift);
            int day = date.getDayOfMonth();
            int month = date.getMonthValue();
            int year = date.getYear();

            result.setTimex(FormatUtil.formatDate(date) + timeStr);

            Pair<LocalDateTime, LocalDateTime> resultValue = new Pair<LocalDateTime,LocalDateTime>(
                DateUtil.safeCreateFromMinValue(year, month, day, beginHour, 0, 0),
                DateUtil.safeCreateFromMinValue(year, month, day, endHour, endMin, endMin)
            );

            result.setFutureValue(resultValue);
            result.setPastValue(resultValue);

            result.setSuccess(true);

            return result;
        }

        // Handle Date followed by morning, afternoon and morning, afternoon followed by Date
        match = Arrays.stream(RegExpUtility.getMatches(config.getPeriodTimeOfDayWithDateRegex(), trimmedText)).findFirst();

        if (!match.isPresent()) {
            match = Arrays.stream(RegExpUtility.getMatches(config.getAmDescRegex(), trimmedText)).findFirst();

            if (!match.isPresent()) {
                match = Arrays.stream(RegExpUtility.getMatches(config.getPmDescRegex(), trimmedText)).findFirst();
            }
        }

        if (match.isPresent()) {
            String beforeStr = trimmedText.substring(0, match.get().index);
            String afterStr = trimmedText.substring(match.get().index + match.get().length).trim();

            // Eliminate time period, if any
            List<ExtractResult> timePeriodErs = config.getTimePeriodExtractor().extract(beforeStr);
            if (timePeriodErs.size() > 0) {
                beforeStr = beforeStr.substring(0, timePeriodErs.get(0).start) + beforeStr.substring(timePeriodErs.get(0).start + timePeriodErs.get(0).length);
            } else {
                timePeriodErs = config.getTimePeriodExtractor().extract(afterStr);
                if (timePeriodErs.size() > 0) {
                    afterStr = afterStr.substring(0, timePeriodErs.get(0).start) + beforeStr.substring(timePeriodErs.get(0).start + timePeriodErs.get(0).length);
                }
            }

            List<ExtractResult> ers = config.getDateExtractor().extract(beforeStr, referenceDate);

            if (ers.size() == 0 || ers.get(0).length != beforeStr.length()) {
                boolean valid = false;

                if (ers.size() > 0 && ers.get(0).start == 0) {
                    String midStr = beforeStr.substring(ers.get(0).start + ers.get(0).length);
                    if (StringUtility.isNullOrWhiteSpace(midStr.replace(",", " "))) {
                        valid = true;
                    }
                }

                if (!valid) {
                    ers = config.getDateExtractor().extract(afterStr, referenceDate);

                    if (ers.size() == 0 || ers.get(0).length != beforeStr.length()) {
                        if (ers.size() > 0 && ers.get(0).start + ers.get(0).length == afterStr.length()) {
                            String midStr = afterStr.substring(0, ers.get(0).start);
                            if (StringUtility.isNullOrWhiteSpace(midStr.replace(",", " "))) {
                                valid = true;
                            }
                        }
                    } else {
                        valid = true;
                    }
                }

                if (!valid) {
                    return result;
                }
            }

            boolean hasSpecificTimePeriod = false;
            if (timePeriodErs.size() > 0) {
                DateTimeParseResult timePr = config.getTimePeriodParser().parse(timePeriodErs.get(0), referenceDate);
                if (timePr != null) {
                    Pair<LocalDateTime, LocalDateTime> periodFuture = (Pair<LocalDateTime, LocalDateTime>)((DateTimeResolutionResult)timePr.value).getFutureValue();
                    Pair<LocalDateTime, LocalDateTime> periodPast = (Pair<LocalDateTime, LocalDateTime>)((DateTimeResolutionResult)timePr.value).getPastValue();

                    if (periodFuture == periodPast) {
                        beginHour = periodFuture.getValue0().getHour();
                        endHour = periodFuture.getValue1().getHour();
                    } else {
                        if (periodFuture.getValue0().getHour() >= beginHour || periodFuture.getValue1().getHour() <= endHour) {
                            beginHour = periodFuture.getValue0().getHour();
                            endHour = periodFuture.getValue1().getHour();
                        } else {
                            beginHour = periodPast.getValue0().getHour();
                            endHour = periodPast.getValue1().getHour();
                        }
                    }

                    hasSpecificTimePeriod = true;
                }
            }

            DateTimeParseResult pr = config.getDateParser().parse(ers.get(0), referenceDate);
            LocalDateTime futureDate = (LocalDateTime)((DateTimeResolutionResult)pr.value).getFutureValue();
            LocalDateTime pastDate = (LocalDateTime)((DateTimeResolutionResult)pr.value).getPastValue();

            if (!hasSpecificTimePeriod) {
                result.setTimex(pr.timexStr + timeStr);
            } else {
                result.setTimex(String.format("(%sT%d,%sT%d,PT%dH)", pr.timexStr, beginHour, pr.timexStr, endHour, endHour - beginHour));
            }

            Pair<LocalDateTime, LocalDateTime> futureResult = new Pair<LocalDateTime,LocalDateTime>(
                DateUtil.safeCreateFromMinValue(
                    futureDate.getYear(), futureDate.getMonthValue(), futureDate.getDayOfMonth(),
                    beginHour, 0, 0),
                DateUtil.safeCreateFromMinValue(
                    futureDate.getYear(), futureDate.getMonthValue(), futureDate.getDayOfMonth(),
                    endHour, endMin, endMin)
            );

            Pair<LocalDateTime, LocalDateTime> pastResult = new Pair<LocalDateTime,LocalDateTime>(
                DateUtil.safeCreateFromMinValue(
                    pastDate.getYear(), pastDate.getMonthValue(), pastDate.getDayOfMonth(),
                    beginHour, 0, 0),
                DateUtil.safeCreateFromMinValue(
                    pastDate.getYear(), pastDate.getMonthValue(), pastDate.getDayOfMonth(),
                    endHour, endMin, endMin)
            );

            result.setFutureValue(futureResult);
            result.setPastValue(pastResult);

            result.setSuccess(true);

            return result;
        }

        return result;
    }
    
    // TODO: this can be abstracted with the similar method in BaseDatePeriodParser
    // Parse "in 20 minutes"
    private DateTimeResolutionResult parseDuration(String text, LocalDateTime referenceDate) {
        DateTimeResolutionResult result = new DateTimeResolutionResult();
        
        // For the rest of datetime, it will be handled in next function
        if (RegExpUtility.getMatches(config.getRestOfDateTimeRegex(), text).length > 0) {
            return result;
        }

        List<ExtractResult> ers = config.getDurationExtractor().extract(text, referenceDate);

        if (ers.size() == 1) {
            ParseResult pr = config.getDurationParser().parse(ers.get(0));

            String beforeStr = text.substring(0, pr.start).trim().toLowerCase();
            String afterStr = text.substring(pr.start + pr.length).trim().toLowerCase();

            if (pr.value != null) {
                int swiftSeconds = 0;
                String mod = "";
                DateTimeResolutionResult durationResult = (DateTimeResolutionResult)pr.value;

                if (durationResult.getPastValue() instanceof Double && durationResult.getFutureValue() instanceof Double) {
                    swiftSeconds = Math.round(((Double)durationResult.getPastValue()).floatValue());
                }

                LocalDateTime beginTime = referenceDate;
                LocalDateTime endTime = referenceDate;

                Optional<Match> prefixMatch = Arrays.stream(RegExpUtility.getMatches(config.getPastRegex(), beforeStr)).findFirst();
                if (prefixMatch.isPresent() && prefixMatch.get().length == beforeStr.length()) {
                    mod = Constants.BEFORE_MOD;
                    beginTime = referenceDate.minusSeconds(swiftSeconds);
                }

                // Handle the "within (the) (next) xx seconds/minutes/hours" case
                // Should also handle the multiple duration case like P1DT8H
                // Set the beginTime equal to reference time for now
                prefixMatch = Arrays.stream(RegExpUtility.getMatches(config.getWithinNextPrefixRegex(), beforeStr)).findFirst();
                if (prefixMatch.isPresent() && prefixMatch.get().length == beforeStr.length()) {
                    endTime = beginTime.plusSeconds(swiftSeconds);
                }

                prefixMatch = Arrays.stream(RegExpUtility.getMatches(config.getFutureRegex(), beforeStr)).findFirst();
                if (prefixMatch.isPresent() && prefixMatch.get().length == beforeStr.length()) {
                    mod = Constants.AFTER_MOD;
                    endTime = beginTime.plusSeconds(swiftSeconds);
                }

                Optional<Match> suffixMatch = Arrays.stream(RegExpUtility.getMatches(config.getPastRegex(), afterStr)).findFirst();
                if (suffixMatch.isPresent() && suffixMatch.get().length == afterStr.length()) {
                    mod = Constants.BEFORE_MOD;
                    beginTime = referenceDate.minusSeconds(swiftSeconds);
                }

                suffixMatch = Arrays.stream(RegExpUtility.getMatches(config.getFutureRegex(), afterStr)).findFirst();
                if (suffixMatch.isPresent() && suffixMatch.get().length == afterStr.length()) {
                    mod = Constants.AFTER_MOD;
                    endTime = beginTime.plusSeconds(swiftSeconds);
                }

                suffixMatch = Arrays.stream(RegExpUtility.getMatches(config.getFutureSuffixRegex(), afterStr)).findFirst();
                if (suffixMatch.isPresent() && suffixMatch.get().length == afterStr.length()) {
                    mod = Constants.AFTER_MOD;
                    endTime = beginTime.plusSeconds(swiftSeconds);
                }

                result.setTimex(String.format("(%sT%s,%sT%s,%s)",
                    FormatUtil.luisDate(beginTime),
                    FormatUtil.luisTime(beginTime),
                    FormatUtil.luisDate(endTime),
                    FormatUtil.luisTime(endTime),
                    durationResult.getTimex()
                ));

                Pair<LocalDateTime, LocalDateTime> resultValue = new Pair<LocalDateTime, LocalDateTime>(beginTime, endTime);

                result.setFutureValue(resultValue);
                result.setPastValue(resultValue);

                result.setSuccess(true);

                if (!StringUtility.isNullOrEmpty(mod)) {
                    ((DateTimeResolutionResult)pr.value).setMod(mod);
                }

                List<Object> subDateTimeEntities = new ArrayList<Object>();
                subDateTimeEntities.add(pr);
                result.setSubDateTimeEntities(subDateTimeEntities);

                return result;
            }
        }

        return result;
    }

    // Parse "last minute", "next hour"
    private DateTimeResolutionResult parseRelativeUnit(String text, LocalDateTime referenceDate) {
        DateTimeResolutionResult result = new DateTimeResolutionResult();

        Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(config.getRelativeTimeUnitRegex(), text)).findFirst();
        if (!match.isPresent()) {
            match = Arrays.stream(RegExpUtility.getMatches(config.getRestOfDateTimeRegex(), text)).findFirst();
        }

        if (match.isPresent()) {
            String srcUnit = match.get().getGroup("unit").value;

            String unitStr = config.getUnitMap().get(srcUnit);

            int swiftValue = 1;
            Optional<Match> prefixMatch = Arrays.stream(RegExpUtility.getMatches(config.getPastRegex(), text)).findFirst();
            if (prefixMatch.isPresent()) {
                swiftValue = -1;
            }

            LocalDateTime beginTime = referenceDate;
            LocalDateTime endTime = referenceDate;
            String ptTimex = "";

            if (config.getUnitMap().containsKey(srcUnit)) {
                switch (unitStr) {
                    case "D":
                        endTime = DateUtil.safeCreateFromMinValue(beginTime.getYear(), beginTime.getMonthValue(), beginTime.getDayOfMonth());
                        endTime = endTime.plusDays(1).minusSeconds(1);
                        ptTimex = String.format("PT%dS", ChronoUnit.SECONDS.between(beginTime, endTime));
                        break;
                    case "H":
                        beginTime = swiftValue > 0 ? beginTime : referenceDate.plusHours(swiftValue);
                        endTime = swiftValue > 0 ? referenceDate.plusHours(swiftValue) : endTime;
                        ptTimex = "PT1H";
                        break;
                    case "M":
                        beginTime = swiftValue > 0 ? beginTime : referenceDate.plusMinutes(swiftValue);
                        endTime = swiftValue > 0 ? referenceDate.plusMinutes(swiftValue) : endTime;
                        ptTimex = "PT1M";
                        break;
                    case "S":
                        beginTime = swiftValue > 0 ? beginTime : referenceDate.plusSeconds(swiftValue);
                        endTime = swiftValue > 0 ? referenceDate.plusSeconds(swiftValue) : endTime;
                        ptTimex = "PT1S";
                        break;
                    default:
                        return result;
                }

                result.setTimex(String.format("(%sT%s,%sT%s,%s)",
                    FormatUtil.luisDate(beginTime),
                    FormatUtil.luisTime(beginTime),
                    FormatUtil.luisDate(endTime),
                    FormatUtil.luisTime(endTime),
                    ptTimex
                ));
    
                Pair<LocalDateTime, LocalDateTime> resultValue = new Pair<LocalDateTime, LocalDateTime>(beginTime, endTime);
    
                result.setFutureValue(resultValue);
                result.setPastValue(resultValue);
    
                result.setSuccess(true);
    
                return result;
            }
        }

        return result;
    }

    private DateTimeResolutionResult parseDateWithPeriodPrefix(String text, LocalDateTime referenceDate) {
        DateTimeResolutionResult result = new DateTimeResolutionResult();

        List<ExtractResult> dateResult = config.getDateExtractor().extract(text);
        if (dateResult.size() > 0) {
            String beforeStr = StringUtility.trimEnd(text.substring(0, dateResult.get(dateResult.size() - 1).start));
            Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(config.getPrefixDayRegex(), beforeStr)).findFirst();
            if (match.isPresent()) {
                DateTimeParseResult pr = config.getDateParser().parse(dateResult.get(dateResult.size() - 1), referenceDate);
                if (pr.value != null) {
                    LocalDateTime startTime = (LocalDateTime)((DateTimeResolutionResult)pr.value).getFutureValue();
                    startTime = LocalDateTime.of(startTime.getYear(), startTime.getMonthValue(), startTime.getDayOfMonth(), 0, 0, 0);
                    LocalDateTime endTime = startTime;

                    if (!StringUtility.isNullOrEmpty(match.get().getGroup("EarlyPrefix").value)) {
                        endTime = endTime.plusHours(Constants.HalfDayHourCount);
                        result.setMod(Constants.EARLY_MOD);
                    } else if (!StringUtility.isNullOrEmpty(match.get().getGroup("MidPrefix").value)) {
                        startTime = startTime.plusHours(Constants.HalfDayHourCount - Constants.HalfMidDayDurationHourCount);
                        endTime = endTime.plusHours(Constants.HalfDayHourCount + Constants.HalfMidDayDurationHourCount);
                        result.setMod(Constants.MID_MOD);
                    } else if (!StringUtility.isNullOrEmpty(match.get().getGroup("LatePrefix").value)) {
                        startTime = startTime.plusHours(Constants.HalfDayHourCount);
                        endTime = startTime.plusHours(Constants.HalfDayHourCount);
                        result.setMod(Constants.LATE_MOD);
                    } else {
                        return result;
                    }

                    result.setTimex(pr.timexStr);
                    
                    Pair<LocalDateTime, LocalDateTime> resultValue = new Pair<LocalDateTime, LocalDateTime>(startTime, endTime);
        
                    result.setFutureValue(resultValue);
                    result.setPastValue(resultValue);
        
                    result.setSuccess(true);
                }
            }
        }

        return result;
    }

    private DateTimeResolutionResult parseDateWithTimePeriodSuffix(String text, LocalDateTime referenceDate) {
        DateTimeResolutionResult result = new DateTimeResolutionResult();

        Optional<ExtractResult> dateEr = config.getDateExtractor().extract(text).stream().findFirst();
        Optional<ExtractResult> timeEr = config.getTimeExtractor().extract(text).stream().findFirst();

        if (dateEr.isPresent() && timeEr.isPresent()) {
            int dateStrEnd = dateEr.get().start + dateEr.get().length;

            if (dateStrEnd < timeEr.get().start) {
                String midStr = text.substring(dateStrEnd, timeEr.get().start);

                if (isValidConnectorForDateAndTimePeriod(midStr)) {
                    DateTimeParseResult datePr = config.getDateParser().parse(dateEr.get(), referenceDate);
                    DateTimeParseResult timePr = config.getTimeParser().parse(timeEr.get(), referenceDate);

                    if (datePr != null && timePr != null) {
                        DateTimeResolutionResult timeResolutionResult = (DateTimeResolutionResult)timePr.value;
                        DateTimeResolutionResult dateResolutionResult = (DateTimeResolutionResult)datePr.value;
                        LocalDateTime futureDateValue = (LocalDateTime)dateResolutionResult.getFutureValue();
                        LocalDateTime pastDateValue = (LocalDateTime)dateResolutionResult.getPastValue();
                        LocalDateTime futureTimeValue = (LocalDateTime)timeResolutionResult.getFutureValue();
                        LocalDateTime pastTimeValue = (LocalDateTime)timeResolutionResult.getPastValue();

                        result.setComment(timeResolutionResult.getComment());
                        result.setTimex(datePr.timexStr + timePr.timexStr);

                        result.setFutureValue(DateUtil.safeCreateFromMinValue(futureDateValue.toLocalDate(), futureTimeValue.toLocalTime()));
                        result.setPastValue(DateUtil.safeCreateFromMinValue(pastDateValue.toLocalDate(), pastTimeValue.toLocalTime()));

                        if (RegExpUtility.getMatches(config.getBeforeRegex(), midStr).length > 0) {
                            result.setMod(Constants.BEFORE_MOD);
                        } else {
                            result.setMod(Constants.AFTER_MOD);
                        }

                        List<Object> subDateTimeEntities = new ArrayList<>();
                        subDateTimeEntities.add(datePr);
                        subDateTimeEntities.add(timePr);

                        result.setSubDateTimeEntities(subDateTimeEntities);

                        result.setSuccess(true);
                    }
                }
            }
        }

        return result;
    }

    // Cases like "today after 2:00pm", "1/1/2015 before 2:00 in the afternoon"
    // Valid connector in English for Before include: "before", "no later than", "in advance of", "prior to", "earlier than", "sooner than", "by", "till", "until"...
    // Valid connector in English for After include: "after", "later than"
    private boolean isValidConnectorForDateAndTimePeriod(String text) {
        List<Pattern> beforeAfterRegexes = new ArrayList<>();
        beforeAfterRegexes.add(config.getBeforeRegex());
        beforeAfterRegexes.add(config.getAfterRegex());
        text = text.trim();

        for (Pattern regex : beforeAfterRegexes) {
            Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(regex, text)).findFirst();
            if (match.isPresent() && match.get().length == text.length()) {
                return true;
            }
        }

        return false;
    }
}
