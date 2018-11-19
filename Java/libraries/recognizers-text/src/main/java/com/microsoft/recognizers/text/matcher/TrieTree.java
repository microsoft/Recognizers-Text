package com.microsoft.recognizers.text.matcher;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Iterator;
import java.util.List;

public class TrieTree<T> extends AbstractMatcher<T> {
    protected final Node<T> root;

    public TrieTree() {
        root = new Node<>();
    }

    @Override
    void insert(Iterable<T> value, String id) {
        Node<T> node = root;

        for (T item : value) {
            Node<T> child = node.get(item);

            if (child == null) {
                node.put(item, new Node<>());
                child = node.get(item);
            }

            node = child;
        }

        node.addValue(id);
    }

    @Override
    public void init(List<List<T>> values, String[] ids) {
        batchInsert(values, ids);
    }

    @Override
    public Iterable<MatchResult<T>> find(Iterable<T> queryText) {
        List<MatchResult<T>> result = new ArrayList<>();

        ArrayList<T> queryArray = new ArrayList<>();
        queryText.iterator().forEachRemaining(queryArray::add);

        for (int i = 0; i < queryArray.size(); i++) {
            Node<T> node = root;
            for (int j = i; j <= queryArray.size(); j++) {
                if (node.getEnd()) {
                    result.add(new MatchResult<>(i, j - i, node.values));
                }

                if (j == queryArray.size()) {
                    break;
                }

                T text = queryArray.get(j);
                if (node.get(text) == null) {
                    break;
                }

                node = node.get(text);
            }
        }

        return  result;
    }
}
