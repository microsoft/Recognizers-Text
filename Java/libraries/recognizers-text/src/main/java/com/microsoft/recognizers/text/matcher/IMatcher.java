package com.microsoft.recognizers.text.matcher;

import java.util.List;

public interface IMatcher<T> {
    void init(List<List<T>> values, String[] ids);

    Iterable<MatchResult<T>> find(Iterable<T> queryText);

}
