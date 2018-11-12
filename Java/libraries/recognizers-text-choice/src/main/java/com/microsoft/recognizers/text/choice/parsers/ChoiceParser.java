package com.microsoft.recognizers.text.choice.parsers;

import java.util.List;
import java.util.Map;
import java.util.stream.Collectors;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.choice.config.IChoiceParserConfiguration;
import com.microsoft.recognizers.text.choice.extractors.ChoiceExtractDataResult;

public class ChoiceParser<T> implements IParser {

	private IChoiceParserConfiguration<T> config;

	public ChoiceParser(IChoiceParserConfiguration<T> config) {
		this.config = config;
	}

	public ParseResult parse(ExtractResult extractResult) {
		ParseResult parseResult = new ParseResult(extractResult);
		ChoiceExtractDataResult data = (ChoiceExtractDataResult) extractResult.data;
		Map<String, Boolean> resolutions = this.config.getResolutions();
		parseResult = parseResult.withValue((boolean) resolutions.get(parseResult.type));
		List<OptionsOtherMatchParseResult> matches = data.otherMatches.stream().map(match-> TOptionsOtherMatchResult(match)).collect(Collectors.toList());
		parseResult = parseResult.withData(new OptionsParseDataResult(data.score, matches));

		return parseResult;
	}

	private OptionsOtherMatchParseResult TOptionsOtherMatchResult(ExtractResult extractResult) {
		ParseResult parseResult = new ParseResult(extractResult);
		ChoiceExtractDataResult data = (ChoiceExtractDataResult) extractResult.data;
		Map<String, Boolean> resolutions = this.config.getResolutions();

		OptionsOtherMatchParseResult result = new OptionsOtherMatchParseResult(parseResult.text, (boolean) resolutions.get(parseResult.type), data.score);

		return result;
	}
}