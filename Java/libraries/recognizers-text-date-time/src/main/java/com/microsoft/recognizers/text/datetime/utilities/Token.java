package com.microsoft.recognizers.text.datetime.utilities;

import com.microsoft.recognizers.text.ExtractResult;

import java.util.ArrayList;
import java.util.Collections;
import java.util.Comparator;
import java.util.List;

public class Token {
    private final int start;
    private final int end;

    public Token(int start, int end) {
        this.start = start;
        this.end = end;
    }

    public int getStart() {
        return start;
    }

    public int getEnd() {
        return end;
    }

    public int getLength() {
        return end < start ? 0 : end - start;
    }

    public static List<ExtractResult> mergeAllTokens(List<Token> tokens, String text, String extractorName) {
        List<ExtractResult> result = new ArrayList<>();
        List<Token> mergedTokens = new ArrayList<>();

        tokens.sort((o1, o2) -> {
            if (o1.start != o2.start) {
                return o1.start - o2.start;
            }

            return o2.getLength() - o1.getLength();
        });

        for (Token token : tokens) {
            if (token != null) {
                boolean bAdd = true;
                for (int i = 0; i < mergedTokens.size() && bAdd; i++) {
                    // It is included in one of the current tokens
                    if (token.start >= mergedTokens.get(i).start && token.end <= mergedTokens.get(i).end) {
                        bAdd = false;
                    }

                    // If it contains overlaps
                    if (token.start > mergedTokens.get(i).start && token.start < mergedTokens.get(i).end) {
                        bAdd = false;
                    }

                    // It includes one of the tokens and should replace the included one
                    if (token.start <= mergedTokens.get(i).start && token.end >= mergedTokens.get(i).end) {
                        bAdd = false;
                        mergedTokens.set(i, token);
                    }
                }

                if (bAdd) {
                    mergedTokens.add(token);
                }
            }
        }

        for (Token token : mergedTokens) {
            String substring = text.substring(token.start, token.end);

            ExtractResult er = new ExtractResult(token.start, token.getLength(), substring, extractorName, null);

            result.add(er);
        }

        return result;
    }
}
