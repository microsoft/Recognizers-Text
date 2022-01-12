// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.datetime.german.extractors;

import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.BaseDurationExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseTimeZoneExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.config.ITimeExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.resources.GermanDateTime;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.ArrayList;
import java.util.regex.Pattern;

public class GermanTimeExtractorConfiguration extends BaseOptionsConfiguration implements ITimeExtractorConfiguration {

    // part 1: smallest component
    // --------------------------------------
    public static final Pattern DescRegex = RegExpUtility.getSafeRegExp(GermanDateTime.DescRegex);
    public static final Pattern HourNumRegex = RegExpUtility.getSafeRegExp(GermanDateTime.HourNumRegex);
    public static final Pattern MinuteNumRegex = RegExpUtility.getSafeRegExp(GermanDateTime.MinuteNumRegex);

    // part 2: middle level component
    // --------------------------------------
    // handle "... o'clock"
    public static final Pattern OclockRegex = RegExpUtility.getSafeRegExp(GermanDateTime.OclockRegex);

    // handle "... afternoon"
    public static final Pattern PmRegex = RegExpUtility.getSafeRegExp(GermanDateTime.PmRegex);

    // handle "... in the morning"
    public static final Pattern AmRegex = RegExpUtility.getSafeRegExp(GermanDateTime.AmRegex);

    // handle "half past ..." "a quarter to ..."
    // rename 'min' group to 'deltamin'
    public static final Pattern LessThanOneHour = RegExpUtility.getSafeRegExp(GermanDateTime.LessThanOneHour);

    // handle "six thirty", "six twenty one"
    public static final Pattern BasicTime = RegExpUtility.getSafeRegExp(GermanDateTime.BasicTime);
    public static final Pattern TimePrefix = RegExpUtility.getSafeRegExp(GermanDateTime.TimePrefix);
    public static final Pattern TimeSuffix = RegExpUtility.getSafeRegExp(GermanDateTime.TimeSuffix);
    public static final Pattern WrittenTimeRegex = RegExpUtility.getSafeRegExp(GermanDateTime.WrittenTimeRegex);

    // handle special time such as 'at midnight', 'midnight', 'midday'
    public static final Pattern MiddayRegex = RegExpUtility.getSafeRegExp(GermanDateTime.MiddayRegex);
    public static final Pattern MidTimeRegex = RegExpUtility.getSafeRegExp(GermanDateTime.MidTimeRegex);
    public static final Pattern MidnightRegex = RegExpUtility.getSafeRegExp(GermanDateTime.MidnightRegex);
    public static final Pattern MidmorningRegex = RegExpUtility.getSafeRegExp(GermanDateTime.MidmorningRegex);
    public static final Pattern MidafternoonRegex = RegExpUtility.getSafeRegExp(GermanDateTime.MidafternoonRegex);

    // part 3: regex for time
    // --------------------------------------
    // handle "at four" "at 3"
    public static final Pattern AtRegex = RegExpUtility.getSafeRegExp(GermanDateTime.AtRegex);
    public static final Pattern IshRegex = RegExpUtility.getSafeRegExp(GermanDateTime.IshRegex);
    public static final Pattern TimeUnitRegex = RegExpUtility.getSafeRegExp(GermanDateTime.TimeUnitRegex);
    public static final Pattern ConnectNumRegex = RegExpUtility.getSafeRegExp(GermanDateTime.ConnectNumRegex);
    public static final Pattern TimeBeforeAfterRegex = RegExpUtility.getSafeRegExp(GermanDateTime.TimeBeforeAfterRegex);

    public static final Iterable<Pattern> TimeRegexList = new ArrayList<Pattern>() {
        {
            // (three min past)? seven|7|(senven thirty) pm
            add(RegExpUtility.getSafeRegExp(GermanDateTime.TimeRegex1));

            // (three min past)? 3:00(:00)? (pm)?
            add(RegExpUtility.getSafeRegExp(GermanDateTime.TimeRegex2));

            // (three min past)? 3.00 (pm)
            add(RegExpUtility.getSafeRegExp(GermanDateTime.TimeRegex3));

            // (three min past) (five thirty|seven|7|7:00(:00)?) (pm)? (in the night)
            add(RegExpUtility.getSafeRegExp(GermanDateTime.TimeRegex4));

            // (three min past) (five thirty|seven|7|7:00(:00)?) (pm)?
            add(RegExpUtility.getSafeRegExp(GermanDateTime.TimeRegex5));

            // (five thirty|seven|7|7:00(:00)?) (pm)? (in the night)
            add(RegExpUtility.getSafeRegExp(GermanDateTime.TimeRegex6));

            // (in the night) at (five thirty|seven|7|7:00(:00)?) (pm)?
            add(RegExpUtility.getSafeRegExp(GermanDateTime.TimeRegex7));

            // (in the night) (five thirty|seven|7|7:00(:00)?) (pm)?
            add(RegExpUtility.getSafeRegExp(GermanDateTime.TimeRegex8));

            add(RegExpUtility.getSafeRegExp(GermanDateTime.TimeRegex9));

            // (three min past)? 3h00 (pm)?
            add(RegExpUtility.getSafeRegExp(GermanDateTime.TimeRegex10));

            // at 2.30, "at" prefix is required here
            // 3.30pm, "am/pm" suffix is required here
            add(RegExpUtility.getSafeRegExp(GermanDateTime.TimeRegex11));

            // 340pm
            add(ConnectNumRegex);
        }
    };

    public final Iterable<Pattern> getTimeRegexList() {
        return TimeRegexList;
    }

    public final Pattern getAtRegex() {
        return AtRegex;
    }

    public final Pattern getIshRegex() {
        return IshRegex;
    }

    public final Pattern getTimeBeforeAfterRegex() {
        return TimeBeforeAfterRegex;
    }

    private IDateTimeExtractor durationExtractor;

    public final IDateTimeExtractor getDurationExtractor() {
        return durationExtractor;
    }

    private IDateTimeExtractor timeZoneExtractor;

    public final IDateTimeExtractor getTimeZoneExtractor() {
        return timeZoneExtractor;
    }


    public GermanTimeExtractorConfiguration() {
        this(DateTimeOptions.None);
    }

    //C# TO JAVA CONVERTER NOTE: Java does not support optional parameters. Overloaded method(s) are created above:
    //ORIGINAL LINE: public GermanTimeExtractorConfiguration(DateTimeOptions options = DateTimeOptions.None)
    public GermanTimeExtractorConfiguration(DateTimeOptions options) {
        super(options);
        durationExtractor = new BaseDurationExtractor(new GermanDurationExtractorConfiguration());
        timeZoneExtractor = new BaseTimeZoneExtractor(new GermanTimeZoneExtractorConfiguration(options));
    }
}
