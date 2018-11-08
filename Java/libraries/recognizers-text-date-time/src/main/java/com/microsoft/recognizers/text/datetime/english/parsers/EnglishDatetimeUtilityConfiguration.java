package com.microsoft.recognizers.text.datetime.english.parsers;

import com.microsoft.recognizers.text.datetime.resources.EnglishDateTime;
import com.microsoft.recognizers.text.datetime.utilities.IDateTimeUtilityConfiguration;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.regex.Pattern;

public class EnglishDatetimeUtilityConfiguration implements IDateTimeUtilityConfiguration {
    public static final Pattern AgoRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AgoRegex, Pattern.CASE_INSENSITIVE);
    public static final Pattern LaterRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.LaterRegex, Pattern.CASE_INSENSITIVE);
    public static final Pattern InConnectorRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.InConnectorRegex, Pattern.CASE_INSENSITIVE);
    public static final Pattern WithinNextPrefixRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.WithinNextPrefixRegex, Pattern.CASE_INSENSITIVE);
    public static final Pattern AmDescRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AmDescRegex, Pattern.CASE_INSENSITIVE);
    public static final Pattern PmDescRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PmDescRegex, Pattern.CASE_INSENSITIVE);
    public static final Pattern AmPmDescRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AmPmDescRegex, Pattern.CASE_INSENSITIVE);
    public static final Pattern RangeUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.RangeUnitRegex, Pattern.CASE_INSENSITIVE);
    public static final Pattern TimeUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.TimeUnitRegex, Pattern.CASE_INSENSITIVE);
    public static final Pattern DateUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.DateUnitRegex, Pattern.CASE_INSENSITIVE);
    public static final Pattern CommonDatePrefixRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.CommonDatePrefixRegex, Pattern.CASE_INSENSITIVE);

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
