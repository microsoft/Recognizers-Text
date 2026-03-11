package com.microsoft.recognizers.text.datetime.chinese.extractors;

import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.BaseTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.IDateTimeExtractor;
import com.microsoft.recognizers.text.datetime.extractors.config.ITimePeriodExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.config.ResultIndex;
import com.microsoft.recognizers.text.datetime.resources.ChineseDateTime;
import com.microsoft.recognizers.text.number.chinese.extractors.IntegerExtractor;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.ArrayList;
import java.util.regex.Pattern;

public class ChineseTimePeriodExtractorConfiguration extends BaseOptionsConfiguration implements ITimePeriodExtractorConfiguration {

    public static final Pattern TimePeriodRegexes1 = RegExpUtility.getSafeRegExp(ChineseDateTime.TimePeriodRegexes1);
    public static final Pattern TimePeriodRegexes2 = RegExpUtility.getSafeRegExp(ChineseDateTime.TimePeriodRegexes2);
    public static final Pattern TimeOfDayRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.TimeOfDayRegex);
    public static final Pattern DateTimePeriodTillRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DateTimePeriodTillRegex);

    private String tokenBeforeDate;
    private IDateTimeExtractor singleTimeExtractor;
    private IExtractor integerExtractor;
    private IDateTimeExtractor timeZoneExtractor;

    public final Iterable<Pattern> simpleCasesRegex = new ArrayList<Pattern>() {
        {
            add(TimePeriodRegexes1);
            add(TimePeriodRegexes2);
        }
    };

    public ChineseTimePeriodExtractorConfiguration() {
        this(DateTimeOptions.None);
    }

    public ChineseTimePeriodExtractorConfiguration(DateTimeOptions options) {
        super(options);

        tokenBeforeDate = ChineseDateTime.PrepositionRegex;
        singleTimeExtractor = new BaseTimeExtractor(new ChineseTimeExtractorConfiguration(options));
        integerExtractor = new IntegerExtractor();
        timeZoneExtractor = null;
    }

    @Override
    public String getTokenBeforeDate() {
        return tokenBeforeDate;
    }

    @Override
    public Iterable<Pattern> getSimpleCasesRegex() {
        return simpleCasesRegex;
    }

    @Override
    public Pattern getTillRegex() {
        return DateTimePeriodTillRegex;
    }

    @Override
    public Pattern getTimeOfDayRegex() {
        return TimeOfDayRegex;
    }

    @Override
    public Pattern getGeneralEndingRegex() {
        return null;
    }

    @Override
    public IDateTimeExtractor getSingleTimeExtractor() {
        return singleTimeExtractor;
    }

    @Override
    public IExtractor getIntegerExtractor() {
        return integerExtractor;
    }

    @Override
    public IDateTimeExtractor getTimeZoneExtractor() {
        return timeZoneExtractor;
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
        return false;
    }
}
