package com.microsoft.recognizers.text;

public class ExtractResult {

    public final Integer start;
    public final Integer length;
    public final String text;
    public final String type;
    public final Object data;

    public ExtractResult() {
        this(null, null, null, null);
    }

    public ExtractResult(Integer start, Integer length, String text, String type) {
        this(start, length, text, type, null);
    }

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

    private boolean isOverlap(ExtractResult er1, ExtractResult er2) {
        return !(er1.start >= er2.start + er2.length) &&
                !(er2.start >= er1.start + er1.length);
    }

    public boolean isOverlap(ExtractResult er) {
        return isOverlap(this, er);
    }

    private boolean isCover(ExtractResult er1, ExtractResult er2) {
        return ((er2.start < er1.start) && ((er2.start + er2.length) >= (er1.start + er1.length))) ||
                ((er2.start <= er1.start) && ((er2.start + er2.length) > (er1.start + er1.length)));
    }

    public boolean isCover(ExtractResult er) {
        return isCover(this, er);
    }
}