package com.microsoft.recognizers.text;

public class ParseResult extends ExtractResult {

    // Value is for resolution.
    // e.g. 1000 for "one thousand".
    // The resolutions are different for different parsers.
    // Therefore, we use object here.
    public final Object value;

    // Output the value in string format.
    // It is used in some parsers.
    public final String resolutionStr;

    public ParseResult(Integer start, Integer length, String text, String type, Object data, Object value, String resolutionStr) {
        super(start, length, text, type, data);
        this.value = value;
        this.resolutionStr = resolutionStr;
    }

    public ParseResult(ExtractResult er) {
        this(er.start, er.length, er.text, er.type, er.data, null, null);
    }

    public ParseResult withLength(int newLength) {
        return new ParseResult(
                this.start,
                newLength,
                this.text,
                this.type,
                this.data,
                this.value,
                this.resolutionStr);
    }

    public ParseResult withText(String newText) {
        return new ParseResult(
                this.start,
                this.length,
                newText,
                this.type,
                this.data,
                this.value,
                this.resolutionStr);
    }

    public ParseResult withData(Object newData) {
        return new ParseResult(
                this.start,
                this.length,
                this.text,
                this.type,
                newData,
                this.value,
                this.resolutionStr);
    }

    public ParseResult withValue(Object newVale) {
        return new ParseResult(
                this.start,
                this.length,
                this.text,
                this.type,
                this.data,
                newVale,
                this.resolutionStr);
    }

    public ParseResult withResolutionStr(String newResolutionStr) {
        return new ParseResult(
                this.start,
                this.length,
                this.text,
                this.type,
                this.data,
                this.value,
                newResolutionStr);
    }
}
