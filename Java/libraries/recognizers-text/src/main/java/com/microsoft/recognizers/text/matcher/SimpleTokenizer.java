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

    // Check the character is Chinese/Japanese/Korean.
    // For those languages which are not using whitespace delimited symbol, we only simply tokenize the sentence by each single character.
    private boolean isCjk(char c) {
        return Character.isIdeographic((int)c);
    }
}
