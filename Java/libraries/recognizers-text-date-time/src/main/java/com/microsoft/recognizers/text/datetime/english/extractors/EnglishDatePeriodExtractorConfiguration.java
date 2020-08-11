package com.microsoft.recognizers.text.datetime.english.extractors;

import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.config.IOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.BaseDateExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseDurationExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.config.IDatePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.config.ResultIndex;
import com.microsoft.recognizers.text.datetime.resources.BaseDateTime;
import com.microsoft.recognizers.text.datetime.resources.EnglishDateTime;
import com.microsoft.recognizers.text.datetime.utilities.RegexExtension;
import com.microsoft.recognizers.text.number.english.extractors.CardinalExtractor;
import com.microsoft.recognizers.text.number.english.extractors.OrdinalExtractor;
import com.microsoft.recognizers.text.number.english.parsers.EnglishNumberParserConfiguration;
import com.microsoft.recognizers.text.number.parsers.BaseNumberParser;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Optional;
import java.util.regex.Pattern;

public class EnglishDatePeriodExtractorConfiguration extends BaseOptionsConfiguration implements IDatePeriodExtractorConfiguration {

    public static final Pattern YearRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.YearRegex);
    public static final Pattern TillRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.TillRegex);
    public static final Pattern DateUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.DateUnitRegex);
    public static final Pattern TimeUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.TimeUnitRegex);
    public static final Pattern FollowedDateUnit = RegExpUtility.getSafeRegExp(EnglishDateTime.FollowedDateUnit);
    public static final Pattern NumberCombinedWithDateUnit = RegExpUtility.getSafeRegExp(EnglishDateTime.NumberCombinedWithDateUnit);
    public static final Pattern PreviousPrefixRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PreviousPrefixRegex);
    public static final Pattern NextPrefixRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.NextPrefixRegex);
    public static final Pattern FutureSuffixRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.FutureSuffixRegex);
    public static final Pattern WeekOfRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.WeekOfRegex);
    public static final Pattern MonthOfRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.MonthOfRegex);
    public static final Pattern RangeUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.RangeUnitRegex);
    public static final Pattern InConnectorRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.InConnectorRegex);
    public static final Pattern WithinNextPrefixRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.WithinNextPrefixRegex);
    public static final Pattern YearPeriodRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.YearPeriodRegex);
    public static final Pattern RelativeDecadeRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.RelativeDecadeRegex);
    public static final Pattern ComplexDatePeriodRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.ComplexDatePeriodRegex);
    public static final Pattern ReferenceDatePeriodRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.ReferenceDatePeriodRegex);
    public static final Pattern AgoRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AgoRegex);
    public static final Pattern LaterRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.LaterRegex);
    public static final Pattern LessThanRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.LessThanRegex);
    public static final Pattern MoreThanRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.MoreThanRegex);
    public static final Pattern CenturySuffixRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.CenturySuffixRegex);
    public static final Pattern IllegalYearRegex = RegExpUtility.getSafeRegExp(BaseDateTime.IllegalYearRegex);
    public static final Pattern NowRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.NowRegex);

    // composite regexes
    public static final Pattern SimpleCasesRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SimpleCasesRegex);
    public static final Pattern BetweenRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.BetweenRegex);
    public static final Pattern OneWordPeriodRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.OneWordPeriodRegex);
    public static final Pattern MonthWithYear = RegExpUtility.getSafeRegExp(EnglishDateTime.MonthWithYear);
    public static final Pattern MonthNumWithYear = RegExpUtility.getSafeRegExp(EnglishDateTime.MonthNumWithYear);
    public static final Pattern WeekOfMonthRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.WeekOfMonthRegex);
    public static final Pattern WeekOfYearRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.WeekOfYearRegex);
    public static final Pattern MonthFrontBetweenRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.MonthFrontBetweenRegex);
    public static final Pattern MonthFrontSimpleCasesRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.MonthFrontSimpleCasesRegex);
    public static final Pattern QuarterRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.QuarterRegex);
    public static final Pattern QuarterRegexYearFront = RegExpUtility.getSafeRegExp(EnglishDateTime.QuarterRegexYearFront);
    public static final Pattern AllHalfYearRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AllHalfYearRegex);
    public static final Pattern SeasonRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SeasonRegex);
    public static final Pattern WhichWeekRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.WhichWeekRegex);
    public static final Pattern RestOfDateRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.RestOfDateRegex);
    public static final Pattern LaterEarlyPeriodRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.LaterEarlyPeriodRegex);
    public static final Pattern WeekWithWeekDayRangeRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.WeekWithWeekDayRangeRegex);
    public static final Pattern YearPlusNumberRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.YearPlusNumberRegex);
    public static final Pattern DecadeWithCenturyRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.DecadeWithCenturyRegex);
    
    public static final Iterable<Pattern> SimpleCasesRegexes = new ArrayList<Pattern>() {
        {
            add(SimpleCasesRegex);
            add(BetweenRegex);
            add(OneWordPeriodRegex);
            add(MonthWithYear);
            add(MonthNumWithYear);
            add(YearRegex);
            add(WeekOfMonthRegex);
            add(WeekOfYearRegex);
            add(MonthFrontBetweenRegex);
            add(MonthFrontSimpleCasesRegex);
            add(QuarterRegex);
            add(QuarterRegexYearFront);
            add(AllHalfYearRegex);
            add(SeasonRegex);
            add(WhichWeekRegex);
            add(RestOfDateRegex);
            add(LaterEarlyPeriodRegex);
            add(WeekWithWeekDayRangeRegex);
            add(YearPlusNumberRegex);
            add(DecadeWithCenturyRegex);
            add(RelativeDecadeRegex);
            add(ReferenceDatePeriodRegex);
        }
    };

    public static final Pattern rangeConnectorRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.RangeConnectorRegex);
    private final String[] durationDateRestrictions = EnglishDateTime.DurationDateRestrictions.toArray(new String[0]);

    private final IDateTimeExtractor datePointExtractor;
    private final IExtractor cardinalExtractor;
    private final IExtractor ordinalExtractor;
    private final IDateTimeExtractor durationExtractor;
    private final IParser numberParser;

    public EnglishDatePeriodExtractorConfiguration(IOptionsConfiguration config) {
        super(config.getOptions());

        datePointExtractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration(this));
        cardinalExtractor = CardinalExtractor.getInstance();
        ordinalExtractor = OrdinalExtractor.getInstance();
        durationExtractor = new BaseDurationExtractor(new EnglishDurationExtractorConfiguration());
        numberParser = new BaseNumberParser(new EnglishNumberParserConfiguration());
    }

    @Override
    public Iterable<Pattern> getSimpleCasesRegexes() {
        return SimpleCasesRegexes;
    }

    @Override
    public Pattern getIllegalYearRegex() {
        return IllegalYearRegex;
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
        return TimeUnitRegex;
    }

    @Override
    public Pattern getFollowedDateUnit() {
        return FollowedDateUnit;
    }

    @Override
    public Pattern getNumberCombinedWithDateUnit() {
        return NumberCombinedWithDateUnit;
    }

    @Override
    public Pattern getPastRegex() {
        return PreviousPrefixRegex;
    }

    @Override
    public Pattern getFutureRegex() {
        return NextPrefixRegex;
    }

    @Override
    public Pattern getFutureSuffixRegex() {
        return FutureSuffixRegex;
    }

    @Override
    public Pattern getWeekOfRegex() {
        return WeekOfRegex;
    }

    @Override
    public Pattern getMonthOfRegex() {
        return MonthOfRegex;
    }

    @Override
    public Pattern getRangeUnitRegex() {
        return RangeUnitRegex;
    }

    @Override
    public Pattern getInConnectorRegex() {
        return InConnectorRegex;
    }

    @Override
    public Pattern getWithinNextPrefixRegex() {
        return WithinNextPrefixRegex;
    }

    @Override
    public Pattern getYearPeriodRegex() {
        return YearPeriodRegex;
    }

    @Override
    public Pattern getRelativeDecadeRegex() {
        return RelativeDecadeRegex;
    }

    @Override
    public Pattern getComplexDatePeriodRegex() {
        return ComplexDatePeriodRegex;
    }

    @Override
    public Pattern getReferenceDatePeriodRegex() {
        return ReferenceDatePeriodRegex;
    }

    @Override
    public Pattern getAgoRegex() {
        return AgoRegex;
    }

    @Override
    public Pattern getLaterRegex() {
        return LaterRegex;
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
    public Pattern getCenturySuffixRegex() {
        return CenturySuffixRegex;
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
        if (text.endsWith("from")) {
            result = true;
            index = text.lastIndexOf("from");
        }

        return new ResultIndex(result, index);
    }

    @Override
    public ResultIndex getBetweenTokenIndex(String text) {
        int index = -1;
        boolean result = false;
        if (text.endsWith("between")) {
            result = true;
            index = text.lastIndexOf("between");
        }
        
        return new ResultIndex(result, index);
    }

    @Override
    public boolean hasConnectorToken(String text) {
        return RegexExtension.isExactMatch(rangeConnectorRegex, text, true);
    }
}
