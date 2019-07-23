package com.microsoft.recognizers.text.datetime.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.TimeTypeConstants;
import com.microsoft.recognizers.text.datetime.parsers.config.ITimePeriodParserConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.config.MatchedTimeRangeResult;
import com.microsoft.recognizers.text.datetime.utilities.ConditionalMatch;
import com.microsoft.recognizers.text.datetime.utilities.DateTimeFormatUtil;
import com.microsoft.recognizers.text.datetime.utilities.DateTimeResolutionResult;
import com.microsoft.recognizers.text.datetime.utilities.DateUtil;
import com.microsoft.recognizers.text.datetime.utilities.RegexExtension;
import com.microsoft.recognizers.text.datetime.utilities.TimeZoneUtility;
import com.microsoft.recognizers.text.utilities.Capture;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.MatchGroup;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import com.microsoft.recognizers.text.utilities.StringUtility;

import java.time.Duration;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Comparator;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Optional;
import org.javatuples.Pair;

public class BaseTimePeriodParser implements IDateTimeParser {

    private final ITimePeriodParserConfiguration config;

    private static final String parserName = Constants.SYS_DATETIME_TIMEPERIOD; //"TimePeriod";

    public BaseTimePeriodParser(ITimePeriodParserConfiguration config) {
        this.config = config;
    }

    @Override
    public String getParserName() {
        return parserName;
    }

    @Override
    public ParseResult parse(ExtractResult extractResult) {
        return this.parse(extractResult, LocalDateTime.now());
    }

    @Override
    public DateTimeParseResult parse(ExtractResult er, LocalDateTime reference) {
        Object value = null;

        if (er.getType().equals(getParserName())) {

            DateTimeResolutionResult innerResult;

            if (TimeZoneUtility.shouldResolveTimeZone(er, config.getOptions())) {
                Map<String, Object> metadata = (HashMap<String, Object>)er.getData();

                ExtractResult timezoneEr = (ExtractResult)metadata.get(Constants.SYS_DATETIME_TIMEZONE);
                ParseResult timezonePr = config.getTimeZoneParser().parse(timezoneEr);

                innerResult = internalParse(er.getText().substring(0, er.getLength() - timezoneEr.getLength()), reference);

                if (timezonePr.getValue() != null) {
                    innerResult.setTimeZoneResolution(((DateTimeResolutionResult)timezonePr.getValue()).getTimeZoneResolution());
                }
            } else {
                innerResult = internalParse(er.getText(), reference);
            }

            if (innerResult.getSuccess()) {
                ImmutableMap.Builder<String, String> futureResolution = ImmutableMap.builder();
                futureResolution.put(
                        TimeTypeConstants.START_TIME,
                        DateTimeFormatUtil.formatTime(((Pair<LocalDateTime, LocalDateTime>)innerResult.getFutureValue()).getValue0()));
                futureResolution.put(
                        TimeTypeConstants.END_TIME,
                        DateTimeFormatUtil.formatTime(((Pair<LocalDateTime, LocalDateTime>)innerResult.getFutureValue()).getValue1()));

                innerResult.setFutureResolution(futureResolution.build());

                ImmutableMap.Builder<String, String> pastResolution = ImmutableMap.builder();
                pastResolution.put(
                        TimeTypeConstants.START_TIME,
                        DateTimeFormatUtil.formatTime(((Pair<LocalDateTime, LocalDateTime>)innerResult.getPastValue()).getValue0()));
                pastResolution.put(
                        TimeTypeConstants.END_TIME,
                        DateTimeFormatUtil.formatTime(((Pair<LocalDateTime, LocalDateTime>)innerResult.getPastValue()).getValue1()));

                innerResult.setPastResolution(pastResolution.build());

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
        DateTimeResolutionResult innerResult = parseSimpleCases(text, reference);

        if (!innerResult.getSuccess()) {
            innerResult = mergeTwoTimePoints(text, reference);
        }

        if (!innerResult.getSuccess()) {
            innerResult = parseTimeOfDay(text, reference);
        }

        return innerResult;
    }

    // Cases like "from 3 to 5am" or "between 3:30 and 5" are parsed here
    private DateTimeResolutionResult parseSimpleCases(String text, LocalDateTime referenceTime) {
        // Cases like "from 3 to 5pm" or "between 4 and 6am", time point is pure number without colon
        DateTimeResolutionResult ret = parsePureNumCases(text, referenceTime);

        if (!ret.getSuccess()) {
            // Cases like "from 3:30 to 5" or "netween 3:30am to 6pm", at least one of the time point contains colon
            ret = parseSpecificTimeCases(text, referenceTime);
        }

        return ret;
    }

    // Cases like "from 3 to 5pm" or "between 4 and 6am", time point is pure number without colon
    private DateTimeResolutionResult parsePureNumCases(String text, LocalDateTime referenceTime) {
        DateTimeResolutionResult ret = new DateTimeResolutionResult();

        int year = referenceTime.getYear();
        int month = referenceTime.getMonthValue();
        int day = referenceTime.getDayOfMonth();
        String trimmedText = text.trim().toLowerCase();

        ConditionalMatch match = RegexExtension.matchBegin(this.config.getPureNumberFromToRegex(), trimmedText, true);

        if (!match.getSuccess()) {
            match = RegexExtension.matchBegin(this.config.getPureNumberBetweenAndRegex(), trimmedText, true);
        }

        if (match.getSuccess()) {
            // this "from .. to .." pattern is valid if followed by a Date OR Constants.PmGroupName
            boolean isValid = false;

            // get hours
            MatchGroup hourGroup = match.getMatch().get().getGroup(Constants.HourGroupName);
            String hourStr = hourGroup.captures[0].value;
            int afterHourIndex = hourGroup.captures[0].index + hourGroup.captures[0].length;

            // hard to integrate this part into the regex
            if (afterHourIndex == trimmedText.length() || !trimmedText.substring(afterHourIndex).trim().startsWith(":")) {

                int beginHour;
                if (!this.config.getNumbers().containsKey(hourStr)) {
                    beginHour = Integer.parseInt(hourStr);
                } else {
                    beginHour = this.config.getNumbers().get(hourStr);
                }

                hourStr = hourGroup.captures[1].value;
                afterHourIndex = hourGroup.captures[1].index + hourGroup.captures[1].length;

                if (afterHourIndex == trimmedText.length() || !trimmedText.substring(afterHourIndex).trim().startsWith(":")) {
                    int endHour;
                    if (!this.config.getNumbers().containsKey(hourStr)) {
                        endHour = Integer.parseInt(hourStr);
                    } else {
                        endHour = this.config.getNumbers().get(hourStr);
                    }

                    // parse Constants.PmGroupName 
                    String leftDesc = match.getMatch().get().getGroup("leftDesc").value;
                    String rightDesc = match.getMatch().get().getGroup("rightDesc").value;
                    String pmStr = match.getMatch().get().getGroup(Constants.PmGroupName).value;
                    String amStr = match.getMatch().get().getGroup(Constants.AmGroupName).value;
                    String descStr = match.getMatch().get().getGroup(Constants.DescGroupName).value;

                    // The "ampm" only occurs in time, we don't have to consider it here
                    if (StringUtility.isNullOrEmpty(leftDesc)) {

                        boolean rightAmValid = !StringUtility.isNullOrEmpty(rightDesc) &&
                                Arrays.stream(RegExpUtility.getMatches(config.getUtilityConfiguration().getAmDescRegex(), rightDesc.toLowerCase())).findFirst().isPresent();
                        boolean rightPmValid = !StringUtility.isNullOrEmpty(rightDesc) &&
                                Arrays.stream(RegExpUtility.getMatches(config.getUtilityConfiguration().getPmDescRegex(), rightDesc.toLowerCase())).findFirst().isPresent();

                        if (!StringUtility.isNullOrEmpty(amStr) || rightAmValid) {
                            if (endHour >= Constants.HalfDayHourCount) {
                                endHour -= Constants.HalfDayHourCount;
                            }

                            if (beginHour >= Constants.HalfDayHourCount && beginHour - Constants.HalfDayHourCount < endHour) {
                                beginHour -= Constants.HalfDayHourCount;
                            }

                            // Resolve case like "11 to 3am"
                            if (beginHour < Constants.HalfDayHourCount && beginHour > endHour) {
                                beginHour += Constants.HalfDayHourCount;
                            }

                            isValid = true;

                        } else if (!StringUtility.isNullOrEmpty(pmStr) || rightPmValid) {

                            if (endHour < Constants.HalfDayHourCount) {
                                endHour += Constants.HalfDayHourCount;
                            }

                            // Resolve case like "11 to 3pm"
                            if (beginHour + Constants.HalfDayHourCount < endHour) {
                                beginHour += Constants.HalfDayHourCount;
                            }

                            isValid = true;

                        }
                    }

                    if (isValid) {
                        String beginStr = String.format("T%02d", beginHour);
                        String endStr = String.format("T%02d", endHour);

                        if (endHour >= beginHour) {
                            ret.setTimex(String.format("(%s,%s,PT%sH)", beginStr, endStr, (endHour - beginHour)));
                        } else {
                            ret.setTimex(String.format("(%s,%s,PT%sH)", beginStr, endStr, (endHour - beginHour + 24)));
                        }

                        // Try to get the timezone resolution
                        List<ExtractResult> timeErs = config.getTimeExtractor().extract(trimmedText);
                        for (ExtractResult er : timeErs) {
                            DateTimeParseResult pr = config.getTimeParser().parse(er, referenceTime);
                            if (((DateTimeResolutionResult)pr.getValue()).getTimeZoneResolution() != null) {
                                ret.setTimeZoneResolution(((DateTimeResolutionResult)pr.getValue()).getTimeZoneResolution());
                                break;
                            }
                        }

                        ret.setFutureValue(
                                new Pair<LocalDateTime, LocalDateTime>(DateUtil.safeCreateFromMinValue(year, month, day, beginHour, 0, 0),
                                        DateUtil.safeCreateFromMinValue(year, month, day, endHour, 0, 0)));
                        ret.setPastValue(
                                new Pair<LocalDateTime, LocalDateTime>(DateUtil.safeCreateFromMinValue(year, month, day, beginHour, 0, 0),
                                        DateUtil.safeCreateFromMinValue(year, month, day, endHour, 0, 0)));

                        ret.setSuccess(true);
                    }
                }
            }
        }

        return ret;
    }

    // Cases like "from 3:30 to 5" or "between 3:30am to 6pm", at least one of the time point contains colon
    private DateTimeResolutionResult parseSpecificTimeCases(String text, LocalDateTime referenceTime) {
        DateTimeResolutionResult ret = new DateTimeResolutionResult();

        int year = referenceTime.getYear();
        int month = referenceTime.getMonthValue();
        int day = referenceTime.getDayOfMonth();

        // Handle cases like "from 4:30 to 5"
        ConditionalMatch match = RegexExtension.matchExact(config.getSpecificTimeFromToRegex(), text, true);

        if (!match.getSuccess()) {
            // Handle cases like "between 5:10 and 7"
            match = RegexExtension.matchExact(config.getSpecificTimeBetweenAndRegex(), text, true);
        }

        if (match.getSuccess()) {
            // Cases like "half past seven" are not handled here
            if (!match.getMatch().get().getGroup(Constants.PrefixGroupName).value.equals("")) {
                return ret;
            }

            // Cases like "4" is different with "4:00" as the Timex is different "T04H" vs "T04H00M"
            // Uses this invalidFlag to differentiate
            int beginHour;
            int beginMinute = Constants.InvalidMinute;
            int beginSecond = Constants.InvalidSecond;
            int endHour;
            int endMinute = Constants.InvalidMinute;
            int endSecond = Constants.InvalidSecond;

            // Get time1 and time2
            MatchGroup hourGroup = match.getMatch().get().getGroup(Constants.HourGroupName);

            String hourStr = hourGroup.captures[0].value;

            if (config.getNumbers().containsKey(hourStr)) {
                beginHour = config.getNumbers().get(hourStr);
            } else {
                beginHour = Integer.parseInt(hourStr);
            }


            hourStr = hourGroup.captures[1].value;

            if (config.getNumbers().containsKey(hourStr)) {
                endHour = config.getNumbers().get(hourStr);
            } else {
                endHour = Integer.parseInt(hourStr);
            }

            int time1StartIndex = match.getMatch().get().getGroup("time1").index;
            int time1EndIndex = time1StartIndex + match.getMatch().get().getGroup("time1").length;
            int time2StartIndex = match.getMatch().get().getGroup("time2").index;
            int time2EndIndex = time2StartIndex + match.getMatch().get().getGroup("time2").length;

            // Get beginMinute (if exists) and endMinute (if exists)
            for (int i = 0; i < match.getMatch().get().getGroup(Constants.MinuteGroupName).captures.length; i++) {
                Capture minuteCapture = match.getMatch().get().getGroup(Constants.MinuteGroupName).captures[i];
                if (minuteCapture.index >= time1StartIndex && (minuteCapture.index + minuteCapture.length) <= time1EndIndex) {
                    beginMinute = Integer.parseInt(minuteCapture.value);
                } else if (minuteCapture.index >= time2StartIndex && (minuteCapture.index + minuteCapture.length) <= time2EndIndex) {
                    endMinute = Integer.parseInt(minuteCapture.value);
                }
            }

            // Get beginSecond (if exists) and endSecond (if exists)
            for (int i = 0; i < match.getMatch().get().getGroup(Constants.SecondGroupName).captures.length; i++) {
                Capture secondCapture = match.getMatch().get().getGroup(Constants.SecondGroupName).captures[i];
                if (secondCapture.index >= time1StartIndex && (secondCapture.index + secondCapture.length) <= time1EndIndex) {
                    beginSecond = Integer.parseInt(secondCapture.value);
                } else if (secondCapture.index >= time2StartIndex && (secondCapture.index + secondCapture.length) <= time2EndIndex) {
                    endSecond = Integer.parseInt(secondCapture.value);
                }
            }

            // Desc here means descriptions like "am / pm / o'clock"
            // Get leftDesc (if exists) and rightDesc (if exists)
            String leftDesc = match.getMatch().get().getGroup("leftDesc").value;
            String rightDesc = match.getMatch().get().getGroup("rightDesc").value;

            for (int i = 0; i < match.getMatch().get().getGroup(Constants.DescGroupName).captures.length; i++) {
                Capture descCapture = match.getMatch().get().getGroup(Constants.DescGroupName).captures[i];
                if (descCapture.index >= time1StartIndex && (descCapture.index + descCapture.length) <= time1EndIndex && StringUtility.isNullOrEmpty(leftDesc)) {
                    leftDesc = descCapture.value;
                } else if (descCapture.index >= time2StartIndex && (descCapture.index + descCapture.length) <= time2EndIndex && StringUtility.isNullOrEmpty(rightDesc)) {
                    rightDesc = descCapture.value;
                }
            }

            LocalDateTime beginDateTime = DateUtil.safeCreateFromMinValue(
                    year,
                    month,
                    day,
                    beginHour,
                    beginMinute >= 0 ? beginMinute : 0,
                    beginSecond >= 0 ? beginSecond : 0);

            LocalDateTime endDateTime = DateUtil.safeCreateFromMinValue(
                    year,
                    month,
                    day,
                    endHour,
                    endMinute >= 0 ? endMinute : 0,
                    endSecond >= 0 ? endSecond : 0);

            boolean hasLeftAm = !StringUtility.isNullOrEmpty(leftDesc) && leftDesc.toLowerCase().startsWith("a");
            boolean hasLeftPm = !StringUtility.isNullOrEmpty(leftDesc) && leftDesc.toLowerCase().startsWith("p");
            boolean hasRightAm = !StringUtility.isNullOrEmpty(rightDesc) && rightDesc.toLowerCase().startsWith("a");
            boolean hasRightPm = !StringUtility.isNullOrEmpty(rightDesc) && rightDesc.toLowerCase().startsWith("p");
            boolean hasLeft = hasLeftAm || hasLeftPm;
            boolean hasRight = hasRightAm || hasRightPm;

            // Both timepoint has description like 'am' or 'pm'
            if (hasLeft && hasRight) {
                if (hasLeftAm) {
                    if (beginHour >= Constants.HalfDayHourCount) {
                        beginDateTime = beginDateTime.minusHours(Constants.HalfDayHourCount);
                    }
                } else if (hasLeftPm) {
                    if (beginHour < Constants.HalfDayHourCount) {
                        beginDateTime = beginDateTime.plusHours(Constants.HalfDayHourCount);
                    }
                }

                if (hasRightAm) {
                    if (endHour > Constants.HalfDayHourCount) {
                        endDateTime = endDateTime.minusHours(Constants.HalfDayHourCount);
                    }
                } else if (hasRightPm) {
                    if (endHour < Constants.HalfDayHourCount) {
                        endDateTime = endDateTime.plusHours(Constants.HalfDayHourCount);
                    }
                }
            } else if (hasLeft || hasRight) { // one of the timepoint has description like 'am' or 'pm'
                if (hasLeftAm) {
                    if (beginHour >= Constants.HalfDayHourCount) {
                        beginDateTime = beginDateTime.minusHours(Constants.HalfDayHourCount);
                    }

                    if (endHour < Constants.HalfDayHourCount) {
                        if (endDateTime.isBefore(beginDateTime)) {
                            endDateTime = endDateTime.plusHours(Constants.HalfDayHourCount);
                        }
                    }
                } else if (hasLeftPm) {
                    if (beginHour < Constants.HalfDayHourCount) {
                        beginDateTime = beginDateTime.plusHours(Constants.HalfDayHourCount);
                    }

                    if (endHour < Constants.HalfDayHourCount) {
                        if (endDateTime.isBefore(beginDateTime)) {
                            Duration span = Duration.between(endDateTime, beginDateTime).abs();
                            if (span.toHours() >= Constants.HalfDayHourCount) {
                                endDateTime = endDateTime.plusHours(24);
                            } else {
                                endDateTime = endDateTime.plusHours(Constants.HalfDayHourCount);
                            }
                        }
                    }
                }

                if (hasRightAm) {
                    if (endHour >= Constants.HalfDayHourCount) {
                        endDateTime = endDateTime.minusHours(Constants.HalfDayHourCount);
                    }

                    if (beginHour < Constants.HalfDayHourCount) {
                        if (endDateTime.isBefore(beginDateTime)) {
                            beginDateTime = beginDateTime.minusHours(Constants.HalfDayHourCount);
                        }
                    }
                } else if (hasRightPm) {
                    if (endHour < Constants.HalfDayHourCount) {
                        endDateTime = endDateTime.plusHours(Constants.HalfDayHourCount);
                    }

                    if (beginHour < Constants.HalfDayHourCount) {
                        if (endDateTime.isBefore(beginDateTime)) {
                            beginDateTime = beginDateTime.minusHours(Constants.HalfDayHourCount);
                        } else {
                            Duration span = Duration.between(beginDateTime, endDateTime);
                            if (span.toHours() > Constants.HalfDayHourCount) {
                                beginDateTime = beginDateTime.plusHours(Constants.HalfDayHourCount);
                            }
                        }
                    }
                }
            } else if (beginHour <= Constants.HalfDayHourCount && endHour <= Constants.HalfDayHourCount) {
                // No 'am' or 'pm' indicator
                if (beginHour > endHour) {
                    if (beginHour == Constants.HalfDayHourCount) {
                        beginDateTime = beginDateTime.minusHours(Constants.HalfDayHourCount);
                    } else {
                        endDateTime = endDateTime.plusHours(Constants.HalfDayHourCount);
                    }
                }
                ret.setComment(Constants.Comment_AmPm);
            }

            if (endDateTime.isBefore(beginDateTime)) {
                endDateTime = endDateTime.plusHours(24);
            }

            String beginStr = DateTimeFormatUtil.shortTime(beginDateTime.getHour(), beginMinute, beginSecond);
            String endStr = DateTimeFormatUtil.shortTime(endDateTime.getHour(), endMinute, endSecond);

            ret.setSuccess(true);

            ret.setTimex(String.format("(%s,%s,%s)", beginStr, endStr, DateTimeFormatUtil.luisTimeSpan(Duration.between(beginDateTime, endDateTime))));

            ret.setFutureValue(new Pair<LocalDateTime, LocalDateTime>(beginDateTime, endDateTime));
            ret.setPastValue(new Pair<LocalDateTime, LocalDateTime>(beginDateTime, endDateTime));

            List<Object> subDateTimeEntities = new ArrayList<>();

            // In SplitDateAndTime mode, time points will be get from these SubDateTimeEntities
            // Cases like "from 4 to 5pm", "4" should not be treated as SubDateTimeEntity
            if (hasLeft || beginMinute != Constants.InvalidMinute || beginSecond != Constants.InvalidSecond) {
                ExtractResult er = new ExtractResult(
                        time1StartIndex,
                        time1EndIndex - time1StartIndex,
                        text.substring(time1StartIndex, time1EndIndex),
                        Constants.SYS_DATETIME_TIME);

                DateTimeParseResult pr = this.config.getTimeParser().parse(er, referenceTime);
                subDateTimeEntities.add(pr);
            }

            // Cases like "from 4am to 5", "5" should not be treated as SubDateTimeEntity
            if (hasRight || endMinute != Constants.InvalidMinute || endSecond != Constants.InvalidSecond) {
                ExtractResult er = new ExtractResult(

                        time2StartIndex,
                        time2EndIndex - time2StartIndex,
                        text.substring(time2StartIndex, time2EndIndex),
                        Constants.SYS_DATETIME_TIME
                );

                DateTimeParseResult pr = this.config.getTimeParser().parse(er, referenceTime);
                subDateTimeEntities.add(pr);
            }
            ret.setSubDateTimeEntities(subDateTimeEntities);
            ret.setSuccess(true);
        }

        return ret;
    }

    private DateTimeResolutionResult mergeTwoTimePoints(String text, LocalDateTime referenceTime) {
        DateTimeResolutionResult ret = new DateTimeResolutionResult();
        DateTimeParseResult pr1 = null;
        DateTimeParseResult pr2 = null;
        boolean validTimeNumber = false;

        List<ExtractResult> ers = this.config.getTimeExtractor().extract(text, referenceTime);
        if (ers.size() != 2) {
            if (ers.size() == 1) {
                List<ExtractResult> numErs = this.config.getIntegerExtractor().extract(text);
                int erStart = ers.get(0).getStart() != null ? ers.get(0).getStart() : 0;
                int erLength = ers.get(0).getLength() != null ? ers.get(0).getLength() : 0;

                for (ExtractResult num : numErs) {
                    int numStart = num.getStart() != null ? num.getStart() : 0;
                    int numLength = num.getLength() != null ? num.getLength() : 0;
                    int midStrBegin = 0;
                    int midStrEnd = 0;
                    // ending number
                    if (numStart > erStart + erLength) {
                        midStrBegin = erStart + erLength;
                        midStrEnd = numStart - midStrBegin;
                    } else if (numStart + numLength < erStart) {
                        midStrBegin = numStart + numLength;
                        midStrEnd = erStart - midStrBegin;
                    }

                    // check if the middle string between the time point and the valid number is a connect string.
                    String middleStr = text.substring(midStrBegin, midStrBegin + midStrEnd);
                    Optional<Match> tillMatch = Arrays.stream(RegExpUtility.getMatches(this.config.getTillRegex(), middleStr)).findFirst();
                    if (tillMatch.isPresent()) {
                        num.setData(null);
                        num.setType(Constants.SYS_DATETIME_TIME);
                        ers.add(num);
                        validTimeNumber = true;
                        break;
                    }
                }

                ers.sort(Comparator.comparingInt(x -> x.getStart()));
            }

            if (!validTimeNumber) {
                return ret;
            }
        }

        pr1 = this.config.getTimeParser().parse(ers.get(0), referenceTime);
        pr2 = this.config.getTimeParser().parse(ers.get(1), referenceTime);

        if (pr1.getValue() == null || pr2.getValue() == null) {
            return ret;
        }

        String ampmStr1 = ((DateTimeResolutionResult)pr1.getValue()).getComment();
        String ampmStr2 = ((DateTimeResolutionResult)pr2.getValue()).getComment();

        LocalDateTime beginTime = (LocalDateTime)((DateTimeResolutionResult)pr1.getValue()).getFutureValue();
        LocalDateTime endTime = (LocalDateTime)((DateTimeResolutionResult)pr2.getValue()).getFutureValue();

        if (!StringUtility.isNullOrEmpty(ampmStr2) && ampmStr2.endsWith(Constants.Comment_AmPm) &&
                (endTime.compareTo(beginTime) < 1) && endTime.plusHours(Constants.HalfDayHourCount).isAfter(beginTime)) {
            endTime = endTime.plusHours(Constants.HalfDayHourCount);
            ((DateTimeResolutionResult)pr2.getValue()).setFutureValue(endTime);
            pr2.setTimexStr(String.format("T%s", endTime.getHour()));
            if (endTime.getMinute() > 0) {
                pr2.setTimexStr(String.format("%s:%s", pr2.getTimexStr(), endTime.getMinute()));
            }
        }

        if (!StringUtility.isNullOrEmpty(ampmStr1) && ampmStr1.endsWith(Constants.Comment_AmPm) && endTime.isAfter(beginTime.plusHours(Constants.HalfDayHourCount))) {
            beginTime = beginTime.plusHours(Constants.HalfDayHourCount);
            ((DateTimeResolutionResult)pr1.getValue()).setFutureValue(beginTime);
            pr1.setTimexStr(String.format("T%s", beginTime.getHour()));
            if (beginTime.getMinute() > 0) {
                pr1.setTimexStr(String.format("%s:%s", pr1.getTimexStr(), beginTime.getMinute()));
            }
        }

        if (endTime.isBefore(beginTime)) {
            endTime = endTime.plusDays(1);
        }

        long minutes = (Duration.between(beginTime, endTime).toMinutes() % 60);
        long hours = (Duration.between(beginTime, endTime).toHours() % 24);
        ret.setTimex(String.format("(%s,%s,PT", pr1.getTimexStr(), pr2.getTimexStr()));

        if (hours > 0) {
            ret.setTimex(String.format("%s%sH", ret.getTimex(), hours));
        }
        if (minutes > 0) {
            ret.setTimex(String.format("%s%sM", ret.getTimex(), minutes));
        }
        ret.setTimex(ret.getTimex() + ")");

        ret.setFutureValue(new Pair<LocalDateTime, LocalDateTime>(beginTime, endTime));
        ret.setPastValue(new Pair<LocalDateTime, LocalDateTime>(beginTime, endTime));
        ret.setSuccess(true);

        if (!StringUtility.isNullOrEmpty(ampmStr1) && ampmStr1.endsWith(Constants.Comment_AmPm) &&
                !StringUtility.isNullOrEmpty(ampmStr2) && ampmStr2.endsWith(Constants.Comment_AmPm)) {
            ret.setComment(Constants.Comment_AmPm);
        }

        if (((DateTimeResolutionResult)pr1.getValue()).getTimeZoneResolution() != null) {
            ret.setTimeZoneResolution(((DateTimeResolutionResult)pr1.getValue()).getTimeZoneResolution());
        } else if (((DateTimeResolutionResult)pr2.getValue()).getTimeZoneResolution() != null) {
            ret.setTimeZoneResolution(((DateTimeResolutionResult)pr2.getValue()).getTimeZoneResolution());
        }

        List<Object> subDateTimeEntities = new ArrayList<>();
        subDateTimeEntities.add(pr1);
        subDateTimeEntities.add(pr2);
        ret.setSubDateTimeEntities(subDateTimeEntities);

        return ret;
    }

    private DateTimeResolutionResult parseTimeOfDay(String text, LocalDateTime referenceTime) {
        int day = referenceTime.getDayOfMonth();
        int month = referenceTime.getMonthValue();
        int year = referenceTime.getYear();
        DateTimeResolutionResult ret = new DateTimeResolutionResult();

        // extract early/late prefix from text
        Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(this.config.getTimeOfDayRegex(), text)).findFirst();
        boolean hasEarly = false;
        boolean hasLate = false;
        if (match.isPresent()) {
            if (!StringUtility.isNullOrEmpty(match.get().getGroup("early").value)) {
                String early = match.get().getGroup("early").value;
                text = text.replace(early, "");
                hasEarly = true;
                ret.setComment(Constants.Comment_Early);
                ret.setMod(Constants.EARLY_MOD);
            }

            if (!hasEarly && !StringUtility.isNullOrEmpty(match.get().getGroup("late").value)) {
                String late = match.get().getGroup("late").value;
                text = text.replace(late, "");
                hasLate = true;
                ret.setComment(Constants.Comment_Late);
                ret.setMod(Constants.LATE_MOD);
            }
        }
        MatchedTimeRangeResult timexResult = this.config.getMatchedTimexRange(text, "", 0, 0, 0);
        if (!timexResult.getMatched()) {
            return new DateTimeResolutionResult();
        }

        // modify time period if "early" or "late" is existed
        if (hasEarly) {
            timexResult.setEndHour(timexResult.getBeginHour() + 2);
            // handling case: night end with 23:59
            if (timexResult.getEndMin() == 59) {
                timexResult.setEndMin(0);
            }
        } else if (hasLate) {
            timexResult.setBeginHour(timexResult.getBeginHour() + 2);
        }

        ret.setTimex(timexResult.getTimeStr());

        ret.setFutureValue(new Pair<>(
                DateUtil.safeCreateFromValue(LocalDateTime.MIN, year, month, day, timexResult.getBeginHour(), 0, 0),
                DateUtil.safeCreateFromValue(LocalDateTime.MIN, year, month, day, timexResult.getEndHour(), timexResult.getEndMin(), timexResult.getEndMin())));
        ret.setPastValue(new Pair<>(
                DateUtil.safeCreateFromValue(LocalDateTime.MIN, year, month, day, timexResult.getBeginHour(), 0, 0),
                DateUtil.safeCreateFromValue(LocalDateTime.MIN, year, month, day, timexResult.getEndHour(), timexResult.getEndMin(), timexResult.getEndMin())));

        ret.setSuccess(true);

        return ret;
    }

    @Override
    public List<DateTimeParseResult> filterResults(String query, List<DateTimeParseResult> candidateResults) {
        return candidateResults;
    }
}
