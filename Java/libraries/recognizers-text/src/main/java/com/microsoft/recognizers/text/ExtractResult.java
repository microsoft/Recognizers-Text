package com.microsoft.recognizers.text;

public class ExtractResult {

    private Integer start;
    private Integer length;
    private Object data;
    private String type;
    private String text;
    private Metadata metadata;

    public ExtractResult() {
        this(null, null, null, null);
    }

    public ExtractResult(Integer start, Integer length, String text, String type) {
        this(start, length, text, type, null, null);
    }

    public ExtractResult(Integer start, Integer length, String text, String type, Object data, Metadata metadata) {
        this.start = start;
        this.length = length;
        this.text = text;
        this.type = type;
        this.data = data;
        this.metadata = metadata;
    }

    public ExtractResult(Integer start, Integer length, String text, String type, Object data) {
        this.start = start;
        this.length = length;
        this.text = text;
        this.type = type;
        this.data = data;
        this.metadata = null;
    }

    private boolean isOverlap(ExtractResult er1, ExtractResult er2) {
        return !(er1.getStart() >= er2.getStart() + er2.getLength()) &&
                !(er2.getStart() >= er1.getStart() + er1.getLength());
    }

    public boolean isOverlap(ExtractResult er) {
        return isOverlap(this, er);
    }

    private boolean isCover(ExtractResult er1, ExtractResult er2) {
        return ((er2.getStart() < er1.getStart()) && ((er2.getStart() + er2.getLength()) >= (er1.getStart() + er1.getLength()))) ||
                ((er2.getStart() <= er1.getStart()) && ((er2.getStart() + er2.getLength()) > (er1.getStart() + er1.getLength())));
    }

    public boolean isCover(ExtractResult er) {
        return isCover(this, er);
    }

    public Integer getStart() {
        return start;
    }

    public void setStart(Integer start) {
        this.start = start;
    }

    public Integer getLength() {
        return length;
    }

    public void setLength(Integer length) {
        this.length = length;
    }

    public Object getData() {
        return data;
    }

    public void setData(Object data) {
        this.data = data;
    }

    public String getType() {
        return type;
    }

    public void setType(String type) {
        this.type = type;
    }

    public String getText() {
        return text;
    }

    public void setText(String text) {
        this.text = text;
    }

    public Metadata getMetadata() {
        return metadata;
    }

    public void setMetadata(Metadata metadata) {
        this.metadata = metadata;
    }
}
