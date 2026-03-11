package com.microsoft.recognizers.text.datetime.chinese.extractors;

import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.BaseDateExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseDurationExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.config.IDateTimeExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.resources.ChineseDateTime;
import com.microsoft.recognizers.text.datetime.utilities.IDateTimeUtilityConfiguration;
import com.microsoft.recognizers.text.number.chinese.extractors.IntegerExtractor;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import com.microsoft.recognizers.text.utilities.StringUtility;

import java.util.Arrays;
import java.util.regex.Pattern;

public class ChineseDateTimeExtractorConfiguration extends BaseOptionsConfiguration implements IDateTimeExtractorConfiguration {

    public static final Pattern PrepositionRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.PrepositionRegex);
    public static final Pattern NowRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.NowRegex);
    public static final Pattern TimeOfSpecialDayRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.TimeOfSpecialDayRegex);
    public static final Pattern TimeOfDayRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.TimeOfDayRegex);
    public static final Pattern SpecificTimeOfDayRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.SpecificTimeOfDayRegex);
    public static final Pattern BeforeRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.BeforeRegex);
    public static final Pattern AfterRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.AfterRegex);

    private IExtractor integerExtractor;
    private IDateTimeExtractor datePointExtractor;
    private IDateTimeExtractor timePointExtractor;
    private IDateTimeExtractor durationExtractor;
    private IDateTimeUtilityConfiguration utilityConfiguration;

    public ChineseDateTimeExtractorConfiguration(DateTimeOptions options) {
        super(options);

        integerExtractor = new IntegerExtractor();
        datePointExtractor = new BaseDateExtractor(new ChineseDateExtractorConfiguration(this));
        timePointExtractor = new BaseTimeExtractor(new ChineseTimeExtractorConfiguration(options));
        durationExtractor = new BaseDurationExtractor(new ChineseDurationExtractorConfiguration(options));

        utilityConfiguration = null;
    }

    public ChineseDateTimeExtractorConfiguration() {
        this(DateTimeOptions.None);
    }

    @Override
    public Pattern getNowRegex() {
        return NowRegex;
    }

    @Override
    public Pattern getSuffixRegex() {
        return null;
    }

    @Override
    public Pattern getTimeOfTodayAfterRegex() {
        return null;
    }

    @Override
    public Pattern getSimpleTimeOfTodayAfterRegex() {
        return null;
    }

    @Override
    public Pattern getTimeOfTodayBeforeRegex() {
        return TimeOfSpecialDayRegex;
    }

    @Override
    public Pattern getSimpleTimeOfTodayBeforeRegex() {
        return null;
    }

    @Override
    public Pattern getTimeOfDayRegex() {
        return TimeOfDayRegex;
    }

    @Override
    public Pattern getSpecificEndOfRegex() {
        return null;
    }

    @Override
    public Pattern getUnspecificEndOfRegex() {
        return null;
    }

    @Override
    public Pattern getUnitRegex() {
        return null;
    }

    @Override
    public Pattern getNumberAsTimeRegex() {
        return null;
    }

    @Override
    public Pattern getDateNumberConnectorRegex() {
        return null;
    }

    @Override
    public Pattern getSuffixAfterRegex() {
        return null;
    }

    @Override
    public IDateTimeExtractor getDurationExtractor() {
        return durationExtractor;
    }

    @Override
    public IDateTimeExtractor getDatePointExtractor() {
        return datePointExtractor;
    }

    @Override
    public IDateTimeExtractor getTimePointExtractor() {
        return timePointExtractor;
    }

    @Override
    public IExtractor getIntegerExtractor() {
        return integerExtractor;
    }

    @Override
    public IDateTimeUtilityConfiguration getUtilityConfiguration() {
        return utilityConfiguration;
    }

    public boolean isConnector(String text) {
        text = text.trim();
        boolean isPreposition = Arrays.stream(RegExpUtility.getMatches(PrepositionRegex, text)).findFirst().isPresent();
        return (StringUtility.isNullOrEmpty(text) || isPreposition);
    }
}
