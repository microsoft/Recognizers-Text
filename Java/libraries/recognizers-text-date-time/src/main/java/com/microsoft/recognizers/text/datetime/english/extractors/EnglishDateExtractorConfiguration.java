package com.microsoft.recognizers.text.datetime.english.extractors;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.english.parsers.EnglishDatetimeUtilityConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.BaseDurationExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.config.IDateExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.resources.EnglishDateTime;
import com.microsoft.recognizers.text.datetime.utilities.IDateTimeUtilityConfiguration;
import com.microsoft.recognizers.text.number.english.extractors.IntegerExtractor;
import com.microsoft.recognizers.text.number.english.extractors.OrdinalExtractor;
import com.microsoft.recognizers.text.number.english.parsers.EnglishNumberParserConfiguration;
import com.microsoft.recognizers.text.number.parsers.BaseNumberParser;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.ArrayList;
import java.util.List;
import java.util.regex.Pattern;

public class EnglishDateExtractorConfiguration implements IDateExtractorConfiguration {

    public static final Pattern MonthRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.MonthRegex, Pattern.CASE_INSENSITIVE);
    public static final Pattern DayRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.DayRegex, Pattern.CASE_INSENSITIVE);
    public static final Pattern MonthNumRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.MonthNumRegex, Pattern.CASE_INSENSITIVE);
    public static final Pattern YearRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.YearRegex, Pattern.CASE_INSENSITIVE);
    public static final Pattern WeekDayRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.WeekDayRegex, Pattern.CASE_INSENSITIVE);
    public static final Pattern SingleWeekDayRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SingleWeekDayRegex, Pattern.CASE_INSENSITIVE);
    public static final Pattern OnRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.OnRegex, Pattern.CASE_INSENSITIVE);
    public static final Pattern RelaxedOnRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.RelaxedOnRegex);
    public static final Pattern ThisRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.ThisRegex, Pattern.CASE_INSENSITIVE);
    public static final Pattern LastDateRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.LastDateRegex, Pattern.CASE_INSENSITIVE);
    public static final Pattern NextDateRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.NextDateRegex, Pattern.CASE_INSENSITIVE);
    public static final Pattern DateUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.DateUnitRegex, Pattern.CASE_INSENSITIVE);
    public static final Pattern SpecialDayRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SpecialDayRegex, Pattern.CASE_INSENSITIVE);
    public static final Pattern WeekDayOfMonthRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.WeekDayOfMonthRegex, Pattern.CASE_INSENSITIVE);
    public static final Pattern RelativeWeekDayRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.RelativeWeekDayRegex, Pattern.CASE_INSENSITIVE);
    public static final Pattern SpecialDate = RegExpUtility.getSafeRegExp(EnglishDateTime.SpecialDate, Pattern.CASE_INSENSITIVE);
    public static final Pattern SpecialDayWithNumRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SpecialDayWithNumRegex, Pattern.CASE_INSENSITIVE);
    public static final Pattern ForTheRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.ForTheRegex, Pattern.CASE_INSENSITIVE);
    public static final Pattern WeekDayAndDayOfMonthRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.WeekDayAndDayOfMonthRegex, Pattern.CASE_INSENSITIVE);
    public static final Pattern RelativeMonthRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.RelativeMonthRegex, Pattern.CASE_INSENSITIVE);
    public static final Pattern PrefixArticleRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PrefixArticleRegex, Pattern.CASE_INSENSITIVE);
    public static final Pattern InConnectorRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.InConnectorRegex, Pattern.CASE_INSENSITIVE);
    public static final Pattern RangeUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.RangeUnitRegex, Pattern.CASE_INSENSITIVE);

    public static final List<Pattern> DateRegexList = new ArrayList<Pattern>() {
        {
            add(RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor1, Pattern.CASE_INSENSITIVE));
            add(RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor2, Pattern.CASE_INSENSITIVE));
            add(RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor3, Pattern.CASE_INSENSITIVE));
            add(RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor4, Pattern.CASE_INSENSITIVE));
            add(RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor5, Pattern.CASE_INSENSITIVE));
            add(RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor6, Pattern.CASE_INSENSITIVE));
            add(RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor7, Pattern.CASE_INSENSITIVE));
            add(RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor8, Pattern.CASE_INSENSITIVE));
            add(RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor9, Pattern.CASE_INSENSITIVE));
            add(RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractorA, Pattern.CASE_INSENSITIVE));
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

    public static final Pattern OfMonth = RegExpUtility.getSafeRegExp(EnglishDateTime.OfMonth, Pattern.CASE_INSENSITIVE);
    public static final Pattern MonthEnd = RegExpUtility.getSafeRegExp(EnglishDateTime.MonthEnd, Pattern.CASE_INSENSITIVE);
    public static final Pattern WeekDayEnd = RegExpUtility.getSafeRegExp(EnglishDateTime.WeekDayEnd, Pattern.CASE_INSENSITIVE);
    public static final Pattern YearSuffix = RegExpUtility.getSafeRegExp(EnglishDateTime.YearSuffix, Pattern.CASE_INSENSITIVE);
    public static final Pattern LessThanRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.LessThanRegex, Pattern.CASE_INSENSITIVE);
    public static final Pattern MoreThanRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.MoreThanRegex, Pattern.CASE_INSENSITIVE);

    public static final ImmutableMap<String, Integer> DayOfWeek = EnglishDateTime.DayOfWeek;
    public static final ImmutableMap<String, Integer> MonthOfYear = EnglishDateTime.MonthOfYear;

    private final IExtractor integerExtractor;
    private final IExtractor ordinalExtractor;
    private final IParser numberParser;
    private final IDateTimeExtractor durationExtractor;
    private final IDateTimeUtilityConfiguration utilityConfiguration;

    public EnglishDateExtractorConfiguration() {
        integerExtractor = IntegerExtractor.getInstance();
        ordinalExtractor = OrdinalExtractor.getInstance();
        numberParser = new BaseNumberParser(new EnglishNumberParserConfiguration());
        durationExtractor = new BaseDurationExtractor(new EnglishDurationExtractorConfiguration());
        utilityConfiguration = new EnglishDatetimeUtilityConfiguration();
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

    @Override
    public DateTimeOptions getOptions() {
        return DateTimeOptions.None;
    }
}
