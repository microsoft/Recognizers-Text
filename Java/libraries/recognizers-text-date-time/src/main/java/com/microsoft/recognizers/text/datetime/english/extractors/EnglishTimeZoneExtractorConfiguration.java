package com.microsoft.recognizers.text.datetime.english.extractors;

import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.config.ITimeZoneExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.resources.EnglishTimeZone;
import com.microsoft.recognizers.text.matcher.MatchStrategy;
import com.microsoft.recognizers.text.matcher.NumberWithUnitTokenizer;
import com.microsoft.recognizers.text.matcher.StringMatcher;
import com.microsoft.recognizers.text.utilities.QueryProcessor;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.ArrayList;
import java.util.List;
import java.util.regex.Pattern;
import java.util.stream.Collectors;

public class EnglishTimeZoneExtractorConfiguration extends BaseOptionsConfiguration implements ITimeZoneExtractorConfiguration {

    // These regexes do need to be case insensitive for them to work correctly
    public static final Pattern DirectUtcRegex = RegExpUtility.getSafeRegExp(EnglishTimeZone.DirectUtcRegex, Pattern.CASE_INSENSITIVE);
    public static final List<String> AbbreviationsList = EnglishTimeZone.AbbreviationsList;
    public static final List<String> FullNameList = EnglishTimeZone.FullNameList;
    public static final Pattern LocationTimeSuffixRegex = RegExpUtility.getSafeRegExp(EnglishTimeZone.LocationTimeSuffixRegex, Pattern.CASE_INSENSITIVE);
    public static final StringMatcher LocationMatcher = new StringMatcher();
    public static final StringMatcher TimeZoneMatcher = buildMatcherFromLists(AbbreviationsList, FullNameList);

    public static final List<String> AmbiguousTimezoneList = EnglishTimeZone.AmbiguousTimezoneList;

    public EnglishTimeZoneExtractorConfiguration()  {
        this(DateTimeOptions.None);
    }

    public EnglishTimeZoneExtractorConfiguration(DateTimeOptions options) {

        super(options);

        if (options.match(DateTimeOptions.EnablePreview)) {
            LocationMatcher.init(
                    EnglishTimeZone.MajorLocations.stream()
                            .map(o -> QueryProcessor.removeDiacritics(o.toLowerCase()))
                            .collect(Collectors.toCollection(ArrayList::new)));
        }
    }

    protected static StringMatcher buildMatcherFromLists(List<String>...collections) {
        StringMatcher matcher = new StringMatcher(MatchStrategy.TrieTree, new NumberWithUnitTokenizer());
        List<String> matcherList = new ArrayList<String>();

        for (List<String> collection : collections) {
            for (String item : collection) {
                matcherList.add(item.toLowerCase());
            }
        }

        matcherList.stream().forEach(
            item -> {
                if (!matcherList.contains(item)) {
                    matcherList.add(item);
                }
            }
        );
        
        matcher.init(matcherList);

        return matcher;
    }

    @Override
    public Pattern getDirectUtcRegex() {
        return DirectUtcRegex;
    }

    @Override
    public Pattern getLocationTimeSuffixRegex() {
        return LocationTimeSuffixRegex;
    }

    @Override
    public StringMatcher getLocationMatcher() {
        return LocationMatcher;
    }

    @Override
    public StringMatcher getTimeZoneMatcher() {
        return TimeZoneMatcher;
    }

    @Override
    public List<String> getAmbiguousTimezoneList() {
        return AmbiguousTimezoneList;
    }
}
