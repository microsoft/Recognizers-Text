package com.microsoft.recognizers.text.choice.parsers;

public class OptionsOtherMatchParseResult {
	public double score = 0;
	public String text = "";
	public Object value = null;
	public OptionsOtherMatchParseResult(String Text, Object Value, double Score) {
		score = Score;
		text = Text;
		value = Value;
	}
}