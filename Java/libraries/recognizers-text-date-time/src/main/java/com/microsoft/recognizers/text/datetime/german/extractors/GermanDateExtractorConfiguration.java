// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.datetime.german.extractors;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.config.IOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.BaseDurationExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.config.IDateExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.german.utilities.GermanDatetimeUtilityConfiguration;
import com.microsoft.recognizers.text.datetime.resources.BaseDateTime;
import com.microsoft.recognizers.text.datetime.resources.GermanDateTime;
import com.microsoft.recognizers.text.datetime.utilities.IDateTimeUtilityConfiguration;
import com.microsoft.recognizers.text.number.english.extractors.IntegerExtractor;
import com.microsoft.recognizers.text.number.english.extractors.OrdinalExtractor;
import com.microsoft.recognizers.text.number.german.parsers.GermanNumberParserConfiguration;
import com.microsoft.recognizers.text.number.parsers.BaseNumberParser;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.ArrayList;
import java.util.List;
import java.util.regex.Pattern;

public class GermanDateExtractorConfiguration extends BaseOptionsConfiguration implements IDateExtractorConfiguration {

    public static final Pattern MonthRegex = RegExpUtility.getSafeRegExp(GermanDateTime.MonthRegex);
    public static final Pattern DayRegex = RegExpUtility.getSafeRegExp(GermanDateTime.ImplicitDayRegex);
    public static final Pattern MonthNumRegex = RegExpUtility.getSafeRegExp(GermanDateTime.MonthNumRegex);
    public static final Pattern YearRegex = RegExpUtility.getSafeRegExp(GermanDateTime.YearRegex);
    public static final Pattern WeekDayRegex = RegExpUtility.getSafeRegExp(GermanDateTime.WeekDayRegex);
    public static final Pattern SingleWeekDayRegex = RegExpUtility.getSafeRegExp(GermanDateTime.SingleWeekDayRegex);
    public static final Pattern OnRegex = RegExpUtility.getSafeRegExp(GermanDateTime.OnRegex);
    public static final Pattern RelaxedOnRegex = RegExpUtility.getSafeRegExp(GermanDateTime.RelaxedOnRegex);
    public static final Pattern ThisRegex = RegExpUtility.getSafeRegExp(GermanDateTime.ThisRegex);
    public static final Pattern LastDateRegex = RegExpUtility.getSafeRegExp(GermanDateTime.LastDateRegex);
    public static final Pattern NextDateRegex = RegExpUtility.getSafeRegExp(GermanDateTime.NextDateRegex);
    public static final Pattern DateUnitRegex = RegExpUtility.getSafeRegExp(GermanDateTime.DateUnitRegex);
    public static final Pattern SpecialDayRegex = RegExpUtility.getSafeRegExp(GermanDateTime.SpecialDayRegex);
    public static final Pattern WeekDayOfMonthRegex = RegExpUtility.getSafeRegExp(GermanDateTime.WeekDayOfMonthRegex);
    public static final Pattern RelativeWeekDayRegex = RegExpUtility.getSafeRegExp(GermanDateTime.RelativeWeekDayRegex);
    public static final Pattern SpecialDate = RegExpUtility.getSafeRegExp(GermanDateTime.SpecialDate);
    public static final Pattern SpecialDayWithNumRegex = RegExpUtility.getSafeRegExp(GermanDateTime.SpecialDayWithNumRegex);
    public static final Pattern ForTheRegex = RegExpUtility.getSafeRegExp(GermanDateTime.ForTheRegex);
    public static final Pattern WeekDayAndDayOfMonthRegex = RegExpUtility.getSafeRegExp(GermanDateTime.WeekDayAndDayOfMonthRegex);
    public static final Pattern RelativeMonthRegex = RegExpUtility.getSafeRegExp(GermanDateTime.RelativeMonthRegex);
    public static final Pattern StrictRelativeRegex = RegExpUtility.getSafeRegExp(GermanDateTime.StrictRelativeRegex);
    public static final Pattern PrefixArticleRegex = RegExpUtility.getSafeRegExp(GermanDateTime.PrefixArticleRegex);
    public static final Pattern InConnectorRegex = RegExpUtility.getSafeRegExp(GermanDateTime.InConnectorRegex);
    public static final Pattern RangeUnitRegex = RegExpUtility.getSafeRegExp(GermanDateTime.RangeUnitRegex);
    public static final Pattern RangeConnectorSymbolRegex = RegExpUtility.getSafeRegExp(BaseDateTime.RangeConnectorSymbolRegex);

    public static final List<Pattern> DateRegexList = new ArrayList<Pattern>() {
        {
            add(RegExpUtility.getSafeRegExp(GermanDateTime.DateExtractor1));
            add(RegExpUtility.getSafeRegExp(GermanDateTime.DateExtractor3));
            add(RegExpUtility.getSafeRegExp(GermanDateTime.DateExtractor4));
            add(RegExpUtility.getSafeRegExp(GermanDateTime.DateExtractor5));
            add(RegExpUtility.getSafeRegExp(GermanDateTime.DateExtractor6));
            add(RegExpUtility.getSafeRegExp(GermanDateTime.DateExtractor7L));
            add(RegExpUtility.getSafeRegExp(GermanDateTime.DateExtractor7S));
            add(RegExpUtility.getSafeRegExp(GermanDateTime.DateExtractor8));
            add(RegExpUtility.getSafeRegExp(GermanDateTime.DateExtractor9L));
            add(RegExpUtility.getSafeRegExp(GermanDateTime.DateExtractor9S));
            add(RegExpUtility.getSafeRegExp(GermanDateTime.DateExtractorA));
        }
    };

    public static final List<Pattern> ImplicitDateList = new ArrayList<Pattern>() {
        {
            add(OnRegex);
            add(RelaxedOnRegex);
            add(SpecialDayRegex);
            add(ThisRegex);
            add(LastDateRegex);
            add(NextDateRegex);
            add(SingleWeekDayRegex);
            add(WeekDayOfMonthRegex);
            add(SpecialDate);
            add(SpecialDayWithNumRegex);
            add(RelativeWeekDayRegex);
        }
    };

    public static final Pattern OfMonth = RegExpUtility.getSafeRegExp(GermanDateTime.OfMonth);
    public static final Pattern MonthEnd = RegExpUtility.getSafeRegExp(GermanDateTime.MonthEnd);
    public static final Pattern WeekDayEnd = RegExpUtility.getSafeRegExp(GermanDateTime.WeekDayEnd);
    public static final Pattern YearSuffix = RegExpUtility.getSafeRegExp(GermanDateTime.YearSuffix);
    public static final Pattern LessThanRegex = RegExpUtility.getSafeRegExp(GermanDateTime.LessThanRegex);
    public static final Pattern MoreThanRegex = RegExpUtility.getSafeRegExp(GermanDateTime.MoreThanRegex);

    public static final ImmutableMap<String, Integer> DayOfWeek = GermanDateTime.DayOfWeek;
    public static final ImmutableMap<String, Integer> MonthOfYear = GermanDateTime.MonthOfYear;

    private final IExtractor integerExtractor;
    private final IExtractor ordinalExtractor;
    private final IParser numberParser;
    private final IDateTimeExtractor durationExtractor;
    private final IDateTimeUtilityConfiguration utilityConfiguration;
    private final List<Pattern> implicitDateList;

    public GermanDateExtractorConfiguration(IOptionsConfiguration config) {
        super(config.getOptions());
        integerExtractor = IntegerExtractor.getInstance();
        ordinalExtractor = OrdinalExtractor.getInstance();
        numberParser = new BaseNumberParser(new GermanNumberParserConfiguration());
        durationExtractor = new BaseDurationExtractor(new GermanDurationExtractorConfiguration());
        utilityConfiguration = new GermanDatetimeUtilityConfiguration();

        implicitDateList = new ArrayList<>(ImplicitDateList);
        if (this.getOptions().match(DateTimeOptions.CalendarMode)) {
            implicitDateList.add(DayRegex);
        }
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
        return OfMonth;
    }

    @Override
    public Pattern getMonthEnd() {
        return MonthEnd;
    }

    @Override
    public Pattern getWeekDayEnd() {
        return WeekDayEnd;
    }

    @Override
    public Pattern getDateUnitRegex() {
        return DateUnitRegex;
    }

    @Override
    public Pattern getForTheRegex() {
        return ForTheRegex;
    }

    @Override
    public Pattern getWeekDayAndDayOfMonthRegex() {
        return WeekDayAndDayOfMonthRegex;
    }

    @Override
    public Pattern getRelativeMonthRegex() {
        return RelativeMonthRegex;
    }

    @Override
    public Pattern getStrictRelativeRegex() {
        return StrictRelativeRegex;
    }

    @Override
    public Pattern getWeekDayRegex() {
        return WeekDayRegex;
    }

    @Override
    public Pattern getPrefixArticleRegex() {
        return PrefixArticleRegex;
    }

    @Override
    public Pattern getYearSuffix() {
        return YearSuffix;
    }

    @Override
    public Pattern getMoreThanRegex() {
        return MoreThanRegex;
    }

    @Override
    public Pattern getLessThanRegex() {
        return LessThanRegex;
    }

    @Override
    public Pattern getInConnectorRegex() {
        return InConnectorRegex;
    }

    @Override
    public Pattern getRangeUnitRegex() {
        return RangeUnitRegex;
    }

    @Override
    public Pattern getRangeConnectorSymbolRegex() {
        return RangeConnectorSymbolRegex;
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
