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

    public static final Pattern H = RegExpUtility.getSafeRegExp(EnglishDateTime.HolidayRegex);

    public static final Iterable<Pattern> HolidayRegexList = new ArrayList<Pattern>() {
        {
            add(H);
        }
    };

    public EnglishHolidayExtractorConfiguration() {
        super(DateTimeOptions.None);
    }

    public Iterable<Pattern> getHolidayRegexes() {
        return HolidayRegexList;
    }
}
