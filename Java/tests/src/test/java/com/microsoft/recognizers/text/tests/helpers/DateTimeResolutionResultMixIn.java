package com.microsoft.recognizers.text.tests.helpers;

import com.fasterxml.jackson.annotation.JsonProperty;

import java.util.HashMap;
import java.util.List;

public abstract class DateTimeResolutionResultMixIn {
    DateTimeResolutionResultMixIn(@JsonProperty("success") Boolean success,
                                  @JsonProperty("timex") String timex,
                                  @JsonProperty("isLunar") Boolean isLunar,
                                  @JsonProperty("mod") String mod,
                                  @JsonProperty("comment") String comment,
                                  @JsonProperty("futureValue") Object futureValue,
                                  @JsonProperty("resolutionStr") Object pastValue,
                                  @JsonProperty("futureResolution") HashMap<String, String> futureResolution,
                                  @JsonProperty("pastResolution") HashMap<String, String> pastResolution,
                                  @JsonProperty("subDateTimeEntities") List<Object> subDateTimeEntities,
                                  @JsonProperty("timeZoneResolution") Object timeZoneResolution){
    }
}
