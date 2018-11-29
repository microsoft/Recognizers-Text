package com.microsoft.recognizers.text.datetime.english.extractors;

import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.config.IHolidayExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.resources.EnglishDateTime;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.ArrayList;
import java.util.regex.Pattern;

public class EnglishHolidayExtractorConfiguration extends BaseOptionsConfiguration implements IHolidayExtractorConfiguration {

    public static final Pattern YearPattern = RegExpUtility.getSafeRegExp(EnglishDateTime.YearRegex);

    public static final Pattern H1 = RegExpUtility.getSafeRegExp(EnglishDateTime.HolidayRegex1, Pattern.CASE_INSENSITIVE);

    public static final Pattern H2 = RegExpUtility.getSafeRegExp(EnglishDateTime.HolidayRegex2);

    public static final Pattern H3 = RegExpUtility.getSafeRegExp(EnglishDateTime.HolidayRegex3, Pattern.CASE_INSENSITIVE);

    public static final Iterable<Pattern> HolidayRegexList = new ArrayList<Pattern>() {
        {
            add(H1);
            add(H2);
            add(H3);
        }
    };

    public EnglishHolidayExtractorConfiguration() {
        super(DateTimeOptions.None);
    }

    public Iterable<Pattern> getHolidayRegexes() {
        return HolidayRegexList;
    }
}
