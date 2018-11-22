package com.microsoft.recognizers.text.datetime.extractors.config;

import java.util.regex.Pattern;

public interface IHolidayExtractorConfiguration {
    Iterable<Pattern> getHolidayRegexes();
}
