package com.microsoft.recognizers.text.datetime.french.extractors;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.config.IOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.BaseDurationExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.config.IDateExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.french.utilities.FrenchDatetimeUtilityConfiguration;
import com.microsoft.recognizers.text.datetime.resources.BaseDateTime;
import com.microsoft.recognizers.text.datetime.resources.FrenchDateTime;
import com.microsoft.recognizers.text.datetime.utilities.IDateTimeUtilityConfiguration;
import com.microsoft.recognizers.text.number.french.extractors.IntegerExtractor;
import com.microsoft.recognizers.text.number.french.extractors.OrdinalExtractor;
import com.microsoft.recognizers.text.number.french.parsers.FrenchNumberParserConfiguration;
import com.microsoft.recognizers.text.number.parsers.BaseNumberParser;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import java.util.ArrayList;
import java.util.List;
import java.util.regex.Pattern;

public class FrenchDateExtractorConfiguration extends BaseOptionsConfiguration implements IDateExtractorConfiguration {

    public static final Pattern MonthRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.MonthRegex);
    public static final Pattern MonthNumRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.MonthNumRegex);
    public static final Pattern YearRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.YearRegex);
    public static final Pattern WeekDayRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.WeekDayRegex);
    public static final Pattern SingleWeekDayRegex = RegExpUtility
        .getSafeRegExp(FrenchDateTime.SingleAmbiguousMonthRegex);
    public static final Pattern OnRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.OnRegex);
    public static final Pattern RelaxedOnRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.RelaxedOnRegex);
    public static final Pattern ThisRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.ThisRegex);
    public static final Pattern LastDateRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.LastDateRegex);
    public static final Pattern NextDateRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.NextDateRegex);
    public static final Pattern StrictWeekDay = RegExpUtility.getSafeRegExp(FrenchDateTime.StrictWeekDay);
    public static final Pattern DateUnitRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.DateUnitRegex);
    public static final Pattern SpecialDayRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.SpecialDayRegex);
    public static final Pattern WeekDayOfMonthRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.WeekDayOfMonthRegex);
    public static final Pattern RelativeWeekDayRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.RelativeWeekDayRegex);
    public static final Pattern SpecialDate = RegExpUtility.getSafeRegExp(FrenchDateTime.SpecialDate);
    public static final Pattern SpecialDayWithNumRegex = RegExpUtility
        .getSafeRegExp(FrenchDateTime.SpecialDayWithNumRegex);
    public static final Pattern ForTheRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.ForTheRegex);
    public static final Pattern WeekDayAndDayOfMonthRegex = RegExpUtility
        .getSafeRegExp(FrenchDateTime.WeekDayAndDayOfMonthRegex);
    public static final Pattern RelativeMonthRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.RelativeMonthRegex);
    public static final Pattern StrictRelativeRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.StrictRelativeRegex);
    public static final Pattern PrefixArticleRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.PrefixArticleRegex);
    public static final Pattern InConnectorRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.InConnectorRegex);
    public static final Pattern RangeUnitRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.RangeUnitRegex);
    public static final Pattern RangeConnectorSymbolRegex = RegExpUtility
        .getSafeRegExp(BaseDateTime.RangeConnectorSymbolRegex);

    public static final List<Pattern> DateRegexList = new ArrayList<Pattern>() {
        {
            add(RegExpUtility.getSafeRegExp(FrenchDateTime.DateExtractor1));
            add(RegExpUtility.getSafeRegExp(FrenchDateTime.DateExtractor2));
            add(RegExpUtility.getSafeRegExp(FrenchDateTime.DateExtractor3));
            add(FrenchDateTime.DefaultLanguageFallback == "DMY" ?
                    RegExpUtility.getSafeRegExp(FrenchDateTime.DateExtractor5) :
                    RegExpUtility.getSafeRegExp(FrenchDateTime.DateExtractor4));
            add(FrenchDateTime.DefaultLanguageFallback == "DMY" ?
                    RegExpUtility.getSafeRegExp(FrenchDateTime.DateExtractor4) :
                    RegExpUtility.getSafeRegExp(FrenchDateTime.DateExtractor5));
            add(FrenchDateTime.DefaultLanguageFallback == "DMY" ?
                    RegExpUtility.getSafeRegExp(FrenchDateTime.DateExtractor7) :
                    RegExpUtility.getSafeRegExp(FrenchDateTime.DateExtractor6));
            add(FrenchDateTime.DefaultLanguageFallback == "DMY" ?
                    RegExpUtility.getSafeRegExp(FrenchDateTime.DateExtractor6) :
                    RegExpUtility.getSafeRegExp(FrenchDateTime.DateExtractor7));
            add(RegExpUtility.getSafeRegExp(FrenchDateTime.DateExtractor8));
            add(RegExpUtility.getSafeRegExp(FrenchDateTime.DateExtractor9));
            add(RegExpUtility.getSafeRegExp(FrenchDateTime.DateExtractorA));
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
            add(StrictWeekDay);
            add(WeekDayOfMonthRegex);
            add(SpecialDate);
        }
    };

    public static final Pattern OfMonth = RegExpUtility.getSafeRegExp(FrenchDateTime.OfMonth);
    public static final Pattern MonthEnd = RegExpUtility.getSafeRegExp(FrenchDateTime.MonthEnd);
    public static final Pattern WeekDayEnd = RegExpUtility.getSafeRegExp(FrenchDateTime.WeekDayEnd);
    public static final Pattern YearSuffix = RegExpUtility.getSafeRegExp(FrenchDateTime.YearSuffix);
    public static final Pattern LessThanRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.LessThanRegex);
    public static final Pattern MoreThanRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.MoreThanRegex);

    public static final ImmutableMap<String, Integer> DayOfWeek = FrenchDateTime.DayOfWeek;
    public static final ImmutableMap<String, Integer> MonthOfYear = FrenchDateTime.MonthOfYear;

    private final IExtractor integerExtractor;
    private final IExtractor ordinalExtractor;
    private final IParser numberParser;
    private final IDateTimeExtractor durationExtractor;
    private final IDateTimeUtilityConfiguration utilityConfiguration;
    private final List<Pattern> implicitDateList;

    public FrenchDateExtractorConfiguration(final IOptionsConfiguration config) {
        super(config.getOptions());
        integerExtractor = new IntegerExtractor();
        ordinalExtractor = new OrdinalExtractor();
        numberParser = new BaseNumberParser(new FrenchNumberParserConfiguration());
        durationExtractor = new BaseDurationExtractor(new FrenchDurationExtractorConfiguration());
        utilityConfiguration = new FrenchDatetimeUtilityConfiguration();

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
