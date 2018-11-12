package com.microsoft.recognizers.text.choice.models;

import java.util.List;
import java.util.Map;

import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IModel;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.ModelResult;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.choice.Constants;

public class ChoiceModel implements IModel {
	protected IExtractor Extractor;
	protected IParser Parser;
	
	public ChoiceModel(IParser parser, IExtractor extractor) {
		this.Parser = parser;
		this.Extractor = extractor;
	}

	@Override
	public String getModelTypeName() {
		return Constants.MODEL_BOOLEAN;
	}

	@Override
	public List<ModelResult> parse(String query) {
		throw new UnsupportedOperationException();
	}

	protected Map<String, Object> GetResolution(ParseResult parseResult) {
		throw new UnsupportedOperationException();
	}
}