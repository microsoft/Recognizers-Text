package com.microsoft.recognizers.text.tests;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;

import java.time.LocalDateTime;
import java.time.ZonedDateTime;
import java.time.ZoneId;
import java.time.format.DateTimeFormatter;
import java.util.List;
import java.util.Map;
import java.util.stream.Collectors;
import java.util.stream.IntStream;

@JsonIgnoreProperties(ignoreUnknown = true)
public class TestCase {

    public String language;
    public String recognizerName;
    public String modelName;

    public String testType;
    public String input;
    public Map<String, Object> context;
    public Boolean debug = false;
    public String notSupported;
    public String notSupportedByDesign;
    public List<Object> results;

    public LocalDateTime getReferenceDateTime() {
        if (context != null && context.containsKey("ReferenceDateTime")) {
            Object objectDateTime = context.get("ReferenceDateTime");
            String formatPattern = getDateTimePattern(objectDateTime.toString());
            DateTimeFormatter FORMATTER = DateTimeFormatter.ofPattern(formatPattern);
            return ZonedDateTime.parse(objectDateTime.toString(), FORMATTER.withZone(ZoneId.systemDefault())).toLocalDateTime();
        }

        return LocalDateTime.now();
    }

    public String toString() {
        return String.format("%sRecognizer - %s - %s - \"%s\"", this.recognizerName, this.language, this.modelName, this.input);
    }

    private String getDateTimePattern(String datetime) {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.append("yyyy-MM-dd'T'HH:mm:ss");
        stringBuilder.append(getMillisecondsPatten(datetime, ".", "+"));
        stringBuilder.append(getTimeZonePattern(datetime));

        return stringBuilder.toString();
    }

    private String getMillisecondsPatten(String text, String leftBound, String rightBound) {
        if (text.contains(leftBound)) {
            String milliseconds = text.substring(text.indexOf(leftBound) + 1 , text.contains(rightBound) ? text.indexOf(rightBound) : text.length());
            return leftBound + IntStream.range(0, milliseconds.length()).mapToObj(i -> "S").collect(Collectors.joining(""));
        }

        return "";
    }

    private String getTimeZonePattern(String text) {
        if (text.contains("+")) {
            String timezone = text.substring(text.indexOf("+") + 1);
            switch (timezone.length()) {
                case 2:
                    return "X";
                case 4:
                    return "XX";
                case 5:
                    return "XXX";
                case 6:
                    return "XXXX";
                case 8:
                    return "XXXXX";
                default:
                    throw new Error("Time Zone format not supported.");
            }
        }

        return "";
    }
}
