package com.microsoft.recognizers.text.choice.english.extractors;

import com.microsoft.recognizers.text.choice.Constants;
import com.microsoft.recognizers.text.choice.extractors.IBooleanExtractorConfiguration;
import com.microsoft.recognizers.text.choice.resources.EnglishChoice;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.HashMap;
import java.util.Map;
import java.util.regex.Pattern;

public class EnglishBooleanExtractorConfiguration implements IBooleanExtractorConfiguration {
    public static final Pattern  trueRegex = RegExpUtility.getSafeRegExp(EnglishChoice.TrueRegex);
    public static final Pattern falseRegex = RegExpUtility.getSafeRegExp(EnglishChoice.FalseRegex);
    public static final Pattern tokenRegex = RegExpUtility.getSafeRegExp(EnglishChoice.TokenizerRegex);
    public static final Map<Pattern, String> mapRegexes;

    static {
        mapRegexes = new HashMap<Pattern, String>();
        mapRegexes.put(trueRegex, Constants.SYS_BOOLEAN_TRUE);
        mapRegexes.put(falseRegex, Constants.SYS_BOOLEAN_FALSE);
    }

    public boolean allowPartialMatch = false;
    public int maxDistance = 2;
    public boolean onlyTopMatch;

    public EnglishBooleanExtractorConfiguration(boolean topMatch) {
        onlyTopMatch = topMatch;
    }

    public EnglishBooleanExtractorConfiguration() {
        this(true);
    }

    @Override
    public Map<Pattern, String> getMapRegexes() {
        return mapRegexes;
    }

    @Override
    public Pattern getTrueRegex() {
        return trueRegex;
    }

    @Override
    public Pattern getFalseRegex() {
        return falseRegex;
    }

    @Override
    public Pattern getTokenRegex() {
        return tokenRegex;
    }

    @Override
    public boolean getAllowPartialMatch() {
        return allowPartialMatch;
    }

    @Override
    public int getMaxDistance() {
        return maxDistance;
    }

    @Override
    public boolean getOnlyTopMatch() {
        return onlyTopMatch;
    }
}