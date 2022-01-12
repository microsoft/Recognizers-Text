// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.datetime.german.utilities;

import com.microsoft.recognizers.text.datetime.resources.GermanDateTime;
import com.microsoft.recognizers.text.datetime.utilities.IDateTimeUtilityConfiguration;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.regex.Pattern;

public class GermanDatetimeUtilityConfiguration implements IDateTimeUtilityConfiguration {

    public static final Pattern AgoRegex = RegExpUtility.getSafeRegExp(GermanDateTime.AgoRegex);
    public static final Pattern LaterRegex = RegExpUtility.getSafeRegExp(GermanDateTime.LaterRegex);
    public static final Pattern InConnectorRegex = RegExpUtility.getSafeRegExp(GermanDateTime.InConnectorRegex);
    public static final Pattern WithinNextPrefixRegex = RegExpUtility.getSafeRegExp(GermanDateTime.WithinNextPrefixRegex);
    public static final Pattern AmDescRegex = RegExpUtility.getSafeRegExp(GermanDateTime.AmDescRegex);
    public static final Pattern PmDescRegex = RegExpUtility.getSafeRegExp(GermanDateTime.PmDescRegex);
    public static final Pattern AmPmDescRegex = RegExpUtility.getSafeRegExp(GermanDateTime.AmPmDescRegex);
    public static final Pattern RangeUnitRegex = RegExpUtility.getSafeRegExp(GermanDateTime.RangeUnitRegex);
    public static final Pattern TimeUnitRegex = RegExpUtility.getSafeRegExp(GermanDateTime.TimeUnitRegex);
    public static final Pattern DateUnitRegex = RegExpUtility.getSafeRegExp(GermanDateTime.DateUnitRegex);
    public static final Pattern CommonDatePrefixRegex = RegExpUtility.getSafeRegExp(GermanDateTime.CommonDatePrefixRegex);

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
