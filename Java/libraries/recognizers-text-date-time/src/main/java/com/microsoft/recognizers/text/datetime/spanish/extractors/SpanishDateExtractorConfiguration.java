package com.microsoft.recognizers.text.datetime.spanish.extractors;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.config.IOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.BaseDurationExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.config.IDateExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.resources.BaseDateTime;
import com.microsoft.recognizers.text.datetime.resources.SpanishDateTime;
import com.microsoft.recognizers.text.datetime.spanish.utilities.SpanishDatetimeUtilityConfiguration;
import com.microsoft.recognizers.text.datetime.utilities.IDateTimeUtilityConfiguration;
import com.microsoft.recognizers.text.number.parsers.BaseNumberParser;
import com.microsoft.recognizers.text.number.spanish.extractors.IntegerExtractor;
import com.microsoft.recognizers.text.number.spanish.extractors.OrdinalExtractor;
import com.microsoft.recognizers.text.number.spanish.parsers.SpanishNumberParserConfiguration;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.ArrayList;
import java.util.List;
import java.util.regex.Pattern;


public class SpanishDateExtractorConfiguration extends BaseOptionsConfiguration implements IDateExtractorConfiguration {

    public static final Pattern MonthRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.MonthRegex);
    public static final Pattern DayRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.DayRegex);
    public static final Pattern MonthNumRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.MonthNumRegex);
    public static final Pattern YearRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.YearRegex);
    public static final Pattern WeekDayRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.WeekDayRegex);
    public static final Pattern OnRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.OnRegex);
    public static final Pattern RelaxedOnRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.RelaxedOnRegex);
    public static final Pattern ThisRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.ThisRegex);
    public static final Pattern LastDateRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.LastDateRegex);
    public static final Pattern NextDateRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.NextDateRegex);
    public static final Pattern SpecialDayRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.SpecialDayRegex);
    public static final Pattern SpecialDayWithNumRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.SpecialDayWithNumRegex);
    public static final Pattern DateUnitRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.DateUnitRegex);
    public static final Pattern WeekDayOfMonthRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.WeekDayOfMonthRegex);
    public static final Pattern SpecialDateRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.SpecialDateRegex);
    public static final Pattern RelativeWeekDayRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.RelativeWeekDayRegex);
    public static final Pattern ForTheRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.ForTheRegex);
    public static final Pattern WeekDayAndDayOfMonthRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.WeekDayAndDayOfMonthRegex);
    public static final Pattern RelativeMonthRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.RelativeMonthRegex);
    public static final Pattern StrictRelativeRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.StrictRelativeRegex);
    public static final Pattern PrefixArticleRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.PrefixArticleRegex);
    public static final Pattern RangeConnectorSymbolRegex = RegExpUtility.getSafeRegExp(BaseDateTime.RangeConnectorSymbolRegex);
    public static final List<Pattern> ImplicitDateList = new ArrayList<Pattern>() {
        {
            add(OnRegex);
            add(RelaxedOnRegex);
            add(SpecialDayRegex);
            add(ThisRegex);
            add(LastDateRegex);
            add(NextDateRegex);
            add(WeekDayRegex);
            add(WeekDayOfMonthRegex);
            add(SpecialDateRegex);
        }
    };

    public static final Pattern OfMonth = RegExpUtility.getSafeRegExp(SpanishDateTime.OfMonthRegex);
    public static final Pattern MonthEnd = RegExpUtility.getSafeRegExp(SpanishDateTime.MonthEndRegex);
    public static final Pattern WeekDayEnd = RegExpUtility.getSafeRegExp(SpanishDateTime.WeekDayEnd);
    public static final Pattern YearSuffix = RegExpUtility.getSafeRegExp(SpanishDateTime.YearSuffix);
    public static final Pattern LessThanRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.LessThanRegex);
    public static final Pattern MoreThanRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.MoreThanRegex);
    public static final Pattern InConnectorRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.InConnectorRegex);
    public static final Pattern RangeUnitRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.RangeUnitRegex);
    public static final ImmutableMap<String, Integer> DayOfWeek = SpanishDateTime.DayOfWeek;
    public static final ImmutableMap<String, Integer> MonthOfYear = SpanishDateTime.MonthOfYear;

    public static List<Pattern> DateRegexList;

    public SpanishDateExtractorConfiguration(IOptionsConfiguration config) {
        super(config.getOptions());
        integerExtractor = new IntegerExtractor(); // in other languages (english) has a method named get instance
        ordinalExtractor = new OrdinalExtractor(); // in other languages (english) has a method named get instance
        numberParser = new BaseNumberParser(new SpanishNumberParserConfiguration());
        durationExtractor = new BaseDurationExtractor(new SpanishDurationExtractorConfiguration());
        utilityConfiguration = new SpanishDatetimeUtilityConfiguration();

        DateRegexList = new ArrayList<Pattern>() {
            {
                add(RegExpUtility.getSafeRegExp(SpanishDateTime.DateExtractor1));
                add(RegExpUtility.getSafeRegExp(SpanishDateTime.DateExtractor2));
                add(RegExpUtility.getSafeRegExp(SpanishDateTime.DateExtractor3));
            }
        };

        boolean enableDmy = getDmyDateFormat() || SpanishDateTime.DefaultLanguageFallback == Constants.DefaultLanguageFallback_DMY;

        if (enableDmy) {
            DateRegexList.add(RegExpUtility.getSafeRegExp(SpanishDateTime.DateExtractor5));
            DateRegexList.add(RegExpUtility.getSafeRegExp(SpanishDateTime.DateExtractor8));
            DateRegexList.add(RegExpUtility.getSafeRegExp(SpanishDateTime.DateExtractor9));
            DateRegexList.add(RegExpUtility.getSafeRegExp(SpanishDateTime.DateExtractor4));
            DateRegexList.add(RegExpUtility.getSafeRegExp(SpanishDateTime.DateExtractor6));
            DateRegexList.add(RegExpUtility.getSafeRegExp(SpanishDateTime.DateExtractor7));
            DateRegexList.add(RegExpUtility.getSafeRegExp(SpanishDateTime.DateExtractor10));
        } else {
            DateRegexList.add(RegExpUtility.getSafeRegExp(SpanishDateTime.DateExtractor4));
            DateRegexList.add(RegExpUtility.getSafeRegExp(SpanishDateTime.DateExtractor6));
            DateRegexList.add(RegExpUtility.getSafeRegExp(SpanishDateTime.DateExtractor7));
            DateRegexList.add(RegExpUtility.getSafeRegExp(SpanishDateTime.DateExtractor5));
            DateRegexList.add(RegExpUtility.getSafeRegExp(SpanishDateTime.DateExtractor8));
            DateRegexList.add(RegExpUtility.getSafeRegExp(SpanishDateTime.DateExtractor9));
            DateRegexList.add(RegExpUtility.getSafeRegExp(SpanishDateTime.DateExtractor10));
        }
    }

    private final IExtractor integerExtractor;
    private final IExtractor ordinalExtractor;
    private final IParser numberParser;
    private final IDateTimeExtractor durationExtractor;
    private final IDateTimeUtilityConfiguration utilityConfiguration;

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
    public Pattern getLessThanRegex() {
        return LessThanRegex;
    }

    @Override
    public Pattern getMoreThanRegex() {
        return MoreThanRegex;
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
    public Iterable<Pattern> getDateRegexList() {
        return DateRegexList;
    }

    @Override
    public Iterable<Pattern> getImplicitDateList() {
        return ImplicitDateList;
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
