package com.microsoft.recognizers.text.choice.extractors;

import java.util.Dictionary;
import java.util.regex.Pattern;

public interface IChoiceExtractorConfiguration {
	public Dictionary<Pattern, String> MapRegexes();

	public Pattern getTokenRegex();

	public boolean getAllowPartialMatch();

	public int getMaxDistance();

	public boolean getOnlyTopMatch();
}