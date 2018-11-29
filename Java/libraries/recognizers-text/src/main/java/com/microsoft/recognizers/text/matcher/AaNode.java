package com.microsoft.recognizers.text.matcher;

import java.util.HashMap;
import java.util.Iterator;

public class AaNode<T> extends Node<T> {
    T word;
    int depth;
    AaNode<T> parent;
    AaNode<T> fail;

    public AaNode(T word, int depth, AaNode<T> parent) {
        this.word = word;
        this.depth = depth;
        this.parent = parent;
        this.fail = null;
    }

    public AaNode(T word, int depth) {
        this(word, depth, null);
    }

    public AaNode() {
        this(null, 0);
    }

    AaNode<T> get(T c) {
        return children != null && children.containsKey(c) ? (AaNode<T>)children.get(c) : null;
    }

    void put(T c, AaNode<T> value) {
        if (children == null) {
            children = new HashMap<>();
        }

        children.put(c, value);
    }

    @Override
    Iterator getEnumerator() {
        return children != null ? children.values().stream().map(x -> (AaNode<T>)x).iterator() : null;
    }

    @Override
    Iterable getIterable() {
        return  children != null ? (Iterable)children.values().stream().map(x -> (AaNode<T>)x) : null;
    }

    @Override
    public String toString() {
        return word.toString();
    }
}
