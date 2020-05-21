package com.microsoft.recognizers.text.datetime.french.utilities;

import com.microsoft.recognizers.text.datetime.resources.FrenchDateTime;
import com.microsoft.recognizers.text.datetime.utilities.IDateTimeUtilityConfiguration;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import java.util.regex.Pattern;

public class FrenchDatetimeUtilityConfiguration implements IDateTimeUtilityConfiguration {
    public static final Pattern AgoRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.AgoRegex);

    public static final Pattern LaterRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.LaterRegex);

    public static final Pattern InConnectorRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.InConnectorRegex);

    public static final Pattern WithinNextPrefixRegex = RegExpUtility
        .getSafeRegExp(FrenchDateTime.WithinNextPrefixRegex);

    public static final Pattern AmDescRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.AmDescRegex);

    public static final Pattern PmDescRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.PmDescRegex);

    public static final Pattern AmPmDescRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.AmPmDescRegex);

    public static final Pattern RangeUnitRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.RangeUnitRegex);

    public static final Pattern TimeUnitRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.TimeUnitRegex);

    public static final Pattern DateUnitRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.DateUnitRegex);

    public static final Pattern CommonDatePrefixRegex = RegExpUtility
        .getSafeRegExp(FrenchDateTime.CommonDatePrefixRegex);

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
