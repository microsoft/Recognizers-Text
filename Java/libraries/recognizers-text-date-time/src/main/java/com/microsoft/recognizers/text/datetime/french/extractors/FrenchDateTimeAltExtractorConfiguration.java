package com.microsoft.recognizers.text.datetime.french.extractors;

import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.config.IOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.BaseDateExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseDatePeriodExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.config.IDateTimeAltExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.resources.FrenchDateTime;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import java.util.ArrayList;
import java.util.regex.Pattern;

public class FrenchDateTimeAltExtractorConfiguration extends BaseOptionsConfiguration implements IDateTimeAltExtractorConfiguration {

    public static final Pattern ThisPrefixRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.ThisPrefixRegex);
    public static final Pattern PreviousPrefixRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.PreviousPrefixRegex);
    public static final Pattern NextPrefixRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.NextPrefixRegex);
    public static final Pattern AmRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.AmRegex);
    public static final Pattern PmRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.PmRegex);
    public static final Pattern RangePrefixRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.RangePrefixRegex);
    public static final Iterable<Pattern> RelativePrefixList = new ArrayList<Pattern>() {
        {
            add(ThisPrefixRegex);
            add(PreviousPrefixRegex);
            add(NextPrefixRegex);
        }
    };
    public static final Iterable<Pattern> AmPmRegexList = new ArrayList<Pattern>() {
        {
            add(AmRegex);
            add(PmRegex);
        }
    };
    private static final Pattern OrRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.OrRegex);
    private static final Pattern DayRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.DayRegex);
    private final IDateExtractor dateExtractor;
    private final IDateTimeExtractor datePeriodExtractor;

    public FrenchDateTimeAltExtractorConfiguration(final IOptionsConfiguration config) {
        super(config.getOptions());
        dateExtractor = new BaseDateExtractor(new FrenchDateExtractorConfiguration(this));
        datePeriodExtractor = new BaseDatePeriodExtractor(new FrenchDatePeriodExtractorConfiguration(this));
    }

    @Override
    public IDateExtractor getDateExtractor() {
        return dateExtractor;
    }

    @Override
    public IDateTimeExtractor getDatePeriodExtractor() {
        return datePeriodExtractor;
    }

    @Override
    public Iterable<Pattern> getRelativePrefixList() {
        return RelativePrefixList;
    }

    @Override
    public Iterable<Pattern> getAmPmRegexList() {
        return AmPmRegexList;
    }

    @Override
    public Pattern getOrRegex() {
        return OrRegex;
    }

    @Override
    public Pattern getThisPrefixRegex() {
        return ThisPrefixRegex;
    }

    @Override
    public Pattern getDayRegex() {
        return DayRegex;
    }

    @Override
    public Pattern getRangePrefixRegex() {
        return RangePrefixRegex;
    }
}
