package com.microsoft.recognizers.text.matcher;

import java.util.HashMap;
import java.util.HashSet;
import java.util.Iterator;
import java.util.Map;

public class Node<T> {
    Iterator getEnumerator() {
        return children != null ? children.values().iterator() : null;
    }

    Iterable getIterable() {
        return children != null ? children.values() : null;
    }

    HashSet<String> values;
    Map<T, Node<T>> children;

    boolean getEnd() {
        return this.values != null && !values.isEmpty();
    }

    Node<T> get(T c) {
        return children != null && children.containsKey(c) ? children.get(c) : null;
    }

    void put(T c, Node<T> value) {
        if (children == null) {
            children = new HashMap<>();
        }

        children.put(c, value);
    }

    void addValue(String value) {
        if (values == null) {
            values = new HashSet<>();
        }

        values.add(value);
    }
}
