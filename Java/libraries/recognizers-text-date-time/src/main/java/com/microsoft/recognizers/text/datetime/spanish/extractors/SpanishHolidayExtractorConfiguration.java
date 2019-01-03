package com.microsoft.recognizers.text.datetime.spanish.extractors;

import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.config.IHolidayExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.resources.SpanishDateTime;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.Arrays;
import java.util.regex.Pattern;

public class SpanishHolidayExtractorConfiguration extends BaseOptionsConfiguration implements IHolidayExtractorConfiguration {

    public static Pattern[] HolidayRegexes = {
    RegExpUtility.getSafeRegExp(SpanishDateTime.HolidayRegex1),
    RegExpUtility.getSafeRegExp(SpanishDateTime.HolidayRegex2),
    RegExpUtility.getSafeRegExp(SpanishDateTime.HolidayRegex3)};
    
   
    public SpanishHolidayExtractorConfiguration() {

    }

    @Override
    public Iterable<Pattern> getHolidayRegexes() {
        return Arrays.asList(HolidayRegexes);
    }
}
