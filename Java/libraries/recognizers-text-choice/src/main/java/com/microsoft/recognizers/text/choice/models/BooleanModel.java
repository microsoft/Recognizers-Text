package com.microsoft.recognizers.text.choice.models;

import java.util.Map;

import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.choice.Constants;

public class BooleanModel extends ChoiceModel{
	
	public BooleanModel(IParser parser, IExtractor extractor){
		throw new UnsupportedOperationException();
	}

	public String getModelTypeName(){
		return Constants.MODEL_BOOLEAN;
	}

	protected Map<String, Object> GetResolution(ParseResult parseResult){
		throw new UnsupportedOperationException();
	}
}