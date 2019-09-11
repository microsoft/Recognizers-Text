package com.microsoft.recognizers.text.datetime.parsers;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.resources.EnglishTimeZone;
import com.microsoft.recognizers.text.datetime.utilities.DateTimeResolutionResult;
import com.microsoft.recognizers.text.datetime.utilities.TimeZoneResolutionResult;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.time.Duration;
import java.time.LocalDateTime;
import java.util.Arrays;
import java.util.List;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class BaseTimeZoneParser implements IDateTimeParser {
    private final Pattern directUtcRegex;

    public BaseTimeZoneParser() {
        directUtcRegex = RegExpUtility.getSafeRegExp(EnglishTimeZone.DirectUtcRegex);
    }

    @Override
    public String getParserName() {
        return Constants.SYS_DATETIME_TIME;
    }

    @Override
    public List<DateTimeParseResult> filterResults(String query, List<DateTimeParseResult> candidateResults) {
        return candidateResults;
    }

    public String normalizeText(String text) {
        text = text.replaceAll("\\s+", " ");
        text = text.replaceAll("time$|timezone$", "");
        return  text.trim();
    }

    @Override
    public ParseResult parse(ExtractResult extractResult) {
        return this.parse(extractResult, LocalDateTime.now());
    }

    @Override
    public DateTimeParseResult parse(ExtractResult er, LocalDateTime reference) {
        DateTimeParseResult result;
        result = new DateTimeParseResult(er);

        String text = er.getText().toLowerCase();
        String normalizedText = normalizeText(text);
        Matcher match = directUtcRegex.matcher(text);
        String matched = match.find() ? match.group(2) : "";
        int offsetInMinutes = matched != null ? computeMinutes(matched) : Constants.InvalidOffsetValue;

        if (offsetInMinutes != Constants.InvalidOffsetValue) {
            DateTimeResolutionResult value = getDateTimeResolutionResult(offsetInMinutes, text);
            String resolutionStr = String.format("%s: %d", Constants.UtcOffsetMinsKey, offsetInMinutes);

            result.setValue(value);
            result.setResolutionStr(resolutionStr);
        } else if (checkAbbrToMin(normalizedText)) {
            int utcMinuteShift = EnglishTimeZone.AbbrToMinMapping.getOrDefault(normalizedText, 0);

            DateTimeResolutionResult value = getDateTimeResolutionResult(utcMinuteShift, text);
            String resolutionStr = String.format("%s: %d", Constants.UtcOffsetMinsKey, utcMinuteShift);

            result.setValue(value);
            result.setResolutionStr(resolutionStr);
        } else if (checkFullToMin(normalizedText)) {
            int utcMinuteShift = EnglishTimeZone.FullToMinMapping.getOrDefault(normalizedText, 0);

            DateTimeResolutionResult value = getDateTimeResolutionResult(utcMinuteShift, text);
            String resolutionStr = String.format("%s: %d", Constants.UtcOffsetMinsKey, utcMinuteShift);

            result.setValue(value);
            result.setResolutionStr(resolutionStr);
        } else {
            // TODO: Temporary solution for city timezone and ambiguous data
            DateTimeResolutionResult value = new DateTimeResolutionResult();
            value.setSuccess(true);
            value.setTimeZoneResolution(new TimeZoneResolutionResult("UTC+XX:XX", Constants.InvalidOffsetValue, text));
            String resolutionStr = String.format("%s: %s", Constants.UtcOffsetMinsKey, "XX:XX");

            result.setValue(value);
            result.setResolutionStr(resolutionStr);
        }

        return result;
    }

    private boolean checkAbbrToMin(String text) {
        if (EnglishTimeZone.AbbrToMinMapping.containsKey(text)) {
            return EnglishTimeZone.AbbrToMinMapping.get(text) != Constants.InvalidOffsetValue;
        }
        return false;
    }

    private boolean checkFullToMin(String text) {
        if (EnglishTimeZone.FullToMinMapping.containsKey(text)) {
            return EnglishTimeZone.FullToMinMapping.get(text) != Constants.InvalidOffsetValue;
        }
        return false;
    }

    private DateTimeResolutionResult getDateTimeResolutionResult(int offsetInMinutes, String text) {
        DateTimeResolutionResult value = new DateTimeResolutionResult();
        value.setSuccess(true);
        value.setTimeZoneResolution(new TimeZoneResolutionResult(convertOffsetInMinsToOffsetString(offsetInMinutes), offsetInMinutes, text));
        return value;
    }

    private String convertOffsetInMinsToOffsetString(int offsetInMinutes) {
        return String.format("UTC%s%s", offsetInMinutes >= 0 ? "+" : "-", convertMinsToRegularFormat(Math.abs(offsetInMinutes)));
    }

    private String convertMinsToRegularFormat(int offsetMins) {
        Duration duration = Duration.ofMinutes(offsetMins);
        return String.format("%02d:%02d", duration.toHours() % 24, duration.toMinutes() % 60);
    }

    // Compute UTC offset in minutes from matched timezone offset in text. e.g. "-4:30" -> -270; "+8"-> 480.
    public int computeMinutes(String utcOffset) {
        if (utcOffset.length() == 0) {
            return Constants.InvalidOffsetValue;
        }

        utcOffset = utcOffset.trim();
        int sign = Constants.PositiveSign; // later than utc, default value
        if (utcOffset.startsWith("+") || utcOffset.startsWith("-") || utcOffset.startsWith("±")) {
            if (utcOffset.startsWith("-")) {
                sign = Constants.NegativeSign; // earlier than utc 0
            }

            utcOffset = utcOffset.substring(1).trim();
        }

        int hours = 0;
        final int minutes;
        if (utcOffset.contains(":")) {
            String[] tokens = utcOffset.split(":");
            hours = Integer.parseInt(tokens[0]);
            minutes = Integer.parseInt(tokens[1]);
        } else {
            minutes = 0;
            try {
                hours = Integer.parseInt(utcOffset);
            } catch (Exception e) {
                hours = 0;
            }
        }

        if (hours > Constants.HalfDayHourCount) {
            return Constants.InvalidOffsetValue;
        }

        if (Arrays.stream(new int[]{0, 15, 30, 45, 60}).anyMatch(x -> x == minutes)) {
            return Constants.InvalidOffsetValue;
        }

        int offsetInMinutes = hours * 60 + minutes;
        offsetInMinutes *= sign;

        return offsetInMinutes;
    }
}
