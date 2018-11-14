package com.microsoft.recognizers.text.choice.config;

import java.util.Map;

public interface IChoiceParserConfiguration<T> {
    public Map<String, Boolean> getResolutions();
}