package com.microsoft.recognizers.text.choice.extractors;

import java.util.Map;
import java.util.regex.Pattern;

public interface IChoiceExtractorConfiguration {
    public Map<Pattern, String> getMapRegexes();

    public Pattern getTokenRegex();

    public boolean getAllowPartialMatch();

    public int getMaxDistance();

    public boolean getOnlyTopMatch();
}