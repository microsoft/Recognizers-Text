package com.microsoft.recognizers.text.matcher;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.Map;
import java.util.stream.Collectors;
import java.util.stream.StreamSupport;

public class StringMatcher {
    private final ITokenizer tokenizer;
    private final IMatcher<String> matcher;

    public StringMatcher(MatchStrategy strategy, ITokenizer tokenizer) {
        this.tokenizer = tokenizer != null ? tokenizer : new SimpleTokenizer();
        switch (strategy) {
            case AcAutomaton:
                matcher = new AcAutomation<>();
                break;
            case TrieTree:
                matcher = new TrieTree<>();
                break;
            default:
                throw new IllegalArgumentException();
        }
    }

    public StringMatcher(MatchStrategy strategy) {
        this(strategy, null);
    }

    public StringMatcher(ITokenizer tokenizer) {
        this(MatchStrategy.TrieTree, tokenizer);
    }

    public StringMatcher() {
        this(MatchStrategy.TrieTree, null);
    }

    public void init(Iterable<String> values) {
        init(values, StreamSupport.stream(values.spliterator(), false).toArray(size -> new String[size]));
    }

    void init(Iterable<String> values, String[] ids) {
        List<List<String>> tokenizedValues = getTokenizedText(values);
        init(tokenizedValues, ids);
    }

    void init(Map<String, List<String>> valuesMap) {
        ArrayList<String> values = new ArrayList<>();
        ArrayList<String> ids = new ArrayList<>();

        for (Map.Entry<String, List<String>> item : valuesMap.entrySet()) {
            String id = item.getKey();
            for (String value : item.getValue()) {
                values.add(value);
                ids.add(id);
            }
        }

        List<List<String>> tokenizedValues = getTokenizedText(values);
        init(tokenizedValues, ids.toArray(new String[0]));
    }

    void init(List<List<String>> tokenizedValues, String[] ids) {
        matcher.init(tokenizedValues, ids);
    }

    private List<List<String>> getTokenizedText(Iterable<String> values) {
        List<List<String>> result = new ArrayList<>();
        for (String value: values) {
            result.add(tokenizer.tokenize(value).stream().map(i -> i.text).collect(Collectors.toCollection(ArrayList::new)));
        }

        return result;
    }

    public Iterable<MatchResult<String>> find(Iterable<String> tokenizedQuery) {
        return matcher.find(tokenizedQuery);
    }

    public Iterable<MatchResult<String>> find(String queryText) {
        List<Token> queryTokens = tokenizer.tokenize(queryText);
        Iterable<String> tokenizedQueryText = queryTokens.stream().map(t -> t.text).collect(Collectors.toCollection(ArrayList::new));

        List<MatchResult<String>> result = new ArrayList<>();
        for (MatchResult<String> r : find(tokenizedQueryText)) {
            Token startToken = queryTokens.get(r.getStart());
            Token endToken = queryTokens.get(r.getStart() + r.getLength() - 1);
            int start = startToken.getStart();
            int length = endToken.getEnd() - startToken.getStart();
            String rtext = queryText.substring(start, start + length);

            result.add(new MatchResult<String>(start, length, r.getCanonicalValues(), rtext));
        }

        return result;
    }
}
