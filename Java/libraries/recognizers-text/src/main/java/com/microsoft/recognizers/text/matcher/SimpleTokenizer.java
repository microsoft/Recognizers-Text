package com.microsoft.recognizers.text.matcher;

import com.microsoft.recognizers.text.utilities.StringUtility;

import java.util.ArrayList;
import java.util.List;

public class SimpleTokenizer implements ITokenizer {
    @Override
    public List<Token> tokenize(String input) {
        List<Token> tokens = new ArrayList<>();

        if (StringUtility.isNullOrEmpty(input)) {
            return tokens;
        }

        boolean inToken = false;
        int tokenStart = 0;
        char[] chars = input.toCharArray();
        for (int i = 0; i < chars.length; i++) {
            char c = chars[i];

            if (Character.isWhitespace(c)) {
                if (inToken) {
                    tokens.add(new Token(tokenStart, i - tokenStart, input.substring(tokenStart, i)));
                    inToken = false;
                }
            } else if (!Character.isLetterOrDigit(c) || isCjk(c)) {
                if (inToken) {
                    tokens.add(new Token(tokenStart, i - tokenStart, input.substring(tokenStart, i)));
                    inToken = false;
                }

                tokens.add(new Token(i, 1, input.substring(i, i + 1)));
            } else {
                if (!inToken) {
                    tokenStart = i;
                    inToken = true;
                }
            }
        }

        if (inToken) {
            tokens.add(new Token(tokenStart, chars.length - tokenStart, input.substring(tokenStart)));
        }

        return tokens;
    }

    protected boolean isChinese(char c) {
        int uc = (int)c;

        return (uc >= (int)0x4E00 && uc <= (int)0x9FBF) || (uc >= (int)0x3400 && uc <= (int)0x4DBF);
    }

    protected boolean isJapanese(char c) {
        int uc = (int)c;

        return (uc >= 0x3040 && uc <= 0x309F) ||
            (uc >= 0x30A0 && uc <= (int)0x30FF) ||
            (uc >= (int)0xFF66 && uc <= (int)0xFF9D);
    }

    protected boolean isKorean(char c) {
        int uc = (int)c;

        return (uc >= (int)0xAC00 && uc <= (int)0xD7AF) ||
            (uc >= (int)0x1100 && uc <= (int)0x11FF) ||
            (uc >= (int)0x3130 && uc <= (int)0x318F) ||
            (uc >= (int)0xFFB0 && uc <= (int)0xFFDC);
    }

    // Check the character is Chinese/Japanese/Korean.
    // For those languages which are not using whitespace delimited symbol, we only simply tokenize the sentence by each single character.
    private boolean isCjk(char c) {
        return isChinese(c) || isJapanese(c) || isKorean(c);
    }
}
