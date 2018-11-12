package com.microsoft.recognizers.text.datetime.parsers;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.ParseResult;

import java.util.Optional;

public class DateTimeParseResult extends ParseResult {
    //TimexStr is only used in extractors related with date and time
    //It will output the TIMEX representation of a time string.
    public final String timexStr;

    public DateTimeParseResult(Integer start, Integer length, String text, String type, Object data, Object value, String resolutionStr, String timexStr) {
        super(start, length, text, type, data, value, resolutionStr);
        this.timexStr = timexStr;
    }

    public DateTimeParseResult(ExtractResult er) {
        this(er.start, er.length, er.text, er.type, er.data, null, null, null);
    }

    public  DateTimeParseResult(ParseResult pr) {
        this(pr.start, pr.length, pr.text, pr.type, pr.data, pr.value, pr.resolutionStr, null);
    }

    public DateTimeParseResult withTimexStr(String newTimexStr) {
        return new DateTimeParseResult(
                this.start,
                this.length,
                this.text,
                this.type,
                this.data,
                this.value,
                this.resolutionStr,
                newTimexStr);
    }
}
