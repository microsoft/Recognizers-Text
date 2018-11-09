package com.microsoft.recognizers.text.matcher;

public class Token {
    private final int start;
    private final int length;

    String text;

    public Token(int start, int length, String text) {
        this.start = start;
        this.length = length;
        this.text = text;
    }

    public Token(int start, int length) {
        this(start, length, null);
    }

    int getStart() {
        return start;
    }

    int getLength() {
        return length;
    }

    int getEnd() {
        return start + length;
    }
}
