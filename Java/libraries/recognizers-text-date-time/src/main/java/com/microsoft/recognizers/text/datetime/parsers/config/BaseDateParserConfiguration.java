package com.microsoft.recognizers.text.datetime.parsers.config;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.resources.BaseDateTime;

public abstract class BaseDateParserConfiguration extends BaseOptionsConfiguration implements ICommonDateTimeParserConfiguration {
    protected BaseDateParserConfiguration(DateTimeOptions options) {
        super(options);
    }

    @Override
    public ImmutableMap<String, Integer> getDayOfMonth() {
        return BaseDateTime.DayOfMonthDictionary;
    }
}
