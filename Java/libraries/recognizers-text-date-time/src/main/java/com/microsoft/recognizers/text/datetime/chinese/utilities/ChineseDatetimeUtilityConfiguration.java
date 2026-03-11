// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.datetime.chinese.utilities;

import com.microsoft.recognizers.text.datetime.resources.ChineseDateTime;
import com.microsoft.recognizers.text.datetime.utilities.IDateTimeUtilityConfiguration;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.regex.Pattern;

public class ChineseDatetimeUtilityConfiguration implements IDateTimeUtilityConfiguration {

    public static final Pattern AgoRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.BeforeRegex);
    public static final Pattern LaterRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.AfterRegex);
    public static final Pattern InConnectorRegex = RegExpUtility.getSafeRegExp("^[.]");
    public static final Pattern WithinNextPrefixRegex = RegExpUtility.getSafeRegExp("^[.]");
    public static final Pattern AmDescRegex = RegExpUtility.getSafeRegExp("^[.]");
    public static final Pattern PmDescRegex = RegExpUtility.getSafeRegExp("^[.]");
    public static final Pattern AmPmDescRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.AmPmDescRegex);
    public static final Pattern RangeUnitRegex = RegExpUtility.getSafeRegExp("^[.]");
    public static final Pattern TimeUnitRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DateTimePeriodUnitRegex);
    public static final Pattern DateUnitRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DateUnitRegex);
    public static final Pattern CommonDatePrefixRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DatePeriodRangePrefixRegex);

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
