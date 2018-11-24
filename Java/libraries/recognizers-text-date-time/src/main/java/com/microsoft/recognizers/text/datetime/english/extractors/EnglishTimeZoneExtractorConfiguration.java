package com.microsoft.recognizers.text.datetime.english.extractors;

import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.config.ITimeZoneExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.resources.EnglishTimeZone;
import com.microsoft.recognizers.text.matcher.StringMatcher;
import com.microsoft.recognizers.text.utilities.QueryProcessor;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.ArrayList;
import java.util.List;
import java.util.regex.Pattern;
import java.util.stream.Collectors;

public class EnglishTimeZoneExtractorConfiguration extends BaseOptionsConfiguration implements ITimeZoneExtractorConfiguration {

    public static final Pattern DirectUtcRegex = RegExpUtility.getSafeRegExp(EnglishTimeZone.DirectUtcRegex, Pattern.CASE_INSENSITIVE);
    public static final Pattern AbbreviationRegex = RegExpUtility.getSafeRegExp(EnglishTimeZone.AbbreviationsRegex, Pattern.CASE_INSENSITIVE);
    public static final Pattern StandardTimeRegex = RegExpUtility.getSafeRegExp(EnglishTimeZone.FullNameRegex, Pattern.CASE_INSENSITIVE);
    public static final Pattern LocationTimeSuffixRegex = RegExpUtility.getSafeRegExp(EnglishTimeZone.LocationTimeSuffixRegex, Pattern.CASE_INSENSITIVE);

    public static final Iterable<Pattern> TimeZoneRegexList = new ArrayList<Pattern>() {
        {
            add(DirectUtcRegex);
            add(AbbreviationRegex);
            add(StandardTimeRegex);
        }
    };

    public static final StringMatcher CityMatcher = new StringMatcher();

    public static final List<String> AmbiguousTimezoneList = EnglishTimeZone.AmbiguousTimezoneList;

    public EnglishTimeZoneExtractorConfiguration()  {
        this(DateTimeOptions.None);
    }

    public EnglishTimeZoneExtractorConfiguration(DateTimeOptions options) {

        super(options);

        if (options.match(DateTimeOptions.EnablePreview)) {
            CityMatcher.init(
                    EnglishTimeZone.MajorLocations.stream()
                            .map(o -> QueryProcessor.removeDiacritics(o.toLowerCase()))
                            .collect(Collectors.toCollection(ArrayList::new)));
        }
    }

    @Override
    public Iterable<Pattern> getTimeZoneRegexes() {
        return TimeZoneRegexList;
    }

    @Override
    public Pattern getLocationTimeSuffixRegex() {
        return LocationTimeSuffixRegex;
    }

    @Override
    public StringMatcher getCityMatcher() {
        return CityMatcher;
    }

    @Override
    public List<String> getAmbiguousTimezoneList() {
        return AmbiguousTimezoneList;
    }
}
