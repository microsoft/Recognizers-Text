package com.microsoft.recognizers.text.choice.extractors;
<<<<<<< HEAD

import java.util.Dictionary;
import java.util.regex.Pattern;

public interface IChoiceExtractorConfiguration {
	public Dictionary<Pattern, String> MapRegexes();

	public Pattern getTokenRegex();

	public boolean getAllowPartialMatch();

	public int getMaxDistance();

=======
import java.util.Map;
import java.util.regex.Pattern;

public interface IChoiceExtractorConfiguration{
	public Map <Pattern, String> getMapRegexes();
	public Pattern getTokenRegex();
	public boolean getAllowPartialMatch();
	public int getMaxDistance();
>>>>>>> WIP port for ChoiceExtractor
	public boolean getOnlyTopMatch();
}