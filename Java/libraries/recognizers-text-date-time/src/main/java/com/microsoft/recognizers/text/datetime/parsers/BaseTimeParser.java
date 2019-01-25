package com.microsoft.recognizers.text.datetime.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.TimeTypeConstants;
import com.microsoft.recognizers.text.datetime.parsers.config.ITimeParserConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.config.PrefixAdjustResult;
import com.microsoft.recognizers.text.datetime.parsers.config.SuffixAdjustResult;
import com.microsoft.recognizers.text.datetime.utilities.ConditionalMatch;
import com.microsoft.recognizers.text.datetime.utilities.DateTimeFormatUtil;
import com.microsoft.recognizers.text.datetime.utilities.DateTimeResolutionResult;
import com.microsoft.recognizers.text.datetime.utilities.DateUtil;
import com.microsoft.recognizers.text.datetime.utilities.RegexExtension;
import com.microsoft.recognizers.text.datetime.utilities.TimeZoneResolutionResult;
import com.microsoft.recognizers.text.datetime.utilities.TimeZoneUtility;
import com.microsoft.recognizers.text.utilities.IntegerUtility;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import com.microsoft.recognizers.text.utilities.StringUtility;

import java.time.LocalDateTime;
import java.util.Arrays;
import java.util.List;
import java.util.Map;
import java.util.Optional;
import java.util.regex.Pattern;

public class BaseTimeParser implements IDateTimeParser {

    private final ITimeParserConfiguration config;

    public BaseTimeParser(ITimeParserConfiguration config) {
        this.config = config;
    }

    @Override
    public String getParserName() {
        return Constants.SYS_DATETIME_TIME;
    }

    @Override
    public List<DateTimeParseResult> filterResults(String query, List<DateTimeParseResult> candidateResults) {
        return candidateResults;
    }

    @Override
    public ParseResult parse(ExtractResult extractResult) {
        return this.parse(extractResult, LocalDateTime.now());
    }

    @Override
    public DateTimeParseResult parse(ExtractResult er, LocalDateTime reference) {

        LocalDateTime referenceTime = reference;

        Object value = null;

        if (er.getType().equals(getParserName())) {
            DateTimeResolutionResult innerResult;

            // Resolve timezome
            if (TimeZoneUtility.shouldResolveTimeZone(er, config.getOptions())) {
                Map<String, Object> metadata = (Map)er.getData();
                ExtractResult timezoneEr = (ExtractResult)metadata.get(Constants.SYS_DATETIME_TIMEZONE);
                ParseResult timezonePr = config.getTimeZoneParser().parse(timezoneEr);

                innerResult = internalParse(er.getText().substring(0, er.getText().length() - timezoneEr.getLength()), referenceTime);

                if (timezonePr.getValue() != null) {
                    TimeZoneResolutionResult timeZoneResolution = ((DateTimeResolutionResult)timezonePr.getValue()).getTimeZoneResolution();
                    innerResult.setTimeZoneResolution(timeZoneResolution);
                }

            } else {
                innerResult = internalParse(er.getText(), referenceTime);
            }

            if (innerResult.getSuccess()) {
                ImmutableMap.Builder<String, String> futureResolution = ImmutableMap.builder();
                futureResolution.put(TimeTypeConstants.TIME, DateTimeFormatUtil.formatTime((LocalDateTime)innerResult.getFutureValue()));

                innerResult.setFutureResolution(futureResolution.build());

                ImmutableMap.Builder<String, String> pastResolution = ImmutableMap.builder();
                pastResolution.put(TimeTypeConstants.TIME, DateTimeFormatUtil.formatTime((LocalDateTime)innerResult.getPastValue()));

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

    protected DateTimeResolutionResult internalParse(String text, LocalDateTime referenceTime) {

        DateTimeResolutionResult innerResult = parseBasicRegexMatch(text, referenceTime);
        return innerResult;
    }

    // parse basic patterns in TimeRegexList
    private DateTimeResolutionResult parseBasicRegexMatch(String text, LocalDateTime referenceTime) {

        String trimmedText = text.trim().toLowerCase();
        int offset = 0;
        Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(config.getAtRegex(), trimmedText)).findFirst();
        if (!match.isPresent()) {
            match = Arrays.stream(RegExpUtility.getMatches(config.getAtRegex(), config.getTimeTokenPrefix() + trimmedText)).findFirst();
            offset = config.getTimeTokenPrefix().length();
        }

        if (match.isPresent() && match.get().index == offset && match.get().length == trimmedText.length()) {
            return match2Time(match.get(), referenceTime);
        }

        // parse hour pattern, like "twenty one", "16"
        // create a extract comments which content the pass-in text
        Integer hour = null;
        if (config.getNumbers().containsKey(text)) {
            hour = config.getNumbers().get(text);
        } else {
            try {
                hour = Integer.parseInt(text);
            } catch (Exception ignored) {
                hour = null;
            }
        }

        if (hour != null && hour >= 0 && hour <= 24) {
            DateTimeResolutionResult result = new DateTimeResolutionResult();

            if (hour == 24) {
                hour = 0;
            }

            if (hour <= Constants.HalfDayHourCount && hour != 0) {
                result.setComment(Constants.Comment_AmPm);
            }

            result.setTimex(String.format("T%02d", hour));
            LocalDateTime value = DateUtil.safeCreateFromMinValue(referenceTime.getYear(), referenceTime.getMonthValue(), referenceTime.getDayOfMonth(), hour, 0, 0);
            result.setFutureValue(value);
            result.setPastValue(value);
            result.setSuccess(true);
            return result;
        }

        for (Pattern regex : config.getTimeRegexes()) {
            ConditionalMatch exactMatch = RegexExtension.matchExact(regex, trimmedText, true);

            if (exactMatch.getSuccess()) {
                return match2Time(exactMatch.getMatch().get(), referenceTime);
            }
        }

        return new DateTimeResolutionResult();
    }

    private DateTimeResolutionResult match2Time(Match match, LocalDateTime referenceTime) {

        DateTimeResolutionResult result = new DateTimeResolutionResult();
        boolean hasMin = false;
        boolean hasSec = false;
        boolean hasAm = false;
        boolean hasPm = false;
        boolean hasMid = false;
        int hour = 0;
        int minute = 0;
        int second = 0;
        int day = referenceTime.getDayOfMonth();
        int month = referenceTime.getMonthValue();
        int year = referenceTime.getYear();

        String writtenTimeStr = match.getGroup("writtentime").value;

        if (!StringUtility.isNullOrEmpty(writtenTimeStr)) {
            // get hour
            String hourStr = match.getGroup("hournum").value.toLowerCase();
            hour = config.getNumbers().get(hourStr);

            // get minute
            String minStr = match.getGroup("minnum").value.toLowerCase();
            String tensStr = match.getGroup("tens").value.toLowerCase();

            if (!StringUtility.isNullOrEmpty(minStr)) {
                minute = config.getNumbers().get(minStr);
                if (!StringUtility.isNullOrEmpty(tensStr)) {
                    minute += config.getNumbers().get(tensStr);
                }
                hasMin = true;
            }
        } else if (!StringUtility.isNullOrEmpty(match.getGroup("mid").value)) {
            hasMid = true;
            if (!StringUtility.isNullOrEmpty(match.getGroup("midnight").value)) {
                hour = 0;
                minute = 0;
                second = 0;
            } else if (!StringUtility.isNullOrEmpty(match.getGroup("midmorning").value)) {
                hour = 10;
                minute = 0;
                second = 0;
            } else if (!StringUtility.isNullOrEmpty(match.getGroup("midafternoon").value)) {
                hour = 14;
                minute = 0;
                second = 0;
            } else if (!StringUtility.isNullOrEmpty(match.getGroup("midday").value)) {
                hour = Constants.HalfDayHourCount;
                minute = 0;
                second = 0;
            }
        } else {
            // get hour
            String hourStr = match.getGroup(Constants.HourGroupName).value;
            if (StringUtility.isNullOrEmpty(hourStr)) {
                hourStr = match.getGroup("hournum").value.toLowerCase();
                if (!config.getNumbers().containsKey(hourStr)) {
                    return result;
                }

                hour = config.getNumbers().get(hourStr);
            } else {
                if (!IntegerUtility.canParse(hourStr)) {
                    if (!config.getNumbers().containsKey(hourStr.toLowerCase())) {
                        return result;
                    }

                    hour = config.getNumbers().get(hourStr.toLowerCase());
                } else {
                    hour = Integer.parseInt(hourStr);
                }
            }

            // get minute
            String minStr = match.getGroup(Constants.MinuteGroupName).value.toLowerCase();
            if (StringUtility.isNullOrEmpty(minStr)) {
                minStr = match.getGroup("minnum").value;
                if (!StringUtility.isNullOrEmpty(minStr)) {
                    minute = config.getNumbers().get(minStr);
                    hasMin = true;
                }

                String tensStr = match.getGroup("tens").value;
                if (!StringUtility.isNullOrEmpty(tensStr)) {
                    minute += config.getNumbers().get(tensStr);
                    hasMin = true;
                }
            } else {
                minute = Integer.parseInt(minStr);
                hasMin = true;
            }

            // get second
            String secStr = match.getGroup(Constants.SecondGroupName).value.toLowerCase();
            if (!StringUtility.isNullOrEmpty(secStr)) {
                second = Integer.parseInt(secStr);
                hasSec = true;
            }
        }

        // Adjust by desc string
        String descStr = match.getGroup(Constants.DescGroupName).value.toLowerCase();

        // ampm is a special case in which at 6ampm = at 6
        if (isAmDesc(descStr, match)) {
            if (hour >= Constants.HalfDayHourCount) {
                hour -= Constants.HalfDayHourCount;
            }

            if (!checkRegex(config.getUtilityConfiguration().getAmPmDescRegex(), descStr)) {
                hasAm = true;
            }

        } else if (isPmDesc(descStr, match)) {
            if (hour < Constants.HalfDayHourCount) {
                hour += Constants.HalfDayHourCount;
            }

            hasPm = true;
        }

        // adjust min by prefix
        String timePrefix = match.getGroup(Constants.PrefixGroupName).value.toLowerCase();
        if (!StringUtility.isNullOrEmpty(timePrefix)) {
            PrefixAdjustResult prefixResult = config.adjustByPrefix(timePrefix, hour, minute, hasMin);
            hour = prefixResult.hour;
            minute = prefixResult.minute;
            hasMin = prefixResult.hasMin;
        }

        // adjust hour by suffix
        String timeSuffix = match.getGroup(Constants.SuffixGroupName).value.toLowerCase();
        if (!StringUtility.isNullOrEmpty(timeSuffix)) {
            SuffixAdjustResult suffixResult = config.adjustBySuffix(timeSuffix, hour, minute, hasMin, hasAm, hasPm);
            hour = suffixResult.hour;
            minute = suffixResult.minute;
            hasMin = suffixResult.hasMin;
            hasAm = suffixResult.hasAm;
            hasPm = suffixResult.hasPm;
        }

        if (hour == 24) {
            hour = 0;
        }

        StringBuilder timex = new StringBuilder(String.format("T%02d", hour));

        if (hasMin) {
            timex.append(String.format(":%02d", minute));
        }

        if (hasSec) {
            timex.append(String.format(":%02d", second));
        }

        result.setTimex(timex.toString());

        if (hour <= Constants.HalfDayHourCount && !hasPm && !hasAm && !hasMid) {
            result.setComment(Constants.Comment_AmPm);
        }

        LocalDateTime resultTime = DateUtil.safeCreateFromMinValue(year, month, day, hour, minute, second);
        result.setFutureValue(resultTime);
        result.setPastValue(resultTime);

        result.setSuccess(true);

        return result;
    }

    private boolean isAmDesc(String descStr, Match match) {
        return checkRegex(config.getUtilityConfiguration().getAmDescRegex(), descStr) ||
                checkRegex(config.getUtilityConfiguration().getAmPmDescRegex(), descStr) ||
                !StringUtility.isNullOrEmpty(match.getGroup(Constants.ImplicitAmGroupName).value);
    }

    private boolean isPmDesc(String descStr, Match match) {
        return checkRegex(config.getUtilityConfiguration().getPmDescRegex(), descStr) ||
                !StringUtility.isNullOrEmpty(match.getGroup(Constants.ImplicitPmGroupName).value);
    }

    private boolean checkRegex(Pattern regex, String input) {
        Optional<Match> result = Arrays.stream(RegExpUtility.getMatches(regex, input)).findFirst();
        return result.isPresent();
    }
}
