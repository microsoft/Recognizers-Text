package com.microsoft.recognizers.text.datetime.english.extractors;

import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.BaseDurationExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseTimeZoneExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.config.ITimeExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.resources.EnglishDateTime;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.ArrayList;
import java.util.regex.Pattern;

public class EnglishTimeExtractorConfiguration extends BaseOptionsConfiguration implements ITimeExtractorConfiguration {

    // handle "half past ..." "a quarter to ..."
    // rename 'min' group to 'deltamin'
    public static final Pattern LessThanOneHour = RegExpUtility.getSafeRegExp(EnglishDateTime.LessThanOneHour);

    // handle "at four" "at 3"
    public static final Pattern AtRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AtRegex);
    public static final Pattern IshRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.IshRegex);
    public static final Pattern TimeUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.TimeUnitRegex);
    public static final Pattern ConnectNumRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.ConnectNumRegex);
    public static final Pattern TimeBeforeAfterRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.TimeBeforeAfterRegex);

    public static final Iterable<Pattern> TimeRegexList = new ArrayList<Pattern>() {
        {
            // (three min past)? seven|7|(senven thirty) pm
            add(RegExpUtility.getSafeRegExp(EnglishDateTime.TimeRegex1));

            // (three min past)? 3:00(:00)? (pm)?
            add(RegExpUtility.getSafeRegExp(EnglishDateTime.TimeRegex2));

            // (three min past)? 3.00 (pm)
            add(RegExpUtility.getSafeRegExp(EnglishDateTime.TimeRegex3));

            // (three min past) (five thirty|seven|7|7:00(:00)?) (pm)? (in the night)
            add(RegExpUtility.getSafeRegExp(EnglishDateTime.TimeRegex4));

            // (three min past) (five thirty|seven|7|7:00(:00)?) (pm)?
            add(RegExpUtility.getSafeRegExp(EnglishDateTime.TimeRegex5));

            // (five thirty|seven|7|7:00(:00)?) (pm)? (in the night)
            add(RegExpUtility.getSafeRegExp(EnglishDateTime.TimeRegex6));

            // (in the night) at (five thirty|seven|7|7:00(:00)?) (pm)?
            add(RegExpUtility.getSafeRegExp(EnglishDateTime.TimeRegex7));

            // (in the night) (five thirty|seven|7|7:00(:00)?) (pm)?
            add(RegExpUtility.getSafeRegExp(EnglishDateTime.TimeRegex8));

            add(RegExpUtility.getSafeRegExp(EnglishDateTime.TimeRegex9));

            // (three min past)? 3h00 (pm)?
            add(RegExpUtility.getSafeRegExp(EnglishDateTime.TimeRegex10));

            // at 2.30, "at" prefix is required here
            // 3.30pm, "am/pm" suffix is required here
            add(RegExpUtility.getSafeRegExp(EnglishDateTime.TimeRegex11));

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


    public EnglishTimeExtractorConfiguration() {
        this(DateTimeOptions.None);
    }

    public EnglishTimeExtractorConfiguration(DateTimeOptions options) {
        super(options);
        durationExtractor = new BaseDurationExtractor(new EnglishDurationExtractorConfiguration());
        timeZoneExtractor = new BaseTimeZoneExtractor(new EnglishTimeZoneExtractorConfiguration(options));
    }
}
