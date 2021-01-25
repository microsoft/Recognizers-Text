package com.microsoft.recognizers.text.datetime.french.extractors;

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
import com.microsoft.recognizers.text.datetime.extractors.IDateExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeListExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeZoneExtractor;
import com.microsoft.recognizers.text.datetime.extractors.config.IMergedExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.resources.FrenchDateTime;
import com.microsoft.recognizers.text.matcher.StringMatcher;
import com.microsoft.recognizers.text.number.french.extractors.IntegerExtractor;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import java.util.regex.Pattern;
import java.util.stream.Collectors;

import org.javatuples.Pair;

public class FrenchMergedExtractorConfiguration extends BaseOptionsConfiguration implements IMergedExtractorConfiguration {

    public static final Pattern BeforeRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.BeforeRegex);
    public static final Pattern AfterRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.AfterRegex);
    public static final Pattern SinceRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.SinceRegex);
    public static final Pattern AroundRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.AroundRegex);
    public static final Pattern FromToRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.FromToRegex);
    public static final Pattern SingleAmbiguousMonthRegex = RegExpUtility
        .getSafeRegExp(FrenchDateTime.SingleAmbiguousMonthRegex);
    public static final Pattern PrepositionSuffixRegex = RegExpUtility
        .getSafeRegExp(FrenchDateTime.PrepositionSuffixRegex);
    public static final Pattern AmbiguousRangeModifierPrefix = RegExpUtility
        .getSafeRegExp(FrenchDateTime.AmbiguousRangeModifierPrefix);
    public static final Pattern NumberEndingPattern = RegExpUtility.getSafeRegExp(FrenchDateTime.NumberEndingPattern);
    public static final Pattern SuffixAfterRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.SuffixAfterRegex);
    public static final Pattern UnspecificDatePeriodRegex = RegExpUtility
        .getSafeRegExp(FrenchDateTime.UnspecificDatePeriodRegex);
    public static final StringMatcher SuperfluousWordMatcher = new StringMatcher();
    public final Iterable<Pair<Pattern, Pattern>> ambiguityFiltersDict = FrenchDateTime.AmbiguityFiltersDict.entrySet().stream().map(pair -> {
        Pattern key = RegExpUtility.getSafeRegExp(pair.getKey());
        Pattern val = RegExpUtility.getSafeRegExp(pair.getValue());
        return new Pair<Pattern, Pattern>(key, val);
    }).collect(Collectors.toList());
    private final IDateExtractor dateExtractor;
    private final IDateTimeExtractor timeExtractor;
    private final IDateTimeExtractor dateTimeExtractor;
    private final IDateTimeExtractor datePeriodExtractor;
    private final IDateTimeExtractor timePeriodExtractor;
    private final IDateTimeExtractor dateTimePeriodExtractor;
    private final IDateTimeExtractor durationExtractor;
    private final IDateTimeExtractor setExtractor;
    private final IDateTimeExtractor holidayExtractor;
    private final IDateTimeZoneExtractor timeZoneExtractor;
    private final IDateTimeListExtractor dateTimeAltExtractor;
    private final IExtractor integerExtractor;

    public FrenchMergedExtractorConfiguration(final DateTimeOptions options) {
        super(options);

        setExtractor = new BaseSetExtractor(new FrenchSetExtractorConfiguration(options));
        dateExtractor = new BaseDateExtractor(new FrenchDateExtractorConfiguration(this));
        timeExtractor = new BaseTimeExtractor(new FrenchTimeExtractorConfiguration(options));
        holidayExtractor = new BaseHolidayExtractor(new FrenchHolidayExtractorConfiguration());
        datePeriodExtractor = new BaseDatePeriodExtractor(new FrenchDatePeriodExtractorConfiguration(this));
        dateTimeExtractor = new BaseDateTimeExtractor(new FrenchDateTimeExtractorConfiguration(options));
        durationExtractor = new BaseDurationExtractor(new FrenchDurationExtractorConfiguration(options));
        timeZoneExtractor = new BaseTimeZoneExtractor(new FrenchTimeZoneExtractorConfiguration(options));
        dateTimeAltExtractor = new BaseDateTimeAltExtractor(new FrenchDateTimeAltExtractorConfiguration(this));
        timePeriodExtractor = new BaseTimePeriodExtractor(new FrenchTimePeriodExtractorConfiguration(options));
        dateTimePeriodExtractor = new BaseDateTimePeriodExtractor(
            new FrenchDateTimePeriodExtractorConfiguration(options));
        integerExtractor = new IntegerExtractor();
    }

    public final StringMatcher getSuperfluousWordMatcher() {
        return SuperfluousWordMatcher;
    }

    public final IDateExtractor getDateExtractor() {
        return dateExtractor;
    }

    public final IDateTimeExtractor getTimeExtractor() {
        return timeExtractor;
    }

    public final IDateTimeExtractor getDateTimeExtractor() {
        return dateTimeExtractor;
    }

    public final IDateTimeExtractor getDatePeriodExtractor() {
        return datePeriodExtractor;
    }

    public final IDateTimeExtractor getTimePeriodExtractor() {
        return timePeriodExtractor;
    }

    public final IDateTimeExtractor getDateTimePeriodExtractor() {
        return dateTimePeriodExtractor;
    }

    public final IDateTimeExtractor getDurationExtractor() {
        return durationExtractor;
    }

    public final IDateTimeExtractor getSetExtractor() {
        return setExtractor;
    }

    public final IDateTimeExtractor getHolidayExtractor() {
        return holidayExtractor;
    }

    public final IDateTimeZoneExtractor getTimeZoneExtractor() {
        return timeZoneExtractor;
    }

    public final IDateTimeListExtractor getDateTimeAltExtractor() {
        return dateTimeAltExtractor;
    }

    public final IExtractor getIntegerExtractor() {
        return integerExtractor;
    }

    public final Iterable<Pair<Pattern, Pattern>> getAmbiguityFiltersDict() {
        return ambiguityFiltersDict;
    }

    @Override
    public Iterable<Pattern> getFilterWordRegexList() {
        return null;
    }

    public final Pattern getAfterRegex() {
        return AfterRegex;
    }

    public final Pattern getBeforeRegex() {
        return BeforeRegex;
    }

    public final Pattern getSinceRegex() {
        return SinceRegex;
    }

    public final Pattern getAroundRegex() {
        return AroundRegex;
    }

    public final Pattern getFromToRegex() {
        return FromToRegex;
    }

    public final Pattern getSingleAmbiguousMonthRegex() {
        return SingleAmbiguousMonthRegex;
    }

    public final Pattern getPrepositionSuffixRegex() {
        return PrepositionSuffixRegex;
    }

    public final Pattern getAmbiguousRangeModifierPrefix() {
        return null;
    }

    public final Pattern getPotentialAmbiguousRangeRegex() {
        return null;
    }

    public final Pattern getNumberEndingPattern() {
        return NumberEndingPattern;
    }

    public final Pattern getSuffixAfterRegex() {
        return SuffixAfterRegex;
    }

    public final Pattern getUnspecificDatePeriodRegex() {
        return UnspecificDatePeriodRegex;
    }

}
