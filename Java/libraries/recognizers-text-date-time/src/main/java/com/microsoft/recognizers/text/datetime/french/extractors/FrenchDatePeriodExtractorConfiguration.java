package com.microsoft.recognizers.text.datetime.french.extractors;

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
import com.microsoft.recognizers.text.datetime.resources.FrenchDateTime;
import com.microsoft.recognizers.text.number.french.extractors.CardinalExtractor;
import com.microsoft.recognizers.text.number.french.extractors.OrdinalExtractor;
import com.microsoft.recognizers.text.number.french.parsers.FrenchNumberParserConfiguration;
import com.microsoft.recognizers.text.number.parsers.BaseNumberParser;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Optional;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class FrenchDatePeriodExtractorConfiguration extends BaseOptionsConfiguration implements IDatePeriodExtractorConfiguration {
    public static final Pattern TillRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.TillRegex);
    public static final Pattern RangeConnectorRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.RangeConnectorRegex);
    public static final Pattern DayRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.DayRegex);
    public static final Pattern MonthNumRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.MonthNumRegex);
    public static final Pattern IllegalYearRegex = RegExpUtility.getSafeRegExp(BaseDateTime.IllegalYearRegex);
    public static final Pattern YearRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.YearRegex);
    public static final Pattern RelativeMonthRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.RelativeMonthRegex);
    public static final Pattern MonthRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.MonthRegex);
    public static final Pattern MonthSuffixRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.MonthSuffixRegex);
    public static final Pattern DateUnitRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.DateUnitRegex);
    public static final Pattern TimeUnitRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.TimeUnitRegex);
    public static final Pattern PastRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.PastSuffixRegex);
    public static final Pattern FutureRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.NextSuffixRegex);
    public static final Pattern FutureSuffixRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.FutureSuffixRegex);

    // composite regexes
    public static final Pattern SimpleCasesRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.SimpleCasesRegex);
    public static final Pattern MonthFrontSimpleCasesRegex = RegExpUtility
        .getSafeRegExp(FrenchDateTime.MonthFrontSimpleCasesRegex);
    public static final Pattern MonthFrontBetweenRegex = RegExpUtility
        .getSafeRegExp(FrenchDateTime.MonthFrontBetweenRegex);
    public static final Pattern BetweenRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.BetweenRegex);
    public static final Pattern OneWordPeriodRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.OneWordPeriodRegex);
    public static final Pattern MonthWithYearRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.MonthWithYear);
    public static final Pattern MonthNumWithYearRegex = RegExpUtility
        .getSafeRegExp(FrenchDateTime.MonthNumWithYear);
    public static final Pattern WeekOfMonthRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.WeekOfMonthRegex);
    public static final Pattern WeekOfYearRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.WeekOfYearRegex);
    public static final Pattern FollowedDateUnit = RegExpUtility.getSafeRegExp(FrenchDateTime.FollowedDateUnit);
    public static final Pattern NumberCombinedWithDateUnit = RegExpUtility
        .getSafeRegExp(FrenchDateTime.NumberCombinedWithDateUnit);
    public static final Pattern QuarterRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.QuarterRegex);
    public static final Pattern QuarterRegexYearFront = RegExpUtility
        .getSafeRegExp(FrenchDateTime.QuarterRegexYearFront);
    public static final Pattern AllHalfYearRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.AllHalfYearRegex);
    public static final Pattern SeasonRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.SeasonRegex);
    public static final Pattern WhichWeekRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.WhichWeekRegex);
    public static final Pattern WeekOfRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.WeekOfRegex);
    public static final Pattern MonthOfRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.MonthOfRegex);
    public static final Pattern RangeUnitRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.RangeUnitRegex);
    public static final Pattern InConnectorRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.InConnectorRegex);
    public static final Pattern WithinNextPrefixRegex = RegExpUtility
        .getSafeRegExp(FrenchDateTime.WithinNextPrefixRegex);
    public static final Pattern LaterEarlyPeriodRegex = RegExpUtility
        .getSafeRegExp(FrenchDateTime.LaterEarlyPeriodRegex);
    public static final Pattern RestOfDateRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.RestOfDateRegex);
    public static final Pattern WeekWithWeekDayRangeRegex = RegExpUtility
        .getSafeRegExp(FrenchDateTime.WeekWithWeekDayRangeRegex);
    public static final Pattern YearPlusNumberRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.YearPlusNumberRegex);
    public static final Pattern DecadeWithCenturyRegex = RegExpUtility
        .getSafeRegExp(FrenchDateTime.DecadeWithCenturyRegex);
    public static final Pattern YearPeriodRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.YearPeriodRegex);
    public static final Pattern ComplexDatePeriodRegex = RegExpUtility
        .getSafeRegExp(FrenchDateTime.ComplexDatePeriodRegex);
    public static final Pattern RelativeDecadeRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.RelativeDecadeRegex);
    public static final Pattern ReferenceDatePeriodRegex = RegExpUtility
        .getSafeRegExp(FrenchDateTime.ReferenceDatePeriodRegex);
    public static final Pattern AgoRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.AgoRegex);
    public static final Pattern LaterRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.LaterRegex);
    public static final Pattern LessThanRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.LessThanRegex);
    public static final Pattern MoreThanRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.MoreThanRegex);
    public static final Pattern CenturySuffixRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.CenturySuffixRegex);
    public static final Pattern NowRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.NowRegex);

    public static final Iterable<Pattern> SimpleCasesRegexes = new ArrayList<Pattern>() {
        {
            add(SimpleCasesRegex);
            add(BetweenRegex);
            add(OneWordPeriodRegex);
            add(RegExpUtility.getSafeRegExp(FrenchDateTime.MonthWithYear));
            add(RegExpUtility.getSafeRegExp(FrenchDateTime.MonthNumWithYear));
            add(YearRegex);
            add(YearPeriodRegex);
            add(WeekOfYearRegex);
            add(RegExpUtility.getSafeRegExp(FrenchDateTime.WeekDayOfMonthRegex));
            add(MonthFrontBetweenRegex);
            add(MonthFrontSimpleCasesRegex);
            add(QuarterRegex);
            add(QuarterRegexYearFront);
            add(SeasonRegex);
            add(LaterEarlyPeriodRegex);
            add(YearPlusNumberRegex);
            add(DecadeWithCenturyRegex);
            add(RelativeDecadeRegex);
        }
    };

    private static final Pattern fromRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.FromRegex);
    private static final Pattern betweenRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.BetweenRegex);

    private final IDateTimeExtractor datePointExtractor;
    private final IExtractor cardinalExtractor;
    private final IExtractor ordinalExtractor;
    private final IDateTimeExtractor durationExtractor;
    private final IParser numberParser;
    private final String[] durationDateRestrictions;

    public FrenchDatePeriodExtractorConfiguration(final IOptionsConfiguration config) {
        super(config.getOptions());

        datePointExtractor = new BaseDateExtractor(new FrenchDateExtractorConfiguration(this));
        cardinalExtractor = CardinalExtractor.getInstance();
        ordinalExtractor = new OrdinalExtractor();
        durationExtractor = new BaseDurationExtractor(new FrenchDurationExtractorConfiguration());
        numberParser = new BaseNumberParser(new FrenchNumberParserConfiguration());

        durationDateRestrictions = FrenchDateTime.DurationDateRestrictions.toArray(new String[0]);
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
    public ResultIndex getFromTokenIndex(final String text) {
        int index = -1;
        boolean result = false;
        final Matcher matcher = fromRegex.matcher(text);
        if (matcher.find()) {
            result = true;
            index = matcher.start();
        }

        return new ResultIndex(result, index);
    }

    @Override
    public ResultIndex getBetweenTokenIndex(final String text) {
        int index = -1;
        boolean result = false;
        final Matcher matcher = betweenRegex.matcher(text);
        if (matcher.find()) {
            result = true;
            index = matcher.start();
        }

        return new ResultIndex(result, index);
    }

    @Override
    public boolean hasConnectorToken(final String text) {
        final Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(RegExpUtility.getSafeRegExp(FrenchDateTime.ConnectorAndRegex), text)).findFirst();
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
