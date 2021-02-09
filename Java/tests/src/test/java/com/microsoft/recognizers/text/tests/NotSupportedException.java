package com.microsoft.recognizers.text.tests;

// This Exception represents a extractor/parser/model not yet implemented in Java
public class NotSupportedException extends Exception {

    public NotSupportedException(String message) {
        super(message);
    }

    public NotSupportedException(String message, Exception ex) {
        super(message, ex);
    }
}
