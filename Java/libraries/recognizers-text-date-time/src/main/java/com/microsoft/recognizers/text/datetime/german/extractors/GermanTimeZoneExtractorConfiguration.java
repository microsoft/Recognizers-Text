// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.datetime.german.extractors;

import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.config.ITimeZoneExtractorConfiguration;
import com.microsoft.recognizers.text.matcher.StringMatcher;

import java.util.ArrayList;
import java.util.regex.Pattern;

public class GermanTimeZoneExtractorConfiguration extends BaseOptionsConfiguration implements ITimeZoneExtractorConfiguration {
    public GermanTimeZoneExtractorConfiguration(DateTimeOptions options) {
        super(options);
    }

    private Pattern directUtcRegex;

    public final Pattern getDirectUtcRegex() {
        return directUtcRegex;
    }

    private Pattern locationTimeSuffixRegex;

    public final Pattern getLocationTimeSuffixRegex() {
        return locationTimeSuffixRegex;
    }

    private StringMatcher locationMatcher;

    public final StringMatcher getLocationMatcher() {
        return locationMatcher;
    }

    private StringMatcher timeZoneMatcher;

    public final StringMatcher getTimeZoneMatcher() {
        return timeZoneMatcher;
    }

    public final ArrayList<String> getAmbiguousTimezoneList() {
        return new ArrayList<>();
    }
}
