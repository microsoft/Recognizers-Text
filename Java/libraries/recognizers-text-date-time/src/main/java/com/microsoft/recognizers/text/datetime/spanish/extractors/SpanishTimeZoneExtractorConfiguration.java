package com.microsoft.recognizers.text.datetime.spanish.extractors;

import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.config.ITimeZoneExtractorConfiguration;
import com.microsoft.recognizers.text.matcher.StringMatcher;

import java.util.ArrayList;
import java.util.regex.Pattern;

public class SpanishTimeZoneExtractorConfiguration extends BaseOptionsConfiguration implements ITimeZoneExtractorConfiguration {

    public static final Iterable<Pattern> TimeZoneRegexList = new ArrayList<>();

    public SpanishTimeZoneExtractorConfiguration(DateTimeOptions options) {
        super(options);

    }

    public final Iterable<Pattern> getTimeZoneRegexes() {
        return TimeZoneRegexList;
    }

    private Pattern locationTimeSuffixRegex;

    public final Pattern getLocationTimeSuffixRegex() {
        return locationTimeSuffixRegex;
    }

    private StringMatcher locationMatcher;

    public final StringMatcher getLocationMatcher() {
        return locationMatcher;
    }

    public final ArrayList<String> getAmbiguousTimezoneList() {
        return new ArrayList<>();
    }
}
