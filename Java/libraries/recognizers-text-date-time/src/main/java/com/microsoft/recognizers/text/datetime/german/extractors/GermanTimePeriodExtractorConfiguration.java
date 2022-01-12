// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.datetime.german.extractors;

import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.BaseTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseTimeZoneExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.config.ITimePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.config.ResultIndex;
import com.microsoft.recognizers.text.datetime.german.utilities.GermanDatetimeUtilityConfiguration;
import com.microsoft.recognizers.text.datetime.resources.GermanDateTime;
import com.microsoft.recognizers.text.datetime.utilities.IDateTimeUtilityConfiguration;
import com.microsoft.recognizers.text.number.german.extractors.IntegerExtractor;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.ArrayList;
import java.util.regex.Pattern;

public class GermanTimePeriodExtractorConfiguration extends BaseOptionsConfiguration implements ITimePeriodExtractorConfiguration {

    private String tokenBeforeDate;

    public final String getTokenBeforeDate() {
        return tokenBeforeDate;
    }

    public static final Pattern AmRegex = RegExpUtility.getSafeRegExp(GermanDateTime.AmRegex);
    public static final Pattern PmRegex = RegExpUtility.getSafeRegExp(GermanDateTime.PmRegex);
    public static final Pattern HourRegex = RegExpUtility.getSafeRegExp(GermanDateTime.HourRegex);
    public static final Pattern TillRegex = RegExpUtility.getSafeRegExp(GermanDateTime.TillRegex);
    public static final Pattern PeriodDescRegex = RegExpUtility.getSafeRegExp(GermanDateTime.DescRegex);
    public static final Pattern PureNumFromTo = RegExpUtility.getSafeRegExp(GermanDateTime.PureNumFromTo);
    public static final Pattern TimeUnitRegex = RegExpUtility.getSafeRegExp(GermanDateTime.TimeUnitRegex);
    public static final Pattern TimeOfDayRegex = RegExpUtility.getSafeRegExp(GermanDateTime.TimeOfDayRegex);
    public static final Pattern PrepositionRegex = RegExpUtility.getSafeRegExp(GermanDateTime.PrepositionRegex);
    public static final Pattern TimeFollowedUnit = RegExpUtility.getSafeRegExp(GermanDateTime.TimeFollowedUnit);
    public static final Pattern PureNumBetweenAnd = RegExpUtility.getSafeRegExp(GermanDateTime.PureNumBetweenAnd);
    public static final Pattern GeneralEndingRegex = RegExpUtility.getSafeRegExp(GermanDateTime.GeneralEndingRegex);
    public static final Pattern PeriodHourNumRegex = RegExpUtility.getSafeRegExp(GermanDateTime.PeriodHourNumRegex);
    public static final Pattern SpecificTimeFromTo = RegExpUtility.getSafeRegExp(GermanDateTime.SpecificTimeFromTo);
    public static final Pattern SpecificTimeBetweenAnd = RegExpUtility.getSafeRegExp(GermanDateTime.SpecificTimeBetweenAnd);
    public static final Pattern SpecificTimeOfDayRegex = RegExpUtility.getSafeRegExp(GermanDateTime.SpecificTimeOfDayRegex);
    public static final Pattern TimeNumberCombinedWithUnit = RegExpUtility.getSafeRegExp(GermanDateTime.TimeNumberCombinedWithUnit);

    public GermanTimePeriodExtractorConfiguration() {
        this(DateTimeOptions.None);
    }

    public GermanTimePeriodExtractorConfiguration(DateTimeOptions options) {

        super(options);

        tokenBeforeDate = GermanDateTime.TokenBeforeDate;
        singleTimeExtractor = new BaseTimeExtractor(new GermanTimeExtractorConfiguration(options));
        utilityConfiguration = new GermanDatetimeUtilityConfiguration();
        integerExtractor = new IntegerExtractor();
        timeZoneExtractor = new BaseTimeZoneExtractor(new GermanTimeZoneExtractorConfiguration(options));
    }

    private IDateTimeUtilityConfiguration utilityConfiguration;

    public final IDateTimeUtilityConfiguration getUtilityConfiguration() {
        return utilityConfiguration;
    }

    private IDateTimeExtractor singleTimeExtractor;

    public final IDateTimeExtractor getSingleTimeExtractor() {
        return singleTimeExtractor;
    }

    private IExtractor integerExtractor;

    public final IExtractor getIntegerExtractor() {
        return integerExtractor;
    }

    private final IDateTimeExtractor timeZoneExtractor;

    public IDateTimeExtractor getTimeZoneExtractor() {
        return timeZoneExtractor;
    }


    public Iterable<Pattern> getSimpleCasesRegex() {
        return getSimpleCasesRegex;
    }

    public final Iterable<Pattern> getSimpleCasesRegex = new ArrayList<Pattern>() {
        {
            add(PureNumFromTo);
            add(PureNumBetweenAnd);
            add(SpecificTimeFromTo);
            add(SpecificTimeBetweenAnd);
        }
    };

    public final Pattern getTillRegex() {
        return TillRegex;
    }

    public final Pattern getTimeOfDayRegex() {
        return TimeOfDayRegex;
    }

    public final Pattern getGeneralEndingRegex() {
        return GeneralEndingRegex;
    }

    public final ResultIndex getFromTokenIndex(String input) {
        ResultIndex result = new ResultIndex(false, -1);
        if (input.endsWith("from")) {
            result = new ResultIndex(true, input.lastIndexOf("from"));
        }

        return result;
    }

    public final ResultIndex getBetweenTokenIndex(String input) {
        ResultIndex result = new ResultIndex(false, -1);
        if (input.endsWith("between")) {
            result = new ResultIndex(true, input.lastIndexOf("between"));
        }

        return result;
    }

    public final boolean hasConnectorToken(String input) {
        return input.equals("and");
    }
}
