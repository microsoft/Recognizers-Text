package com.microsoft.recognizers.text.datetime.spanish.extractors;

import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.BaseDurationExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseTimeZoneExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.config.ITimeExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.resources.SpanishDateTime;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.ArrayList;
import java.util.regex.Pattern;

public class SpanishTimeExtractorConfiguration extends BaseOptionsConfiguration
    implements ITimeExtractorConfiguration {
    // handle "y media ..." "menos cuarto ..."
    public static final Pattern LessThanOneHour = RegExpUtility.getSafeRegExp(SpanishDateTime.LessThanOneHour);

    // handle "seis treinta", "seis veintiuno", "seis menos diez"
    public static final Pattern TimeSuffix = RegExpUtility.getSafeRegExp(SpanishDateTime.TimeSuffix);
    public static final Pattern BasicTime = RegExpUtility.getSafeRegExp(SpanishDateTime.BasicTime);

    // handle "a las cuatro" "a las 3"
    //TODO: add some new regex which have used in AtRegex
    //TODO: modify according to corresponding English regex
    public static final Pattern AtRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.AtRegex);
    public static final Pattern ConnectNumRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.ConnectNumRegex);
    public static final Pattern TimeBeforeAfterRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.TimeBeforeAfterRegex);
    public static final Iterable<Pattern> TimeRegexList = new ArrayList<Pattern>() {
        {
            // (tres min pasadas las)? siete|7|(siete treinta) pm
            add(RegExpUtility.getSafeRegExp(SpanishDateTime.TimeRegex1));

            // (tres min pasadas las)? 3:00(:00)? (pm)?
            add(RegExpUtility.getSafeRegExp(SpanishDateTime.TimeRegex2));

            // (tres min pasadas las)? 3.00 (pm)
            add(RegExpUtility.getSafeRegExp(SpanishDateTime.TimeRegex3));

            // (tres min pasadas las) (cinco treinta|siete|7|7:00(:00)?) (pm)?
            add(RegExpUtility.getSafeRegExp(SpanishDateTime.TimeRegex4));

            // (tres min pasadas las) (cinco treinta|siete|7|7:00(:00)?) (pm)? (de la noche)
            add(RegExpUtility.getSafeRegExp(SpanishDateTime.TimeRegex5));

            // (cinco treinta|siete|7|7:00(:00)?) (pm)? (de la noche)
            add(RegExpUtility.getSafeRegExp(SpanishDateTime.TimeRegex6));

            // (En la noche) a las (cinco treinta|siete|7|7:00(:00)?) (pm)?
            add(RegExpUtility.getSafeRegExp(SpanishDateTime.TimeRegex7));

            // (En la noche) (cinco treinta|siete|7|7:00(:00)?) (pm)?
            add(RegExpUtility.getSafeRegExp(SpanishDateTime.TimeRegex8));

            // once (y)? veinticinco
            add(RegExpUtility.getSafeRegExp(SpanishDateTime.TimeRegex9));

            add(RegExpUtility.getSafeRegExp(SpanishDateTime.TimeRegex10));

            // (tres menos veinte) (pm)?
            add(RegExpUtility.getSafeRegExp(SpanishDateTime.TimeRegex11));

            // (tres min pasadas las)? 3h00 (pm)?
            add(RegExpUtility.getSafeRegExp(SpanishDateTime.TimeRegex12));

            // 340pm
            add(ConnectNumRegex);
        }
    };

    public final Pattern getIshRegex() {
        return null;
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

    private IDateTimeExtractor durationExtractor;

    public final IDateTimeExtractor getDurationExtractor() {
        return durationExtractor;
    }

    private IDateTimeExtractor timeZoneExtractor;

    public final IDateTimeExtractor getTimeZoneExtractor() {
        return timeZoneExtractor;
    }

    public SpanishTimeExtractorConfiguration() {
        this(DateTimeOptions.None);
    }

    public SpanishTimeExtractorConfiguration(DateTimeOptions options) {
        super(options);
        durationExtractor = new BaseDurationExtractor(new SpanishDurationExtractorConfiguration());
        timeZoneExtractor = new BaseTimeZoneExtractor(new SpanishTimeZoneExtractorConfiguration(options));
    }
}
