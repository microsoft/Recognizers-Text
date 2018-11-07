package com.microsoft.recognizers.text.choice.config;

import java.util.Dictionary;

public interface IChoiceParserConfiguration<T>{
	public Dictionary<String, T> getResolutions();
}