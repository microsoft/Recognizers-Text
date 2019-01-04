package com.microsoft.recognizers.text.datetime.spanish.utilities;

import com.microsoft.recognizers.text.datetime.resources.SpanishDateTime;
import com.microsoft.recognizers.text.datetime.utilities.IDateTimeUtilityConfiguration;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.regex.Pattern;

public class SpanishDatetimeUtilityConfiguration implements IDateTimeUtilityConfiguration {
    public static final Pattern AgoRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.AgoRegex);

    public static final Pattern LaterRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.LaterRegex);

    public static final Pattern InConnectorRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.InConnectorRegex);

    public static final Pattern WithinNextPrefixRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.WithinNextPrefixRegex);

    public static final Pattern AmDescRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.AmDescRegex);

    public static final Pattern PmDescRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.PmDescRegex);

    public static final Pattern AmPmDescRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.AmPmDescRegex);

    public static final Pattern RangeUnitRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.RangeUnitRegex);

    public static final Pattern TimeUnitRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.TimeUnitRegex);

    public static final Pattern DateUnitRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.DateUnitRegex);

    public static final Pattern CommonDatePrefixRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.CommonDatePrefixRegex);

    @Override
    public final Pattern getLaterRegex() {
        return LaterRegex;
    }

    @Override
    public final Pattern getAgoRegex() {
        return AgoRegex;
    }

    @Override
    public final Pattern getInConnectorRegex() {
        return InConnectorRegex;
    }

    @Override
    public final Pattern getWithinNextPrefixRegex() {
        return WithinNextPrefixRegex;
    }

    @Override
    public final Pattern getAmDescRegex() {
        return AmDescRegex;
    }

    @Override
    public final Pattern getPmDescRegex() {
        return PmDescRegex;
    }

    @Override
    public final Pattern getAmPmDescRegex() {
        return AmPmDescRegex;
    }

    @Override
    public final Pattern getRangeUnitRegex() {
        return RangeUnitRegex;
    }

    @Override
    public final Pattern getTimeUnitRegex() {
        return TimeUnitRegex;
    }

    @Override
    public final Pattern getDateUnitRegex() {
        return DateUnitRegex;
    }

    @Override
    public final Pattern getCommonDatePrefixRegex() {
        return CommonDatePrefixRegex;
    }
}
