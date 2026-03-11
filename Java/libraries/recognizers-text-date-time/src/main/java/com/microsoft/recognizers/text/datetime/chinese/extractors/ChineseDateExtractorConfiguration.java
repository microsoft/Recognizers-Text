package com.microsoft.recognizers.text.datetime.chinese.extractors;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.config.IOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.config.IDateExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.resources.ChineseDateTime;
import com.microsoft.recognizers.text.datetime.utilities.IDateTimeUtilityConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.number.chinese.extractors.IntegerExtractor;
import com.microsoft.recognizers.text.number.chinese.extractors.OrdinalExtractor;
import com.microsoft.recognizers.text.number.chinese.parsers.ChineseNumberParserConfiguration;
import com.microsoft.recognizers.text.number.parsers.BaseNumberParser;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.ArrayList;
import java.util.List;
import java.util.regex.Pattern;

public class ChineseDateExtractorConfiguration extends BaseOptionsConfiguration implements IDateExtractorConfiguration {

    public static final Pattern MonthRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.MonthRegex);
    public static final Pattern DayRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DayRegex);
    public static final Pattern MonthNumRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.MonthNumRegex);
    public static final Pattern YearRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.YearRegex);
    public static final Pattern WeekDayRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.WeekDayRegex);
    public static final Pattern DateUnitRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DateUnitRegex);
    public static final Pattern SpecialDayRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.SpecialDayRegex);
    public static final Pattern DateThisRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DateThisRegex);
    public static final Pattern DateLastRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DateLastRegex);
    public static final Pattern DateNextRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DateNextRegex);
    public static final Pattern WeekDayOfMonthRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.WeekDayOfMonthRegex);
    public static final Pattern SpecialDate = RegExpUtility.getSafeRegExp(ChineseDateTime.SpecialDate);
    public static final Pattern WeekDayStartEnd = RegExpUtility.getSafeRegExp(ChineseDateTime.WeekDayStartEnd);
    public static final Pattern BeforeRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.BeforeRegex);
    public static final Pattern AfterRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.AfterRegex);

    public static final List<Pattern> DateRegexList = new ArrayList<Pattern>() {
        {
            add(RegExpUtility.getSafeRegExp(ChineseDateTime.DateRegexList1));
            add(RegExpUtility.getSafeRegExp(ChineseDateTime.DateRegexList2));
            add(RegExpUtility.getSafeRegExp(ChineseDateTime.DateRegexList3));
            add(RegExpUtility.getSafeRegExp(ChineseDateTime.DateRegexList4));
            add(RegExpUtility.getSafeRegExp(ChineseDateTime.DateRegexList5));
            add(RegExpUtility.getSafeRegExp(ChineseDateTime.DateRegexList6));
            add(RegExpUtility.getSafeRegExp(ChineseDateTime.DateRegexList7));
            add(RegExpUtility.getSafeRegExp(ChineseDateTime.DateRegexList8));
        }
    };

    public static final List<Pattern> ImplicitDateList = new ArrayList<Pattern>() {
        {
            add(SpecialDayRegex);
            add(WeekDayRegex);
            add(DateThisRegex);
            add(DateLastRegex);
            add(DateNextRegex);
            add(WeekDayOfMonthRegex);
            add(SpecialDate);
        }
    };

    public static final ImmutableMap<String, Integer> DayOfWeek = ChineseDateTime.ParserConfigurationDayOfWeek;
    public static final ImmutableMap<String, Integer> MonthOfYear = ChineseDateTime.ParserConfigurationMonthOfYear;

    private final IExtractor integerExtractor;
    private final IExtractor ordinalExtractor;
    private final IParser numberParser;
    private final IDateTimeExtractor durationExtractor;
    private final IDateTimeUtilityConfiguration utilityConfiguration;
    private final List<Pattern> implicitDateList;

    public ChineseDateExtractorConfiguration(IOptionsConfiguration config) {
        super(config.getOptions());
        integerExtractor = new IntegerExtractor();
        ordinalExtractor = new OrdinalExtractor();
        numberParser = new BaseNumberParser(new ChineseNumberParserConfiguration());
        durationExtractor = null;
        utilityConfiguration = null;

        implicitDateList = new ArrayList<>(ImplicitDateList);
    }

    @Override
    public Iterable<Pattern> getDateRegexList() {
        return DateRegexList;
    }

    @Override
    public Iterable<Pattern> getImplicitDateList() {
        return implicitDateList;
    }

    @Override
    public Pattern getOfMonth() {
        return null;
    }

    @Override
    public Pattern getMonthEnd() {
        return null;
    }

    @Override
    public Pattern getWeekDayEnd() {
        return WeekDayStartEnd;
    }

    @Override
    public Pattern getDateUnitRegex() {
        return DateUnitRegex;
    }

    @Override
    public Pattern getForTheRegex() {
        return null;
    }

    @Override
    public Pattern getWeekDayAndDayOfMonthRegex() {
        return null;
    }

    @Override
    public Pattern getRelativeMonthRegex() {
        return null;
    }

    @Override
    public Pattern getStrictRelativeRegex() {
        return null;
    }

    @Override
    public Pattern getWeekDayRegex() {
        return WeekDayRegex;
    }

    @Override
    public Pattern getPrefixArticleRegex() {
        return null;
    }

    @Override
    public Pattern getYearSuffix() {
        return null;
    }

    @Override
    public Pattern getMoreThanRegex() {
        return null;
    }

    @Override
    public Pattern getLessThanRegex() {
        return null;
    }

    @Override
    public Pattern getInConnectorRegex() {
        return null;
    }

    @Override
    public Pattern getRangeUnitRegex() {
        return null;
    }

    @Override
    public Pattern getRangeConnectorSymbolRegex() {
        return null;
    }

    @Override
    public IExtractor getIntegerExtractor() {
        return integerExtractor;
    }

    @Override
    public IExtractor getOrdinalExtractor() {
        return ordinalExtractor;
    }

    @Override
    public IParser getNumberParser() {
        return numberParser;
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
    public ImmutableMap<String, Integer> getDayOfWeek() {
        return DayOfWeek;
    }

    @Override
    public ImmutableMap<String, Integer> getMonthOfYear() {
        return MonthOfYear;
    }
}
