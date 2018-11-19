package com.microsoft.recognizers.text.matcher;

import java.util.List;

public abstract class AbstractMatcher<T> implements IMatcher<T> {
    abstract void insert(Iterable<T> value, String id);

    protected void batchInsert(List<List<T>> values, String[] ids) {
        if (values.size() != ids.length) {
            throw new IllegalArgumentException();
        }

        for (int i = 0; i < values.size(); i++) {
            insert(values.get(i), ids[i]);
        }
    }

    boolean isMatch(Iterable<T> queryText) {
        return find(queryText) != null;
    }
}
