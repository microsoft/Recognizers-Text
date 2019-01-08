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

    // part 1: smallest component
    // --------------------------------------
    public static final Pattern DescRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.DescRegex);
    public static final Pattern HourNumRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.HourNumRegex);
    public static final Pattern MinuteNumRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.MinuteNumRegex);

    // part 2: middle level component
    // --------------------------------------
    // handle "... o'clock"
    public static final Pattern OclockRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.OclockRegex);

    // handle "... afternoon"
    public static final Pattern PmRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PmRegex);

    // handle "... in the morning"
    public static final Pattern AmRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AmRegex);

    // handle "half past ..." "a quarter to ..."
    // rename 'min' group to 'deltamin'
    public static final Pattern LessThanOneHour = RegExpUtility.getSafeRegExp(EnglishDateTime.LessThanOneHour);

    // handle "six thirty", "six twenty one"
    public static final Pattern BasicTime = RegExpUtility.getSafeRegExp(EnglishDateTime.BasicTime);
    public static final Pattern TimePrefix = RegExpUtility.getSafeRegExp(EnglishDateTime.TimePrefix);
    public static final Pattern TimeSuffix = RegExpUtility.getSafeRegExp(EnglishDateTime.TimeSuffix);
    public static final Pattern WrittenTimeRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.WrittenTimeRegex);

    // handle special time such as 'at midnight', 'midnight', 'midday'
    public static final Pattern MiddayRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.MiddayRegex);
    public static final Pattern MidTimeRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.MidTimeRegex);
    public static final Pattern MidnightRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.MidnightRegex);
    public static final Pattern MidmorningRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.MidmorningRegex);
    public static final Pattern MidafternoonRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.MidafternoonRegex);

    // part 3: regex for time
    // --------------------------------------
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

    //C# TO JAVA CONVERTER NOTE: Java does not support optional parameters. Overloaded method(s) are created above:
    //ORIGINAL LINE: public EnglishTimeExtractorConfiguration(DateTimeOptions options = DateTimeOptions.None)
    public EnglishTimeExtractorConfiguration(DateTimeOptions options) {
        super(options);
        durationExtractor = new BaseDurationExtractor(new EnglishDurationExtractorConfiguration());
        timeZoneExtractor = new BaseTimeZoneExtractor(new EnglishTimeZoneExtractorConfiguration(options));
    }
}
