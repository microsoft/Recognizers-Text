package com.microsoft.recognizers.text.datetime.parsers;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.Metadata;
import com.microsoft.recognizers.text.ParseResult;

public class DateTimeParseResult extends ParseResult {
    //TimexStr is only used in extractors related with date and time
    //It will output the TIMEX representation of a time string.
    private String timexStr;

    public DateTimeParseResult(Integer start, Integer length, String text, String type, Object data, Object value, String resolutionStr, String timexStr) {
        super(start, length, text, type, data, value, resolutionStr);
        this.timexStr = timexStr;
    }

    public DateTimeParseResult(ExtractResult er) {
        this(er.getStart(), er.getLength(), er.getText(), er.getType(), er.getData(), null, null, null);
    }

    public DateTimeParseResult(ParseResult pr) {
        this(pr.getStart(), pr.getLength(), pr.getText(), pr.getType(), pr.getData(), pr.getValue(), pr.getResolutionStr(), null);
    }

    public DateTimeParseResult(Integer start, Integer length, String text, String type, Object data, Object value, String resolutionStr, String timexStr, Metadata metadata) {
        super(start, length, text, type, data, value, resolutionStr, metadata);
        this.timexStr = timexStr;
    }

    public String getTimexStr() {
        return timexStr;
    }

    public void setTimexStr(String timexStr) {
        this.timexStr = timexStr;
    }
}
