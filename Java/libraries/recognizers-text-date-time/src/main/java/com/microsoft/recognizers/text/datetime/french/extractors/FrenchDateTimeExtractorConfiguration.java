package com.microsoft.recognizers.text.datetime.french.extractors;

import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.BaseDateExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseDurationExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.config.IDateTimeExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.french.utilities.FrenchDatetimeUtilityConfiguration;
import com.microsoft.recognizers.text.datetime.resources.FrenchDateTime;
import com.microsoft.recognizers.text.datetime.utilities.IDateTimeUtilityConfiguration;
import com.microsoft.recognizers.text.number.english.extractors.IntegerExtractor;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import com.microsoft.recognizers.text.utilities.StringUtility;
import java.util.Arrays;
import java.util.regex.Pattern;

public class FrenchDateTimeExtractorConfiguration extends BaseOptionsConfiguration implements IDateTimeExtractorConfiguration {

    public static final Pattern PrepositionRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.PrepositionRegex);
    public static final Pattern NowRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.NowRegex);
    public static final Pattern SuffixRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.SuffixRegex);

    //TODO: modify it according to the corresponding English regex

    public static final Pattern TimeOfDayRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.TimeOfDayRegex);
    public static final Pattern SpecificTimeOfDayRegex = RegExpUtility
        .getSafeRegExp(FrenchDateTime.SpecificTimeOfDayRegex);
    public static final Pattern TimeOfTodayAfterRegex = RegExpUtility
        .getSafeRegExp(FrenchDateTime.TimeOfTodayAfterRegex);
    public static final Pattern TimeOfTodayBeforeRegex = RegExpUtility
        .getSafeRegExp(FrenchDateTime.TimeOfTodayBeforeRegex);
    public static final Pattern SimpleTimeOfTodayAfterRegex = RegExpUtility
        .getSafeRegExp(FrenchDateTime.SimpleTimeOfTodayAfterRegex);
    public static final Pattern SimpleTimeOfTodayBeforeRegex = RegExpUtility
        .getSafeRegExp(FrenchDateTime.SimpleTimeOfTodayBeforeRegex);
    public static final Pattern SpecificEndOfRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.SpecificEndOfRegex);
    public static final Pattern UnspecificEndOfRegex = RegExpUtility
        .getSafeRegExp(FrenchDateTime.UnspecificEndOfRegex);

    //TODO: add this for french
    public static final Pattern UnitRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.TimeUnitRegex);
    public static final Pattern ConnectorRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.ConnectorRegex);
    public static final Pattern NumberAsTimeRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.NumberAsTimeRegex);
    public static final Pattern DateNumberConnectorRegex = RegExpUtility
        .getSafeRegExp(FrenchDateTime.DateNumberConnectorRegex);
    public static final Pattern SuffixAfterRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.SuffixAfterRegex);
    private final IExtractor integerExtractor;
    private final IDateExtractor datePointExtractor;
    private final IDateTimeExtractor timePointExtractor;
    private final IDateTimeExtractor durationExtractor;
    private final IDateTimeUtilityConfiguration utilityConfiguration;

    public FrenchDateTimeExtractorConfiguration(final DateTimeOptions options) {

        super(options);

        integerExtractor = IntegerExtractor.getInstance();
        datePointExtractor = new BaseDateExtractor(new FrenchDateExtractorConfiguration(this));
        timePointExtractor = new BaseTimeExtractor(new FrenchTimeExtractorConfiguration(options));
        durationExtractor = new BaseDurationExtractor(new FrenchDurationExtractorConfiguration(options));

        utilityConfiguration = new FrenchDatetimeUtilityConfiguration();
    }

    public FrenchDateTimeExtractorConfiguration() {
        this(DateTimeOptions.None);
    }

    @Override
    public IExtractor getIntegerExtractor() {
        return integerExtractor;
    }

    @Override
    public IDateExtractor getDatePointExtractor() {
        return datePointExtractor;
    }

    @Override
    public IDateTimeExtractor getTimePointExtractor() {
        return timePointExtractor;
    }

    @Override
    public IDateTimeExtractor getDurationExtractor() {
        return durationExtractor;
    }

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

        final boolean isPreposition = Arrays.stream(RegExpUtility.getMatches(PrepositionRegex, text)).findFirst().isPresent();
        final boolean isConnector = Arrays.stream(RegExpUtility.getMatches(ConnectorRegex, text)).findFirst().isPresent();
        return (StringUtility.isNullOrEmpty(text) || isPreposition || isConnector);
    }
}
