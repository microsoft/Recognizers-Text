// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.datetime.german.extractors;

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
import com.microsoft.recognizers.text.datetime.resources.GermanDateTime;
import com.microsoft.recognizers.text.datetime.utilities.RegexExtension;
import com.microsoft.recognizers.text.number.NumberOptions;
import com.microsoft.recognizers.text.number.german.extractors.CardinalExtractor;
import com.microsoft.recognizers.text.number.german.extractors.OrdinalExtractor;
import com.microsoft.recognizers.text.number.german.parsers.GermanNumberParserConfiguration;
import com.microsoft.recognizers.text.number.parsers.BaseNumberParser;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.ArrayList;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class GermanDatePeriodExtractorConfiguration extends BaseOptionsConfiguration implements IDatePeriodExtractorConfiguration {

    public static final Pattern TillRegex = RegExpUtility.getSafeRegExp(GermanDateTime.TillRegex);
    public static final Pattern RangeConnectorRegex = RegExpUtility.getSafeRegExp(GermanDateTime.RangeConnectorRegex);
    public static final Pattern DayRegex = RegExpUtility.getSafeRegExp(GermanDateTime.DayRegex);
    public static final Pattern MonthNumRegex = RegExpUtility.getSafeRegExp(GermanDateTime.MonthNumRegex);
    public static final Pattern IllegalYearRegex = RegExpUtility.getSafeRegExp(BaseDateTime.IllegalYearRegex);
    public static final Pattern YearRegex = RegExpUtility.getSafeRegExp(GermanDateTime.YearRegex);
    public static final Pattern WeekDayRegex = RegExpUtility.getSafeRegExp(GermanDateTime.WeekDayRegex);
    public static final Pattern RelativeMonthRegex = RegExpUtility.getSafeRegExp(GermanDateTime.RelativeMonthRegex);
    public static final Pattern WrittenMonthRegex = RegExpUtility.getSafeRegExp(GermanDateTime.WrittenMonthRegex);
    public static final Pattern MonthSuffixRegex = RegExpUtility.getSafeRegExp(GermanDateTime.MonthSuffixRegex);

    public static final Pattern DateUnitRegex = RegExpUtility.getSafeRegExp(GermanDateTime.DateUnitRegex);
    public static final Pattern TimeUnitRegex = RegExpUtility.getSafeRegExp(GermanDateTime.TimeUnitRegex);
    public static final Pattern PreviousPrefixRegex = RegExpUtility.getSafeRegExp(GermanDateTime.PreviousPrefixRegex);
    public static final Pattern NextPrefixRegex = RegExpUtility.getSafeRegExp(GermanDateTime.NextPrefixRegex);
    public static final Pattern FutureSuffixRegex = RegExpUtility.getSafeRegExp(GermanDateTime.FutureSuffixRegex);


    // composite regexes
    public static final Pattern SimpleCasesRegex = RegExpUtility.getSafeRegExp(GermanDateTime.SimpleCasesRegex);
    public static final Pattern MonthFrontSimpleCasesRegex = RegExpUtility.getSafeRegExp(GermanDateTime.MonthFrontSimpleCasesRegex);
    public static final Pattern MonthFrontBetweenRegex = RegExpUtility.getSafeRegExp(GermanDateTime.MonthFrontBetweenRegex);
    public static final Pattern BetweenRegex = RegExpUtility.getSafeRegExp(GermanDateTime.BetweenRegex);
    public static final Pattern MonthWithYear = RegExpUtility.getSafeRegExp(GermanDateTime.MonthWithYear);
    public static final Pattern OneWordPeriodRegex = RegExpUtility.getSafeRegExp(GermanDateTime.OneWordPeriodRegex);
    public static final Pattern MonthNumWithYear = RegExpUtility.getSafeRegExp(GermanDateTime.MonthNumWithYear);
    public static final Pattern WeekOfMonthRegex = RegExpUtility.getSafeRegExp(GermanDateTime.WeekOfMonthRegex);
    public static final Pattern WeekOfYearRegex = RegExpUtility.getSafeRegExp(GermanDateTime.WeekOfYearRegex);
    public static final Pattern FollowedDateUnit = RegExpUtility.getSafeRegExp(GermanDateTime.FollowedDateUnit);
    public static final Pattern NumberCombinedWithDateUnit = RegExpUtility.getSafeRegExp(GermanDateTime.NumberCombinedWithDateUnit);
    public static final Pattern QuarterRegex = RegExpUtility.getSafeRegExp(GermanDateTime.QuarterRegex);
    public static final Pattern QuarterRegexYearFront = RegExpUtility.getSafeRegExp(GermanDateTime.QuarterRegexYearFront);
    public static final Pattern AllHalfYearRegex = RegExpUtility.getSafeRegExp(GermanDateTime.AllHalfYearRegex);
    public static final Pattern SeasonRegex = RegExpUtility.getSafeRegExp(GermanDateTime.SeasonRegex);
    public static final Pattern WhichWeekRegex = RegExpUtility.getSafeRegExp(GermanDateTime.WhichWeekRegex);
    public static final Pattern WeekOfRegex = RegExpUtility.getSafeRegExp(GermanDateTime.WeekOfRegex);
    public static final Pattern MonthOfRegex = RegExpUtility.getSafeRegExp(GermanDateTime.MonthOfRegex);
    public static final Pattern RangeUnitRegex = RegExpUtility.getSafeRegExp(GermanDateTime.RangeUnitRegex);
    public static final Pattern InConnectorRegex = RegExpUtility.getSafeRegExp(GermanDateTime.InConnectorRegex);
    public static final Pattern WithinNextPrefixRegex = RegExpUtility.getSafeRegExp(GermanDateTime.WithinNextPrefixRegex);
    public static final Pattern RestOfDateRegex = RegExpUtility.getSafeRegExp(GermanDateTime.RestOfDateRegex);
    public static final Pattern LaterEarlyPeriodRegex = RegExpUtility.getSafeRegExp(GermanDateTime.LaterEarlyPeriodRegex);
    public static final Pattern WeekWithWeekDayRangeRegex = RegExpUtility.getSafeRegExp(GermanDateTime.WeekWithWeekDayRangeRegex);
    public static final Pattern YearPlusNumberRegex = RegExpUtility.getSafeRegExp(GermanDateTime.YearPlusNumberRegex);
    public static final Pattern DecadeWithCenturyRegex = RegExpUtility.getSafeRegExp(GermanDateTime.DecadeWithCenturyRegex);
    public static final Pattern YearPeriodRegex = RegExpUtility.getSafeRegExp(GermanDateTime.YearPeriodRegex);
    public static final Pattern ComplexDatePeriodRegex = RegExpUtility.getSafeRegExp(GermanDateTime.ComplexDatePeriodRegex);
    public static final Pattern RelativeDecadeRegex = RegExpUtility.getSafeRegExp(GermanDateTime.RelativeDecadeRegex);
    public static final Pattern ReferenceDatePeriodRegex = RegExpUtility.getSafeRegExp(GermanDateTime.ReferenceDatePeriodRegex);
    public static final Pattern AgoRegex = RegExpUtility.getSafeRegExp(GermanDateTime.AgoRegex);
    public static final Pattern LaterRegex = RegExpUtility.getSafeRegExp(GermanDateTime.LaterRegex);
    public static final Pattern LessThanRegex = RegExpUtility.getSafeRegExp(GermanDateTime.LessThanRegex);
    public static final Pattern MoreThanRegex = RegExpUtility.getSafeRegExp(GermanDateTime.MoreThanRegex);
    public static final Pattern CenturySuffixRegex = RegExpUtility.getSafeRegExp(GermanDateTime.CenturySuffixRegex);
    public static final Pattern NowRegex = RegExpUtility.getSafeRegExp(GermanDateTime.NowRegex);
    public static final Pattern FirstLastRegex = RegExpUtility.getSafeRegExp(GermanDateTime.FirstLastRegex);
    public static final Pattern OfYearRegex = RegExpUtility.getSafeRegExp(GermanDateTime.OfYearRegex);
    public static final Pattern FromTokenRegex = RegExpUtility.getSafeRegExp(GermanDateTime.FromRegex);
    public static final Pattern BetweenTokenRegex = RegExpUtility.getSafeRegExp(GermanDateTime.BetweenTokenRegex);


    
    public static final Iterable<Pattern> SimpleCasesRegexes = new ArrayList<Pattern>() {
        {
            add(SimpleCasesRegex);
            add(BetweenRegex);
            add(OneWordPeriodRegex);
            add(MonthWithYear);
            add(MonthNumWithYear);
            add(YearRegex);
            add(YearPeriodRegex);
            add(WeekOfMonthRegex);
            add(WeekOfYearRegex);
            add(MonthFrontBetweenRegex);
            add(MonthFrontSimpleCasesRegex);
            add(QuarterRegex);
            add(QuarterRegexYearFront);
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

    private final String[] durationDateRestrictions = GermanDateTime.DurationDateRestrictions.toArray(new String[0]);

    private final IDateTimeExtractor datePointExtractor;
    private final IExtractor cardinalExtractor;
    private final IExtractor ordinalExtractor;
    private final IDateTimeExtractor durationExtractor;
    private final IParser numberParser;

    public GermanDatePeriodExtractorConfiguration(IOptionsConfiguration config) {
        super(config.getOptions());

        datePointExtractor = new BaseDateExtractor(new GermanDateExtractorConfiguration(this));
        cardinalExtractor = CardinalExtractor.getInstance();
        ordinalExtractor = new OrdinalExtractor();
        durationExtractor = new BaseDurationExtractor(new GermanDurationExtractorConfiguration());
        numberParser = new BaseNumberParser(new GermanNumberParserConfiguration());
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
    public Pattern getFollowedDateUnit() {
        return FollowedDateUnit;
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
    public Pattern getComplexDatePeriodRegex() {
        return ComplexDatePeriodRegex;
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

    public Pattern getMonthNumRegex() {return MonthNumRegex;}

    @Override
    public Pattern getNowRegex() {
        return NowRegex;
    }

    public Pattern getFirstLastRegex() { return FirstLastRegex; }

    public Pattern getOfYearRegex() { return OfYearRegex; }

    @Override
    public IExtractor getCardinalExtractor() {
        return cardinalExtractor;
    }

    @Override
    public IDateTimeExtractor getDatePointExtractor() {
        return datePointExtractor;
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
        final Matcher fromMatch = FromTokenRegex.matcher(text);
        if (fromMatch.find()) {
            result = true;
            index = fromMatch.start();
        }

        return new ResultIndex(result, index);
    }

    @Override
    public ResultIndex getBetweenTokenIndex(String text) {
        int index = -1;
        boolean result = false;
        final Matcher betweenMatch = BetweenTokenRegex.matcher(text);
        if (betweenMatch.find()) {
            result = true;
            index = betweenMatch.start();
        }
        
        return new ResultIndex(result, index);
    }

    @Override
    public boolean hasConnectorToken(String text) {
        return RegexExtension.isExactMatch(RangeConnectorRegex, text, true);
    }
}
