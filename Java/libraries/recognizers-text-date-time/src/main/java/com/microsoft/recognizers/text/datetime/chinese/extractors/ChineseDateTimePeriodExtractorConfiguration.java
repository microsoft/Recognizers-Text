package com.microsoft.recognizers.text.datetime.chinese.extractors;

import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.config.IDateTimePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.config.ResultIndex;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.resources.ChineseDateTime;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.ArrayList;
import java.util.regex.Pattern;

public class ChineseDateTimePeriodExtractorConfiguration extends BaseOptionsConfiguration implements IDateTimePeriodExtractorConfiguration {

    public static final Pattern DateTimePeriodTillRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DateTimePeriodTillRegex);
    public static final Pattern DateTimePeriodPrepositionRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DateTimePeriodPrepositionRegex);
    public static final Pattern SpecificTimeOfDayRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.SpecificTimeOfDayRegex);
    public static final Pattern TimeOfDayRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.TimeOfDayRegex);
    public static final Pattern DateTimePeriodUnitRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DateTimePeriodUnitRegex);
    public static final Pattern DateTimePeriodFollowedUnit = RegExpUtility.getSafeRegExp(ChineseDateTime.DateTimePeriodFollowedUnit);
    public static final Pattern DateTimePeriodNumberCombinedWithUnit = RegExpUtility.getSafeRegExp(ChineseDateTime.DateTimePeriodNumberCombinedWithUnit);
    public static final Pattern PastRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.PastRegex);
    public static final Pattern FutureRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.FutureRegex);
    public static final Pattern WeekDayRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.WeekDayRegex);

    public static final Iterable<Pattern> SimpleCases = new ArrayList<Pattern>();

    private final String tokenBeforeDate;

    public ChineseDateTimePeriodExtractorConfiguration() {
        this(DateTimeOptions.None);
    }

    public ChineseDateTimePeriodExtractorConfiguration(DateTimeOptions options) {
        super(options);
        tokenBeforeDate = ChineseDateTime.PrepositionRegex;
    }

    @Override
    public String getTokenBeforeDate() {
        return tokenBeforeDate;
    }

    @Override
    public Iterable<Pattern> getSimpleCasesRegex() {
        return SimpleCases;
    }

    @Override
    public Pattern getPrepositionRegex() {
        return DateTimePeriodPrepositionRegex;
    }

    @Override
    public Pattern getTillRegex() {
        return DateTimePeriodTillRegex;
    }

    @Override
    public Pattern getSpecificTimeOfDayRegex() {
        return SpecificTimeOfDayRegex;
    }

    @Override
    public Pattern getTimeOfDayRegex() {
        return TimeOfDayRegex;
    }

    @Override
    public Pattern getFollowedUnit() {
        return DateTimePeriodFollowedUnit;
    }

    @Override
    public Pattern getNumberCombinedWithUnit() {
        return DateTimePeriodNumberCombinedWithUnit;
    }

    @Override
    public Pattern getTimeUnitRegex() {
        return DateTimePeriodUnitRegex;
    }

    @Override
    public Pattern getPastPrefixRegex() {
        return PastRegex;
    }

    @Override
    public Pattern getNextPrefixRegex() {
        return FutureRegex;
    }

    @Override
    public Pattern getFutureSuffixRegex() {
        return FutureRegex;
    }

    @Override
    public Pattern getWeekDayRegex() {
        return WeekDayRegex;
    }

    @Override
    public Pattern getPeriodTimeOfDayWithDateRegex() {
        return null;
    }

    @Override
    public Pattern getRelativeTimeUnitRegex() {
        return null;
    }

    @Override
    public Pattern getRestOfDateTimeRegex() {
        return null;
    }

    @Override
    public Pattern getGeneralEndingRegex() {
        return null;
    }

    @Override
    public Pattern getMiddlePauseRegex() {
        return null;
    }

    @Override
    public Pattern getAmDescRegex() {
        return null;
    }

    @Override
    public Pattern getPmDescRegex() {
        return null;
    }

    @Override
    public Pattern getWithinNextPrefixRegex() {
        return null;
    }

    @Override
    public Pattern getDateUnitRegex() {
        return null;
    }

    @Override
    public Pattern getPrefixDayRegex() {
        return null;
    }

    @Override
    public Pattern getSuffixRegex() {
        return null;
    }

    @Override
    public Pattern getBeforeRegex() {
        return null;
    }

    @Override
    public Pattern getAfterRegex() {
        return null;
    }

    @Override
    public IExtractor getCardinalExtractor() {
        return null;
    }

    @Override
    public IDateTimeExtractor getSingleDateExtractor() {
        return null;
    }

    @Override
    public IDateTimeExtractor getSingleTimeExtractor() {
        return null;
    }

    @Override
    public IDateTimeExtractor getSingleDateTimeExtractor() {
        return null;
    }

    @Override
    public IDateTimeExtractor getDurationExtractor() {
        return null;
    }

    @Override
    public IDateTimeExtractor getTimePeriodExtractor() {
        return null;
    }

    @Override
    public IDateTimeExtractor getTimeZoneExtractor() {
        return null;
    }

    @Override
    public ResultIndex getFromTokenIndex(String text) {
        int index = -1;
        boolean result = false;
        if (text.endsWith("从")) {
            result = true;
            index = text.lastIndexOf("从");
        }
        return new ResultIndex(result, index);
    }

    @Override
    public ResultIndex getBetweenTokenIndex(String text) {
        int index = -1;
        boolean result = false;
        return new ResultIndex(result, index);
    }

    @Override
    public boolean hasConnectorToken(String text) {
        return false;
    }
}
