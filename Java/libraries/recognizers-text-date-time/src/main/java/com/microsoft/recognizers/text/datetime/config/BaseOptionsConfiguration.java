package com.microsoft.recognizers.text.datetime.config;

import com.microsoft.recognizers.text.datetime.DateTimeOptions;

public class BaseOptionsConfiguration implements IOptionsConfiguration {
    private final DateTimeOptions options;
    private final boolean dmyDateFormat;

    public BaseOptionsConfiguration() {
        this(DateTimeOptions.None, false);
    }

    public BaseOptionsConfiguration(DateTimeOptions options) {
        this(options, false);
    }

    public BaseOptionsConfiguration(DateTimeOptions options, boolean dmyDateFormat) {
        this.options = options;
        this.dmyDateFormat = dmyDateFormat;
    }

    @Override
    public DateTimeOptions getOptions() {
        return options;
    }

    @Override
    public boolean getDmyDateFormat() {
        return dmyDateFormat;
    }
}
