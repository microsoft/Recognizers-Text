package com.microsoft.recognizers.text.datetime.spanish.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.BaseTimeZoneParser;
import com.microsoft.recognizers.text.datetime.parsers.IDateTimeParser;
import com.microsoft.recognizers.text.datetime.parsers.config.ICommonDateTimeParserConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.config.ITimeParserConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.config.PrefixAdjustResult;
import com.microsoft.recognizers.text.datetime.parsers.config.SuffixAdjustResult;
import com.microsoft.recognizers.text.datetime.resources.SpanishDateTime;
import com.microsoft.recognizers.text.datetime.spanish.extractors.SpanishTimeExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.utilities.ConditionalMatch;
import com.microsoft.recognizers.text.datetime.utilities.IDateTimeUtilityConfiguration;
import com.microsoft.recognizers.text.datetime.utilities.RegexExtension;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import com.microsoft.recognizers.text.utilities.StringUtility;

import java.util.Arrays;
import java.util.Optional;
import java.util.regex.Pattern;

public class SpanishTimeParserConfiguration extends BaseOptionsConfiguration implements ITimeParserConfiguration {

    public String timeTokenPrefix = SpanishDateTime.TimeTokenPrefix;

    public final Pattern atRegex;
    public Pattern mealTimeRegex;

    private final Iterable<Pattern> timeRegexes;
    private final ImmutableMap<String, Integer> numbers;
    private final IDateTimeUtilityConfiguration utilityConfiguration;
    private final IDateTimeParser timeZoneParser;

    public SpanishTimeParserConfiguration(ICommonDateTimeParserConfiguration config) {

        super(config.getOptions());

        numbers = config.getNumbers();
        utilityConfiguration = config.getUtilityConfiguration();
        timeZoneParser = new BaseTimeZoneParser();

        atRegex = SpanishTimeExtractorConfiguration.AtRegex;
        timeRegexes = SpanishTimeExtractorConfiguration.TimeRegexList;
    }

    @Override
    public String getTimeTokenPrefix() {
        return timeTokenPrefix;
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

        int deltaMin = 0;
        String trimmedPrefix = prefix.trim().toLowerCase();

        if (trimmedPrefix.startsWith("cuarto") || trimmedPrefix.startsWith("y cuarto")) {
            deltaMin = 15;
        } else if (trimmedPrefix.startsWith("menos cuarto")) {
            deltaMin = -15;
        } else if (trimmedPrefix.startsWith("media") || trimmedPrefix.startsWith("y media")) {
            deltaMin = 30;
        } else {
            Optional<Match> match = Arrays.stream(RegExpUtility.getMatches(SpanishTimeExtractorConfiguration.LessThanOneHour, trimmedPrefix)).findFirst();
            if (match.isPresent()) {
                String minStr = match.get().getGroup("deltamin").value;
                if (!StringUtility.isNullOrWhiteSpace(minStr)) {
                    deltaMin = Integer.parseInt(minStr);
                } else {
                    minStr = match.get().getGroup("deltaminnum").value.toLowerCase();
                    deltaMin = numbers.getOrDefault(minStr, 0);
                }
            }
        }

        if (trimmedPrefix.endsWith("pasadas") || trimmedPrefix.endsWith("pasados") || trimmedPrefix.endsWith("pasadas las") ||
            trimmedPrefix.endsWith("pasados las") || trimmedPrefix.endsWith("pasadas de las") || trimmedPrefix.endsWith("pasados de las")) {
            //deltaMin is positive
        } else if (trimmedPrefix.endsWith("para la") || trimmedPrefix.endsWith("para las") ||
            trimmedPrefix.endsWith("antes de la") || trimmedPrefix.endsWith("antes de las")) {
            deltaMin = -deltaMin;
        }

        min += deltaMin;
        if (min < 0) {
            min += 60;
            hour -= 1;
        }

        hasMin = hasMin || (min != 0);

        return new PrefixAdjustResult(hour, min, hasMin);
    }

    @Override
    public SuffixAdjustResult adjustBySuffix(String suffix, int hour, int min, boolean hasMin, boolean hasAm, boolean hasPm) {

        String trimmedSuffix = suffix.trim().toLowerCase();
        PrefixAdjustResult prefixAdjustResult = adjustByPrefix(trimmedSuffix,hour, min, hasMin);
        hour = prefixAdjustResult.hour;
        min = prefixAdjustResult.minute;
        hasMin = prefixAdjustResult.hasMin;

        int deltaHour = 0;
        ConditionalMatch match = RegexExtension.matchExact(SpanishTimeExtractorConfiguration.TimeSuffix, trimmedSuffix, true);
        if (match.getSuccess()) {

            String oclockStr = match.getMatch().get().getGroup("oclock").value;
            if (StringUtility.isNullOrEmpty(oclockStr)) {

                String amStr = match.getMatch().get().getGroup(Constants.AmGroupName).value;
                if (!StringUtility.isNullOrEmpty(amStr)) {
                    if (hour >= Constants.HalfDayHourCount) {
                        deltaHour = -Constants.HalfDayHourCount;
                    }
                    hasAm = true;
                }

                String pmStr = match.getMatch().get().getGroup(Constants.PmGroupName).value;
                if (!StringUtility.isNullOrEmpty(pmStr)) {
                    if (hour < Constants.HalfDayHourCount) {
                        deltaHour = Constants.HalfDayHourCount;
                    }
                    hasPm = true;
                }
            }
        }

        hour = (hour + deltaHour) % 24;

        return new SuffixAdjustResult(hour, min, hasMin, hasAm, hasPm);
    }
}
