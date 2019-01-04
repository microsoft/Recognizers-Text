package com.microsoft.recognizers.text;

public class ParseResult extends ExtractResult {

    // Value is for resolution.
    // e.g. 1000 for "one thousand".
    // The resolutions are different for different parsers.
    // Therefore, we use object here.
    private Object value;

    // Output the value in string format.
    // It is used in some parsers.
    private String resolutionStr;

    public ParseResult(Integer start, Integer length, String text, String type, Object data, Object value, String resolutionStr) {
        super(start, length, text, type, data, null);
        this.value = value;
        this.resolutionStr = resolutionStr;
    }

    public ParseResult(ExtractResult er) {
        this(er.getStart(), er.getLength(), er.getText(), er.getType(), er.getData(), null, null);
    }

    public ParseResult(Integer start, Integer length, String text, String type, Object data, Object value, String resolutionStr, Metadata metadata) {
        super(start, length, text, type, data, metadata);
        this.value = value;
        this.resolutionStr = resolutionStr;
    }

    public Object getValue() {
        return value;
    }

    public void setValue(Object value) {
        this.value = value;
    }

    public String getResolutionStr() {
        return resolutionStr;
    }

    public void setResolutionStr(String resolutionStr) {
        this.resolutionStr = resolutionStr;
    }
}
