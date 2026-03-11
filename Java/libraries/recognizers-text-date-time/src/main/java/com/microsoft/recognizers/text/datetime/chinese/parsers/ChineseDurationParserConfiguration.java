package com.microsoft.recognizers.text.datetime.chinese.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.datetime.chinese.extractors.ChineseDurationExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.config.BaseOptionsConfiguration;
import com.microsoft.recognizers.text.datetime.extractors.BaseDurationExtractor;
import com.microsoft.recognizers.text.datetime.parsers.config.IDurationParserConfiguration;
import com.microsoft.recognizers.text.datetime.resources.ChineseDateTime;
import com.microsoft.recognizers.text.number.chinese.extractors.CardinalExtractor;
import com.microsoft.recognizers.text.number.chinese.parsers.ChineseNumberParserConfiguration;
import com.microsoft.recognizers.text.number.parsers.BaseNumberParser;

import java.util.regex.Pattern;

public class ChineseDurationParserConfiguration extends BaseOptionsConfiguration implements IDurationParserConfiguration {

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

    public ChineseDurationParserConfiguration() {
        this(DateTimeOptions.None);
    }

    public ChineseDurationParserConfiguration(DateTimeOptions options) {
        super(options);

        cardinalExtractor = new CardinalExtractor();
        numberParser = new BaseNumberParser(new ChineseNumberParserConfiguration());
        durationExtractor = new BaseDurationExtractor(new ChineseDurationExtractorConfiguration(options), false);

        numberCombinedWithUnit = ChineseDurationExtractorConfiguration.NumberCombinedWithUnit;
        anUnitRegex = ChineseDurationExtractorConfiguration.AnUnitRegex;
        duringRegex = ChineseDurationExtractorConfiguration.DurationDuringRegex;
        allDateUnitRegex = ChineseDurationExtractorConfiguration.DurationAllRegex;
        halfDateUnitRegex = ChineseDurationExtractorConfiguration.DurationHalfRegex;
        suffixAndRegex = null;
        followedUnit = ChineseDurationExtractorConfiguration.FollowedUnit;
        conjunctionRegex = null;
        inexactNumberRegex = null;
        inexactNumberUnitRegex = null;
        durationUnitRegex = ChineseDurationExtractorConfiguration.DurationUnitRegex;

        unitMap = ChineseDateTime.ParserConfigurationUnitMap;
        unitValueMap = ChineseDateTime.ParserConfigurationUnitValueMap;
        doubleNumbers = ImmutableMap.of();
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
    public Pattern getSpecialNumberUnitRegex() {
        return null;
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
