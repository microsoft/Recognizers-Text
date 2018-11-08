package com.microsoft.recognizers.text.choice.config;

import java.util.Dictionary;

public class BooleanParserConfiguration implements IChoiceParserConfiguration<Boolean>{
	public Dictionary<String,Boolean> Resolutions;

	@Override
	public Dictionary<String,Boolean> getResolutions(){
		return this.Resolutions;
	}
}