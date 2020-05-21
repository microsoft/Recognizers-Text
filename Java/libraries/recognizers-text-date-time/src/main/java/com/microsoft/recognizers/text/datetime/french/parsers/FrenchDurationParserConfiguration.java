package com.microsoft.recognizers.text.datetime.french.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.BaseDurationExtractor;
import com.microsoft.recognizers.text.datetime.french.extractors.FrenchDurationExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.config.ICommonDateTimeParserConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.config.IDurationParserConfiguration;
import java.util.regex.Pattern;

public class FrenchDurationParserConfiguration extends BaseOptionsConfiguration implements IDurationParserConfiguration {

    private final IExtractor cardinalExtractor;
    private final IExtractor durationExtractor;
    private final IParser numberParser;

    private final Pattern numberCombinedWithUnit;
    private final Pattern anUnitRegex;
    private final Pattern duringRegex;
    private final Pattern allDateUnitRegex;
    private final Pattern halfDateUnitRegex;
    private final Pattern suffixAndRegex;
    private final Pattern followedUnit;
    private final Pattern conjunctionRegex;
    private final Pattern inexactNumberRegex;
    private final Pattern inexactNumberUnitRegex;
    private final Pattern durationUnitRegex;

    private final ImmutableMap<String, String> unitMap;
    private final ImmutableMap<String, Long> unitValueMap;
    private final ImmutableMap<String, Double> doubleNumbers;

    public FrenchDurationParserConfiguration(final ICommonDateTimeParserConfiguration config) {

        super(config.getOptions());

        cardinalExtractor = config.getCardinalExtractor();
        numberParser = config.getNumberParser();
        durationExtractor = new BaseDurationExtractor(new FrenchDurationExtractorConfiguration(), false);
        numberCombinedWithUnit = FrenchDurationExtractorConfiguration.NumberCombinedWithUnit;

        anUnitRegex = FrenchDurationExtractorConfiguration.AnUnitRegex;
        duringRegex = FrenchDurationExtractorConfiguration.DuringRegex;
        allDateUnitRegex = FrenchDurationExtractorConfiguration.AllRegex;
        halfDateUnitRegex = FrenchDurationExtractorConfiguration.HalfRegex;
        suffixAndRegex = FrenchDurationExtractorConfiguration.SuffixAndRegex;
        followedUnit = FrenchDurationExtractorConfiguration.FollowedUnit;
        conjunctionRegex = FrenchDurationExtractorConfiguration.ConjunctionRegex;
        inexactNumberRegex = FrenchDurationExtractorConfiguration.InexactNumberRegex;
        inexactNumberUnitRegex = FrenchDurationExtractorConfiguration.InexactNumberUnitRegex;
        durationUnitRegex = FrenchDurationExtractorConfiguration.DurationUnitRegex;

        unitMap = config.getUnitMap();
        unitValueMap = config.getUnitValueMap();
        doubleNumbers = config.getDoubleNumbers();
    }

    @Override
    public IExtractor getCardinalExtractor() {
        return cardinalExtractor;
    }

    @Override
    public IExtractor getDurationExtractor() {
        return durationExtractor;
    }

    @Override
    public IParser getNumberParser() {
        return numberParser;
    }

    @Override
    public Pattern getNumberCombinedWithUnit() {
        return numberCombinedWithUnit;
    }

    @Override
    public Pattern getAnUnitRegex() {
        return anUnitRegex;
    }

    @Override
    public Pattern getDuringRegex() {
        return duringRegex;
    }

    @Override
    public Pattern getAllDateUnitRegex() {
        return allDateUnitRegex;
    }

    @Override
    public Pattern getHalfDateUnitRegex() {
        return halfDateUnitRegex;
    }

    @Override
    public Pattern getSuffixAndRegex() {
        return suffixAndRegex;
    }

    @Override
    public Pattern getFollowedUnit() {
        return followedUnit;
    }

    @Override
    public Pattern getConjunctionRegex() {
        return conjunctionRegex;
    }

    @Override
    public Pattern getInexactNumberRegex() {
        return inexactNumberRegex;
    }

    @Override
    public Pattern getInexactNumberUnitRegex() {
        return inexactNumberUnitRegex;
    }

    @Override
    public Pattern getDurationUnitRegex() {
        return durationUnitRegex;
    }

    @Override
    public ImmutableMap<String, String> getUnitMap() {
        return unitMap;
    }

    @Override
    public ImmutableMap<String, Long> getUnitValueMap() {
        return unitValueMap;
    }

    @Override
    public ImmutableMap<String, Double> getDoubleNumbers() {
        return doubleNumbers;
    }
}
