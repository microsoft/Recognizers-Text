package com.microsoft.recognizers.text.tests.helpers;

import com.fasterxml.jackson.annotation.JsonProperty;

import java.util.SortedMap;

public abstract class ModelResultMixIn {
    ModelResultMixIn(@JsonProperty("text") String text,
                     @JsonProperty("start") Integer start,
                     @JsonProperty("end") Integer end,
                     @JsonProperty("typeName") String typeName,
                     @JsonProperty("resolution") SortedMap<String, Object> resolution,
                     @JsonProperty("parentText") String parentText) {
    }
}
