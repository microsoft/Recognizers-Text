package com.microsoft.recognizers.text.datetime.english.extractors;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.config.IOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.english.utilities.EnglishDatetimeUtilityConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.BaseDurationExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.config.IDateExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.resources.BaseDateTime;
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

public class EnglishDateExtractorConfiguration extends BaseOptionsConfiguration implements IDateExtractorConfiguration {

    public static final Pattern MonthRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.MonthRegex);
    public static final Pattern DayRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.ImplicitDayRegex);
    public static final Pattern MonthNumRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.MonthNumRegex);
    public static final Pattern YearRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.YearRegex);
    public static final Pattern WeekDayRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.WeekDayRegex);
    public static final Pattern SingleWeekDayRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SingleWeekDayRegex);
    public static final Pattern OnRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.OnRegex);
    public static final Pattern RelaxedOnRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.RelaxedOnRegex);
    public static final Pattern ThisRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.ThisRegex);
    public static final Pattern LastDateRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.LastDateRegex);
    public static final Pattern NextDateRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.NextDateRegex);
    public static final Pattern DateUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.DateUnitRegex);
    public static final Pattern SpecialDayRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SpecialDayRegex);
    public static final Pattern WeekDayOfMonthRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.WeekDayOfMonthRegex);
    public static final Pattern RelativeWeekDayRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.RelativeWeekDayRegex);
    public static final Pattern SpecialDate = RegExpUtility.getSafeRegExp(EnglishDateTime.SpecialDate);
    public static final Pattern SpecialDayWithNumRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SpecialDayWithNumRegex);
    public static final Pattern ForTheRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.ForTheRegex);
    public static final Pattern WeekDayAndDayOfMonthRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.WeekDayAndDayOfMonthRegex);
    public static final Pattern RelativeMonthRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.RelativeMonthRegex);
    public static final Pattern StrictRelativeRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.StrictRelativeRegex);
    public static final Pattern PrefixArticleRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PrefixArticleRegex);
    public static final Pattern InConnectorRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.InConnectorRegex);
    public static final Pattern RangeUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.RangeUnitRegex);
    public static final Pattern RangeConnectorSymbolRegex = RegExpUtility.getSafeRegExp(BaseDateTime.RangeConnectorSymbolRegex);

    public static final List<Pattern> DateRegexList = new ArrayList<Pattern>() {
        {
            add(RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor1));
            add(RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor3));
            add(RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor4));
            add(RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor5));
            add(RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor6));
            add(RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor7L));
            add(RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor7S));
            add(RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor8));
            add(RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor9L));
            add(RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor9S));
            add(RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractorA));
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

    public static final Pattern OfMonth = RegExpUtility.getSafeRegExp(EnglishDateTime.OfMonth);
    public static final Pattern MonthEnd = RegExpUtility.getSafeRegExp(EnglishDateTime.MonthEnd);
    public static final Pattern WeekDayEnd = RegExpUtility.getSafeRegExp(EnglishDateTime.WeekDayEnd);
    public static final Pattern YearSuffix = RegExpUtility.getSafeRegExp(EnglishDateTime.YearSuffix);
    public static final Pattern LessThanRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.LessThanRegex);
    public static final Pattern MoreThanRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.MoreThanRegex);

    public static final ImmutableMap<String, Integer> DayOfWeek = EnglishDateTime.DayOfWeek;
    public static final ImmutableMap<String, Integer> MonthOfYear = EnglishDateTime.MonthOfYear;

    private final IExtractor integerExtractor;
    private final IExtractor ordinalExtractor;
    private final IParser numberParser;
    private final IDateTimeExtractor durationExtractor;
    private final IDateTimeUtilityConfiguration utilityConfiguration;
    private final List<Pattern> implicitDateList;

    public EnglishDateExtractorConfiguration(IOptionsConfiguration config) {
        super(config.getOptions());
        integerExtractor = IntegerExtractor.getInstance();
        ordinalExtractor = OrdinalExtractor.getInstance();
        numberParser = new BaseNumberParser(new EnglishNumberParserConfiguration());
        durationExtractor = new BaseDurationExtractor(new EnglishDurationExtractorConfiguration());
        utilityConfiguration = new EnglishDatetimeUtilityConfiguration();

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
