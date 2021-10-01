// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.sequence.config;

import com.microsoft.recognizers.text.sequence.SequenceOptions;

import java.util.List;
import java.util.regex.Pattern;

public class PhoneNumberConfiguration implements ISequenceConfiguration {
    private SequenceOptions options;
    private Pattern falsePositivePrefixRegex;
    private String wordBoundariesRegex;
    private String nonWordBoundariesRegex;
    private String endWordBoundariesRegex;
    private Pattern colonPrefixCheckRegex;
    private List<Character> colonMarkers;
    private List<Character> forbiddenPrefixMarkers;
    private List<Character> forbiddenSuffixMarkers;

    public PhoneNumberConfiguration(SequenceOptions options) {
        this.options = options != null ? options : SequenceOptions.None;
    }

    @Override
    public SequenceOptions getOptions() {
        return this.options;
    }

    public Pattern getFalsePositivePrefixRegex() {
        return falsePositivePrefixRegex;
    }

    public void setFalsePositivePrefixRegex(Pattern withFalsePositivePrefixRegex) {
        this.falsePositivePrefixRegex = withFalsePositivePrefixRegex;
    }

    public String getWordBoundariesRegex() {
        return wordBoundariesRegex;
    }

    public void setWordBoundariesRegex(String wordBoundariesRegex) {
        this.wordBoundariesRegex = wordBoundariesRegex;
    }

    public String getNonWordBoundariesRegex() {
        return nonWordBoundariesRegex;
    }

    public void setNonWordBoundariesRegex(String withNonWordBoundariesRegex) {
        this.nonWordBoundariesRegex = withNonWordBoundariesRegex;
    }

    public String getEndWordBoundariesRegex() {
        return endWordBoundariesRegex;
    }

    public void setEndWordBoundariesRegex(String withEndWordBoundariesRegex) {
        this.endWordBoundariesRegex = withEndWordBoundariesRegex;
    }

    public Pattern getColonPrefixCheckRegex() {
        return colonPrefixCheckRegex;
    }

    public void setColonPrefixCheckRegex(Pattern withColonPrefixCheckRegex) {
        this.colonPrefixCheckRegex = withColonPrefixCheckRegex;
    }

    public List<Character> getColonMarkers() {
        return colonMarkers;
    }

    public void setColonMarkers(List<Character> withColonMarkers) {
        this.colonMarkers = withColonMarkers;
    }

    public List<Character> getForbiddenPrefixMarkers() {
        return forbiddenPrefixMarkers;
    }

    public void setForbiddenPrefixMarkers(List<Character> withForbiddenPrefixMarkers) {
        this.forbiddenPrefixMarkers = withForbiddenPrefixMarkers;
    }

    public List<Character> getForbiddenSuffixMarkers() {
        return forbiddenSuffixMarkers;
    }

    public void setForbiddenSuffixMarkers(List<Character> withForbiddenSuffixMarkers) {
        this.forbiddenSuffixMarkers = withForbiddenSuffixMarkers;
    }
}
