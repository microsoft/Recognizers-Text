package com.microsoft.recognizers.text.matcher;

import java.util.List;

public interface ITokenizer {
    List<Token> tokenize(String input);
}
