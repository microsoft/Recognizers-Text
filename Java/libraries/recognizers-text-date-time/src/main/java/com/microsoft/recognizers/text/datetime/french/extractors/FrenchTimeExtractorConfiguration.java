package com.microsoft.recognizers.text.datetime.french.extractors;

import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.BaseDurationExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseTimeZoneExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.config.ITimeExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.resources.FrenchDateTime;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import java.util.ArrayList;
import java.util.regex.Pattern;

public class FrenchTimeExtractorConfiguration extends BaseOptionsConfiguration implements ITimeExtractorConfiguration {

    public static final Pattern DescRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.DescRegex);
    public static final Pattern HourNumRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.HourNumRegex);
    public static final Pattern MinuteNumRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.MinuteNumRegex);

    public static final Pattern OclockRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.OclockRegex);
    public static final Pattern PmRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.PmRegex);
    public static final Pattern AmRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.AmRegex);

    public static final Pattern LessThanOneHour = RegExpUtility.getSafeRegExp(FrenchDateTime.LessThanOneHour);
    //     public static final Pattern TensTimeRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.TensTimeRegex);

    public static final Pattern WrittenTimeRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.WrittenTimeRegex);
    public static final Pattern TimePrefix = RegExpUtility.getSafeRegExp(FrenchDateTime.TimePrefix);
    public static final Pattern TimeSuffix = RegExpUtility.getSafeRegExp(FrenchDateTime.TimeSuffix);
    public static final Pattern BasicTime = RegExpUtility.getSafeRegExp(FrenchDateTime.BasicTime);
    public static final Pattern IshRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.IshRegex);

    public static final Pattern AtRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.AtRegex);
    public static final Pattern ConnectNumRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.ConnectNumRegex);
    public static final Pattern TimeBeforeAfterRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.TimeBeforeAfterRegex);
    public static final Iterable<Pattern> TimeRegexList = new ArrayList<Pattern>() {
        {
            add(RegExpUtility.getSafeRegExp(FrenchDateTime.TimeRegex1));
            add(RegExpUtility.getSafeRegExp(FrenchDateTime.TimeRegex2));
            add(RegExpUtility.getSafeRegExp(FrenchDateTime.TimeRegex3));
            add(RegExpUtility.getSafeRegExp(FrenchDateTime.TimeRegex4));
            add(RegExpUtility.getSafeRegExp(FrenchDateTime.TimeRegex5));
            add(RegExpUtility.getSafeRegExp(FrenchDateTime.TimeRegex6));
            add(RegExpUtility.getSafeRegExp(FrenchDateTime.TimeRegex7));
            add(RegExpUtility.getSafeRegExp(FrenchDateTime.TimeRegex8));
            add(RegExpUtility.getSafeRegExp(FrenchDateTime.TimeRegex9));
            add(RegExpUtility.getSafeRegExp(FrenchDateTime.TimeRegex10));
            add(ConnectNumRegex);
        }
    };
    private final IDateTimeExtractor durationExtractor;
    private final IDateTimeExtractor timeZoneExtractor;

    public FrenchTimeExtractorConfiguration() {
        this(DateTimeOptions.None);
    }

    public FrenchTimeExtractorConfiguration(final DateTimeOptions options) {
        super(options);
        durationExtractor = new BaseDurationExtractor(new FrenchDurationExtractorConfiguration());
        timeZoneExtractor = new BaseTimeZoneExtractor(new FrenchTimeZoneExtractorConfiguration(options));
    }

    public final Pattern getIshRegex() {
        return IshRegex;
    }

    public final Iterable<Pattern> getTimeRegexList() {
        return TimeRegexList;
    }

    public final Pattern getAtRegex() {
        return AtRegex;
    }

    public final Pattern getTimeBeforeAfterRegex() {
        return TimeBeforeAfterRegex;
    }

    public final IDateTimeExtractor getDurationExtractor() {
        return durationExtractor;
    }

    public final IDateTimeExtractor getTimeZoneExtractor() {
        return timeZoneExtractor;
    }
}
