package com.microsoft.recognizers.text.tests;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import java.time.LocalDateTime;
import java.time.ZoneId;
import java.time.ZonedDateTime;
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
        String testName = String.format("%sRecognizer - %s - %s - \"%s\"", this.recognizerName, this.language, this.modelName, this.input);

        if (this.context != null && this.context.containsKey("ReferenceDateTime")) {

            return String.format("%s - [%s]", testName, this.context.get("ReferenceDateTime"));
        }

        return testName;
    }

    private String getDateTimePattern(String datetime) {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.append("yyyy-MM-dd'T'HH:mm:ss");
        stringBuilder.append(getMillisecondsPatten(datetime, "."));
        stringBuilder.append(getTimeZonePattern(datetime));

        return stringBuilder.toString();
    }

    private String getMillisecondsPatten(String text, String leftBound) {
        if (text.contains(leftBound)) {
            final int leftIndex = text.indexOf(leftBound);
            final int endIndex = getEndIndex(text, leftIndex);
            String milliseconds = text.substring(leftIndex + 1, endIndex);
            return leftBound + IntStream.range(0, milliseconds.length()).mapToObj(i -> "S").collect(Collectors.joining(""));
        }

        return "";
    }

    private int getEndIndex(final String text, final int leftIndex) {
        if (text.contains("+")) {
            return text.indexOf('+');
        } else if (text.contains("-") && text.lastIndexOf('-') > leftIndex) {
            return text.lastIndexOf('-');
        } else {
            return text.length();
        }
    }

    private String getTimeZonePattern(String text) {
        final String result = getTimeZonePattern(text, "+");
        if (result != null) {
            return result;
        }

        final String nextResult = getTimeZonePattern(text, "-");
        if (nextResult != null) {
            return nextResult;
        }

        return "";
    }

    private String getTimeZonePattern(String text, final String timeZoneBound) {
        if (text.contains(timeZoneBound)) {
            String timezone = text.substring(text.lastIndexOf(timeZoneBound) + 1);
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
                    return null;
            }
        }

        return null;
    }
}
