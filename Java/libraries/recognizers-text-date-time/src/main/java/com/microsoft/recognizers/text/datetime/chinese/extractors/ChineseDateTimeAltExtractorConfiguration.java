package com.microsoft.recognizers.text.datetime.chinese.extractors;

import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.config.IOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.BaseDateExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseDatePeriodExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.config.IDateTimeAltExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.resources.ChineseDateTime;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.ArrayList;
import java.util.regex.Pattern;

public class ChineseDateTimeAltExtractorConfiguration extends BaseOptionsConfiguration implements IDateTimeAltExtractorConfiguration {

    private static final Pattern OrRegex = RegExpUtility.getSafeRegExp("或|或者|还是");
    private static final Pattern DayRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DayRegex);
    private static final Pattern RangePrefixRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DatePeriodRangePrefixRegex);

    public static final Pattern ThisPrefixRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.ThisPrefixRegex);
    public static final Pattern LastPrefixRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.LastPrefixRegex);
    public static final Pattern NextPrefixRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.NextPrefixRegex);
    public static final Pattern AmRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DateTimeSimpleAmRegex);
    public static final Pattern PmRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DateTimeSimplePmRegex);

    public static final Iterable<Pattern> RelativePrefixList = new ArrayList<Pattern>() {
        {
            add(ThisPrefixRegex);
            add(LastPrefixRegex);
            add(NextPrefixRegex);
        }
    };

    public static final Iterable<Pattern> AmPmRegexList = new ArrayList<Pattern>() {
        {
            add(AmRegex);
            add(PmRegex);
        }
    };

    private final IDateExtractor dateExtractor;
    private final IDateTimeExtractor datePeriodExtractor;

    public ChineseDateTimeAltExtractorConfiguration(IOptionsConfiguration config) {
        super(config.getOptions());
        dateExtractor = new BaseDateExtractor(new ChineseDateExtractorConfiguration(this));
        datePeriodExtractor = new BaseDatePeriodExtractor(new ChineseDatePeriodExtractorConfiguration(this));
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
