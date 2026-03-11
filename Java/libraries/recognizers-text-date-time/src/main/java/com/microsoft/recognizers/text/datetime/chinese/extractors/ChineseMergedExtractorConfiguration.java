package com.microsoft.recognizers.text.datetime.chinese.extractors;

import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.BaseDateExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseDatePeriodExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseDateTimeAltExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseDateTimePeriodExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseDurationExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseHolidayExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseSetExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseTimePeriodExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseTimeZoneExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeListExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeZoneExtractor;
import com.microsoft.recognizers.text.datetime.extractors.config.IMergedExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.resources.ChineseDateTime;
import com.microsoft.recognizers.text.matcher.StringMatcher;
import com.microsoft.recognizers.text.number.chinese.extractors.IntegerExtractor;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.ArrayList;
import java.util.regex.Pattern;
import java.util.stream.Collectors;

import org.javatuples.Pair;

public class ChineseMergedExtractorConfiguration extends BaseOptionsConfiguration implements IMergedExtractorConfiguration {

    public static final Pattern BeforeRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.MergedBeforeRegex);
    public static final Pattern AfterRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.MergedAfterRegex);
    public static final Pattern SinceRegex = RegExpUtility.getSafeRegExp(
            ChineseDateTime.ParserConfigurationSincePrefix + "|" + ChineseDateTime.ParserConfigurationSinceSuffix);
    public static final Pattern AroundRegex = RegExpUtility.getSafeRegExp(
            ChineseDateTime.ParserConfigurationAroundPrefix + "|" + ChineseDateTime.ParserConfigurationAroundSuffix);
    public static final Pattern FromToRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.FromToRegex);
    public static final Pattern AmbiguousRangeModifierPrefix = RegExpUtility.getSafeRegExp(ChineseDateTime.AmbiguousRangeModifierPrefix);
    public static final Pattern UnspecificDatePeriodRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.UnspecificDatePeriodRegex);

    public static final StringMatcher SuperfluousWordMatcher = new StringMatcher();

    private static final Iterable<Pattern> filterWordRegexList = new ArrayList<Pattern>();

    private final Iterable<Pair<Pattern, Pattern>> ambiguityFiltersDict;

    private IDateTimeExtractor setExtractor;
    private IExtractor integerExtractor;
    private IDateTimeExtractor dateExtractor;
    private IDateTimeExtractor timeExtractor;
    private IDateTimeExtractor holidayExtractor;
    private IDateTimeExtractor dateTimeExtractor;
    private IDateTimeExtractor durationExtractor;
    private IDateTimeExtractor datePeriodExtractor;
    private IDateTimeExtractor timePeriodExtractor;
    private IDateTimeZoneExtractor timeZoneExtractor;
    private IDateTimeListExtractor dateTimeAltExtractor;
    private IDateTimeExtractor dateTimePeriodExtractor;

    public ChineseMergedExtractorConfiguration(DateTimeOptions options) {
        super(options);

        setExtractor = new BaseSetExtractor(new ChineseSetExtractorConfiguration(options));
        dateExtractor = new BaseDateExtractor(new ChineseDateExtractorConfiguration(this));
        timeExtractor = new BaseTimeExtractor(new ChineseTimeExtractorConfiguration(options));
        holidayExtractor = new BaseHolidayExtractor(new ChineseHolidayExtractorConfiguration());
        datePeriodExtractor = new BaseDatePeriodExtractor(new ChineseDatePeriodExtractorConfiguration(this));
        dateTimeExtractor = new BaseDateTimeExtractor(new ChineseDateTimeExtractorConfiguration(options));
        durationExtractor = new BaseDurationExtractor(new ChineseDurationExtractorConfiguration(options));
        timeZoneExtractor = new BaseTimeZoneExtractor(new ChineseTimeZoneExtractorConfiguration(options));
        dateTimeAltExtractor = new BaseDateTimeAltExtractor(new ChineseDateTimeAltExtractorConfiguration(this));
        timePeriodExtractor = new BaseTimePeriodExtractor(new ChineseTimePeriodExtractorConfiguration(options));
        dateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new ChineseDateTimePeriodExtractorConfiguration(options));
        integerExtractor = new IntegerExtractor();

        ambiguityFiltersDict = ChineseDateTime.AmbiguityFiltersDict.entrySet().stream().map(pair -> {
            Pattern key = RegExpUtility.getSafeRegExp(pair.getKey());
            Pattern val = RegExpUtility.getSafeRegExp(pair.getValue());
            return new Pair<Pattern, Pattern>(key, val);
        }).collect(Collectors.toList());
    }

    public final Iterable<Pattern> getFilterWordRegexList() {
        return filterWordRegexList;
    }

    public final StringMatcher getSuperfluousWordMatcher() {
        return SuperfluousWordMatcher;
    }

    public final IDateTimeExtractor getSetExtractor() {
        return setExtractor;
    }

    public final IExtractor getIntegerExtractor() {
        return integerExtractor;
    }

    public final IDateTimeExtractor getDateExtractor() {
        return dateExtractor;
    }

    public final IDateTimeExtractor getTimeExtractor() {
        return timeExtractor;
    }

    public final IDateTimeExtractor getHolidayExtractor() {
        return holidayExtractor;
    }

    public final IDateTimeExtractor getDateTimeExtractor() {
        return dateTimeExtractor;
    }

    public final IDateTimeExtractor getDurationExtractor() {
        return durationExtractor;
    }

    public final IDateTimeExtractor getDatePeriodExtractor() {
        return datePeriodExtractor;
    }

    public final IDateTimeExtractor getTimePeriodExtractor() {
        return timePeriodExtractor;
    }

    public final IDateTimeZoneExtractor getTimeZoneExtractor() {
        return timeZoneExtractor;
    }

    public final IDateTimeListExtractor getDateTimeAltExtractor() {
        return dateTimeAltExtractor;
    }

    public final IDateTimeExtractor getDateTimePeriodExtractor() {
        return dateTimePeriodExtractor;
    }

    public final Pattern getAfterRegex() {
        return AfterRegex;
    }

    public final Pattern getSinceRegex() {
        return SinceRegex;
    }

    public final Pattern getAroundRegex() {
        return AroundRegex;
    }

    public final Pattern getBeforeRegex() {
        return BeforeRegex;
    }

    public final Pattern getFromToRegex() {
        return FromToRegex;
    }

    public final Pattern getSuffixAfterRegex() {
        return null;
    }

    public final Pattern getNumberEndingPattern() {
        return null;
    }

    public final Pattern getPrepositionSuffixRegex() {
        return null;
    }

    public final Pattern getAmbiguousRangeModifierPrefix() {
        return AmbiguousRangeModifierPrefix;
    }

    public final Pattern getPotentialAmbiguousRangeRegex() {
        return FromToRegex;
    }

    public final Pattern getSingleAmbiguousMonthRegex() {
        return null;
    }

    public final Pattern getUnspecificDatePeriodRegex() {
        return UnspecificDatePeriodRegex;
    }

    public final Iterable<Pair<Pattern, Pattern>> getAmbiguityFiltersDict() {
        return ambiguityFiltersDict;
    }
}
