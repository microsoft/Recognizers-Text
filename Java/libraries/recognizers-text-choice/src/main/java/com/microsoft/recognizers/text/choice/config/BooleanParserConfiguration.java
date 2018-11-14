package com.microsoft.recognizers.text.choice.config;

import java.util.Map;

public class BooleanParserConfiguration implements IChoiceParserConfiguration<Boolean> {

    public Map<String,Boolean> resolutions;

    @Override
    public Map<String,Boolean> getResolutions() {
        return this.resolutions;
    }
}