// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.datetime.chinese.extractors;

import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.BaseDurationExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseTimeZoneExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.config.ITimeExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.resources.ChineseDateTime;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.ArrayList;
import java.util.regex.Pattern;

public class ChineseTimeExtractorConfiguration extends BaseOptionsConfiguration implements ITimeExtractorConfiguration {

    public static final Iterable<Pattern> TimeRegexList = new ArrayList<Pattern>() {
        {
            add(RegExpUtility.getSafeRegExp(ChineseDateTime.TimeRegexes1));
            add(RegExpUtility.getSafeRegExp(ChineseDateTime.TimeRegexes2));
            add(RegExpUtility.getSafeRegExp(ChineseDateTime.TimeRegexes3));
        }
    };

    private IDateTimeExtractor durationExtractor;
    private IDateTimeExtractor timeZoneExtractor;

    public ChineseTimeExtractorConfiguration() {
        this(DateTimeOptions.None);
    }

    public ChineseTimeExtractorConfiguration(DateTimeOptions options) {
        super(options);
        durationExtractor = new BaseDurationExtractor(new ChineseDurationExtractorConfiguration());
        timeZoneExtractor = null;
    }

    @Override
    public Iterable<Pattern> getTimeRegexList() {
        return TimeRegexList;
    }

    @Override
    public Pattern getAtRegex() {
        return null;
    }

    @Override
    public Pattern getIshRegex() {
        return null;
    }

    @Override
    public Pattern getTimeBeforeAfterRegex() {
        return null;
    }

    @Override
    public IDateTimeExtractor getTimeZoneExtractor() {
        return timeZoneExtractor;
    }
}
