package com.microsoft.recognizers.text.datetime.english.utilities;

import com.microsoft.recognizers.text.datetime.resources.EnglishDateTime;
import com.microsoft.recognizers.text.datetime.utilities.IDateTimeUtilityConfiguration;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.regex.Pattern;

public class EnglishDatetimeUtilityConfiguration implements IDateTimeUtilityConfiguration {

    public static final Pattern AgoRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AgoRegex);
    public static final Pattern LaterRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.LaterRegex);
    public static final Pattern InConnectorRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.InConnectorRegex);
    public static final Pattern WithinNextPrefixRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.WithinNextPrefixRegex);
    public static final Pattern AmDescRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AmDescRegex);
    public static final Pattern PmDescRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PmDescRegex);
    public static final Pattern AmPmDescRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AmPmDescRegex);
    public static final Pattern RangeUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.RangeUnitRegex);
    public static final Pattern TimeUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.TimeUnitRegex);
    public static final Pattern DateUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.DateUnitRegex);
    public static final Pattern CommonDatePrefixRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.CommonDatePrefixRegex);

    @Override
    public Pattern getAgoRegex() {
        return AgoRegex;
    }

    @Override
    public Pattern getLaterRegex() {
        return LaterRegex;
    }

    @Override
    public Pattern getInConnectorRegex() {
        return InConnectorRegex;
    }

    @Override
    public Pattern getWithinNextPrefixRegex() {
        return WithinNextPrefixRegex;
    }

    @Override
    public Pattern getRangeUnitRegex() {
        return RangeUnitRegex;
    }

    @Override
    public Pattern getTimeUnitRegex() {
        return TimeUnitRegex;
    }

    @Override
    public Pattern getDateUnitRegex() {
        return DateUnitRegex;
    }

    @Override
    public Pattern getAmDescRegex() {
        return AmDescRegex;
    }

    @Override
    public Pattern getPmDescRegex() {
        return PmDescRegex;
    }

    @Override
    public Pattern getAmPmDescRegex() {
        return AmPmDescRegex;
    }

    @Override
    public Pattern getCommonDatePrefixRegex() {
        return CommonDatePrefixRegex;
    }
}
