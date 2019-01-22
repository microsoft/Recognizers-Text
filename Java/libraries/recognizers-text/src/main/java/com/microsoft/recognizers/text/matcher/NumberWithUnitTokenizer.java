package com.microsoft.recognizers.text.matcher;

import com.microsoft.recognizers.text.utilities.StringUtility;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashSet;
import java.util.List;

public class NumberWithUnitTokenizer extends SimpleTokenizer {
    private static final HashSet<Character> specialTokenCharacters = new HashSet<Character>(Arrays.asList('$')); 

    /* The main difference between this strategy and SimpleTokenizer is for cases like
     * 'Bob's $ 100 cash'. 's' and '$' are independent tokens in SimpleTokenizer.
     * However, 's$' will return these two tokens too. Here, we let 's$' be a single
     * token to avoid the false positive.
     * Besides, letters and digits can't be mixed as a token. For cases like '200ml'.
     * '200ml' will be a token in SimpleTokenizer. Here, '200' and 'ml' are independent tokens.
     */
    
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
            } else if ((!specialTokenCharacters.contains(c) && !Character.isLetterOrDigit(c)) || isChinese(c) || isJapanese(c)) {
                // Non-splittable currency units (as "$") are treated as regular letters. For instance, 'us$' should be a single token
                if (inToken) {
                    tokens.add(new Token(tokenStart, i - tokenStart, input.substring(tokenStart, i)));
                    inToken = false;
                }

                tokens.add(new Token(i, 1, input.substring(i, i + 1)));
            } else {
                if (inToken && i > 0) {
                    char preChar = chars[i - 1];
                    if (isSplittableUnit(c, preChar)) {
                        // Split if letters or non-splittable units are adjacent with digits.
                        tokens.add(new Token(tokenStart, i - tokenStart, input.substring(tokenStart, i)));
                        tokenStart = i;
                    }
                }

                if (!inToken) {
                    tokenStart = i;
                    inToken = true;
                }
            }
        }

        if (inToken) {
            tokens.add(new Token(tokenStart, chars.length - tokenStart, input.substring(tokenStart, chars.length)));
        }

        return tokens;
    }

    private boolean isSplittableUnit(char curChar, char preChar) {
        // To handle cases like '200ml', digits and letters cannot be mixed as a single token. '200ml' will be tokenized to '200' and 'ml'.
        if ((Character.isLetter(curChar) && Character.isDigit(preChar)) || (Character.isDigit(curChar) && Character.isLetter(preChar))) {
            return true;
        }

        // Non-splittable currency units can't be mixed with digits. For example, '$100' or '100$' will be tokenized to '$' and '100',
        // '1$50' will be tokenized to '1', '$', and '50'
        if ((Character.isDigit(curChar) && specialTokenCharacters.contains(preChar)) || (specialTokenCharacters.contains(curChar) && Character.isDigit(preChar))) {
            return true;
        }

        // Non-splittable currency units adjacent with letters are treated as regular token characters. For instance, 's$' or 'u$d' are single tokens.
        return false;
    }
}
