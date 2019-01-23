package com.microsoft.recognizers.text.datetime.english.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.english.extractors.EnglishTimeExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.BaseTimeZoneParser;
import com.microsoft.recognizers.text.datetime.parsers.IDateTimeParser;
import com.microsoft.recognizers.text.datetime.parsers.config.ICommonDateTimeParserConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.config.ITimeParserConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.config.PrefixAdjustResult;
import com.microsoft.recognizers.text.datetime.parsers.config.SuffixAdjustResult;
import com.microsoft.recognizers.text.datetime.resources.EnglishDateTime;
import com.microsoft.recognizers.text.datetime.utilities.ConditionalMatch;
import com.microsoft.recognizers.text.datetime.utilities.IDateTimeUtilityConfiguration;
import com.microsoft.recognizers.text.datetime.utilities.RegexExtension;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import com.microsoft.recognizers.text.utilities.StringUtility;

import java.util.Arrays;
import java.util.Optional;
import java.util.regex.Pattern;

public class EnglishTimeParserConfiguration extends BaseOptionsConfiguration implements ITimeParserConfiguration {
    
    private final ImmutableMap<String, Integer> numbers;
    private final IDateTimeUtilityConfiguration utilityConfiguration;
    private final IDateTimeParser timeZoneParser;

    private final Pattern atRegex;
    private final Iterable<Pattern> timeRegexes;
    private final Pattern timeSuffixFull = RegExpUtility.getSafeRegExp(EnglishDateTime.TimeSuffixFull);
    private final Pattern lunchRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.LunchRegex);
    private final Pattern nightRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.NightRegex);

    public EnglishTimeParserConfiguration(ICommonDateTimeParserConfiguration config) {
        
        super(config.getOptions());
        
        numbers = config.getNumbers();
        utilityConfiguration = config.getUtilityConfiguration();
        timeZoneParser = new BaseTimeZoneParser();

        atRegex = EnglishTimeExtractorConfiguration.AtRegex;
        timeRegexes = EnglishTimeExtractorConfiguration.TimeRegexList;
    }

    @Override
    public String getTimeTokenPrefix() {
        return EnglishDateTime.TimeTokenPrefix;
    }

    @Override
    public Pattern getAtRegex() {
        return atRegex;
    }

    @Override
    public Iterable<Pattern> getTimeRegexes() {
        return timeRegexes;
    }

    @Override
    public ImmutableMap<String, Integer> getNumbers() {
        return numbers;
    }

    @Override
    public IDateTimeUtilityConfiguration getUtilityConfiguration() {
        return utilityConfiguration;
    }

    @Override
    public IDateTimeParser getTimeZoneParser() {
        return timeZoneParser;
    }

    @Override
    public PrefixAdjustResult adjustByPrefix(String prefix, int hour, int min, boolean hasMin) {
        
        int deltaMin;
        String trimmedPrefix = prefix.trim().toLowerCase();

        if (trimmedPrefix.startsWith("half")) {
            deltaMin = 30;
        } else if (trimmedPrefix.startsWith("a quarter") || trimmedPrefix.startsWith("quarter")) {
            deltaMin = 15;
        } else if (trimmedPrefix.startsWith("three quarter")) {
            deltaMin = 45;
        } else {
            
            Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(EnglishTimeExtractorConfiguration.LessThanOneHour, trimmedPrefix)).findFirst();
            String minStr = match.get().getGroup("deltamin").value;
            if (!StringUtility.isNullOrWhiteSpace(minStr)) {
                deltaMin = Integer.parseInt(minStr);
            } else {
                minStr = match.get().getGroup("deltaminnum").value;
                deltaMin = numbers.getOrDefault(minStr, 0);
            }
        }

        if (trimmedPrefix.endsWith("to")) {
            deltaMin = -deltaMin;
        }

        min += deltaMin;
        if (min < 0) {
            min += 60;
            hour -= 1;
        }

        hasMin = true;

        return new PrefixAdjustResult(hour, min, hasMin);
    }

    @Override
    public SuffixAdjustResult adjustBySuffix(String suffix, int hour, int min, boolean hasMin, boolean hasAm, boolean hasPm) {
        
        String lowerSuffix = suffix.toLowerCase();
        int deltaHour = 0;
        ConditionalMatch match = RegexExtension.matchExact(timeSuffixFull, lowerSuffix, true);
        if (match.getSuccess()) {
            
            String oclockStr = match.getMatch().get().getGroup("oclock").value;
            if (StringUtility.isNullOrEmpty(oclockStr)) {
                
                String amStr = match.getMatch().get().getGroup(Constants.AmGroupName).value;
                if (!StringUtility.isNullOrEmpty(amStr)) {
                    if (hour >= Constants.HalfDayHourCount) {
                        deltaHour = -Constants.HalfDayHourCount;
                    } else {
                        hasAm = true;
                    }
                    
                }

                String pmStr = match.getMatch().get().getGroup(Constants.PmGroupName).value;
                if (!StringUtility.isNullOrEmpty(pmStr)) {
                    if (hour < Constants.HalfDayHourCount) {
                        deltaHour = Constants.HalfDayHourCount;
                    }
                    
                    if (checkMatch(lunchRegex, pmStr)) {
                        // for hour >= 10, < 12
                        if (hour >= 10 && hour <= Constants.HalfDayHourCount) {
                            deltaHour = 0;
                            if (hour == Constants.HalfDayHourCount) {
                                hasPm = true;
                            } else {
                                hasAm = true;
                            }

                        } else {
                            hasPm = true;
                        }

                    } else if (checkMatch(nightRegex, pmStr)) {
                        //For hour <= 3 or == 12, we treat it as am, for example 1 in the night (midnight) == 1am
                        if (hour <= 3 || hour == Constants.HalfDayHourCount) {
                            if (hour == Constants.HalfDayHourCount) {
                                hour = 0;
                            }

                            deltaHour = 0;
                            hasAm = true;
                        } else {
                            hasPm = true;
                        }

                    } else {
                        hasPm = true;
                    }
                }
            }
        }

        hour = (hour + deltaHour) % 24;

        return new SuffixAdjustResult(hour, min, hasMin, hasAm, hasPm);
    }
    
    private boolean checkMatch(Pattern regex, String input) {
        return RegExpUtility.getMatches(regex, input).length > 0;
    }
}
