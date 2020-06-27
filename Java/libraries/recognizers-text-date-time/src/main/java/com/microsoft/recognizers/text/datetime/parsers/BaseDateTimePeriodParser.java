package com.microsoft.recognizers.text.datetime.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.TimeTypeConstants;
import com.microsoft.recognizers.text.datetime.parsers.config.IDateTimePeriodParserConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.config.MatchedTimeRangeResult;
import com.microsoft.recognizers.text.datetime.utilities.ConditionalMatch;
import com.microsoft.recognizers.text.datetime.utilities.DateTimeFormatUtil;
import com.microsoft.recognizers.text.datetime.utilities.DateTimeResolutionResult;
import com.microsoft.recognizers.text.datetime.utilities.DateUtil;
import com.microsoft.recognizers.text.datetime.utilities.RangeTimexComponents;
import com.microsoft.recognizers.text.datetime.utilities.RegexExtension;
import com.microsoft.recognizers.text.datetime.utilities.TimeZoneUtility;
import com.microsoft.recognizers.text.datetime.utilities.TimexUtility;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.MatchGroup;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import com.microsoft.recognizers.text.utilities.StringUtility;

import java.time.LocalDateTime;
import java.time.temporal.ChronoUnit;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Optional;
import java.util.regex.Pattern;

import org.javatuples.Pair;

public class BaseDateTimePeriodParser implements IDateTimeParser {

    protected final IDateTimePeriodParserConfiguration config;

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

        if (er.getType().equals(getParserName())) {
            DateTimeResolutionResult innerResult = internalParse(er.getText(), referenceDate);

            if (TimeZoneUtility.shouldResolveTimeZone(er, config.getOptions())) {
                Map<String, Object> metadata = (HashMap<String, Object>)er.getData();

                ExtractResult timezoneEr = (ExtractResult)metadata.get(Constants.SYS_DATETIME_TIMEZONE);
                ParseResult timezonePr = config.getTimeZoneParser().parse(timezoneEr);
                if (timezonePr.getValue() != null) {
                    innerResult.setTimeZoneResolution(((DateTimeResolutionResult)timezonePr.getValue()).getTimeZoneResolution());
                }
            }

            if (innerResult.getSuccess()) {
                if (!isBeforeOrAfterMod(innerResult.getMod())) {
                    Map<String, String> futureResolution = ImmutableMap.<String, String>builder()
                            .put(TimeTypeConstants.START_DATETIME,
                                    DateTimeFormatUtil.formatDateTime(((Pair<LocalDateTime, LocalDateTime>)innerResult.getFutureValue()).getValue0()))
                            .put(TimeTypeConstants.END_DATETIME, DateTimeFormatUtil.formatDateTime(((Pair<LocalDateTime, LocalDateTime>)innerResult.getFutureValue()).getValue1()))
                            .build();
                    innerResult.setFutureResolution(futureResolution);

                    Map<String, String> pastResolution = ImmutableMap.<String, String>builder()
                            .put(TimeTypeConstants.START_DATETIME, DateTimeFormatUtil.formatDateTime(((Pair<LocalDateTime, LocalDateTime>)innerResult.getPastValue()).getValue0()))
                            .put(TimeTypeConstants.END_DATETIME, DateTimeFormatUtil.formatDateTime(((Pair<LocalDateTime, LocalDateTime>)innerResult.getPastValue()).getValue1()))
                            .build();
                    innerResult.setPastResolution(pastResolution);

                } else {
                    if (innerResult.getMod().equals(Constants.AFTER_MOD)) {
                        // Cases like "1/1/2015 after 2:00" there is no EndTime
                        Map<String, String> futureResolution = ImmutableMap.<String, String>builder()
                                .put(TimeTypeConstants.START_DATETIME, DateTimeFormatUtil.formatDateTime((LocalDateTime)innerResult.getFutureValue()))
                                .build();
                        innerResult.setFutureResolution(futureResolution);

                        Map<String, String> pastResolution = ImmutableMap.<String, String>builder()
                                .put(TimeTypeConstants.START_DATETIME, DateTimeFormatUtil.formatDateTime((LocalDateTime)innerResult.getPastValue()))
                                .build();
                        innerResult.setPastResolution(pastResolution);
                    } else {
                        // Cases like "1/1/2015 before 5:00 in the afternoon" there is no StartTime
                        Map<String, String> futureResolution = ImmutableMap.<String, String>builder()
                                .put(TimeTypeConstants.END_DATETIME, DateTimeFormatUtil.formatDateTime((LocalDateTime)innerResult.getFutureValue()))
                                .build();
                        innerResult.setFutureResolution(futureResolution);

                        Map<String, String> pastResolution = ImmutableMap.<String, String>builder()
                                .put(TimeTypeConstants.END_DATETIME, DateTimeFormatUtil.formatDateTime((LocalDateTime)innerResult.getPastValue()))
                                .build();
                        innerResult.setPastResolution(pastResolution);
                    }
                }

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
                value == null ? "" : ((DateTimeResolutionResult)value).getTimex());

        return ret;
    }

    private DateTimeResolutionResult internalParse(String text, LocalDateTime reference) {
        DateTimeResolutionResult innerResult = this.mergeDateAndTimePeriods(text, reference);

        if (!innerResult.getSuccess()) {
            innerResult = this.mergeTwoTimePoints(text, reference);
        }

        if (!innerResult.getSuccess()) {
            innerResult = this.parseSpecificTimeOfDay(text, reference);
        }

        if (!innerResult.getSuccess()) {
            innerResult = this.parseDuration(text, reference);
        }

        if (!innerResult.getSuccess()) {
            innerResult = this.parseRelativeUnit(text, reference);
        }

        if (!innerResult.getSuccess()) {
            innerResult = this.parseDateWithPeriodPrefix(text, reference);
        }

        if (!innerResult.getSuccess()) {
            // Cases like "today after 2:00pm", "1/1/2015 before 2:00 in the afternoon"
            innerResult = this.parseDateWithTimePeriodSuffix(text, reference);
        }

        return innerResult;
    }

    private boolean isBeforeOrAfterMod(String mod) {
        return !StringUtility.isNullOrEmpty(mod) &&
                (mod == Constants.BEFORE_MOD || mod == Constants.AFTER_MOD);
    }

    private DateTimeResolutionResult mergeDateAndTimePeriods(String text, LocalDateTime referenceTime) {
        DateTimeResolutionResult ret = new DateTimeResolutionResult();

        String trimmedText = text.trim().toLowerCase();

        List<ExtractResult> ers = config.getTimePeriodExtractor().extract(trimmedText, referenceTime);

        if (ers.size() == 0) {
            return parsePureNumberCases(text, referenceTime);
        } else if (ers.size() == 1) {
            ParseResult timePeriodParseResult = config.getTimePeriodParser().parse(ers.get(0));
            DateTimeResolutionResult timePeriodResolutionResult = (DateTimeResolutionResult)timePeriodParseResult.getValue();

            if (timePeriodResolutionResult == null) {
                return parsePureNumberCases(text, referenceTime);
            }

            String timePeriodTimex = timePeriodResolutionResult.getTimex();


            // If it is a range type timex
            if (TimexUtility.isRangeTimex(timePeriodTimex)) {
                List<ExtractResult> dateResult = config.getDateExtractor().extract(trimmedText.replace(ers.get(0).getText(), ""), referenceTime);
                String dateText = trimmedText.replace(ers.get(0).getText(), "").replace(config.getTokenBeforeDate(), "").trim();

                // If only one Date is extracted and the Date text equals to the rest part of source text
                if (dateResult.size() == 1 && dateText.equals(dateResult.get(0).getText())) {
                    String dateTimex;
                    LocalDateTime futureTime;
                    LocalDateTime pastTime;

                    DateTimeParseResult pr = config.getDateParser().parse(dateResult.get(0), referenceTime);

                    if (pr.getValue() != null) {
                        futureTime = (LocalDateTime)((DateTimeResolutionResult)pr.getValue()).getFutureValue();
                        pastTime = (LocalDateTime)((DateTimeResolutionResult)pr.getValue()).getPastValue();

                        dateTimex = pr.getTimexStr();
                    } else {
                        return parsePureNumberCases(text, referenceTime);
                    }

                    RangeTimexComponents rangeTimexComponents = TimexUtility.getRangeTimexComponents(timePeriodTimex);
                    if (rangeTimexComponents.isValid) {
                        String beginTimex = TimexUtility.combineDateAndTimeTimex(dateTimex, rangeTimexComponents.beginTimex);
                        String endTimex = TimexUtility.combineDateAndTimeTimex(dateTimex, rangeTimexComponents.endTimex);
                        ret.setTimex(TimexUtility.generateDateTimePeriodTimex(beginTimex, endTimex, rangeTimexComponents.durationTimex));

                        Pair<LocalDateTime, LocalDateTime> timePeriodFutureValue = (Pair<LocalDateTime, LocalDateTime>)timePeriodResolutionResult.getFutureValue();
                        LocalDateTime beginTime = timePeriodFutureValue.getValue0();
                        LocalDateTime endTime = timePeriodFutureValue.getValue1();

                        ret.setFutureValue(new Pair<>(
                                DateUtil.safeCreateFromMinValue(futureTime.getYear(), futureTime.getMonthValue(), futureTime.getDayOfMonth(),
                                        beginTime.getHour(), beginTime.getMinute(), beginTime.getSecond()),
                                DateUtil.safeCreateFromMinValue(futureTime.getYear(), futureTime.getMonthValue(), futureTime.getDayOfMonth(),
                                        endTime.getHour(), endTime.getMinute(), endTime.getSecond())
                        ));

                        ret.setPastValue(new Pair<>(
                                DateUtil.safeCreateFromMinValue(pastTime.getYear(), pastTime.getMonthValue(), pastTime.getDayOfMonth(),
                                        beginTime.getHour(), beginTime.getMinute(), beginTime.getSecond()),
                                DateUtil.safeCreateFromMinValue(pastTime.getYear(), pastTime.getMonthValue(), pastTime.getDayOfMonth(),
                                        endTime.getHour(), endTime.getMinute(), endTime.getSecond())
                        ));


                        if (!StringUtility.isNullOrEmpty(timePeriodResolutionResult.getComment()) &&
                                timePeriodResolutionResult.getComment().equals(Constants.Comment_AmPm)) {
                            // AmPm comment is used for later SetParserResult to judge whether this parse comments should have two parsing results
                            // Cases like "from 10:30 to 11 on 1/1/2015" should have AmPm comment, as it can be parsed to "10:30am to 11am" and also be parsed to "10:30pm to 11pm"
                            // Cases like "from 10:30 to 3 on 1/1/2015" should not have AmPm comment
                            if (beginTime.getHour() < Constants.HalfDayHourCount && endTime.getHour() < Constants.HalfDayHourCount) {
                                ret.setComment(Constants.Comment_AmPm);
                            }
                        }

                        ret.setSuccess(true);
                        List<Object> subDateTimeEntities = new ArrayList<>();
                        subDateTimeEntities.add(pr);
                        subDateTimeEntities.add(timePeriodParseResult);
                        ret.setSubDateTimeEntities(subDateTimeEntities);

                        return ret;
                    }
                }

                return parsePureNumberCases(text, referenceTime);
            }
        }

        return ret;
    }

    // Handle cases like "Monday 7-9", where "7-9" can't be extracted by the TimePeriodExtractor
    private DateTimeResolutionResult parsePureNumberCases(String text, LocalDateTime referenceTime) {
        DateTimeResolutionResult ret = new DateTimeResolutionResult();
        String trimmedText = text.trim().toLowerCase();

        Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(config.getPureNumberFromToRegex(), trimmedText)).findFirst();
        if (!match.isPresent()) {
            match = Arrays.stream(RegExpUtility.getMatches(config.getPureNumberBetweenAndRegex(), trimmedText)).findFirst();
        }

        if (match.isPresent() && (match.get().index == 0 || match.get().index + match.get().length == trimmedText.length())) {
            ParseTimePeriodResult parseTimePeriodResult = parseTimePeriod(match.get());
            int beginHour = parseTimePeriodResult.beginHour;
            int endHour = parseTimePeriodResult.endHour;
            ret.setComment(parseTimePeriodResult.comments);

            String dateStr = "";

            // Parse following date
            List<ExtractResult> ers = config.getDateExtractor().extract(trimmedText.replace(match.get().value, ""), referenceTime);
            LocalDateTime futureDate;
            LocalDateTime pastDate;

            if (ers.size() > 0) {
                DateTimeParseResult pr = config.getDateParser().parse(ers.get(0), referenceTime);
                if (pr.getValue() != null) {
                    DateTimeResolutionResult prValue = (DateTimeResolutionResult)pr.getValue();
                    futureDate = (LocalDateTime)prValue.getFutureValue();
                    pastDate = (LocalDateTime)prValue.getPastValue();

                    dateStr = pr.getTimexStr();

                } else {
                    return ret;
                }
            } else {
                return ret;
            }

            int pastHours = endHour - beginHour;
            String beginTimex = TimexUtility.combineDateAndTimeTimex(dateStr, DateTimeFormatUtil.shortTime(beginHour));
            String endTimex = TimexUtility.combineDateAndTimeTimex(dateStr, DateTimeFormatUtil.shortTime(endHour));
            String durationTimex = TimexUtility.generateDurationTimex(endHour - beginHour, Constants.TimexHour, true);

            ret.setTimex(TimexUtility.generateDateTimePeriodTimex(beginTimex, endTimex, durationTimex));

            ret.setFutureValue(new Pair<>(
                    DateUtil.safeCreateFromMinValue(futureDate.getYear(), futureDate.getMonthValue(), futureDate.getDayOfMonth(),
                            beginHour, 0, 0),
                    DateUtil.safeCreateFromMinValue(futureDate.getYear(), futureDate.getMonthValue(), futureDate.getDayOfMonth(),
                            endHour, 0, 0)
            ));

            ret.setPastValue(new Pair<>(
                    DateUtil.safeCreateFromMinValue(pastDate.getYear(), pastDate.getMonthValue(), pastDate.getDayOfMonth(),
                            beginHour, 0, 0),
                    DateUtil.safeCreateFromMinValue(pastDate.getYear(), pastDate.getMonthValue(), pastDate.getDayOfMonth(),
                            endHour, 0, 0)
            ));

            ret.setSuccess(true);
        }

        return ret;
    }

    private ParseTimePeriodResult parseTimePeriod(Match match) {

        ParseTimePeriodResult result = new ParseTimePeriodResult();

        // This "from .. to .." pattern is valid if followed by a Date OR "pm"
        boolean hasAm = false;
        boolean hasPm = false;
        String comments = "";

        // Get hours
        MatchGroup hourGroup = match.getGroup(Constants.HourGroupName);
        String hourStr = hourGroup.captures[0].value;

        if (this.config.getNumbers().containsKey(hourStr)) {
            result.beginHour = this.config.getNumbers().get(hourStr);
        } else {
            result.beginHour = Integer.parseInt(hourStr);
        }

        hourStr = hourGroup.captures[1].value;

        if (this.config.getNumbers().containsKey(hourStr)) {
            result.endHour = this.config.getNumbers().get(hourStr);
        } else {
            result.endHour = Integer.parseInt(hourStr);
        }

        // Parse "pm"
        String pmStr = match.getGroup(Constants.PmGroupName).value;
        String amStr = match.getGroup(Constants.AmGroupName).value;
        String descStr = match.getGroup(Constants.DescGroupName).value;
        if (!StringUtility.isNullOrEmpty(amStr) || !StringUtility.isNullOrEmpty(descStr) && descStr.startsWith("a")) {
            if (result.beginHour >= Constants.HalfDayHourCount) {
                result.beginHour -= Constants.HalfDayHourCount;
            }

            if (result.endHour >= Constants.HalfDayHourCount) {
                result.endHour -= Constants.HalfDayHourCount;
            }

            hasAm = true;
        } else if (!StringUtility.isNullOrEmpty(pmStr) || !StringUtility.isNullOrEmpty(descStr) && descStr.startsWith("p")) {
            if (result.beginHour < Constants.HalfDayHourCount) {
                result.beginHour += Constants.HalfDayHourCount;
            }

            if (result.endHour < Constants.HalfDayHourCount) {
                result.endHour += Constants.HalfDayHourCount;
            }

            hasPm = true;
        }

        if (!hasAm && !hasPm && result.beginHour <= Constants.HalfDayHourCount && result.endHour <= Constants.HalfDayHourCount) {
            if (result.beginHour > result.endHour) {
                if (result.beginHour == Constants.HalfDayHourCount) {
                    result.beginHour = 0;
                } else {
                    result.endHour += Constants.HalfDayHourCount;
                }
            }

            result.comments = Constants.Comment_AmPm;
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

        List<ExtractResult> timeExtractResults = config.getTimeExtractor().extract(text, referenceDate);
        List<ExtractResult> dateTimeExtractResults = config.getDateTimeExtractor().extract(text, referenceDate);

        if (dateTimeExtractResults.size() == 2) {
            pr1 = config.getDateTimeParser().parse(dateTimeExtractResults.get(0), referenceDate);
            pr2 = config.getDateTimeParser().parse(dateTimeExtractResults.get(1), referenceDate);
            bothHaveDates = true;
        } else if (dateTimeExtractResults.size() == 1 && timeExtractResults.size() == 2) {
            if (!dateTimeExtractResults.get(0).isOverlap(timeExtractResults.get(0))) {
                pr1 = config.getTimeParser().parse(timeExtractResults.get(0), referenceDate);
                pr2 = config.getDateTimeParser().parse(dateTimeExtractResults.get(0), referenceDate);
                endHasDate = true;
            } else {
                pr1 = config.getDateTimeParser().parse(dateTimeExtractResults.get(0), referenceDate);
                pr2 = config.getTimeParser().parse(timeExtractResults.get(1), referenceDate);
                beginHasDate = true;
            }
        } else if (dateTimeExtractResults.size() == 1 && timeExtractResults.size() == 1) {
            if (timeExtractResults.get(0).getStart() < dateTimeExtractResults.get(0).getStart()) {
                pr1 = config.getTimeParser().parse(timeExtractResults.get(0), referenceDate);
                pr2 = config.getDateTimeParser().parse(dateTimeExtractResults.get(0), referenceDate);
                endHasDate = true;
            } else if (timeExtractResults.get(0).getStart() >= dateTimeExtractResults.get(0).getStart() + dateTimeExtractResults.get(0).getLength()) {
                pr1 = config.getDateTimeParser().parse(dateTimeExtractResults.get(0), referenceDate);
                pr2 = config.getTimeParser().parse(timeExtractResults.get(0), referenceDate);
                beginHasDate = true;
            } else {
                // If the only TimeExtractResult is part of DateTimeExtractResult, then it should not be handled in this method
                return result;
            }
        } else if (timeExtractResults.size() == 2) {
            // If both ends are Time. then this is a TimePeriod, not a DateTimePeriod
            return result;
        } else {
            return result;
        }

        if (pr1.getValue() == null || pr2.getValue() == null) {
            return result;
        }

        LocalDateTime futureBegin = (LocalDateTime)((DateTimeResolutionResult)pr1.getValue()).getFutureValue();
        LocalDateTime futureEnd = (LocalDateTime)((DateTimeResolutionResult)pr2.getValue()).getFutureValue();

        LocalDateTime pastBegin = (LocalDateTime)((DateTimeResolutionResult)pr1.getValue()).getPastValue();
        LocalDateTime pastEnd = (LocalDateTime)((DateTimeResolutionResult)pr2.getValue()).getPastValue();

        if (bothHaveDates) {
            if (futureBegin.isAfter(futureEnd)) {
                futureBegin = pastBegin;
            }

            if (pastEnd.isBefore(pastBegin)) {
                pastEnd = futureEnd;
            }
        }

        if (bothHaveDates) {
            result.setTimex(String.format("(%s,%s,PT%dH)", pr1.getTimexStr(), pr2.getTimexStr(), Math.round(ChronoUnit.SECONDS.between(futureBegin, futureEnd) / 3600f)));
            // Do nothing
        } else if (beginHasDate) {
            futureEnd = DateUtil.safeCreateFromMinValue(futureBegin.toLocalDate(), futureEnd.toLocalTime());
            pastEnd = DateUtil.safeCreateFromMinValue(pastBegin.toLocalDate(), pastEnd.toLocalTime());

            String dateStr = pr1.getTimexStr().split("T")[0];
            result.setTimex(String.format("(%s,%s,PT%dH)", pr1.getTimexStr(), dateStr + pr2.getTimexStr(), ChronoUnit.HOURS.between(futureBegin, futureEnd)));
        } else if (endHasDate) {
            futureBegin = DateUtil.safeCreateFromMinValue(futureEnd.getYear(), futureEnd.getMonthValue(), futureEnd.getDayOfMonth(),
                    futureBegin.getHour(), futureBegin.getMinute(), futureBegin.getSecond());

            pastBegin = DateUtil.safeCreateFromMinValue(pastEnd.getYear(), pastEnd.getMonthValue(), pastEnd.getDayOfMonth(),
                    pastBegin.getHour(), pastBegin.getMinute(), pastBegin.getSecond());


            String dateStr = pr2.getTimexStr().split("T")[0];
            result.setTimex(String.format("(%s,%s,PT%dH)", dateStr + pr1.getTimexStr(), pr2.getTimexStr(), ChronoUnit.HOURS.between(futureBegin, futureEnd)));
        }

        DateTimeResolutionResult pr1Value = (DateTimeResolutionResult)pr1.getValue();
        DateTimeResolutionResult pr2Value = (DateTimeResolutionResult)pr2.getValue();

        String ampmStr1 = pr1Value.getComment();
        String ampmStr2 = pr2Value.getComment();

        if (!StringUtility.isNullOrEmpty(ampmStr1) && ampmStr1.endsWith(Constants.Comment_AmPm) &&
                !StringUtility.isNullOrEmpty(ampmStr2) && ampmStr2.endsWith(Constants.Comment_AmPm)) {
            result.setComment(Constants.Comment_AmPm);
        }

        if (this.config.getOptions().match(DateTimeOptions.EnablePreview)) {
            if (pr1Value.getTimeZoneResolution() != null) {
                result.setTimeZoneResolution(pr1Value.getTimeZoneResolution());
            }

            if (pr2Value.getTimeZoneResolution() != null) {
                result.setTimeZoneResolution(pr2Value.getTimeZoneResolution());
            }
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
    protected DateTimeResolutionResult parseSpecificTimeOfDay(String text, LocalDateTime referenceDate) {
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
                result.setMod(Constants.EARLY_MOD);
            }

            if (!hasEarly && !StringUtility.isNullOrEmpty(match.get().getGroup("late").value)) {
                hasLate = true;
                result.setComment(Constants.Comment_Late);
                result.setMod(Constants.LATE_MOD);
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

        if (RegexExtension.isExactMatch(config.getSpecificTimeOfDayRegex(), trimmedText, true)) {
            int swift = config.getSwiftPrefix(trimmedText);

            LocalDateTime date = referenceDate.plusDays(swift);
            int day = date.getDayOfMonth();
            int month = date.getMonthValue();
            int year = date.getYear();

            result.setTimex(DateTimeFormatUtil.formatDate(date) + timeStr);

            Pair<LocalDateTime, LocalDateTime> resultValue = new Pair<LocalDateTime, LocalDateTime>(
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
            String beforeStr = trimmedText.substring(0, match.get().index).trim();
            String afterStr = trimmedText.substring(match.get().index + match.get().length).trim();

            // Eliminate time period, if any
            List<ExtractResult> timePeriodErs = config.getTimePeriodExtractor().extract(beforeStr);
            if (timePeriodErs.size() > 0) {
                beforeStr = beforeStr.substring(0, timePeriodErs.get(0).getStart()) + beforeStr.substring(timePeriodErs.get(0).getStart() + timePeriodErs.get(0).getLength())
                        .trim();
            } else {
                timePeriodErs = config.getTimePeriodExtractor().extract(afterStr);
                if (timePeriodErs.size() > 0) {
                    afterStr = afterStr.substring(0, timePeriodErs.get(0).getStart()) + afterStr.substring(timePeriodErs.get(0).getStart() + timePeriodErs.get(0).getLength())
                            .trim();
                }
            }

            List<ExtractResult> ers = config.getDateExtractor().extract(beforeStr + " " + afterStr, referenceDate);

            if (ers.size() == 0 || ers.get(0).getLength() < beforeStr.length()) {
                boolean valid = false;

                if (ers.size() > 0 && ers.get(0).getStart() == 0) {
                    String midStr = beforeStr.substring(ers.get(0).getStart() + ers.get(0).getLength());
                    if (StringUtility.isNullOrWhiteSpace(midStr.replace(",", " "))) {
                        valid = true;
                    }
                }

                if (!valid) {
                    ers = config.getDateExtractor().extract(afterStr, referenceDate);

                    if (ers.size() == 0 || ers.get(0).getLength() != beforeStr.length()) {
                        if (ers.size() > 0 && ers.get(0).getStart() + ers.get(0).getLength() == afterStr.length()) {
                            String midStr = afterStr.substring(0, ers.get(0).getStart());
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
                    Pair<LocalDateTime, LocalDateTime> periodFuture = (Pair<LocalDateTime, LocalDateTime>)((DateTimeResolutionResult)timePr.getValue()).getFutureValue();
                    Pair<LocalDateTime, LocalDateTime> periodPast = (Pair<LocalDateTime, LocalDateTime>)((DateTimeResolutionResult)timePr.getValue()).getPastValue();

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
            LocalDateTime futureDate = (LocalDateTime)((DateTimeResolutionResult)pr.getValue()).getFutureValue();
            LocalDateTime pastDate = (LocalDateTime)((DateTimeResolutionResult)pr.getValue()).getPastValue();

            if (!hasSpecificTimePeriod) {
                result.setTimex(pr.getTimexStr() + timeStr);
            } else {
                result.setTimex(String.format("(%sT%d,%sT%d,PT%dH)", pr.getTimexStr(), beginHour, pr.getTimexStr(), endHour, endHour - beginHour));
            }

            Pair<LocalDateTime, LocalDateTime> futureResult = new Pair<LocalDateTime, LocalDateTime>(
                    DateUtil.safeCreateFromMinValue(
                            futureDate.getYear(), futureDate.getMonthValue(), futureDate.getDayOfMonth(),
                            beginHour, 0, 0),
                    DateUtil.safeCreateFromMinValue(
                            futureDate.getYear(), futureDate.getMonthValue(), futureDate.getDayOfMonth(),
                            endHour, endMin, endMin)
            );

            Pair<LocalDateTime, LocalDateTime> pastResult = new Pair<LocalDateTime, LocalDateTime>(
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
    private DateTimeResolutionResult parseDuration(String text, LocalDateTime referenceTime) {
        DateTimeResolutionResult result = new DateTimeResolutionResult();

        // For the rest of datetime, it will be handled in next function
        if (RegExpUtility.getMatches(config.getRestOfDateTimeRegex(), text).length > 0) {
            return result;
        }

        List<ExtractResult> ers = config.getDurationExtractor().extract(text, referenceTime);

        if (ers.size() == 1) {
            ParseResult pr = config.getDurationParser().parse(ers.get(0));

            String beforeStr = text.substring(0, pr.getStart()).trim().toLowerCase();
            String afterStr = text.substring(pr.getStart() + pr.getLength()).trim().toLowerCase();

            List<ExtractResult> numbersInSuffix = config.getCardinalExtractor().extract(beforeStr);
            List<ExtractResult> numbersInDuration = config.getCardinalExtractor().extract(ers.get(0).getText());

            // Handle cases like "2 upcoming days", "5 previous years"
            if (!numbersInSuffix.isEmpty() && numbersInDuration.isEmpty()) {
                ExtractResult numberEr = numbersInSuffix.get(0);
                String numberText = numberEr.getText();
                String durationText = ers.get(0).getText();
                String combinedText = String.format("%s %s", numberText, durationText);
                List<ExtractResult> combinedDurationEr = config.getDurationExtractor().extract(combinedText, referenceTime);

                if (!combinedDurationEr.isEmpty()) {
                    pr = config.getDurationParser().parse(combinedDurationEr.get(0));
                    int startIndex = numberEr.getStart() + numberEr.getLength();
                    beforeStr = beforeStr.substring(startIndex).trim();
                }
            }

            if (pr.getValue() != null) {
                int swiftSeconds = 0;
                String mod = "";
                DateTimeResolutionResult durationResult = (DateTimeResolutionResult)pr.getValue();

                if (durationResult.getPastValue() instanceof Double && durationResult.getFutureValue() instanceof Double) {
                    swiftSeconds = Math.round(((Double)durationResult.getPastValue()).floatValue());
                }

                LocalDateTime beginTime = referenceTime;
                LocalDateTime endTime = referenceTime;

                if (RegexExtension.isExactMatch(config.getPastRegex(), beforeStr, true)) {
                    mod = Constants.BEFORE_MOD;
                    beginTime = referenceTime.minusSeconds(swiftSeconds);
                }

                // Handle the "within (the) (next) xx seconds/minutes/hours" case
                // Should also handle the multiple duration case like P1DT8H
                // Set the beginTime equal to reference time for now
                if (RegexExtension.isExactMatch(config.getWithinNextPrefixRegex(), beforeStr, true)) {
                    endTime = beginTime.plusSeconds(swiftSeconds);
                }

                if (RegexExtension.isExactMatch(config.getFutureRegex(), beforeStr, true)) {
                    mod = Constants.AFTER_MOD;
                    endTime = beginTime.plusSeconds(swiftSeconds);
                }

                if (RegexExtension.isExactMatch(config.getPastRegex(), afterStr, true)) {
                    mod = Constants.BEFORE_MOD;
                    beginTime = referenceTime.minusSeconds(swiftSeconds);
                }

                if (RegexExtension.isExactMatch(config.getFutureRegex(), afterStr, true)) {
                    mod = Constants.AFTER_MOD;
                    endTime = beginTime.plusSeconds(swiftSeconds);
                }

                if (RegexExtension.isExactMatch(config.getFutureSuffixRegex(), afterStr, true)) {
                    mod = Constants.AFTER_MOD;
                    endTime = beginTime.plusSeconds(swiftSeconds);
                }

                result.setTimex(String.format("(%sT%s,%sT%s,%s)",
                        DateTimeFormatUtil.luisDate(beginTime),
                        DateTimeFormatUtil.luisTime(beginTime),
                        DateTimeFormatUtil.luisDate(endTime),
                        DateTimeFormatUtil.luisTime(endTime),
                        durationResult.getTimex()
                ));

                Pair<LocalDateTime, LocalDateTime> resultValue = new Pair<LocalDateTime, LocalDateTime>(beginTime, endTime);

                result.setFutureValue(resultValue);
                result.setPastValue(resultValue);

                result.setSuccess(true);

                if (!StringUtility.isNullOrEmpty(mod)) {
                    ((DateTimeResolutionResult)pr.getValue()).setMod(mod);
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
                        DateTimeFormatUtil.luisDate(beginTime),
                        DateTimeFormatUtil.luisTime(beginTime),
                        DateTimeFormatUtil.luisDate(endTime),
                        DateTimeFormatUtil.luisTime(endTime),
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
            String beforeStr = StringUtility.trimEnd(text.substring(0, dateResult.get(dateResult.size() - 1).getStart()));
            Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(config.getPrefixDayRegex(), beforeStr)).findFirst();
            if (match.isPresent()) {
                DateTimeParseResult pr = config.getDateParser().parse(dateResult.get(dateResult.size() - 1), referenceDate);
                if (pr.getValue() != null) {
                    LocalDateTime startTime = (LocalDateTime)((DateTimeResolutionResult)pr.getValue()).getFutureValue();
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

                    result.setTimex(pr.getTimexStr());

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
            int dateStrEnd = dateEr.get().getStart() + dateEr.get().getLength();

            if (dateStrEnd < timeEr.get().getStart()) {
                String midStr = text.substring(dateStrEnd, timeEr.get().getStart());

                if (isValidConnectorForDateAndTimePeriod(midStr)) {
                    DateTimeParseResult datePr = config.getDateParser().parse(dateEr.get(), referenceDate);
                    DateTimeParseResult timePr = config.getTimeParser().parse(timeEr.get(), referenceDate);

                    if (datePr != null && timePr != null) {
                        DateTimeResolutionResult timeResolutionResult = (DateTimeResolutionResult)timePr.getValue();
                        DateTimeResolutionResult dateResolutionResult = (DateTimeResolutionResult)datePr.getValue();
                        LocalDateTime futureDateValue = (LocalDateTime)dateResolutionResult.getFutureValue();
                        LocalDateTime pastDateValue = (LocalDateTime)dateResolutionResult.getPastValue();
                        LocalDateTime futureTimeValue = (LocalDateTime)timeResolutionResult.getFutureValue();
                        LocalDateTime pastTimeValue = (LocalDateTime)timeResolutionResult.getPastValue();

                        result.setComment(timeResolutionResult.getComment());
                        result.setTimex(datePr.getTimexStr() + timePr.getTimexStr());

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
            ConditionalMatch match = RegexExtension.matchExact(regex, text, true);
            if (match.getSuccess()) {
                return true;
            }
        }

        return false;
    }

    private class ParseTimePeriodResult {
        String comments;
        int beginHour;
        int endHour;
    }
}
