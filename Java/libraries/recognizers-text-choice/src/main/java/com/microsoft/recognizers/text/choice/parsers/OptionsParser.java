package com.microsoft.recognizers.text.choice.parsers;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.choice.config.IChoiceParserConfiguration;

public class OptionsParser<T> implements IParser {

	private IChoiceParserConfiguration<T> config;

	public OptionsParser(IChoiceParserConfiguration<T> config) {
		this.config = config;
	}

	public ParseResult parse(ExtractResult extractResult) {
		throw new UnsupportedOperationException();
	}

	private OptionsOtherMatchParseResult TOptionsOtherMatchReuslt(ExtractResult extractResult) {
		throw new UnsupportedOperationException();
	}
}