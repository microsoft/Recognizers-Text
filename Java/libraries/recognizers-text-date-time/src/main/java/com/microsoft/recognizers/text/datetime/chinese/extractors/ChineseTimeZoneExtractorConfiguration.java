package com.microsoft.recognizers.text.datetime.chinese.extractors;

import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.config.ITimeZoneExtractorConfiguration;
import com.microsoft.recognizers.text.matcher.StringMatcher;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.ArrayList;
import java.util.List;
import java.util.regex.Pattern;

public class ChineseTimeZoneExtractorConfiguration extends BaseOptionsConfiguration implements ITimeZoneExtractorConfiguration {

    public static final Pattern DirectUtcRegex = RegExpUtility.getSafeRegExp("^$", Pattern.CASE_INSENSITIVE);
    public static final Pattern LocationTimeSuffixRegex = RegExpUtility.getSafeRegExp("^$", Pattern.CASE_INSENSITIVE);
    public static final StringMatcher LocationMatcher = new StringMatcher();
    public static final StringMatcher TimeZoneMatcher = new StringMatcher();
    public static final List<String> AmbiguousTimezoneList = new ArrayList<String>();

    public ChineseTimeZoneExtractorConfiguration() {
        this(DateTimeOptions.None);
    }

    public ChineseTimeZoneExtractorConfiguration(DateTimeOptions options) {
        super(options);
    }

    @Override
    public Pattern getDirectUtcRegex() {
        return DirectUtcRegex;
    }

    @Override
    public Pattern getLocationTimeSuffixRegex() {
        return LocationTimeSuffixRegex;
    }

    @Override
    public StringMatcher getLocationMatcher() {
        return LocationMatcher;
    }

    @Override
    public StringMatcher getTimeZoneMatcher() {
        return TimeZoneMatcher;
    }

    @Override
    public List<String> getAmbiguousTimezoneList() {
        return AmbiguousTimezoneList;
    }
}
