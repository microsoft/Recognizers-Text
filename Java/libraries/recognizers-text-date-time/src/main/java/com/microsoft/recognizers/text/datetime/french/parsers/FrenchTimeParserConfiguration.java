package com.microsoft.recognizers.text.datetime.french.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.french.extractors.FrenchTimeExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.BaseTimeZoneParser;
import com.microsoft.recognizers.text.datetime.parsers.IDateTimeParser;
import com.microsoft.recognizers.text.datetime.parsers.config.ICommonDateTimeParserConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.config.ITimeParserConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.config.PrefixAdjustResult;
import com.microsoft.recognizers.text.datetime.parsers.config.SuffixAdjustResult;
import com.microsoft.recognizers.text.datetime.resources.FrenchDateTime;
import com.microsoft.recognizers.text.datetime.utilities.ConditionalMatch;
import com.microsoft.recognizers.text.datetime.utilities.IDateTimeUtilityConfiguration;
import com.microsoft.recognizers.text.datetime.utilities.RegexExtension;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import com.microsoft.recognizers.text.utilities.StringUtility;
import java.util.regex.Pattern;

public class FrenchTimeParserConfiguration extends BaseOptionsConfiguration implements ITimeParserConfiguration {

    public final Pattern atRegex;
    private final Iterable<Pattern> timeRegexes;
    private final ImmutableMap<String, Integer> numbers;
    private final IDateTimeUtilityConfiguration utilityConfiguration;
    private final IDateTimeParser timeZoneParser;
    public String timeTokenPrefix = FrenchDateTime.TimeTokenPrefix;
    public Pattern mealTimeRegex;

    public FrenchTimeParserConfiguration(final ICommonDateTimeParserConfiguration config) {

        super(config.getOptions());

        numbers = config.getNumbers();
        utilityConfiguration = config.getUtilityConfiguration();
        timeZoneParser = new BaseTimeZoneParser();

        atRegex = FrenchTimeExtractorConfiguration.AtRegex;
        timeRegexes = FrenchTimeExtractorConfiguration.TimeRegexList;
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
    public PrefixAdjustResult adjustByPrefix(final String prefix, int hour, int min, final boolean hasMin) {

        int deltaMin = 0;
        final String trimmedPrefix = prefix.trim();

        // c'este 8 heures et demie, - "it's half past 8"
        if (trimmedPrefix.endsWith("demie")) {
            deltaMin = 30;
        } else if (trimmedPrefix.endsWith("un quart") || trimmedPrefix.endsWith("quart")) {
            deltaMin = 15;
        } else if (trimmedPrefix.endsWith("trois quarts")) {
            deltaMin = 45;
        } else {
            final Match[] match = RegExpUtility
                .getMatches(FrenchTimeExtractorConfiguration.LessThanOneHour, trimmedPrefix);
            String minStr;
            if (match.length > 0) {
                minStr = match[0].getGroup("deltamin").value;
                if (!StringUtility.isNullOrEmpty(minStr)) {
                    deltaMin = Integer.parseInt(minStr);
                } else {
                    minStr = match[0].getGroup("deltaminnum").value;
                    deltaMin = numbers.get(minStr);
                }
            }

        }

        // 'to' i.e 'one to five' = 'un à cinq'
        if (trimmedPrefix.endsWith("à")) {
            deltaMin = -deltaMin;
        }

        min += deltaMin;
        if (min < 0) {
            min += 60;
            hour -= 1;
        }

        return new PrefixAdjustResult(hour, min, true);
    }

    @Override
    public SuffixAdjustResult adjustBySuffix(final String suffix,
                                             int hour,
                                             final int min,
                                             final boolean hasMin,
                                             boolean hasAm,
                                             boolean hasPm) {

        int deltaHour = 0;
        final ConditionalMatch match = RegexExtension
            .matchExact(FrenchTimeExtractorConfiguration.TimeSuffix, suffix, true);

        if (match.getSuccess()) {
            final String oclockStr = match.getMatch().get().getGroup("heures").value;
            if (StringUtility.isNullOrEmpty(oclockStr)) {
                final String matchAmStr = match.getMatch().get().getGroup(Constants.AmGroupName).value;
                if (!StringUtility.isNullOrEmpty(matchAmStr)) {
                    if (hour >= Constants.HalfDayHourCount) {
                        deltaHour = -Constants.HalfDayHourCount;
                    }

                    hasAm = true;
                }

                final String matchPmStr = match.getMatch().get().getGroup(Constants.PmGroupName).value;
                if (!StringUtility.isNullOrEmpty(matchPmStr)) {
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
