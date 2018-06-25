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

    public ExtractResult withStart(int newStart) {
        return new ExtractResult(
                newStart,
                this.length,
                this.text,
                this.type,
                this.data);
    }

    public ExtractResult withLength(int newLength) {
        return new ExtractResult(
                this.start,
                newLength,
                this.text,
                this.type,
                this.data);
    }

    public ExtractResult withText(String newText) {
        return new ExtractResult(
                this.start,
                this.length,
                newText,
                this.type,
                this.data);
    }

    public ExtractResult withType(String newType) {
        return new ExtractResult(
                this.start,
                this.length,
                this.text,
                newType,
                this.data);
    }

    public ExtractResult withData(Object newData) {
        return new ExtractResult(
                this.start,
                this.length,
                this.text,
                this.type,
                newData);
    }
}
