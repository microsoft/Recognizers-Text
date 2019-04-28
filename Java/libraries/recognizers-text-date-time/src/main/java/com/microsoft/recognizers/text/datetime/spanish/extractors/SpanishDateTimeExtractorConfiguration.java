package com.microsoft.recognizers.text.datetime.spanish.extractors;

import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.BaseDateExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseDurationExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.config.IDateTimeExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.resources.SpanishDateTime;
import com.microsoft.recognizers.text.datetime.spanish.utilities.SpanishDatetimeUtilityConfiguration;
import com.microsoft.recognizers.text.datetime.utilities.IDateTimeUtilityConfiguration;
import com.microsoft.recognizers.text.number.english.extractors.IntegerExtractor;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import com.microsoft.recognizers.text.utilities.StringUtility;

import java.util.Arrays;
import java.util.regex.Pattern;

public class SpanishDateTimeExtractorConfiguration extends BaseOptionsConfiguration implements IDateTimeExtractorConfiguration {

    public static final Pattern PrepositionRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.PrepositionRegex);
    public static final Pattern NowRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.NowRegex);
    public static final Pattern SuffixRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.SuffixRegex);

    //TODO: modify it according to the corresponding English regex

    public static final Pattern TimeOfDayRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.TimeOfDayRegex);
    public static final Pattern SpecificTimeOfDayRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.SpecificTimeOfDayRegex);
    public static final Pattern TimeOfTodayAfterRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.TimeOfTodayAfterRegex);
    public static final Pattern TimeOfTodayBeforeRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.TimeOfTodayBeforeRegex);
    public static final Pattern SimpleTimeOfTodayAfterRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.SimpleTimeOfTodayAfterRegex);
    public static final Pattern SimpleTimeOfTodayBeforeRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.SimpleTimeOfTodayBeforeRegex);
    public static final Pattern SpecificEndOfRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.SpecificEndOfRegex);
    public static final Pattern UnspecificEndOfRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.UnspecificEndOfRegex);

    //TODO: add this for Spanish
    public static final Pattern UnitRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.TimeUnitRegex);
    public static final Pattern ConnectorRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.ConnectorRegex);
    public static final Pattern NumberAsTimeRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.NumberAsTimeRegex);
    public static final Pattern DateNumberConnectorRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.DateNumberConnectorRegex);
    public static final Pattern SuffixAfterRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.SuffixAfterRegex);

    public SpanishDateTimeExtractorConfiguration(DateTimeOptions options) {

        super(options);

        integerExtractor = IntegerExtractor.getInstance();
        datePointExtractor = new BaseDateExtractor(new SpanishDateExtractorConfiguration(this));
        timePointExtractor = new BaseTimeExtractor(new SpanishTimeExtractorConfiguration(options));
        durationExtractor = new BaseDurationExtractor(new SpanishDurationExtractorConfiguration(options));

        utilityConfiguration = new SpanishDatetimeUtilityConfiguration();
    }

    public SpanishDateTimeExtractorConfiguration() {
        this(DateTimeOptions.None);
    }

    private IExtractor integerExtractor;

    @Override
    public IExtractor getIntegerExtractor() {
        return integerExtractor;
    }

    private IDateExtractor datePointExtractor;

    @Override
    public IDateExtractor getDatePointExtractor() {
        return datePointExtractor;
    }

    private IDateTimeExtractor timePointExtractor;

    @Override
    public IDateTimeExtractor getTimePointExtractor() {
        return timePointExtractor;
    }

    private IDateTimeExtractor durationExtractor;

    @Override
    public IDateTimeExtractor getDurationExtractor() {
        return durationExtractor;
    }

    private IDateTimeUtilityConfiguration utilityConfiguration;

    @Override
    public IDateTimeUtilityConfiguration getUtilityConfiguration() {
        return utilityConfiguration;
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

    public boolean isConnector(String text) {

        text = text.trim();

        boolean isPreposition = Arrays.stream(RegExpUtility.getMatches(PrepositionRegex, text)).findFirst().isPresent();
        boolean isConnector = Arrays.stream(RegExpUtility.getMatches(ConnectorRegex, text)).findFirst().isPresent();
        return (StringUtility.isNullOrEmpty(text) || isPreposition || isConnector);
    }
}
