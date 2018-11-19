package com.microsoft.recognizers.text.choice.config;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.choice.Constants;

import java.util.Map;

public class BooleanParserConfiguration implements IChoiceParserConfiguration<Boolean> {

    public static Map<String, Boolean> Resolutions = ImmutableMap.<String, Boolean>builder()
            .put(Constants.SYS_BOOLEAN_TRUE, true)
            .put(Constants.SYS_BOOLEAN_FALSE, false)
            .build();

    @Override
    public Map<String, Boolean> getResolutions() {
        return Resolutions;
    }
}