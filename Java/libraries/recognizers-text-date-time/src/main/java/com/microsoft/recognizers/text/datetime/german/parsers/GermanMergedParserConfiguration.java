// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.datetime.german.parsers;

import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.german.extractors.GermanDatePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.german.extractors.GermanMergedExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.*;
import com.microsoft.recognizers.text.datetime.parsers.config.IMergedParserConfiguration;
import com.microsoft.recognizers.text.datetime.resources.GermanDateTime;
import com.microsoft.recognizers.text.matcher.StringMatcher;

import java.util.regex.Pattern;

public class GermanMergedParserConfiguration extends GermanCommonDateTimeParserConfiguration implements IMergedParserConfiguration {

    private final Pattern equalRegex;
    private final Pattern beforeRegex;
    private final Pattern afterRegex;
    private final Pattern sinceRegex;
    private final Pattern aroundRegex;
    private final Pattern suffixAfterRegex;
    private final Pattern yearRegex;
    private final IDateTimeParser getParser;
    private final IDateTimeParser holidayParser;
    private final StringMatcher superfluousWordMatcher;
    private final BaseDatePeriodParser datePeriodParser;
    private final BaseTimeZoneParser timeZoneParser;
    private final BaseTimePeriodParser timePeriodParser;
    private final BaseDateTimePeriodParser dateTimePeriodParser;

    public GermanMergedParserConfiguration(DateTimeOptions options) {
        super(options);

        beforeRegex = GermanMergedExtractorConfiguration.BeforeRegex;
        afterRegex = GermanMergedExtractorConfiguration.AfterRegex;
        sinceRegex = GermanMergedExtractorConfiguration.SinceRegex;
        aroundRegex = GermanMergedExtractorConfiguration.AroundRegex;
        equalRegex = GermanMergedExtractorConfiguration.EqualRegex;
        suffixAfterRegex = GermanMergedExtractorConfiguration.SuffixAfterRegex;
        yearRegex = GermanDatePeriodExtractorConfiguration.YearRegex;

        superfluousWordMatcher = GermanMergedExtractorConfiguration.SuperfluousWordMatcher;

        datePeriodParser = new BaseDatePeriodParser(new GermanDatePeriodParserConfiguration(this));
        timePeriodParser = new BaseTimePeriodParser(new GermanTimePeriodParserConfiguration(this));
        dateTimePeriodParser = new BaseDateTimePeriodParser(new GermanDateTimePeriodParserConfiguration(this));
        getParser = new BaseSetParser(new GermanSetParserConfiguration(this));
        holidayParser = new BaseHolidayParser(new GermanHolidayParserConfiguration());
        timeZoneParser = new BaseTimeZoneParser();
    }

    public Pattern getBeforeRegex() {
        return beforeRegex;
    }

    public Pattern getAfterRegex() {
        return afterRegex;
    }

    public Pattern getSinceRegex() {
        return sinceRegex;
    }

    public Pattern getAroundRegex() {
        return aroundRegex;
    }

    public Pattern getSuffixAfterRegex() {
        return suffixAfterRegex;
    }

    public Pattern getYearRegex() {
        return yearRegex;
    }

    public IDateTimeParser getGetParser() {
        return getParser;
    }

    public IDateTimeParser getHolidayParser() {
        return holidayParser;
    }

    public StringMatcher getSuperfluousWordMatcher() {
        return superfluousWordMatcher;
    }

    public boolean getCheckBeforeAfter() {
        return GermanDateTime.CheckBothBeforeAfter;
    }
}
