package com.microsoft.recognizers.text;

import java.util.SortedMap;

public class ExtendedModelResult extends ModelResult {
    // Parameter Key
    public static final String ParentTextKey = "parentText";

    public final String parentText;

    public ExtendedModelResult(String text, int start, int end, String typeName, SortedMap<String, Object> resolution, String parentText) {
        super(text, start, end, typeName, resolution);
        this.parentText = parentText;
    }
}
