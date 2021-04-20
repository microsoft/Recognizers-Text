package com.microsoft.recognizers.text;

import java.util.SortedMap;

public class ModelResult {

    public static final String ParentTextKey = "parentText";
    public final String parentText;
    public final String text;
    public final int start;
    public final int end;
    public final String typeName;
    public final SortedMap<String, Object> resolution;

    public ModelResult(String text, int start, int end, String typeName, SortedMap<String, Object> resolution) {
        this(text, start, end, typeName, resolution, null);
    }

    public ModelResult(String text, int start, int end, String typeName, SortedMap<String, Object> resolution, String parentText) {
        this.text = text;
        this.start = start;
        this.end = end;
        this.typeName = typeName;
        this.resolution = resolution;
        this.parentText = parentText;
    }
}
