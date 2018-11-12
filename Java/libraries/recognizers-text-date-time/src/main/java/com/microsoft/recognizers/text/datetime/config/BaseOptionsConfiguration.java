package com.microsoft.recognizers.text.datetime.config;

import com.microsoft.recognizers.text.datetime.DateTimeOptions;

public class BaseOptionsConfiguration implements IOptionsConfiguration {
    private final DateTimeOptions options;

    public BaseOptionsConfiguration(DateTimeOptions options) {
        this.options = options;
    }

    @Override
    public DateTimeOptions getOptions() {
        return options;
    }
}
