package com.microsoft.recognizers.text;

public class ExtractResult {

    public final Integer start;
    public final Integer length;
    public final String text;
    public final String type;
    public final Object data;

    public ExtractResult(Integer start, Integer length, String text, String type, Object data) {
        this.start = start;
        this.length = length;
        this.text = text;
        this.type = type;
        this.data = data;
    }
}
