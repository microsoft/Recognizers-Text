package com.microsoft.recognizers.text.datetime.english.extractors;

import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.english.utilities.EnglishDatetimeUtilityConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.BaseTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseTimeZoneExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.config.ITimePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.config.ResultIndex;
import com.microsoft.recognizers.text.datetime.resources.EnglishDateTime;
import com.microsoft.recognizers.text.datetime.utilities.IDateTimeUtilityConfiguration;
import com.microsoft.recognizers.text.number.english.extractors.IntegerExtractor;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.ArrayList;
import java.util.regex.Pattern;

public class EnglishTimePeriodExtractorConfiguration extends BaseOptionsConfiguration implements ITimePeriodExtractorConfiguration {

    private String tokenBeforeDate;

    public final String getTokenBeforeDate() {
        return tokenBeforeDate;
    }

    public static final Pattern AmRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AmRegex);
    public static final Pattern PmRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PmRegex);
    public static final Pattern HourRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.HourRegex);
    public static final Pattern TillRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.TillRegex);
    public static final Pattern PeriodDescRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.DescRegex);
    public static final Pattern PureNumFromTo = RegExpUtility.getSafeRegExp(EnglishDateTime.PureNumFromTo);
    public static final Pattern TimeUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.TimeUnitRegex);
    public static final Pattern TimeOfDayRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.TimeOfDayRegex);
    public static final Pattern PrepositionRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PrepositionRegex);
    public static final Pattern TimeFollowedUnit = RegExpUtility.getSafeRegExp(EnglishDateTime.TimeFollowedUnit);
    public static final Pattern PureNumBetweenAnd = RegExpUtility.getSafeRegExp(EnglishDateTime.PureNumBetweenAnd);
    public static final Pattern GeneralEndingRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.GeneralEndingRegex);
    public static final Pattern PeriodHourNumRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PeriodHourNumRegex);
    public static final Pattern SpecificTimeFromTo = RegExpUtility.getSafeRegExp(EnglishDateTime.SpecificTimeFromTo);
    public static final Pattern SpecificTimeBetweenAnd = RegExpUtility.getSafeRegExp(EnglishDateTime.SpecificTimeBetweenAnd);
    public static final Pattern SpecificTimeOfDayRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SpecificTimeOfDayRegex);
    public static final Pattern TimeNumberCombinedWithUnit = RegExpUtility.getSafeRegExp(EnglishDateTime.TimeNumberCombinedWithUnit);

    public EnglishTimePeriodExtractorConfiguration() {
        this(DateTimeOptions.None);
    }

    public EnglishTimePeriodExtractorConfiguration(DateTimeOptions options) {

        super(options);

        tokenBeforeDate = EnglishDateTime.TokenBeforeDate;
        singleTimeExtractor = new BaseTimeExtractor(new EnglishTimeExtractorConfiguration(options));
        utilityConfiguration = new EnglishDatetimeUtilityConfiguration();
        integerExtractor = IntegerExtractor.getInstance();
        timeZoneExtractor = new BaseTimeZoneExtractor(new EnglishTimeZoneExtractorConfiguration(options));
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
