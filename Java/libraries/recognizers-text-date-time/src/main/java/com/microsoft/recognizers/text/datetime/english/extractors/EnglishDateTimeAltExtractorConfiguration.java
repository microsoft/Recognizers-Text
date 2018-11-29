package com.microsoft.recognizers.text.datetime.english.extractors;

import com.microsoft.recognizers.text.datetime.extractors.BaseDateExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseDatePeriodExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.config.IDateTimeAltExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.resources.EnglishDateTime;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.ArrayList;
import java.util.regex.Pattern;

public class EnglishDateTimeAltExtractorConfiguration implements IDateTimeAltExtractorConfiguration {

    private static final int flags = Pattern.CASE_INSENSITIVE;

    private static final Pattern OrRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.OrRegex, flags);
    private static final Pattern DayRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.DayRegex, flags);

    public static final Pattern ThisPrefixRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.ThisPrefixRegex, flags);
    public static final Pattern PastPrefixRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PastPrefixRegex, flags);
    public static final Pattern NextPrefixRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.NextPrefixRegex, flags);
    public static final Pattern AmRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AmRegex, flags);
    public static final Pattern PmRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PmRegex, flags);

    public static final Iterable<Pattern> RelativePrefixList = new ArrayList<Pattern>() {
        {
            add(ThisPrefixRegex);
            add(PastPrefixRegex);
            add(NextPrefixRegex);
        }
    };

    public static final Iterable<Pattern> AmPmRegexList = new ArrayList<Pattern>() {
        {
            add(AmRegex);
            add(PmRegex);
        }
    };

    private final IDateTimeExtractor dateExtractor;
    private final IDateTimeExtractor datePeriodExtractor;

    public EnglishDateTimeAltExtractorConfiguration() {
        dateExtractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration());
        datePeriodExtractor = new BaseDatePeriodExtractor(new EnglishDatePeriodExtractorConfiguration());
    }

    @Override
    public IDateTimeExtractor getDateExtractor() {
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
    public Pattern getDayRegex() {
        return DayRegex;
    }
}