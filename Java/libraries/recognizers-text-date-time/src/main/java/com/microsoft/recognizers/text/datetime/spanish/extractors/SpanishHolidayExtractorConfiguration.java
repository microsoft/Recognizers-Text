package com.microsoft.recognizers.text.datetime.spanish.extractors;

import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.config.IHolidayExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.resources.SpanishDateTime;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.ArrayList;
import java.util.regex.Pattern;

public class SpanishHolidayExtractorConfiguration extends BaseOptionsConfiguration implements IHolidayExtractorConfiguration {

    public static final Pattern H1 = RegExpUtility.getSafeRegExp(SpanishDateTime.HolidayRegex1);

    public static final Pattern H2 = RegExpUtility.getSafeRegExp(SpanishDateTime.HolidayRegex2);

    public static final Pattern H3 = RegExpUtility.getSafeRegExp(SpanishDateTime.HolidayRegex3);

    public static final Iterable<Pattern> HolidayRegexList = new ArrayList<Pattern>() {
        {
            add(H1);
            add(H2);
            add(H3);
        }
    };

    public SpanishHolidayExtractorConfiguration() {
        super(DateTimeOptions.None);
    }

    @Override
    public Iterable<Pattern> getHolidayRegexes() {
        return HolidayRegexList;
    }
}
