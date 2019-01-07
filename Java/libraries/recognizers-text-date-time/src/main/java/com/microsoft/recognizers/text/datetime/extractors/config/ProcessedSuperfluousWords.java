package com.microsoft.recognizers.text.datetime.extractors.config;

import com.microsoft.recognizers.text.matcher.MatchResult;

import java.util.List;

public class ProcessedSuperfluousWords {
    private String text;
    private Iterable<MatchResult<String>> superfluousWordMatches;

    public ProcessedSuperfluousWords(String text, Iterable<MatchResult<String>> superfluousWordMatches) {
        this.text = text;
        this.superfluousWordMatches = superfluousWordMatches;
    }

    public String getText() {
        return text;
    }

    public Iterable<MatchResult<String>> getSuperfluousWordMatches() {
        return superfluousWordMatches;
    }
}