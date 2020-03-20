package com.microsoft.recognizers.text.datetime.spanish.extractors;

import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.config.IOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.BaseDateExtractor;
import com.microsoft.recognizers.text.datetime.extractors.BaseDurationExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.config.IDatePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.config.ResultIndex;
import com.microsoft.recognizers.text.datetime.resources.BaseDateTime;
import com.microsoft.recognizers.text.datetime.resources.SpanishDateTime;
import com.microsoft.recognizers.text.number.parsers.BaseNumberParser;
import com.microsoft.recognizers.text.number.spanish.extractors.CardinalExtractor;
import com.microsoft.recognizers.text.number.spanish.extractors.OrdinalExtractor;
import com.microsoft.recognizers.text.number.spanish.parsers.SpanishNumberParserConfiguration;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Optional;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class SpanishDatePeriodExtractorConfiguration extends BaseOptionsConfiguration implements IDatePeriodExtractorConfiguration {
    public static final Pattern TillRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.TillRegex);
    public static final Pattern RangeConnectorRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.RangeConnectorRegex);
    public static final Pattern DayRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.DayRegex);
    public static final Pattern MonthNumRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.MonthNumRegex);
    public static final Pattern IllegalYearRegex = RegExpUtility.getSafeRegExp(BaseDateTime.IllegalYearRegex);
    public static final Pattern YearRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.YearRegex);
    public static final Pattern RelativeMonthRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.RelativeMonthRegex);
    public static final Pattern MonthRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.MonthRegex);
    public static final Pattern MonthSuffixRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.MonthSuffixRegex);
    public static final Pattern DateUnitRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.DateUnitRegex);
    public static final Pattern TimeUnitRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.TimeUnitRegex);
    public static final Pattern PastRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.PastRegex);
    public static final Pattern FutureRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.FutureRegex);
    public static final Pattern FutureSuffixRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.FutureSuffixRegex);

    // composite regexes
    public static final Pattern SimpleCasesRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.SimpleCasesRegex);
    public static final Pattern MonthFrontSimpleCasesRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.MonthFrontSimpleCasesRegex);
    public static final Pattern MonthFrontBetweenRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.MonthFrontBetweenRegex);
    public static final Pattern DayBetweenRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.DayBetweenRegex);
    public static final Pattern OneWordPeriodRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.OneWordPeriodRegex);
    public static final Pattern MonthWithYearRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.MonthWithYearRegex);
    public static final Pattern MonthNumWithYearRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.MonthNumWithYearRegex);
    public static final Pattern WeekOfMonthRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.WeekOfMonthRegex);
    public static final Pattern WeekOfYearRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.WeekOfYearRegex);
    public static final Pattern FollowedDateUnit = RegExpUtility.getSafeRegExp(SpanishDateTime.FollowedDateUnit);
    public static final Pattern NumberCombinedWithDateUnit = RegExpUtility.getSafeRegExp(SpanishDateTime.NumberCombinedWithDateUnit);
    public static final Pattern QuarterRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.QuarterRegex);
    public static final Pattern QuarterRegexYearFront = RegExpUtility.getSafeRegExp(SpanishDateTime.QuarterRegexYearFront);
    public static final Pattern AllHalfYearRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.AllHalfYearRegex);
    public static final Pattern SeasonRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.SeasonRegex);
    public static final Pattern WhichWeekRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.WhichWeekRegex);
    public static final Pattern WeekOfRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.WeekOfRegex);
    public static final Pattern MonthOfRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.MonthOfRegex);
    public static final Pattern RangeUnitRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.RangeUnitRegex);
    public static final Pattern InConnectorRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.InConnectorRegex);
    public static final Pattern WithinNextPrefixRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.WithinNextPrefixRegex);
    public static final Pattern LaterEarlyPeriodRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.LaterEarlyPeriodRegex);
    public static final Pattern RestOfDateRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.RestOfDateRegex);
    public static final Pattern WeekWithWeekDayRangeRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.WeekWithWeekDayRangeRegex);
    public static final Pattern YearPlusNumberRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.YearPlusNumberRegex);
    public static final Pattern DecadeWithCenturyRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.DecadeWithCenturyRegex);
    public static final Pattern YearPeriodRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.YearPeriodRegex);
    public static final Pattern ComplexDatePeriodRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.ComplexDatePeriodRegex);
    public static final Pattern RelativeDecadeRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.RelativeDecadeRegex);
    public static final Pattern ReferenceDatePeriodRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.ReferenceDatePeriodRegex);
    public static final Pattern AgoRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.AgoRegex);
    public static final Pattern LaterRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.LaterRegex);
    public static final Pattern LessThanRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.LessThanRegex);
    public static final Pattern MoreThanRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.MoreThanRegex);
    public static final Pattern CenturySuffixRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.CenturySuffixRegex);
    public static final Pattern NowRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.NowRegex);

    public static final Iterable<Pattern> SimpleCasesRegexes = new ArrayList<Pattern>() {
        {
            add(SimpleCasesRegex);
            add(DayBetweenRegex);
            add(OneWordPeriodRegex);
            add(MonthWithYearRegex);
            add(MonthNumWithYearRegex);
            add(YearRegex);
            add(YearPeriodRegex);
            add(WeekOfMonthRegex);
            add(WeekOfYearRegex);
            add(MonthFrontBetweenRegex);
            add(MonthFrontSimpleCasesRegex);
            add(QuarterRegex);
            add(QuarterRegexYearFront);
            add(SeasonRegex);
            add(RestOfDateRegex);
            add(LaterEarlyPeriodRegex);
            add(WeekWithWeekDayRangeRegex);
            add(YearPlusNumberRegex);
            add(DecadeWithCenturyRegex);
            add(RelativeDecadeRegex);
            add(MonthOfRegex);
        }
    };

    private static final Pattern fromRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.FromRegex);
    private static final Pattern betweenRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.BetweenRegex);

    private final IDateTimeExtractor datePointExtractor;
    private final IExtractor cardinalExtractor;
    private final IExtractor ordinalExtractor;
    private final IDateTimeExtractor durationExtractor;
    private final IParser numberParser;
    private final String[] durationDateRestrictions;

    public SpanishDatePeriodExtractorConfiguration(IOptionsConfiguration config) {
        super(config.getOptions());

        datePointExtractor = new BaseDateExtractor(new SpanishDateExtractorConfiguration(this));
        cardinalExtractor = CardinalExtractor.getInstance();
        ordinalExtractor = OrdinalExtractor.getInstance();
        durationExtractor = new BaseDurationExtractor(new SpanishDurationExtractorConfiguration());
        numberParser = new BaseNumberParser(new SpanishNumberParserConfiguration());

        durationDateRestrictions = SpanishDateTime.DurationDateRestrictions.toArray(new String[0]);
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
        return PastRegex;
    }

    @Override
    public Pattern getFutureRegex() {
        return FutureRegex;
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
    public String[] getDurationDateRestrictions() {
        return durationDateRestrictions;
    }

    @Override
    public ResultIndex getFromTokenIndex(String text) {
        int index = -1;
        boolean result = false;
        Matcher matcher = fromRegex.matcher(text);
        if (matcher.find()) {
            result = true;
            index = matcher.start();
        }

        return new ResultIndex(result, index);
    }

    @Override
    public ResultIndex getBetweenTokenIndex(String text) {
        int index = -1;
        boolean result = false;
        Matcher matcher = betweenRegex.matcher(text);
        if (matcher.find()) {
            result = true;
            index = matcher.start();
        }

        return new ResultIndex(result, index);
    }

    @Override
    public boolean hasConnectorToken(String text) {
        Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(RangeConnectorRegex, text)).findFirst();
        return match.isPresent() && match.get().length == text.trim().length();
    }

    @Override
    public Pattern getComplexDatePeriodRegex() {
        return ComplexDatePeriodRegex;
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
}
