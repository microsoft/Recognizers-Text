package com.microsoft.recognizers.text.tests.helpers;

import com.fasterxml.jackson.annotation.JsonProperty;

public abstract class TimeZoneResolutionResultMixIn {
    TimeZoneResolutionResultMixIn(@JsonProperty("value") String value,
                                  @JsonProperty("utcOffsetMins") Integer utcOffsetMins,
                                  @JsonProperty("timeZoneText") String timeZoneText){
    }
}
