package com.microsoft.recognizers.text.choice.utilities;
import java.lang.Character;
import java.util.ArrayList;
import java.util.List;
public class UnicodeUtils{
	public static boolean IsEmoji(String letter){
		final int WhereEmojiLive = 0xFFFF; // Supplementary Unicode Plane. This is where emoji live
		return Character.isHighSurrogate(letter.charAt(0)) && (letter.codePointAt(0) > WhereEmojiLive);
	}

	public static List<String> Letters(String text){
		char codePoint = 0;
		List<String> result = new ArrayList<>();
		for (int i = 0; i < text.length(); i++) {
			char c = text.charAt(i);
			if (codePoint != 0){
				result.add(Character.toString(codePoint));
				codePoint = 0;
			} else if (!Character.isHighSurrogate(c)) {
				result.add(Character.toString(c));
			} else {
				codePoint = c;
			}
		}
		return result;
	}
}