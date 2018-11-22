package com.microsoft.recognizers.text.datetime.extractors.config;

import com.microsoft.recognizers.text.matcher.MatchResult;

import java.util.List;

public class ProcessedSuperfluousWords {
    public final String text;
    public final Iterable<MatchResult<String>> superfluousWordMatches;

    public ProcessedSuperfluousWords(String text, Iterable<MatchResult<String>> superfluousWordMatches) {
        this.text = text;
        this.superfluousWordMatches = superfluousWordMatches;
    }
}