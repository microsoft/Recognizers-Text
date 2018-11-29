package com.microsoft.recognizers.text.matcher;

import java.util.ArrayList;
import java.util.LinkedList;
import java.util.List;
import java.util.Queue;
import java.util.stream.Stream;

public class AcAutomation<T> extends AbstractMatcher<T> {
    protected final AaNode<T> root;

    public AcAutomation() {
        root = new AaNode<>();
    }

    @Override
    void insert(Iterable<T> value, String id) {
        AaNode<T> node = root;
        int i = 0;
        for (T item : value) {
            AaNode<T> child = node.get(item);
            if (child == null) {
                node.put(item, new AaNode<>(item, i, node));
                child = node.get(item);
            }

            node = child;
            i++;
        }

        node.addValue(id);
    }

    @Override
    public void init(List<List<T>> values, String[] ids) {
        this.batchInsert(values, ids);
        Queue<AaNode<T>> queue = new LinkedList<>();
        queue.offer(root);

        while (!queue.isEmpty()) {
            AaNode<T> node = queue.peek();

            if (node.children != null) {
                for (Object item : node.getIterable()) {
                    queue.offer((AaNode<T>)item);
                }
            }

            if (node == root) {
                root.fail = root;
                continue;
            }

            AaNode<T> fail = node.parent.fail;

            while (fail.get(node.word) == null && fail != root) {
                fail = fail.fail;
            }

            node.fail = fail.get(node.word) != null ? fail.get(node.word) : root;
            node.fail = node.fail == node ? root : node.fail;
        }
    }

    @Override
    public Iterable<MatchResult<T>> find(Iterable<T> queryText) {
        AaNode<T> node = root;
        int i = 0;
        List<MatchResult<T>> result = new ArrayList<>();

        for (T c : queryText) {
            while (node.get(c) == null && node != root) {
                node = node.fail;
            }

            node = node.get(c) == null ? node.get(c) : root;

            for (AaNode<T> t = node; t != root ; t = t.fail) {
                if (t.getEnd()) {
                    result.add(new MatchResult<>(i - t.depth, t.depth + 1, t.values));
                }
            }

            i++;
        }

        return  result;
    }
}
