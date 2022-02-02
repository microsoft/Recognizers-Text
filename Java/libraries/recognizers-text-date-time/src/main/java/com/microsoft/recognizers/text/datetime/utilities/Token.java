// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.datetime.utilities;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.Metadata;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.ArrayList;
import java.util.Collection;
import java.util.List;
import java.util.regex.Pattern;

public class Token {
    private final int start;
    private final int end;
    private final Metadata metadata;

    public Token(int start, int end, Metadata metadata) {
        this.start = start;
        this.end = end;
        this.metadata = metadata;
    }

    public Token(int start, int end) {
        this.start = start;
        this.end = end;
        this.metadata = null;
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

            ExtractResult er = new ExtractResult(token.start, token.getLength(), substring, extractorName, null, token.metadata);

            result.add(er);
        }

        return result;
    }

    public static Collection<Token> getTokenFromRegex(Pattern pattern, String text) {
        Collection<Token> result = new ArrayList<>();

        for (Match match : RegExpUtility.getMatches(pattern, text)) {
            result.add(new Token(match.index, match.index + match.length));
        }

        return result;
    }
}
