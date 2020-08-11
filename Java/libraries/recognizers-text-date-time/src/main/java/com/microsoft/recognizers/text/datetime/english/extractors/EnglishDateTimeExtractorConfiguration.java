package com.microsoft.recognizers.text.datetime.english.extractors;

import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.english.utilities.EnglishDatetimeUtilityConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.BaseDateExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseDurationExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.config.IDateTimeExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.resources.EnglishDateTime;
import com.microsoft.recognizers.text.datetime.utilities.IDateTimeUtilityConfiguration;
import com.microsoft.recognizers.text.number.english.extractors.IntegerExtractor;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import com.microsoft.recognizers.text.utilities.StringUtility;

import java.util.Arrays;
import java.util.regex.Pattern;

public class EnglishDateTimeExtractorConfiguration extends BaseOptionsConfiguration implements IDateTimeExtractorConfiguration {

    public static final Pattern PrepositionRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PrepositionRegex);
    public static final Pattern NowRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.NowRegex);
    public static final Pattern SuffixRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SuffixRegex);
    public static final Pattern TimeOfDayRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.TimeOfDayRegex);
    public static final Pattern SpecificTimeOfDayRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SpecificTimeOfDayRegex);
    public static final Pattern TimeOfTodayAfterRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.TimeOfTodayAfterRegex);
    public static final Pattern TimeOfTodayBeforeRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.TimeOfTodayBeforeRegex);
    public static final Pattern SimpleTimeOfTodayAfterRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SimpleTimeOfTodayAfterRegex);
    public static final Pattern SimpleTimeOfTodayBeforeRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SimpleTimeOfTodayBeforeRegex);
    public static final Pattern SpecificEndOfRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SpecificEndOfRegex);
    public static final Pattern UnspecificEndOfRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.UnspecificEndOfRegex);
    public static final Pattern UnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.TimeUnitRegex);
    public static final Pattern ConnectorRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.ConnectorRegex);
    public static final Pattern NumberAsTimeRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.NumberAsTimeRegex);
    public static final Pattern DateNumberConnectorRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.DateNumberConnectorRegex);
    public static final Pattern SuffixAfterRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SuffixAfterRegex);

    public IExtractor integerExtractor;
    public IDateTimeExtractor datePointExtractor;
    public IDateTimeExtractor timePointExtractor;
    public IDateTimeExtractor durationExtractor;
    public IDateTimeUtilityConfiguration utilityConfiguration;

    public EnglishDateTimeExtractorConfiguration(DateTimeOptions options) {

        super(options);

        integerExtractor = IntegerExtractor.getInstance();
        datePointExtractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration(this));
        timePointExtractor = new BaseTimeExtractor(new EnglishTimeExtractorConfiguration(options));
        durationExtractor = new BaseDurationExtractor(new EnglishDurationExtractorConfiguration(options));

        utilityConfiguration = new EnglishDatetimeUtilityConfiguration();
    }

    public EnglishDateTimeExtractorConfiguration() {
        this(DateTimeOptions.None);
    }

    @Override
    public Pattern getNowRegex() {
        return NowRegex;
    }

    @Override
    public Pattern getSuffixRegex() {
        return SuffixRegex;
    }

    @Override
    public Pattern getTimeOfTodayAfterRegex() {
        return TimeOfTodayAfterRegex;
    }

    @Override
    public Pattern getSimpleTimeOfTodayAfterRegex() {
        return SimpleTimeOfTodayAfterRegex;
    }

    @Override
    public Pattern getTimeOfTodayBeforeRegex() {
        return TimeOfTodayBeforeRegex;
    }

    @Override
    public Pattern getSimpleTimeOfTodayBeforeRegex() {
        return SimpleTimeOfTodayBeforeRegex;
    }

    @Override
    public Pattern getTimeOfDayRegex() {
        return TimeOfDayRegex;
    }

    @Override
    public Pattern getSpecificEndOfRegex() {
        return SpecificEndOfRegex;
    }

    @Override
    public Pattern getUnspecificEndOfRegex() {
        return UnspecificEndOfRegex;
    }

    @Override
    public Pattern getUnitRegex() {
        return UnitRegex;
    }

    @Override
    public Pattern getNumberAsTimeRegex() {
        return NumberAsTimeRegex;
    }

    @Override
    public Pattern getDateNumberConnectorRegex() {
        return DateNumberConnectorRegex;
    }

    @Override
    public Pattern getSuffixAfterRegex() {
        return SuffixAfterRegex;
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
        boolean isConnector = Arrays.stream(RegExpUtility.getMatches(ConnectorRegex, text)).findFirst().isPresent();
        return (StringUtility.isNullOrEmpty(text) || isPreposition || isConnector);
    }
}
