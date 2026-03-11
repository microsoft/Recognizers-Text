package com.microsoft.recognizers.text.datetime.chinese.extractors;

import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.config.IOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.BaseDateExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.config.IDatePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.config.ResultIndex;
import com.microsoft.recognizers.text.datetime.resources.ChineseDateTime;
import com.microsoft.recognizers.text.datetime.utilities.RegexExtension;
import com.microsoft.recognizers.text.number.chinese.extractors.CardinalExtractor;
import com.microsoft.recognizers.text.number.chinese.extractors.OrdinalExtractor;
import com.microsoft.recognizers.text.number.chinese.parsers.ChineseNumberParserConfiguration;
import com.microsoft.recognizers.text.number.parsers.BaseNumberParser;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.ArrayList;
import java.util.regex.Pattern;

public class ChineseDatePeriodExtractorConfiguration extends BaseOptionsConfiguration implements IDatePeriodExtractorConfiguration {

    public static final Pattern YearRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.YearRegex);
    public static final Pattern TillRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DatePeriodTillRegex);
    public static final Pattern DateUnitRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DateUnitRegex);
    public static final Pattern FollowedUnit = RegExpUtility.getSafeRegExp(ChineseDateTime.FollowedUnit);
    public static final Pattern NumberCombinedWithUnit = RegExpUtility.getSafeRegExp(ChineseDateTime.NumberCombinedWithUnit);
    public static final Pattern PastRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.PastRegex);
    public static final Pattern FutureRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.FutureRegex);
    public static final Pattern UnitRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.UnitRegex);
    public static final Pattern NowRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.NowRegex);

    public static final Pattern SimpleCasesRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.SimpleCasesRegex);
    public static final Pattern YearAndMonth = RegExpUtility.getSafeRegExp(ChineseDateTime.YearAndMonth);
    public static final Pattern SimpleYearAndMonth = RegExpUtility.getSafeRegExp(ChineseDateTime.SimpleYearAndMonth);
    public static final Pattern PureNumYearAndMonth = RegExpUtility.getSafeRegExp(ChineseDateTime.PureNumYearAndMonth);
    public static final Pattern OneWordPeriodRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.OneWordPeriodRegex);
    public static final Pattern WeekOfMonthRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.WeekOfMonthRegex);
    public static final Pattern WeekOfYearRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.WeekOfYearRegex);
    public static final Pattern QuarterRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.QuarterRegex);
    public static final Pattern SeasonWithYear = RegExpUtility.getSafeRegExp(ChineseDateTime.SeasonWithYear);
    public static final Pattern DecadeRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DecadeRegex);
    public static final Pattern YearToYear = RegExpUtility.getSafeRegExp(ChineseDateTime.YearToYear);
    public static final Pattern YearToYearSuffixRequired = RegExpUtility.getSafeRegExp(ChineseDateTime.YearToYearSuffixRequired);
    public static final Pattern MonthToMonth = RegExpUtility.getSafeRegExp(ChineseDateTime.MonthToMonth);
    public static final Pattern MonthToMonthSuffixRequired = RegExpUtility.getSafeRegExp(ChineseDateTime.MonthToMonthSuffixRequired);
    public static final Pattern DatePeriodYearInCJKRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DatePeriodYearInCJKRegex);
    public static final Pattern FirstLastOfYearRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.FirstLastOfYearRegex);

    public static final Iterable<Pattern> SimpleCasesRegexes = new ArrayList<Pattern>() {
        {
            add(SimpleCasesRegex);
            add(YearAndMonth);
            add(SimpleYearAndMonth);
            add(PureNumYearAndMonth);
            add(OneWordPeriodRegex);
            add(WeekOfMonthRegex);
            add(WeekOfYearRegex);
            add(QuarterRegex);
            add(SeasonWithYear);
            add(DecadeRegex);
            add(YearToYear);
            add(YearToYearSuffixRequired);
            add(MonthToMonth);
            add(MonthToMonthSuffixRequired);
            add(DatePeriodYearInCJKRegex);
            add(YearRegex);
            add(FirstLastOfYearRegex);
        }
    };

    public static final Pattern RangeConnectorRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DatePeriodTillRegex);
    private final String[] durationDateRestrictions = new String[0];

    private final IDateTimeExtractor datePointExtractor;
    private final IExtractor cardinalExtractor;
    private final IExtractor ordinalExtractor;
    private final IDateTimeExtractor durationExtractor;
    private final IParser numberParser;

    public ChineseDatePeriodExtractorConfiguration(IOptionsConfiguration config) {
        super(config.getOptions());

        datePointExtractor = new BaseDateExtractor(new ChineseDateExtractorConfiguration(this));
        cardinalExtractor = new CardinalExtractor();
        ordinalExtractor = new OrdinalExtractor();
        durationExtractor = null;
        numberParser = new BaseNumberParser(new ChineseNumberParserConfiguration());
    }

    @Override
    public Iterable<Pattern> getSimpleCasesRegexes() {
        return SimpleCasesRegexes;
    }

    @Override
    public Pattern getIllegalYearRegex() {
        return null;
    }

    @Override
    public Pattern getYearRegex() {
        return YearRegex;
    }

    @Override
    public Pattern getTillRegex() {
        return TillRegex;
    }

    @Override
    public Pattern getDateUnitRegex() {
        return DateUnitRegex;
    }

    @Override
    public Pattern getTimeUnitRegex() {
        return null;
    }

    @Override
    public Pattern getFollowedDateUnit() {
        return FollowedUnit;
    }

    @Override
    public Pattern getNumberCombinedWithDateUnit() {
        return NumberCombinedWithUnit;
    }

    @Override
    public Pattern getPastRegex() {
        return PastRegex;
    }

    @Override
    public Pattern getFutureRegex() {
        return FutureRegex;
    }

    @Override
    public Pattern getFutureSuffixRegex() {
        return null;
    }

    @Override
    public Pattern getWeekOfRegex() {
        return null;
    }

    @Override
    public Pattern getMonthOfRegex() {
        return null;
    }

    @Override
    public Pattern getRangeUnitRegex() {
        return UnitRegex;
    }

    @Override
    public Pattern getInConnectorRegex() {
        return null;
    }

    @Override
    public Pattern getWithinNextPrefixRegex() {
        return null;
    }

    @Override
    public Pattern getYearPeriodRegex() {
        return null;
    }

    @Override
    public Pattern getRelativeDecadeRegex() {
        return null;
    }

    @Override
    public Pattern getComplexDatePeriodRegex() {
        return null;
    }

    @Override
    public Pattern getReferenceDatePeriodRegex() {
        return null;
    }

    @Override
    public Pattern getAgoRegex() {
        return null;
    }

    @Override
    public Pattern getLaterRegex() {
        return null;
    }

    @Override
    public Pattern getLessThanRegex() {
        return null;
    }

    @Override
    public Pattern getMoreThanRegex() {
        return null;
    }

    @Override
    public Pattern getCenturySuffixRegex() {
        return null;
    }

    @Override
    public Pattern getNowRegex() {
        return NowRegex;
    }

    @Override
    public IDateTimeExtractor getDatePointExtractor() {
        return datePointExtractor;
    }

    @Override
    public IExtractor getCardinalExtractor() {
        return cardinalExtractor;
    }

    @Override
    public IExtractor getOrdinalExtractor() {
        return ordinalExtractor;
    }

    @Override
    public IDateTimeExtractor getDurationExtractor() {
        return durationExtractor;
    }

    @Override
    public IParser getNumberParser() {
        return numberParser;
    }

    @Override
    public String[] getDurationDateRestrictions() {
        return durationDateRestrictions;
    }

    @Override
    public ResultIndex getFromTokenIndex(String text) {
        int index = -1;
        boolean result = false;
        if (text.endsWith("从")) {
            result = true;
            index = text.lastIndexOf("从");
        }

        return new ResultIndex(result, index);
    }

    @Override
    public ResultIndex getBetweenTokenIndex(String text) {
        int index = -1;
        boolean result = false;
        return new ResultIndex(result, index);
    }

    @Override
    public boolean hasConnectorToken(String text) {
        return RegexExtension.isExactMatch(RangeConnectorRegex, text, true);
    }
}
