package com.microsoft.recognizers.text.datetime.chinese.extractors;

import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.config.IHolidayExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.resources.ChineseDateTime;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.ArrayList;
import java.util.regex.Pattern;

public class ChineseHolidayExtractorConfiguration extends BaseOptionsConfiguration implements IHolidayExtractorConfiguration {

    public static final Pattern HolidayRegexList1 = RegExpUtility.getSafeRegExp(ChineseDateTime.HolidayRegexList1);
    public static final Pattern HolidayRegexList2 = RegExpUtility.getSafeRegExp(ChineseDateTime.HolidayRegexList2);
    public static final Pattern LunarHolidayRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.LunarHolidayRegex);

    public static final Iterable<Pattern> HolidayRegexList = new ArrayList<Pattern>() {
        {
            add(HolidayRegexList1);
            add(HolidayRegexList2);
            add(LunarHolidayRegex);
        }
    };

    public ChineseHolidayExtractorConfiguration() {
        super(DateTimeOptions.None);
    }

    public ChineseHolidayExtractorConfiguration(DateTimeOptions options) {
        super(options);
    }

    public Iterable<Pattern> getHolidayRegexes() {
        return HolidayRegexList;
    }
}
