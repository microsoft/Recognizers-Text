package com.microsoft.recognizers.text.datetime.config;

import com.microsoft.recognizers.text.datetime.DateTimeOptions;

public interface IOptionsConfiguration {
    DateTimeOptions getOptions();

    boolean getDmyDateFormat();

}
