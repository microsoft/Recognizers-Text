// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.datetime.german.extractors;

import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.config.IHolidayExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.resources.GermanDateTime;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.ArrayList;
import java.util.regex.Pattern;

public class GermanHolidayExtractorConfiguration extends BaseOptionsConfiguration implements IHolidayExtractorConfiguration {

    public static final Pattern YearPattern = RegExpUtility.getSafeRegExp(GermanDateTime.YearRegex);

    public static final Pattern H1 = RegExpUtility.getSafeRegExp(GermanDateTime.HolidayRegex1);
    public static final Pattern H2 = RegExpUtility.getSafeRegExp(GermanDateTime.HolidayRegex2);
    public static final Pattern H3 = RegExpUtility.getSafeRegExp(GermanDateTime.HolidayRegex3);

    public static final Iterable<Pattern> HolidayRegexList = new ArrayList<Pattern>() {
        {
            add(H1);
            add(H2);
            add(H3);
        }
    };

    public GermanHolidayExtractorConfiguration() {
        super(DateTimeOptions.None);
    }

    public Iterable<Pattern> getHolidayRegexes() {
        return HolidayRegexList;
    }
}
