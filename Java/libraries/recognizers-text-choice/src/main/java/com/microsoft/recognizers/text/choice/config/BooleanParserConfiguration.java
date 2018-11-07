package com.microsoft.recognizers.text.choice.config;

public class BooleanParserConfiguration extends IChoiceParserConfiguration<boolean>{
	public static Dictionary<String, boolean> Resolutions = new Dictionary <String, boolean>{
		{ Constants.SYS_BOOLEAN_TRUE, true },
		{ Constants.SYS_BOOLEAN_FALSE, false }
	}

	Dictionary<String, boolean> IChoiceParserConfiguration<boolean>.Resolutions => Resolutions;
}